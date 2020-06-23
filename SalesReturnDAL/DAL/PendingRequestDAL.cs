using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SalesReturnDAL.DAL
{
    public class PendingRequestDAL
    {
        public static List<PendingRequestModel> getPendingRequest(string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<PendingRequestModel> dataList = null;
                var PendingRequestList = context.SP_GetPendingRequest(EmployeeCode).ToList();
                if (PendingRequestList != null)
                {
                    dataList = PendingRequestList.Select(x => new PendingRequestModel()
                    {
                        BatchNo = x.BatchNo,
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerName = x.DealerName,
                        DepotName = x.DepotName,
                        DepotAddress = x.DepotAddress,
                        DepotCode = x.DepotCode,
                        FutureStatus = x.FutureStatus,
                        RequestHeaderId = x.RequestHeaderId,
                        FutureStatus_Id = x.FutureStatus_Id,
                        CurrentStatus_Id = x.CurrentStatus_Id,
                        SKUCode = x.SKUCode,
                        SKUName = x.SKUName,
                        CreatedBy_EMP_CODE = x.CreatedBy,
                        CreatedBy = context.SP_LFGDetails(x.CreatedBy).FirstOrDefault().Emp_First_name,
                        CreatedDate = x.CreatedDate,
                        
                        TotalSRV = context.tblRequestDtls.Where(o => o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p => p.SRVValue).Value,
                        RequestTypeOption=x.SKUCode,// for access data behalf on skucode
                    }).ToList();
                    return dataList;
                }
                return dataList;
            }
        }

        public static string RecommendRequest(RecommendRequestObj data)
        {

            using (var context = new SalesReturndbEntities())
            {

                string RequestProcessType = "recommended";
                var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == data.Request_Id && x.IsActive == true).ToList();
                var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == data.Request_Id && x.IsActive == true).FirstOrDefault();
                var ReqHdrDetail = context.SP_GetRequestDetail(data.Request_Id).FirstOrDefault();
                var RequestHdr = context.TblRequestHeaders.Where(x => x.RequestHeaderId == data.Request_Id && x.IsActive == true).FirstOrDefault();
                var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                var Reqfuturestatus = context.TblFutureStatus.Where(x => x.IsActive == true && x.Request_ID == ReqHdrDetail.RequestHeaderId).OrderByDescending(y => y.FutStatus_ID).FirstOrDefault();

                string AssignedTo = "";

                string Message = "";

                var CurrStatus = 0;
                var FutStatus = 0;
                var Active_role = 0;
                var Requested_role = 0;

                decimal? TotalSRV_Value = 0;
                DateTime date = DateTime.Now;
                double InvoiceAge = 0;

                //int FutureStatus_Id = 0;
                foreach (var pt in RequestDetail)
                {
                    TotalSRV_Value = TotalSRV_Value + pt.SRVValue;
                    // getting invoice age from the oldest invoice date.
                    double InvoiceDateAge = DateTime.Now.Subtract(pt.InvoiceDate.Value).Days / (365.25 / 12);
                    if (InvoiceAge <= InvoiceDateAge)
                    {
                        InvoiceAge = InvoiceDateAge;
                    }
                }

                //var ApprovalMatrixData = context.TblApprovalMatrices.Where(x => x.RequestType == data.RequestType_Id && x.Country.Equals(data.Country) && x.BUType.Equals(UserDetail.SBU_Name) && x.Division.Equals(UserDetail.Dept_name) && x.IsActive == true).FirstOrDefault();
                var FlowMatrixValue = context.TblFlowMatrices.Where(x => x.RequestType == data.RequestType_Id && x.Options.Equals("Value") && x.IsActive == true).FirstOrDefault();
                var FlowMatrixPeriod = context.TblFlowMatrices.Where(x => x.RequestType == data.RequestType_Id && x.Options.Equals("Period") &&  x.IsActive == true).FirstOrDefault();
                var EmployeeMatrixData = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == ReqHdrDetail.DepotName).FirstOrDefault();

                if (FlowMatrixValue != null && FlowMatrixPeriod != null && EmployeeMatrixData !=null)
                {
                    if (approvalHeader.Status_Id == 1)
                    {
                        if (data.RequestType_Id == 1)
                        {
                            if (EmployeeMatrixData.ComplaintManager != null || EmployeeMatrixData.ComplaintManager != "")
                            {
                                if(TotalSRV_Value <= FlowMatrixValue.ComplaintHandler && InvoiceAge <= FlowMatrixPeriod.ComplaintHandler)
                                {
                                    AssignedTo = EmployeeMatrixData.ComplaintManager;
                                }
                                else
                                {
                                    AssignedTo = UserDetail.President_Code;
                                    data.Requested_Role = 6;
                                    data.FutureStatus_Id = 9;
                                }
                                if (data.EmployeeCode.Equals(AssignedTo))
                                {
                                    //auto approval code here.
                                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                    if (TotalSRV_Value <= FlowMatrixValue.ComplaintHandler && InvoiceAge <= FlowMatrixPeriod.ComplaintHandler)
                                    {
                                        //Recommeneded complaint manager.
                                        CurrStatus = 12;
                                        FutStatus = 9;
                                        Active_role = 3;
                                        Requested_role = 6;
                                    }
                                    else
                                    {
                                        //Approve complaint manager.
                                        CurrStatus = 16;
                                        FutStatus = 0;
                                        Active_role = 3;
                                        Requested_role = 0;
                                    }

                                    data.Active_Role = Active_role;
                                    data.Requested_Role = Requested_role;
                                    data.CurrentStatus_Id = CurrStatus;
                                    data.FutureStatus_Id = FutStatus;
                                }
                            }
                            else
                            {
                                return "Error : Complaint Manager is not defined in Approval Matrix";
                            }
                        }

                        else if (data.RequestType_Id == 2)
                        {
                            if (EmployeeMatrixData.ISC != null || EmployeeMatrixData.ISC != "")
                            {
                                if (TotalSRV_Value <= FlowMatrixValue.LogisticsHead && InvoiceAge <= FlowMatrixPeriod.LogisticsHead)
                                {
                                    AssignedTo = EmployeeMatrixData.ISC;
                                }
                                else
                                {
                                    AssignedTo = UserDetail.President_Code;
                                    data.Requested_Role = 6;
                                    data.FutureStatus_Id = 9;
                                }

                                //AssignedTo = EmployeeMatrixData.LogisticsHead;
                                if (data.EmployeeCode.Equals(AssignedTo))
                                {
                                    //auto approval code here.
                                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                    if (TotalSRV_Value <= FlowMatrixValue.LogisticsHead && InvoiceAge <= FlowMatrixPeriod.LogisticsHead)
                                    {
                                        //Recommeneded Logictic Head/ VP.
                                        CurrStatus = 13;
                                        FutStatus = 9;
                                        Active_role = 5;
                                        Requested_role = 6;
                                    }
                                    else
                                    {
                                        //Approve Logictic Head/ VP.
                                        CurrStatus = 16;
                                        FutStatus = 0;
                                        Active_role = 5;
                                        Requested_role = 0;
                                    }

                                    data.Active_Role = Active_role;
                                    data.Requested_Role = Requested_role;
                                    data.CurrentStatus_Id = CurrStatus;
                                    data.FutureStatus_Id = FutStatus;
                                }
                            }
                            else
                            {
                                return "Error : Logistic Head is not set in Approval Matrix";
                            }

                        }
                        else if (data.RequestType_Id == 3)
                        {
                            var ReqHdr = context.TblRequestHeaders.Where(x => x.RequestHeaderId == data.Request_Id && x.IsActive == true).FirstOrDefault();

                            if (ReqHdr.RequestTypeOption == "D")
                            {
                                //In D type request we will refer VP/SH/President of RM not of requester
                                
                                var RM_Trail = context.SP_LFGDetails(data.EmployeeCode).FirstOrDefault();
                                if (TotalSRV_Value <= FlowMatrixValue.VP && InvoiceAge <= FlowMatrixPeriod.VP)
                                {
                                    if (RM_Trail.VPHead != "NA" && RM_Trail.VPHead != "" && RM_Trail.VPHead != null)
                                    {
                                        AssignedTo = UserDetail.VPHead;
                                        data.FutureStatus_Id = 8; //Pending Sales Director / VP Head;
                                        data.Requested_Role = 9;

                                        if (data.EmployeeCode.Equals(AssignedTo))
                                        {
                                            //auto approval code here.
                                            ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                            data.Active_Role = 9;
                                            data.Requested_Role = 0;
                                            data.CurrentStatus_Id = 16;
                                            data.FutureStatus_Id = 0;
                                        }
                                    }
                                    else
                                    {
                                        return "Error : Sales Director/VP Head is not Defined.";
                                    }
                                }
                                else
                                {
                                    if (RM_Trail.President_Code != "NA" && RM_Trail.President_Code != "" && RM_Trail.President_Code != null)
                                    {
                                        AssignedTo = UserDetail.President_Code;
                                        data.FutureStatus_Id = 9;//Pending President
                                        data.Requested_Role = 6;

                                        if (data.EmployeeCode.Equals(AssignedTo))
                                        {
                                            //auto approval code here.
                                            ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                            data.Active_Role = 6;
                                            data.Requested_Role = 0;
                                            data.CurrentStatus_Id = 16;
                                            data.FutureStatus_Id = 0;
                                        }
                                    }
                                    else
                                    {
                                        return "Error : President code is not in LFG.";
                                    }
                                }
                            }
                            else
                            {
                                if (TotalSRV_Value <= FlowMatrixValue.SegmentHead && InvoiceAge <= FlowMatrixPeriod.SegmentHead)
                                {
                                    var DealerDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(ReqHdr.DealerId).FirstOrDefault();
                                   // if (DealerDtl!=null)
                                    if(UserDetail.segmentHead != "NA" && UserDetail.segmentHead != "" && UserDetail.segmentHead != null)
                                    {
                                        //AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE;
                                        if (UserDetail.segmentHead != "NA" && UserDetail.segmentHead != "" && UserDetail.segmentHead != null)
                                        AssignedTo = UserDetail.segmentHead;
                                        else
                                        {
                                            return "Error : Segment Head is not in LFG.";
                                            if (DealerDtl != null)
                                            {
                                                AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE;
                                            }
                                        }
                                        data.FutureStatus_Id = 7; //Pending Segment Head;
                                        data.Requested_Role = 8;

                                        if (data.EmployeeCode.Equals(AssignedTo))
                                        {
                                            //auto approval code here.

                                            ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                            data.Active_Role = 8;
                                            data.Requested_Role = 0;
                                            data.CurrentStatus_Id = 16;
                                            data.FutureStatus_Id = 0;
                                        }
                                    }
                                    else
                                    {
                                        return "Error : Segment head is not defined in PMS.";
                                    }
                                }
                                else if (TotalSRV_Value <= FlowMatrixValue.VP && InvoiceAge <= FlowMatrixPeriod.VP)
                                {
                                    var DealerDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(ReqHdr.DealerId).FirstOrDefault();
                                    //if (DealerDtl!=null)
                                    if(UserDetail.VPHead != "NA" && UserDetail.VPHead != "" && UserDetail.VPHead != null)
                                    {
                                        // AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().VPHead;
                                        if (UserDetail.VPHead != "NA" && UserDetail.VPHead != "" && UserDetail.VPHead != null)
                                        {
                                            AssignedTo = UserDetail.VPHead;
                                        }
                                        else
                                        {
                                            return "Error : VP Head is not in LFG.";
                                            if (DealerDtl != null)
                                            {
                                                AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().VPHead;
                                            }
                                           
                                        }
                                        data.FutureStatus_Id = 8; //Pending Sales Director / VP Head;
                                        data.Requested_Role = 9;

                                        if (data.EmployeeCode.Equals(AssignedTo))
                                        {
                                            //auto approval code here.
                                            ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                            data.Active_Role = 9;
                                            data.Requested_Role = 0;
                                            data.CurrentStatus_Id = 16;
                                            data.FutureStatus_Id = 0;
                                        }
                                    }
                                    else
                                    {
                                        return "Error : Sales Director/VP Head is not Defined.";
                                    }
                                }
                                else
                                {
                                     var DealerDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(ReqHdr.DealerId).FirstOrDefault();
                                    //if (DealerDtl!=null)
                                    if (UserDetail.President_Code != "NA" && UserDetail.President_Code != "" && UserDetail.President_Code != null)
                                    {
                                        // AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().President_Code;
                                        if (UserDetail.President_Code != "NA" && UserDetail.President_Code != "" && UserDetail.President_Code != null)
                                        {
                                            AssignedTo = UserDetail.President_Code;
                                        }
                                        else
                                        {
                                            return "Error : President code is not in LFG.";
                                            if (DealerDtl != null)
                                            {
                                                AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().President_Code;
                                            }
                                           
                                        }
                                        data.FutureStatus_Id = 9;//Pending President
                                        data.Requested_Role = 6;

                                        if (data.EmployeeCode.Equals(AssignedTo))
                                        {
                                            //auto approval code here.
                                            ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                            data.Active_Role = 6;
                                            data.Requested_Role = 0;
                                            data.CurrentStatus_Id = 16;
                                            data.FutureStatus_Id = 0;
                                        }
                                    }
                                    else
                                    {
                                        return "Error : President code is not in LFG.";
                                    }
                                }
                            }
                        }
                    }
                    else if (approvalHeader.Status_Id == 10 || approvalHeader.Status_Id == 11)
                    {
                        if (UserDetail.President_Code.ToUpper().Trim() != "NA" && UserDetail.President_Code.Trim() != "" && UserDetail.President_Code != null)
                        {
                            AssignedTo = UserDetail.President_Code;//context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault().President_Code;

                            if (data.EmployeeCode.Equals(AssignedTo))
                            {
                                //auto approval code here.
                                ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                                data.Active_Role = 6;
                                data.Requested_Role = 0;
                                data.CurrentStatus_Id = 16;
                                data.FutureStatus_Id = 0;
                            }
                        }
                        else
                        {
                            return "Error : President code is not in LFG.";
                        }
                    }

                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignedTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                    SendMail(approvalHeader, data, RequestProcessType, AssignedTo);

                    Message = "Success : "+ RequestHdr.RequestTypeOption+"-" + data.Request_Id + " Request has been Updated.";
                }
                else
                {
                    Message = "Error : Approval matrix is not Defined. Please contact admin.";
                }
                return Message;
            }
        }

        public static string RejectRequest(RecommendRequestObj data)
        {
            using (var context = new SalesReturndbEntities())
            {
                string RequestProcessType = "reject";
                string AssignTo = string.Empty;
                var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == data.Request_Id).ToList();
                var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == data.Request_Id).FirstOrDefault();
                var ReqHdr= context.TblRequestHeaders.Where(x => x.RequestHeaderId == data.Request_Id && x.IsActive==true).FirstOrDefault();
                var IfDepotPerson = context.TblEmployeeMasters.Where(x => x.Depotcode == ReqHdr.EmployeeCode && x.IsActive == true).FirstOrDefault();
                var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                var Reqfuturestatus = context.TblFutureStatus.Where(x => x.IsActive == true && x.Request_ID== ReqHdr.RequestHeaderId).OrderByDescending(y=>y.FutStatus_ID).FirstOrDefault();
                if (data.FutureStatus_Id == 10026 && IfDepotPerson == null) {
                    if (UserDetail.Regional_Head.Trim().Equals("NA") || UserDetail.Regional_Head.Trim().Equals("N/A"))
                        return "Error: Approval Matrix not defined!!";
                    else
                    {
                        AssignTo = UserDetail.Regional_Head;
                        ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);
                    }
                }
                else if (data.FutureStatus_Id == 10026 && IfDepotPerson != null) {
                    var DealerDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(ReqHdr.DealerId).FirstOrDefault();
                    bool IsRMExist = DealerDtl != null ? (DealerDtl.RMCode.Equals(string.Empty) ? false : (DealerDtl.RMCode.Contains("NA") ? false : (DealerDtl.RMCode == null ? false : true))) : false;
                    //if (IsRMExist) {
                    if (UserDetail.Regional_Head!="NA" && UserDetail.Regional_Head !=null && UserDetail.Regional_Head !="") {
                        // AssignTo = DealerDtl.RMCode;
                        AssignTo = UserDetail.Regional_Head;
                       
                        ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);
                    }
                    else
                        return "Error: Approval Matrix not defined!!";

                }
                else
                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);
                SendMail(approvalHeader,data,  RequestProcessType, ReqHdr.EmployeeCode);
                return "Success : "+ ReqHdr.RequestTypeOption+ data.Request_Id + " Requested has been rejected successfully.";
            }
        }

        public static string GetHtml(string html, RecommendRequestObj obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                RequestDetailObj_Render data1 = CommonDAL.GetRequestDetails(obj.Request_Id, obj.CurrentStatus_Id, obj.FutureStatus_Id);
                
                string RequestNumber = data1.RequestTypeOption + "-" + data1.RequestHeader_Id;
                int SKUCount = data1.RequestDetail.Count;
                html = html.Replace("&lt;CustomerCode&gt;", data1.DealerCodeForMail);
                html = html.Replace("&lt;CustomerName&gt;", data1.DealerNameForMail);
                html = html.Replace("&lt;RequestNumber&gt;", RequestNumber);
                html = html.Replace("&lt;DepotName&gt;", data1.DepotNameForMail);
                var reason = context.TblReasonMasters.Where(x => x.ReasonMaster_Id == data1.ReasonForReturn_Id).FirstOrDefault();
                html = html.Replace("&lt;Reason&gt;", reason.Reason);
                html = html.Replace("&lt;CountofSKUs&gt;", SKUCount.ToString());
                //html = html.Replace("Requestor Name;", SKUCount.ToString());
                decimal? SalesReturnVolume = 0;
                decimal? SRVValue = 0;
                decimal? ExpiredVoulme = 0;
                //ToString("0.00");
                for (int i = 0; i < data1.RequestDetail.Count; i++)
                {
                    if (data1.RequestDetail[i].Volume != null)
                    {
                        SalesReturnVolume += data1.RequestDetail[i].Volume;
                    }
                    if (data1.RequestDetail[i].SRVValue != null)
                    {
                        SRVValue = SRVValue + data1.RequestDetail[i].SRVValue;
                    }
                    if (Convert.ToInt32(data1.RequestDetail[i].selectedSKU.Shelf_Life) < 0)
                    {
                        ExpiredVoulme = ExpiredVoulme + data1.RequestDetail[i].Volume;
                    }
                }
                html = html.Replace("&lt;SalesReturnVolume&gt;", Convert.ToDecimal(string.Format("{0:F2}", SalesReturnVolume)).ToString());
                html = html.Replace("&lt;VolumeExpired&gt;", Convert.ToDecimal(string.Format("{0:F2}", ExpiredVoulme)).ToString());


                html = html.Replace("&lt;SalesReturnAmount&gt;", Convert.ToDecimal(string.Format("{0:F2}", SRVValue)).ToString());

                return html;
            }

        }

        public static MailAddressCollection GetCC(RecommendRequestObj obj)
        {
            try
            {
                using (var context = new SalesReturndbEntities())
                {
                    MailAddressCollection ccEmail = new MailAddressCollection();
                    RequestDetailObj_Render data1 = CommonDAL.GetRequestDetails(obj.Request_Id, obj.CurrentStatus_Id, obj.FutureStatus_Id);
                    var RegionalHead = context.SP_LFGDetails(data1.EmployeeCode).FirstOrDefault();
                    
                    if( RegionalHead != null) {
                        if (!(RegionalHead.Regional_Head.ToUpper().Trim().Equals("NA") || RegionalHead.Regional_Head.Trim().Equals(string.Empty)))
                        {
                            var RegionalHeadData = context.SP_LFGDetails(RegionalHead.Regional_Head).FirstOrDefault();
                            ccEmail.Add(RegionalHeadData.email_id);
                        }
                        if (!(RegionalHead.segmentHead.ToUpper().Trim().Equals("NA") || RegionalHead.segmentHead.Trim().Equals(string.Empty)))
                        {
                            var SegmentHeadData = context.SP_LFGDetails(RegionalHead.segmentHead).FirstOrDefault();
                            ccEmail.Add(SegmentHeadData.email_id);
                        }
                        ccEmail.Add(RegionalHead.email_id);//Email Id of Requestor
                    } 

                    var DepotMaster = context.SP_GetDepotList().Where(x => x.DepotId == data1.DepotId).FirstOrDefault();
                    var AssintoEmp = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == DepotMaster.DepotName).FirstOrDefault();
                    var ComplaintHandler = context.SP_LFGDetails(AssintoEmp.ComplaintHandler).FirstOrDefault();
                    var CompalintManager = context.SP_LFGDetails(AssintoEmp.ComplaintManager).FirstOrDefault();
                    ccEmail.Add(ComplaintHandler.email_id);
                    ccEmail.Add(CompalintManager.email_id);
                    var LogistickHead = context.SP_LFGDetails(AssintoEmp.LogisticsHead).FirstOrDefault();
                    ccEmail.Add(LogistickHead.email_id);
                    if(RegionalHead !=null)
                    {
                        if (!(RegionalHead.VPHead.ToUpper().Trim().Equals("NA") || RegionalHead.VPHead.Trim().Equals(string.Empty)))
                        {
                            var vphead = context.sp_GetuserDetailsFromLFG(RegionalHead.VPHead).FirstOrDefault();
                            ccEmail.Add(vphead.email_id);
                        }
                        if (!(RegionalHead.President_Code.ToUpper().Trim().Equals("NA") || RegionalHead.President_Code.Trim().Equals(string.Empty)))
                        {
                            var President_Code = context.sp_GetuserDetailsFromLFG(RegionalHead.President_Code).FirstOrDefault();
                            ccEmail.Add(President_Code.email_id);
                        }
                    }
                   
                    var depot = context.SP_LFGDetails(AssintoEmp.Depotcode).FirstOrDefault();
                    ccEmail.Add(depot.email_id);
                    var cso = context.SP_LFGDetails(AssintoEmp.CSO).FirstOrDefault();
                    ccEmail.Add(cso.email_id);
                    return ccEmail;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void SendMail(TblApproverHeader ApproverHdr, RecommendRequestObj data,string ProcessType,string to)
        {
            try
            {
                using (var context = new SalesReturndbEntities())
                {
                    TblMailTemplate EmailTemplate = new TblMailTemplate();
                    string subject = string.Empty;

                    if(ProcessType=="reject")
                    {


                        if (data.Active_Role == 2)//ch
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 5).FirstOrDefault();
                        }
                        else if (data.Active_Role == 3)//cm
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 8).FirstOrDefault();
                        }
                        else if (data.Active_Role == 4)//lm
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 11).FirstOrDefault();
                        }
                        else if (data.Active_Role == 5)//ISC/Vp
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 14).FirstOrDefault();
                        }
                       else if (data.Active_Role == 8)//sh
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 21).FirstOrDefault();
                        }
                        else if (data.Active_Role == 7)//Regionl Head
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 18).FirstOrDefault();
                        }
                        else if (data.Active_Role == 5)//Vp Head
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 24).FirstOrDefault();
                        }
                        else if (data.Active_Role == 6)//President
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 27).FirstOrDefault();
                        }
                     //   subject = EmailTemplate.Subject.Replace("<RequestNo.>", "S" + "-" + data.Request_Id);

                    }
                    if(ProcessType == "reconsider")
                    {

                        if (data.Active_Role == 2) //ch
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 6).FirstOrDefault();
                        }
                        else if (data.Active_Role == 3) //cm
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 9).FirstOrDefault();
                        }
                        else if (data.Active_Role == 4) //lm
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 12).FirstOrDefault();
                        }
                        else if (data.Active_Role == 5) //isc Vp
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 15).FirstOrDefault();
                        }
                        else if (data.Active_Role == 7) //regional head
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 19).FirstOrDefault();
                        }
                        else if (data.Active_Role == 8) //Segment head
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 22).FirstOrDefault();
                        }
                        else if (data.Active_Role == 5) //isc Vp
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 25).FirstOrDefault();
                        }
                        else if (data.Active_Role == 6) //presdient
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 28).FirstOrDefault();
                        }
                       // subject = EmailTemplate.Subject.Replace("<RequestNo.>", "S" + "-" + data.Request_Id);

                    }

                    if (ProcessType == "recommended")
                    {

                        if (data.Active_Role == 2)
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 4).FirstOrDefault();
                        }
                        else if (data.Active_Role == 4)
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 10).FirstOrDefault();
                        }
                        else if (data.Active_Role == 7)
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 17).FirstOrDefault();
                        }
                        
                        //subject = EmailTemplate.Subject.Replace("<RequestNo.>", data.Request_Id.ToString());

                    }
                    if (ProcessType == "approve")
                    {
                        if (data.Active_Role == 5)
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 13).FirstOrDefault();
                        }

                        else if (data.Active_Role == 3)//cm
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 7).FirstOrDefault();
                        }
                        else if (data.Active_Role == 7)//rh
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 26).FirstOrDefault();
                        }
                        else if (data.Active_Role == 9)//sh
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 20).FirstOrDefault();
                        }
                        else if (data.Active_Role == 6) //president
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 26).FirstOrDefault();
                        }
                        else if (data.Active_Role == 7) //RegionalHead
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 16).FirstOrDefault();
                        }
                       // subject = EmailTemplate.Subject.Replace("<RequestNo.>", data.Request_Id.ToString());
                    }
                    subject = EmailTemplate.Subject;
                    subject = GetSubject(subject,data);
                    string Body = GetHtml(EmailTemplate.MailBody, data);
                    //var CCMails = GetCC(data);
                    MailAddressCollection CCMails = new MailAddressCollection();
                    //CCMails.Add("pratik.jain@nipponpaint.co.in");
                    CCMails.Add("shubham.jain@phoenixtech.consulting");
                    int RequestType = data.RequestType_Id;
                    var UserDetail = context.SP_LFGDetails(to).FirstOrDefault();
                    var FromEmail = context.SP_LFGDetails(data.EmployeeCode).FirstOrDefault();
                    Body = Body.Replace("Requestor Name", FromEmail.Emp_First_name);
                    MailAddressCollection toMail = new MailAddressCollection();
                    MailAddressCollection fromMail = new MailAddressCollection();
                    subject = subject.Replace("<EmployeeName>", FromEmail.Emp_First_name);
                    // toMail.Add(UserDetail.email_id);
                   // toMail.Add("vivek.anand@nipponpaint.co.in");
                    toMail.Add("amitkumar20895391@gmail.com");
                    
                    fromMail.Add(FromEmail.email_id);
                    Email.sendEmail(subject, "amit.kumar@phoenixtech.consulting", toMail, CCMails, Body);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string GetSubject(String html,RecommendRequestObj obj)
        {
            RequestDetailObj_Render data1 = CommonDAL.GetRequestDetails(obj.Request_Id, obj.CurrentStatus_Id, obj.FutureStatus_Id);
            html = html.Replace("<RequestNo.>", data1.RequestTypeOption + "- " + data1.RequestHeader_Id);
            return html;
        }
        public static string ReconsiderRequest(RecommendRequestObj data)
        {
            using (var context = new SalesReturndbEntities())
            {
                string RequestProcessType = "reconsider";
                var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == data.Request_Id).ToList();
                var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == data.Request_Id).FirstOrDefault();
                //var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                var Reqfuturestatus = context.TblFutureStatus.Where(x => x.IsActive == true && x.Request_ID == approvalHeader.Request_Id).OrderByDescending(y => y.FutStatus_ID).FirstOrDefault();

                ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, approvalHeader.CreatedBy, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);
                SendMail(approvalHeader, data, RequestProcessType, approvalHeader.CreatedBy);
                return "Success : S-"+ data.Request_Id + " Requested has been reconsidered successfully.";
            }
        }

        public static string ApproveRequest(RecommendRequestObj data)
        {
            try
            {

            
            using (var context = new SalesReturndbEntities())
            {
                    string RequestType = "approve";
                var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == data.Request_Id && x.IsActive==true).ToList();
                var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == data.Request_Id && x.IsActive == true).FirstOrDefault();
                var ReqHdr=context.TblRequestHeaders.Where(x=>x.RequestHeaderId== data.Request_Id && x.IsActive == true).FirstOrDefault();
                    RequestDetailObj_Render data1 = CommonDAL.GetRequestDetails(data.Request_Id, data.CurrentStatus_Id, data.FutureStatus_Id);
                var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                var DepotDTl = context.sp_GetDealerDtlBy_DealerRepositoryId(ReqHdr.DealerId).FirstOrDefault();
                string assignTo = string.Empty;
                   
                var DepotPersonDtl = context.TblEmployeeMasters.Where(x => x.DepotName == DepotDTl.Depot).FirstOrDefault();
                if (data.FutureStatus_Id == 10019)
                    assignTo = DepotPersonDtl.Depotcode;
                ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, assignTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);
                    SendMail(approvalHeader, data, RequestType, assignTo);
                    return "Success : "+ ReqHdr .RequestTypeOption+ "-"+ data.Request_Id + " Requested has been approved successfully.";
            }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string CloseRequest(CloseRequestObjForStg_4 data)
        {
            using (var context = new SalesReturndbEntities())
            {
                string AssignTo = string.Empty;
                string RetMsg= string.Empty;
                var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == data.Request_Id).ToList();
                var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == data.Request_Id).FirstOrDefault();
                var ReqHdr = context.TblRequestHeaders.Where(x => x.RequestHeaderId == data.Request_Id && x.IsActive == true).FirstOrDefault();
                var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                var DlrDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(data.Request.DealerId).FirstOrDefault();
                if (data.FutureStatus_Id == 10027 || data.CurrentStatus_Id == 10028 || data.CurrentStatus_Id == 10029)
                {
                    var EmpMaster = context.TblEmployeeMasters.Where(x => x.DepotName == DlrDtl.Depot && x.IsActive == true).FirstOrDefault();
                    AssignTo = data.FutureStatus_Id == 10027 ? EmpMaster.Depotcode : "";
                    if (data.Request.IsCommercialSettlement == true)
                    {
                        var EpCheck = context.sp_CheckEPNumber(data.Request.EPNo).FirstOrDefault();
                        if (EpCheck == null)
                            return "Error: EP Number does not exists";
                    }
                    ReqHdr.IsCommercialSettlement = data.Request.IsCommercialSettlement;
                    ReqHdr.MaterialWillGoToDealer = data.Request.IsCommercialSettlement == true ? false : true;
                    ReqHdr.ReasonForCommercialSettlement = data.Request.IsCommercialSettlement == true ? data.Request.ReasonForCommercialSettlement : "";
                    ReqHdr.DetailsForMaterialGoToDealer = data.Request.IsCommercialSettlement == false ? data.Request.DetailsForMaterialGoToDealer : "";
                    ReqHdr.EPNo = data.Request.IsCommercialSettlement == true ? data.Request.EPNo : "";
                    ReqHdr.ModifiedBy = data.EmployeeCode;
                    ReqHdr.ModifiedDate = DateTime.Now;

                    context.Entry(ReqHdr).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    RetMsg = data.FutureStatus_Id == 10027 ? "Request has been forwarded to Depot!" : "Request has been closed successfully!";
                }
                else if (data.CurrentStatus_Id == 10030)
                {
                    ReqHdr.DocketNumber = data.Request.DocketNumber;
                    ReqHdr.DocketDate = data.Request.DocketDate;
                    context.Entry(ReqHdr).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    RetMsg = "Request has been closed successfully!";
                }
                else { RetMsg = "Request has been closed successfully!"; }

                ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, AssignTo, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                return "Success : " + ReqHdr.RequestTypeOption + data.Request_Id + " "+ RetMsg;
            }
        }

    }
}
