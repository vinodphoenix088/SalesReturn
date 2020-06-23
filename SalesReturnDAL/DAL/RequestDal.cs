using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System.Transactions;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SalesReturnDAL.DAL
{
    public class RequestDal
    {
        public static RequestDetailObj_Render GetSavedAsDraftRequest(string EmpCode, int Depot_Id, int Dealer_Id, int Reason_Id)
        {
            using (var context = new SalesReturndbEntities())
            {
                var Request_Id = context.SP_GetSavedAsDraftRequest(EmpCode, Depot_Id, Dealer_Id, Reason_Id).FirstOrDefault();

                if (Request_Id != null)
                {
                    var requestDetail = CommonDAL.GetRequestDetails(Request_Id, 10017, 10017);


                    return requestDetail;
                }

                return null;
            }


        }

        public static List<int> saveRequestAsDraft(RequestDetailObj obj, ref string StrReturn, ref long RequestHeader_Id)
        {
            using (var context = new SalesReturndbEntities())
            {

                List<int> List_Detail_Id = null;
                //  long RequestHeader_Id = 0;
                int Approver_Id = 0;

                List_Detail_Id = new List<int>();
                using (TransactionScope T3 = new TransactionScope())
                {
                    try
                    {
                        if (obj.RequestHeader_Id == null)
                        {
                            TblRequestHeader header = new TblRequestHeader()
                            {
                                //BatchNo = obj.BatchNo,
                                DealerId = obj.DealerId,
                                DepotId = obj.DepotId,
                                ReasonForReturn = obj.ReasonForReturn_Id,
                                //SKUCode = obj.selectedSKU.SKUCode,
                                //SKUName = obj.selectedSKU.SKUName,
                                EmployeeCode = obj.EmployeeCode,
                                IsActive = true,
                                CreatedBy = obj.EmployeeCode,
                                CreatedDate = DateTime.Now
                            };

                            context.Entry(header).State = EntityState.Added;
                            context.SaveChanges();
                            RequestHeader_Id = header.RequestHeaderId;

                            TblApproverHeader Aheader = new TblApproverHeader()
                            {
                                Active_Role = 1,
                                AssignedTo = "",
                                Requested_Role = 1,
                                Request_Id = Convert.ToInt32(RequestHeader_Id),
                                Status_Id = 10017,
                                IsActive = true,
                                CreatedBy = obj.EmployeeCode,
                                CreatedDate = DateTime.Now,
                            };

                            context.Entry(Aheader).State = EntityState.Added;
                            context.SaveChanges();

                            Approver_Id = Aheader.Approver_Id;

                            TblApproverDetail Appdetail = new TblApproverDetail()
                            {
                                Approver_Id = Approver_Id,
                                AssignedTo = "",
                                Remark = "",
                                Role_Id = 1,
                                Status_Id = 10017,
                                CreatedBy = obj.EmployeeCode,
                                CreatedDate = DateTime.Now,
                                IsActive = true

                            };
                            context.Entry(Appdetail).State = EntityState.Added;
                            context.SaveChanges();

                            TblFutureStatu statu = new TblFutureStatu()
                            {

                                Request_ID = RequestHeader_Id,
                                EmployeeCode = "",
                                Role = 1,
                                Status = 10017,
                                CreatedBy = obj.EmployeeCode,
                                CreatedDate = DateTime.Now,
                                IsActive = true

                            };

                            context.Entry(statu).State = EntityState.Added;
                            context.SaveChanges();

                        }
                        else
                        {

                            var updateHeader = context.TblRequestHeaders.Where(x => x.RequestHeaderId == obj.RequestHeader_Id && x.IsActive == true).FirstOrDefault();
                            updateHeader.DealerId = obj.DealerId;
                            updateHeader.DepotId = obj.DepotId;
                            updateHeader.ReasonForReturn = obj.ReasonForReturn_Id;
                            updateHeader.ModifiedBy = obj.EmployeeCode;
                            updateHeader.ModifiedDate = DateTime.Now;
                            context.Entry(updateHeader).State = EntityState.Modified;
                            context.SaveChanges();

                            RequestHeader_Id = updateHeader.RequestHeaderId;

                            var statu_id = context.TblApproverHeaders.Where(x => x.Request_Id == updateHeader.RequestHeaderId).FirstOrDefault();
                            if (statu_id != null && statu_id.Status_Id == 18 || statu_id.Status_Id == 19 || statu_id.Status_Id == 20 || statu_id.Status_Id == 21
                                 || statu_id.Status_Id == 22 || statu_id.Status_Id == 23 || statu_id.Status_Id == 24 || statu_id.Status_Id == 25
                                  || statu_id.Status_Id == 26)
                            {
                                var upApp = context.TblApproverHeaders.Where(x => x.Request_Id == updateHeader.RequestHeaderId).FirstOrDefault();

                                upApp.Active_Role = 1;
                                upApp.AssignedTo = obj.EmployeeCode;
                                upApp.Requested_Role = 1;
                                upApp.Request_Id = Convert.ToInt32(RequestHeader_Id);
                                upApp.Status_Id = 10017;
                                upApp.IsActive = true;
                                upApp.ModifiedBy = obj.EmployeeCode;
                                upApp.ModifiedDate = DateTime.Now;


                                context.Entry(upApp).State = EntityState.Modified;
                                context.SaveChanges();

                                Approver_Id = upApp.Approver_Id;

                                TblApproverDetail Appdetail = new TblApproverDetail()
                                {
                                    Approver_Id = Approver_Id,
                                    AssignedTo = "",
                                    Remark = "",
                                    Role_Id = 1,
                                    Status_Id = 10017,
                                    CreatedBy = obj.EmployeeCode,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true

                                };
                                context.Entry(Appdetail).State = EntityState.Added;
                                context.SaveChanges();

                                TblFutureStatu statu = new TblFutureStatu()
                                {

                                    Request_ID = RequestHeader_Id,
                                    EmployeeCode = "",
                                    Role = 1,
                                    Status = 10017,
                                    CreatedBy = obj.EmployeeCode,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true

                                };

                                context.Entry(statu).State = EntityState.Added;
                                context.SaveChanges();
                            }
                        }

                        tblRequestDtl thadr = null;
                        foreach (var dt in obj.RequestDetail)
                        {

                            long Detail_Id = 0;

                            //if (dt.Detail_Id == null)
                            //{
                            thadr = new tblRequestDtl();

                            //thadr.BatchNo = dt.selectedSKU == "Other" ? dt.selectedSKU.BatchNoText : dt.selectedSKU.Batch_No;
                            thadr.BatchNo = dt.selectedSKU.Batch_No == "Other" ? dt.selectedSKU.BatchNoText : dt.selectedSKU.Batch_No;
                            thadr.SKUCode = dt.selectedSKU != null ? dt.selectedSKU.SKUCode : "";
                            thadr.SKUName = dt.selectedSKU != null ? dt.selectedSKU.SKUName : "";
                            thadr.InvoiceNumber = dt.InvoiceNo;
                            thadr.InvoiceDate = dt.InvoiceDate;
                            thadr.ReadyToProvideGST = dt.ProvideGST_Yes;
                            thadr.InvoiceQuantity = dt.InvoiceQuantity;
                            thadr.SRVQuantity = dt.SRVQuantity;
                            // thadr.Unit = dt.Unit;
                            //thadr.PackSize = dt.PackSize;
                            thadr.SubReason = dt.SubreasonId;
                            thadr.Unit = dt.selectedSKU != null ? dt.selectedSKU.Unit : "";
                            thadr.PackSize = dt.selectedSKU != null ? dt.selectedSKU.PackSize : 0;

                            thadr.Volume = dt.selectedSKU != null ? dt.InvoiceQuantity * dt.selectedSKU.PackSize : 0;
                            thadr.SRVValue = dt.SRVValue;
                            thadr.CCNo = obj.ReasonForReturn_Id == 1 && dt.selectedComplaint!=null ? Convert.ToInt32(dt.selectedComplaint.ComplaintNumber) : 0;
                            thadr.Remarks = dt.Remarks;
                            thadr.RequestHeaderId = RequestHeader_Id;

                            thadr.IsActive = true;
                            thadr.CreatedBy = obj.EmployeeCode;
                            thadr.CreatedDate = DateTime.Now;
                            thadr.Shelf_Life = dt.selectedSKU.Shelf_Life;
                            thadr.MFG_Date= dt.selectedSKU.Manufacturing_Date == null ? dt.selectedSKU.MGfDate : dt.selectedSKU.Manufacturing_Date;
                            context.Entry(thadr).State = EntityState.Added;
                            context.SaveChanges();

                            Detail_Id = thadr.Id;
                            //}
                            //else
                            //{
                            //    var updetail = context.tblRequestDtls.Where(x => x.Id == dt.Detail_Id && x.IsActive == true).FirstOrDefault();

                            //    updetail.BatchNo = dt.BatchNo;
                            //    updetail.SKUCode = dt.selectedSKU.SKUCode;
                            //    updetail.SKUName = dt.selectedSKU.SKUName;
                            //    updetail.InvoiceNumber = dt.InvoiceNo;
                            //    updetail.InvoiceDate = dt.InvoiceDate;
                            //    updetail.ReadyToProvideGST = dt.ProvideGST_Yes;
                            //    updetail.InvoiceQuantity = dt.InvoiceQuantity;
                            //    updetail.SRVQuantity = dt.SRVQuantity;
                            //    updetail.Unit = dt.Unit;
                            //    updetail.PackSize = dt.PackSize;
                            //    updetail.Volume = dt.InvoiceQuantity * dt.PackSize;
                            //    updetail.SRVValue = dt.SRVValue;
                            //    updetail.CCNo = obj.ReasonForReturn_Id == 1 ? Convert.ToInt32(dt.selectedComplaint.Complaint_ID.Value) : 0;
                            //    updetail.Remarks = dt.Remarks;
                            //    updetail.RequestHeaderId = RequestHeader_Id;

                            //    updetail.IsActive = true;
                            //    updetail.ModifiedBy = obj.EmployeeCode;
                            //    updetail.ModifiedDate = DateTime.Now;

                            //    context.Entry(updetail).State = EntityState.Modified;
                            //    context.SaveChanges();

                            //    Detail_Id = updetail.Id;
                            //}


                            if (dt.ProvideGST_Yes == true && (dt.UploadedInvoice == null || dt.UploadedInvoice == ""))
                            {
                                RequestDal.DisableAllAlreadySavedImages(Detail_Id, obj.EmployeeCode);

                                List_Detail_Id.Add(Convert.ToInt32(Detail_Id));
                            }
                            else if (dt.ProvideGST_Yes == true && (dt.UploadedInvoice != null || dt.UploadedInvoice != ""))
                            {
                                RequestDal.UpdateAllAlreadySavedImages(dt.Detail_Id, Detail_Id, obj.EmployeeCode);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        StrReturn = "Error : " + StrReturn;
                        return null;
                    }

                    StrReturn = "Success : Request saved successfully.";
                    T3.Complete();
                    T3.Dispose();
                }
                return List_Detail_Id;
            }
        }

        public static string AckbyCommercial(RequestDetailObjbyDepot obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope T4 = new TransactionScope())
                {
                    string AssignTo = "";

                    var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == obj.Request_Id).ToList();
                    var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == obj.Request_Id).FirstOrDefault();
                    var currentstatus = context.SP_GetRequestDetail(Convert.ToInt32(obj.Request_Id)).FirstOrDefault();

                    AssignTo = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == currentstatus.DepotName).FirstOrDefault().Depotcode;

                    var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                    foreach (var Reqdtl in obj.RequestDetail)
                    {
                        var Dtl = context.tblRequestDtls.Where(x => x.RequestHeaderId == obj.Request_Id && x.Id== Reqdtl.Detail_Id &&x.IsActive==true).FirstOrDefault();
                        Dtl.ReleaseByCM = Reqdtl.ReleaseByCM;
                        Dtl.DONo = Reqdtl.DONo;
                        Dtl.ReleaseByCM_Date = DateTime.Now;
                        Dtl.ModifiedBy = obj.EmployeeCode;
                        Dtl.ModifiedDate = DateTime.Now;

                        context.Entry(Dtl).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(obj.Request_Id), obj.EmployeeCode, AssignTo, obj.Active_Role, obj.Requested_Role, obj.CurrentStatus_Id, obj.FutureStatus_Id, obj.Remarks);

                    T4.Complete();
                    T4.Dispose();
                }
                return "Success : S-" + obj.Request_Id + " Requested has been updated successfully";
            }
        }

        public static string UpdateRequestbyCSO(RequestDetailObjbyDepot obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                string s = string.Empty;
                string retMsg = string.Empty;
                //TransactionOptions transactionOptions = new TransactionOptions();
                //transactionOptions.Timeout = TimeSpan.MaxValue;
                using (TransactionScope T6 = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
                {
                    string AssignTo = "";
                    RequestDetailObj Excessdata = new RequestDetailObj();
                    Excessdata.RequestDetail = new List<RequestDetailArray>();
                    var ReqHdr=context.TblRequestHeaders.Where(x=>x.RequestHeaderId == obj.Request_Id).FirstOrDefault();
                    var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == obj.Request_Id).ToList();
                    var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == obj.Request_Id).FirstOrDefault();
                    var currentstatus = context.SP_GetRequestDetail(Convert.ToInt32(obj.Request_Id)).FirstOrDefault();
                    var EmployeeMaster = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == currentstatus.DepotName).FirstOrDefault();

                    if (obj.FutureStatus_Id == 10021)
                    {
                        if (EmployeeMaster.CommercialCode.Trim().Equals(string.Empty) || EmployeeMaster.CommercialCode.Trim().Contains("NA"))
                        { return "Error: Commercial Manager not defind, Kindly contact admin!"; }
                        else
                        {
                            AssignTo = EmployeeMaster.CommercialCode;
                        }
                    }
                    if (obj.FutureStatus_Id == 10019)
                    {
                        //var DepotDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(obj.DealerId).FirstOrDefault();
                        AssignTo = EmployeeMaster.Depotcode;
                    }

                    var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();

                    foreach (var dt in obj.RequestDetail)
                    {
                        var thadr = context.tblRequestDtls.Where(x => x.IsActive == true && x.Id == dt.Detail_Id).FirstOrDefault();

                        thadr.DONo = dt.DONo;
                        thadr.ReasonforSAP = dt.SAPsubReasonID;
                        thadr.ModifiedBy = obj.EmployeeCode;
                        thadr.ModifiedDate = DateTime.Now;

                        context.Entry(thadr).State = EntityState.Modified;
                        context.SaveChanges();

                        if (dt.Excess > 0)
                        {
                            Excessdata.DealerId = obj.DealerId;
                            Excessdata.DepotId = obj.DepotId;
                            Excessdata.EmployeeCode = EmployeeMaster.Depotcode;
                            Excessdata.ReasonForReturn_Id = obj.ReasonForReturn_Id;
                            Excessdata.Remarks = obj.Remarks;
                            // Excessdata.RequestHeader_Id = obj.Request_Id;
                            Excessdata.RequestParentId = Convert.ToInt32(obj.Request_Id);
                            RequestDetailArray ReqArry = new RequestDetailArray();
                            SKUClass skudetail = new SKUClass();
                            ComplaintDetail ccdetail = new ComplaintDetail();

                            skudetail.Batch_No = thadr.BatchNo;
                            skudetail.SKUCode = thadr.SKUCode;
                            skudetail.SKUName = thadr.SKUName;
                            skudetail.Unit = thadr.Unit;
                            skudetail.PackSize = thadr.PackSize;
                            ccdetail.ComplaintNumber = thadr.CCNo != null ? thadr.CCNo.ToString() : "";
                            ReqArry.selectedSKU = skudetail;
                            ReqArry.selectedComplaint = ccdetail;
                            ReqArry.InvoiceDate = dt.InvoiceDate;
                            ReqArry.InvoiceNo = dt.InvoiceNo;
                            ReqArry.InvoiceQuantity = dt.InvoiceQuantity;
                            ReqArry.PackSize = thadr.PackSize;
                            ReqArry.ProvideGST_No = thadr.ReadyToProvideGST;
                            ReqArry.ProvideGST_Yes = thadr.ReadyToProvideGST;
                            ReqArry.Remarks = thadr.Remarks;
                            ReqArry.SRVQuantity = dt.Excess;
                            ReqArry.SRVValue = thadr.SRVValue;
                            ReqArry.SubreasonId = thadr.SubReason;
                            ReqArry.Unit = thadr.Unit;
                            ReqArry.Volume = thadr.Volume;
                            //List<RequestDetailArray> LstDetail = new List<RequestDetailArray>();
                            //LstDetail.Add(ReqArry);
                            Excessdata.RequestDetail.Add(ReqArry);
                            //Function call to save the Excess Quantity request
                        }
                    }
                    if (Excessdata.RequestDetail.Count != 0)
                        saveExcessRequest(Excessdata, ref retMsg);
                    if (retMsg.Contains("Error"))
                    {
                        return retMsg;
                    }
                    else
                        ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(obj.Request_Id), obj.EmployeeCode, AssignTo, obj.Active_Role, obj.Requested_Role, obj.CurrentStatus_Id, obj.FutureStatus_Id, obj.Remarks);
                    s = "Success : " + ReqHdr.RequestTypeOption + "-" + obj.Request_Id + " Requested has been updated successfully.";

                    T6.Complete();
                    T6.Dispose();
                }
                return retMsg.Equals(string.Empty)? s : s + " and a Child Request for Excess Quantity has been created :" + retMsg ;
            }
        }

        public static string ApprovebyDepot(RequestDetailObjbyDepot obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope T5 = new TransactionScope())
                {
                    string AssignTo = "";

                    var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == obj.Request_Id).ToList();
                    var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == obj.Request_Id).FirstOrDefault();
                    var currentstatus = context.SP_GetRequestDetail(Convert.ToInt32(obj.Request_Id)).FirstOrDefault();

                    var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();
                    foreach (var dt in obj.RequestDetail)
                    {
                        var thadr = context.tblRequestDtls.Where(x => x.IsActive == true && x.Id == dt.Detail_Id).FirstOrDefault();

                        thadr.SRVInvoiceNo = dt.SRVInvoiceNo;

                        thadr.ModifiedBy = obj.EmployeeCode;
                        thadr.ModifiedDate = DateTime.Now;

                        context.Entry(thadr).State = EntityState.Modified;
                        context.SaveChanges();





                    }

                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(obj.Request_Id), obj.EmployeeCode, AssignTo, obj.Active_Role, obj.Requested_Role, obj.CurrentStatus_Id, obj.FutureStatus_Id, obj.Remarks);

                    T5.Complete();
                    T5.Dispose();

                }

                return "Success : S-" + obj.Request_Id + " Requested has been updated successfully";
            }
        }

        public static string UpdateDONo(RequestDetailObjbyDepot obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                //TransactionOptions transactionOptions = new TransactionOptions();
                //transactionOptions.Timeout = TimeSpan.MaxValue;
                using (TransactionScope T6 = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
                {
                    string AssignTo = "";
                    string s = "";
                    RequestDetailObj Excessdata = new RequestDetailObj();
                    Excessdata.RequestDetail = new List<RequestDetailArray>();

                    var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == obj.Request_Id).ToList();
                    var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == obj.Request_Id).FirstOrDefault();
                    var currentstatus = context.SP_GetRequestDetail(Convert.ToInt32(obj.Request_Id)).FirstOrDefault();
                    var EmployeeMaster = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == currentstatus.DepotName).FirstOrDefault();

                    if (obj.FutureStatus_Id == 10021)
                    {
                        if (EmployeeMaster.CommercialCode.Trim().Equals(string.Empty) || EmployeeMaster.CommercialCode.Trim().Contains("NA"))
                        { return "Error: Commercial Manager not defind, Kindly contact admin!"; }
                        else
                        {
                            AssignTo = EmployeeMaster.CommercialCode;
                        }
                    }
                   
                    var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();

                    foreach (var dt in obj.RequestDetail)
                    {
                        var thadr = context.tblRequestDtls.Where(x => x.IsActive == true && x.Id == dt.Detail_Id).FirstOrDefault();

                        thadr.DONo = dt.DONo;
                        thadr.ReasonforSAP = dt.SAPsubReasonID;
                        thadr.ModifiedBy = obj.EmployeeCode;
                        thadr.ModifiedDate = DateTime.Now;

                        context.Entry(thadr).State = EntityState.Modified;
                        context.SaveChanges();

                    }
                    if (Excessdata.RequestDetail.Count != 0)
                        saveRequest(Excessdata, ref s);
                    if (s.Contains("Error"))
                    {
                        return s;
                    }
                    else
                        ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(obj.Request_Id), obj.EmployeeCode, AssignTo, obj.Active_Role, obj.Requested_Role, obj.CurrentStatus_Id, obj.FutureStatus_Id, obj.Remarks);

                    T6.Complete();
                    T6.Dispose();
                }
                return "Success : S-" + obj.Request_Id + " Requested has been updated successfully";
            }
        }

        public static string AcknowledgeRequestbyDepot(RequestDetailObjbyDepot obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope T7 = new TransactionScope())
                {
                    string AssignTo = "";

                    var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == obj.Request_Id).ToList();
                    var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == obj.Request_Id).FirstOrDefault();
                    var currentstatus = context.SP_GetRequestDetail(Convert.ToInt32(obj.Request_Id)).FirstOrDefault();

                    if (obj.FutureStatus_Id == 10020)
                    {
                        AssignTo = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == currentstatus.DepotName).FirstOrDefault().CSO;
                    }

                    var UserDetail = context.SP_LFGDetails(approvalHeader.CreatedBy).FirstOrDefault();

                    foreach (var dt in obj.RequestDetail)
                    {
                        var thadr = context.tblRequestDtls.Where(x => x.IsActive == true && x.Id == dt.Detail_Id).FirstOrDefault();

                        //int? rcv = 0;
                        //if (dt.Excess > 0)
                        //{
                        //    rcv = dt.ReceivedQuantity - dt.Excess;
                        //}
                        thadr.SubReason = dt.SubReason;
                        thadr.ReceivedQTY = dt.ReceivedQuantity;
                        thadr.DamagedQTY = dt.Damaged;
                        thadr.ExccessQTY = dt.Excess < 0 ? 0 : dt.Excess;
                        thadr.ShortQTY = dt.Short<0?0: dt.Short;//dt.Short < 0 ? 0 : dt.Short
                        thadr.ReasonforSAP = dt.SAPsubReasonID;
                        thadr.Acknowledge = dt.Acknowledge;
                        thadr.ModifiedBy = obj.EmployeeCode;
                        thadr.ModifiedDate = DateTime.Now;

                        context.Entry(thadr).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                    ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(obj.Request_Id), obj.EmployeeCode, AssignTo, obj.Active_Role, obj.Requested_Role, obj.CurrentStatus_Id, obj.FutureStatus_Id, obj.Remarks);

                    T7.Complete();
                    T7.Dispose();

                }

                return "Success : S-" + obj.Request_Id + " Requested has been updated successfully";
            }
        }

        public static List<PendingRequestModel> getRequestforDepot(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<PendingRequestModel> dataList = null;
                var PendingRequestList = context.SP_GetPendingRequestForDepot(empCode).ToList();
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
                        FutureStatus =x.FutureStatus,// "Pending Depot",
                        RequestHeaderId = x.RequestHeaderId,
                        FutureStatus_Id = x.FutStatus_ID,
                        CurrentStatus_Id = x.CurrentStatus_Id,
                        SKUCode = x.SKUCode,
                        SKUName = x.SKUName,
                        CreatedBy_EMP_CODE = x.CreatedBy,
                        CreatedBy = context.SP_LFGDetails(x.CreatedBy).FirstOrDefault().Emp_First_name,
                        CreatedDate = x.CreatedDate,

                        TotalSRV = context.tblRequestDtls.Where(o => o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p => p.SRVValue).Value,
                        RequestTypeOption = x.SKUCode,// for access data behalf on skucode


                    }).ToList();


                    return dataList;
                }
                return dataList;

            };
        }

        public static dynamic saveRequest(RequestDetailObj obj, ref string StrReturn)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<int> List_Detail_Id = null;
                long RequestHeader_Id = 0;
                var AssignedTo = context.TblFlowMatrices.Where(x => x.RequestType == obj.ReasonForReturn_Id && x.IsActive == true).ToList();
                var FindDepot = context.SP_Find_if_ExistIn_EmployeeMsater(obj.EmployeeCode).FirstOrDefault();
                string FinalRemarks = "";
                string OptionReq = "S";
                string to = "";
                if (AssignedTo.Count != 0)
                {
                    List_Detail_Id = new List<int>();
                    using (TransactionScope T1 = new TransactionScope())
                    {
                        try
                        {
                            if (obj.RequestHeader_Id == null)
                            {
                                TblRequestHeader header = new TblRequestHeader()
                                {
                                    DealerId = obj.DealerId,
                                    DepotId = obj.DepotId,
                                    ReasonForReturn = obj.ReasonForReturn_Id,
                                    EmployeeCode = obj.EmployeeCode,
                                    IsActive = true,
                                    CreatedBy = obj.EmployeeCode,
                                    CreatedDate = DateTime.Now,
                                    ParentRequest = obj.RequestParentId == null ? null : obj.RequestParentId,
                                    RequestTypeOption = FindDepot == null ? "S" : "D",
                                };
                                OptionReq = header.RequestTypeOption;
                                context.Entry(header).State = EntityState.Added;
                                context.SaveChanges();
                                RequestHeader_Id = header.RequestHeaderId;
                            }
                            else
                            {
                                var updateHeader = context.TblRequestHeaders.Where(x => x.RequestHeaderId == obj.RequestHeader_Id && x.IsActive == true).FirstOrDefault();
                                updateHeader.DealerId = obj.DealerId;
                                updateHeader.DepotId = obj.DepotId;
                                updateHeader.ReasonForReturn = obj.ReasonForReturn_Id;
                                updateHeader.ModifiedBy = obj.EmployeeCode;
                                updateHeader.ModifiedDate = DateTime.Now;
                                updateHeader.RequestTypeOption = OptionReq;
                                context.Entry(updateHeader).State = EntityState.Modified;
                                context.SaveChanges();

                                RequestHeader_Id = updateHeader.RequestHeaderId;
                            }

                            tblRequestDtl thadr = null;
                            foreach (var dt in obj.RequestDetail)
                            {
                                long Detail_Id = 0;
                                //if (dt.Detail_Id == null)
                                //{
                                thadr = new tblRequestDtl();

                                thadr.BatchNo = dt.selectedSKU.Batch_No == "Other" ? dt.selectedSKU.BatchNoText : dt.selectedSKU.Batch_No;
                                thadr.SKUCode = dt.selectedSKU.SKUCode;
                                thadr.SKUName = dt.selectedSKU.SKUName;
                                thadr.InvoiceNumber = dt.InvoiceNo;
                                thadr.InvoiceDate = dt.InvoiceDate;
                                thadr.ReadyToProvideGST = dt.ProvideGST_Yes;
                                thadr.InvoiceQuantity = dt.InvoiceQuantity;
                                thadr.SRVQuantity = dt.SRVQuantity;
                                thadr.Unit = dt.selectedSKU.Unit;
                                thadr.PackSize = dt.selectedSKU.PackSize;
                                thadr.Volume = dt.InvoiceQuantity * dt.selectedSKU.PackSize;
                                thadr.SRVValue = dt.SRVValue;
                                thadr.CCNo = obj.ReasonForReturn_Id == 1 ? Convert.ToInt32(dt.selectedComplaint.ComplaintNumber) : 0;
                                thadr.Remarks = dt.Remarks;
                                thadr.RequestHeaderId = RequestHeader_Id;
                                thadr.SubReason = dt.SubreasonId;
                                thadr.IsActive = true;
                                thadr.CreatedBy = obj.EmployeeCode;
                                thadr.CreatedDate = DateTime.Now;
                                thadr.Shelf_Life = dt.selectedSKU.Shelf_Life;
                                thadr.MFG_Date = dt.selectedSKU.Manufacturing_Date == null ? dt.selectedSKU.MGfDate : dt.selectedSKU.Manufacturing_Date;

                                context.Entry(thadr).State = EntityState.Added;
                                context.SaveChanges();

                                Detail_Id = thadr.Id;
                                FinalRemarks = FinalRemarks!=""? FinalRemarks + dt.Remarks + ",": dt.Remarks;
                                obj.Remarks = FinalRemarks;

                                if (dt.ProvideGST_Yes == true && (dt.UploadedInvoice == null || dt.UploadedInvoice == ""))
                                {
                                    RequestDal.DisableAllAlreadySavedImages(Detail_Id, obj.EmployeeCode);

                                    List_Detail_Id.Add(Convert.ToInt32(Detail_Id));
                                }
                                else if (dt.ProvideGST_Yes == true && (dt.UploadedInvoice != null || dt.UploadedInvoice != ""))
                                {
                                    RequestDal.UpdateAllAlreadySavedImages(dt.Detail_Id, Detail_Id, obj.EmployeeCode);
                                }
                            }
                            //Creating Request Flow
                             to=CreateRequestFlow(obj, RequestHeader_Id, ref StrReturn);
                        }
                        catch (Exception e)
                        {
                            StrReturn = "Error : " + StrReturn.ToString();
                            return null;
                        }

                        StrReturn = "Success : " + OptionReq + "-" + RequestHeader_Id + " Request created successfully.";
                        SendMail(obj,to);
                        T1.Complete();
                        T1.Dispose();
                    }
                }
                else
                {
                    StrReturn = "Error : Approval matrix is not set please contact Admin.";
                }
                return List_Detail_Id;
            }
        }
        public static void SendMail(RequestDetailObj obj,string to)
        {
          
                using (var context = new SalesReturndbEntities())
                {
                   string AllowEmail = System.Configuration.ConfigurationManager.AppSettings["AllowEmail"];
                   var from = context.sp_GetuserDetailsFromLFG(obj.EmployeeCode).FirstOrDefault();
                   string AllowToCCEmail = System.Configuration.ConfigurationManager.AppSettings["AllowToCCEmail"];
                    string SMTPEmailHost = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
                   
                    string SMTPusername = System.Configuration.ConfigurationManager.AppSettings["SmtpUserName"];
                   
                   string SMTPpass = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
                  
                    int SMTPPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smtpAddressPort"]);
                    if (AllowEmail == "true")
                    {
                        SmtpClient smtp = new SmtpClient(SMTPEmailHost);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(SMTPusername, SMTPpass);
                        smtp.Port = SMTPPort;
                        smtp.EnableSsl = true;
                        TblMailTemplate EmailTemplate;
                      
                        if (context.TblEmployeeMasters.Where(x => x.Depotcode == obj.EmployeeCode).ToList().Count>0)
                        {
                             EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 2).FirstOrDefault();
                        }
                        else
                        {
                            EmailTemplate = context.TblMailTemplates.Where(x => x.Id == 1).FirstOrDefault();
                        }
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("amit.kumar@phoenixtech.consulting");
                    var FromData = context.sp_GetuserDetailsFromLFG(obj.EmployeeCode).FirstOrDefault();
                    //message.From = new MailAddress(FromData.email_id);
                    string subject = EmailTemplate.Subject;
                        string html = GetHtml(EmailTemplate.MailBody,obj,FromData.Emp_First_name);
                      
                        message.IsBodyHtml = true;
                        message.Subject = GetSubject(subject,obj);
                       
                        message.Body =html;
                    // var getCC = GetCC(obj);
                    message.CC.Add("shubham.jain@phoenixtech.consulting");
                        var ToData= context.sp_GetuserDetailsFromLFG(to).FirstOrDefault();
                        // message.To.Add(ToData.email_id);
                        message.To.Add("amitkumar20895391@gmail.com");
                     
                        smtp.Send(message);
                    }
               }
            
           

        }


        public static MailAddressCollection GetCC(RequestDetailObj obj)
        {
            try
            {
                using (var context = new SalesReturndbEntities())
                {
                    MailAddressCollection ccEmail = new MailAddressCollection();
                    //RequestDetailObj_Render data1 = CommonDAL.GetRequestDetails(obj.RequestHeader_Id, obj.C, obj.FutureStatus_Id);
                    var RegionalHead = context.SP_LFGDetails(obj.EmployeeCode).FirstOrDefault();
                    if ((RegionalHead.Regional_Head.ToUpper().Trim().Equals("NA") || RegionalHead.Regional_Head.Trim().Equals(string.Empty)))
                    {
                        var RegionalHeadData = context.SP_LFGDetails(RegionalHead.Regional_Head).FirstOrDefault();
                        ccEmail.Add(RegionalHeadData.email_id);
                    }
                    if ((RegionalHead.segmentHead.ToUpper().Trim().Equals("NA") || RegionalHead.segmentHead.Trim().Equals(string.Empty)))
                    {
                        var SegmentHeadData = context.SP_LFGDetails(RegionalHead.segmentHead).FirstOrDefault();
                        ccEmail.Add(SegmentHeadData.email_id);
                    }
                    ccEmail.Add(RegionalHead.email_id);//Email Id of Requestor

                    var DepotMaster = context.SP_GetDepotList().Where(x => x.DepotId == obj.DepotId).FirstOrDefault();
                    var AssintoEmp = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == DepotMaster.DepotName).FirstOrDefault();
                    var ComplaintHandler = context.SP_LFGDetails(AssintoEmp.ComplaintHandler).FirstOrDefault();
                    var CompalintManager = context.SP_LFGDetails(AssintoEmp.ComplaintManager).FirstOrDefault();
                    ccEmail.Add(ComplaintHandler.email_id);
                    ccEmail.Add(CompalintManager.email_id);
                    var LogistickHead = context.SP_LFGDetails(AssintoEmp.LogisticsHead).FirstOrDefault();
                    ccEmail.Add(LogistickHead.email_id);
                    if ((RegionalHead.VPHead.ToUpper().Trim().Equals("NA") || RegionalHead.VPHead.Trim().Equals(string.Empty)))
                    {
                        var vphead = context.sp_GetuserDetailsFromLFG(RegionalHead.VPHead).FirstOrDefault();
                        ccEmail.Add(vphead.email_id);
                    }
                    if ((RegionalHead.President_Code.ToUpper().Trim().Equals("NA") || RegionalHead.President_Code.Trim().Equals(string.Empty)))
                    {
                        var President_Code = context.sp_GetuserDetailsFromLFG(RegionalHead.President_Code).FirstOrDefault();
                        ccEmail.Add(President_Code.email_id);
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
        public static string GetHtml(string html, RequestDetailObj obj,string EmpName)
        {
            using (var context=new SalesReturndbEntities())
            {
                long CustomerCode = obj.DealerId;
                var data = GetDealerList(obj.DepotId);
                string CustomerName = data.Where(x => x.DealerId == obj.DealerId).FirstOrDefault().DealerName;
                string RequestNumber = "S" + "-" + obj.RequestHeader_Id; ;
                int SKUCount = obj.RequestDetail.Count;
                html = html.Replace("&lt;CustomerCode&gt;", CustomerCode.ToString());
                html = html.Replace("&lt;CustomerName&gt;", CustomerName);
                html = html.Replace("&lt;RequestNumber&gt;", RequestNumber);
                html = html.Replace("&lt;DepotName&gt;", obj.DepotId.ToString());
                var reason = context.TblReasonMasters.Where(x => x.ReasonMaster_Id == obj.ReasonForReturn_Id).FirstOrDefault();
                html = html.Replace("&lt;Reason&gt;", reason.Reason);
                html = html.Replace("&lt;CountofSKUs&gt;", SKUCount.ToString());
                html = html.Replace("Requestor Name", EmpName);
                decimal? SalesReturnVolume = 0;
                decimal? SRVValue = 0;
                decimal? ExpiredVolume = 0;
                for (int i = 0; i < obj.RequestDetail.Count; i++)
                {
                    if (obj.RequestDetail[i].Volume != null)
                    {
                        SalesReturnVolume += obj.RequestDetail[i].Volume;
                    }
                    if (obj.RequestDetail[i].SRVValue != null)
                    {
                        SRVValue = SRVValue + obj.RequestDetail[i].SRVValue;
                    }
                    if (Convert.ToInt32(obj.RequestDetail[i].selectedSKU.Shelf_Life)<0)
                    {
                        ExpiredVolume = ExpiredVolume + obj.RequestDetail[i].Volume;

                    }
                }
                html = html.Replace("&lt;SalesReturnVolume&gt;", SalesReturnVolume.ToString());


                html = html.Replace("&lt;SalesReturnAmount&gt;", SRVValue.ToString());
                html = html.Replace("&lt;VolumeExpired&gt;", ExpiredVolume.ToString());
               
                return html;
            }
           
        }
        public static string GetSubject(string html, RequestDetailObj obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                var data = context.sp_GetuserDetailsFromLFG(obj.EmployeeCode).FirstOrDefault();
                html = html.Replace("<EmployeeName>", data.Emp_First_name);
                html = html.Replace("<RequestNo.>", "S - " + obj.RequestHeader_Id);
                return html;
            }
             
        }
        public static dynamic saveExcessRequest(RequestDetailObj obj, ref string StrReturn)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<int> List_Detail_Id = null;
                long RequestHeader_Id = 0;
                var AssignedTo = context.TblFlowMatrices.Where(x => x.RequestType == obj.ReasonForReturn_Id && x.IsActive == true).ToList();
                var FindDepot = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.Depotcode == obj.EmployeeCode).FirstOrDefault();
                string FinalRemarks = "";
                string OptionReq = "S";

                if (AssignedTo.Count != 0)
                {
                    List_Detail_Id = new List<int>();
                    using (TransactionScope T1 = new TransactionScope())
                    {
                        try
                        {
                            if (obj.RequestHeader_Id == null)
                            {
                                TblRequestHeader header = new TblRequestHeader()
                                {
                                    DealerId = obj.DealerId,
                                    DepotId = obj.DepotId,
                                    ReasonForReturn = obj.ReasonForReturn_Id,
                                    EmployeeCode = obj.EmployeeCode,
                                    IsActive = true,
                                    CreatedBy = obj.EmployeeCode,
                                    CreatedDate = DateTime.Now,
                                    ParentRequest = obj.RequestParentId == null ? null : obj.RequestParentId,
                                    RequestTypeOption = FindDepot == null ? "S" : "D",
                                };
                                OptionReq = header.RequestTypeOption;
                                context.Entry(header).State = EntityState.Added;
                                context.SaveChanges();
                                RequestHeader_Id = header.RequestHeaderId;
                            }
                            else
                            {
                                var updateHeader = context.TblRequestHeaders.Where(x => x.RequestHeaderId == obj.RequestHeader_Id && x.IsActive == true).FirstOrDefault();
                                updateHeader.DealerId = obj.DealerId;
                                updateHeader.DepotId = obj.DepotId;
                                updateHeader.ReasonForReturn = obj.ReasonForReturn_Id;
                                updateHeader.ModifiedBy = obj.EmployeeCode;
                                updateHeader.ModifiedDate = DateTime.Now;
                                updateHeader.RequestTypeOption = OptionReq;
                                context.Entry(updateHeader).State = EntityState.Modified;
                                context.SaveChanges();

                                RequestHeader_Id = updateHeader.RequestHeaderId;
                            }

                            tblRequestDtl thadr = null;
                            foreach (var dt in obj.RequestDetail)
                            {
                                long Detail_Id = 0;
                                //if (dt.Detail_Id == null)
                                //{
                                thadr = new tblRequestDtl();

                                thadr.BatchNo = dt.selectedSKU.Batch_No == null ? dt.BatchNo : dt.selectedSKU.Batch_No;
                                thadr.SKUCode = dt.selectedSKU.SKUCode;
                                thadr.SKUName = dt.selectedSKU.SKUName;
                                thadr.InvoiceNumber = dt.InvoiceNo;
                                thadr.InvoiceDate = dt.InvoiceDate;
                                thadr.ReadyToProvideGST = dt.ProvideGST_Yes;
                                thadr.InvoiceQuantity = dt.InvoiceQuantity;
                                thadr.SRVQuantity = dt.SRVQuantity;
                                thadr.Unit = dt.selectedSKU.Unit;
                                thadr.PackSize = dt.selectedSKU.PackSize;
                                thadr.Volume = dt.InvoiceQuantity * dt.selectedSKU.PackSize;
                                thadr.SRVValue = dt.SRVValue;
                                thadr.CCNo = obj.ReasonForReturn_Id == 1 ? Convert.ToInt32(dt.selectedComplaint.ComplaintNumber) : 0;
                                thadr.Remarks = dt.Remarks;
                                thadr.RequestHeaderId = RequestHeader_Id;
                                thadr.SubReason = dt.SubreasonId;
                                thadr.IsActive = true;
                                thadr.CreatedBy = obj.EmployeeCode;
                                thadr.CreatedDate = DateTime.Now;

                                context.Entry(thadr).State = EntityState.Added;
                                context.SaveChanges();

                                Detail_Id = thadr.Id;
                                FinalRemarks = FinalRemarks + dt.Remarks + ",";
                                obj.Remarks = FinalRemarks;

                                if (dt.ProvideGST_Yes == true && (dt.UploadedInvoice == null || dt.UploadedInvoice == ""))
                                {
                                    RequestDal.DisableAllAlreadySavedImages(Detail_Id, obj.EmployeeCode);

                                    List_Detail_Id.Add(Convert.ToInt32(Detail_Id));
                                }
                                else if (dt.ProvideGST_Yes == true && (dt.UploadedInvoice != null || dt.UploadedInvoice != ""))
                                {
                                    RequestDal.UpdateAllAlreadySavedImages(dt.Detail_Id, Detail_Id, obj.EmployeeCode);
                                }
                            }
                            //Creating Request Flow
                            CreateRequestFlow(obj, RequestHeader_Id, ref StrReturn);
                        }
                        catch (Exception e)
                        {
                            StrReturn = "Error : " + e.Message.ToString();
                            return null;
                        }

                        StrReturn = OptionReq + "-" + RequestHeader_Id ;

                        T1.Complete();
                        T1.Dispose();
                    }
                }
                else
                {
                    StrReturn = "Error : Approval matrix is not set please contact Admin.";
                }
                return List_Detail_Id;
            }
        }

        public static List<DepotModal> GetDepotList()
        {
            using (var context = new SalesReturndbEntities())
            {
                var objList = context.SP_GetDepotList().ToList();

                List<DepotModal> depot = null;

                if (objList != null)
                {
                    depot = objList.Select(x => new DepotModal()
                    {
                        DepotAddress = x.DepotAddress,
                        DepotCode = x.DepotCode,
                        DepotId = x.DepotId,
                        DepotName = x.DepotName

                    }).ToList();

                    return depot;
                }
                return depot;
            }
        }

        public static List<DepotModal> GetDepotList(string EmpCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                var objList = context.SP_GetDepotBasedOnEmpCode(EmpCode).ToList();

                List<DepotModal> depot = null;


                if (objList.Count >0)
                {
                    depot = objList.Select(x => new DepotModal()
                    {
                        DepotAddress = x.DepotAddress,
                        DepotCode = x.DepotCode,
                        DepotId = x.DepotId,
                        DepotName = x.DepotName

                    }).ToList();

                    return depot;
                }
                else
                {
                    return GetDepotList();
                }
                
            }
        }

        public static List<DealerModal> GetDealerList(long Depot_Id)
        {
            try
            {

           
            using (var context = new SalesReturndbEntities())
            {

                var objList = context.SP_GetDealerList(Depot_Id).ToList();
                List<DealerModal> dealer = null;

                if (objList != null)
                {
                    dealer = objList.Select(x => new DealerModal()
                    {
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerId = x.DealerId,
                        DealerName = x.DealerName

                    }).ToList();

                    return dealer;
                }
                return dealer;
            }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<SKUClass> GetSKUCode()
        {
            using (var context = new SalesReturndbEntities())
            {

                var objList = context.SP_GetSKUCode(null).ToList();
                List<SKUClass> dealer = null;

                if (objList != null)
                {
                    dealer = objList.Select(x => new SKUClass()
                    {

                        Product_ID = x.Product_ID,
                        SKUCode = x.SKUCode,
                        SKUName = x.SKUName,
                        SKUDescription = x.SKUDescription,
                        PackSize = x.Pack_Size == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(x.Pack_Size),
                        Unit = x.Unit,
                        Batch_No = null,
                        Manufacturing_Date = x.Manufacturing_Date,
                        Shelf_Life = x.Shelf_Life

                    }).ToList();

                    return dealer;
                }
                return dealer;
            }

        }

        public static List<CustomerComplaintModal> GetCustomerComplaintList()
        {
            using (var context = new SalesReturndbEntities())
            {

                var objList = context.SP_GetCCNumber(null).ToList();
                List<CustomerComplaintModal> dealer = null;

                if (objList != null)
                {
                    dealer = objList.Select(x => new CustomerComplaintModal()
                    {

                        Complaint_ID = x.Complaint_ID,
                        ComplaintDesc = x.ComplaintDesc,
                        ComplaintNumber = x.ComplaintNumber,
                        ComplaintQty = x.ComplaintQty
                    }).ToList();

                    return dealer;
                }
                return dealer;
            }

        }

        public static void UploadInvoicesIntoTable(long Detail_Id, string FilePath, string EmpCode)
        {
            using (var context = new SalesReturndbEntities())
            {

                TblUploadedInvoice invoice = new TblUploadedInvoice()
                {
                    ImageUploaded = FilePath,
                    RequestDetail_Id = Detail_Id,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = EmpCode


                };

                context.Entry(invoice).State = EntityState.Added;
                context.SaveChanges();

            }
        }

        public static string CreateRequestFlow(RequestDetailObj obj, long RequestHeaderId, ref string StrReturn)
        {
            using (var context = new SalesReturndbEntities())
            {
                var UserDetail = context.SP_LFGDetails(obj.EmployeeCode).FirstOrDefault();
                var FindDepot = context.SP_Find_if_ExistIn_EmployeeMsater(obj.EmployeeCode).FirstOrDefault();
                var ParentRequestDtl = context.TblRequestHeaders.Where(x => x.RequestHeaderId == obj.RequestParentId).FirstOrDefault();
                bool isNew = true;
                bool throwException = false;
                decimal? TotalSRV_Value = 0;

                DateTime date = DateTime.Now;
                double InvoiceAge = 0;

                var AssintoPeriod = context.TblFlowMatrices.Where(x => x.IsActive == true && x.RequestType == obj.ReasonForReturn_Id && x.Options == "Period").FirstOrDefault();

                var AssintoValue = context.TblFlowMatrices.Where(x => x.IsActive == true && x.RequestType == obj.ReasonForReturn_Id && x.Options == "Value").FirstOrDefault();

              

                foreach (var pt in obj.RequestDetail)
                {
                    TotalSRV_Value = TotalSRV_Value + pt.SRVValue;
                    double RequestAge = DateTime.Now.Subtract(pt.InvoiceDate.Value).Days / (365.25 / 12);
                    
                    if (InvoiceAge <= RequestAge)
                    {
                        InvoiceAge = RequestAge;
                    }
                }
                if (obj.ReasonForReturn_Id == 3 && UserDetail.Country == null)
                {
                    StrReturn = "Country is not defined!";
                    throwException = true;
                }
                else if (obj.ReasonForReturn_Id == 3 && FindDepot == null)
                {
                    if (UserDetail.Regional_Head == null)
                    {
                        StrReturn = "Approval Matrix is not defined";
                        throwException = true;
                    }
                    else if ((UserDetail.Regional_Head.ToUpper().Trim().Equals("NA") || UserDetail.Regional_Head.Trim().Equals(string.Empty)))
                    {
                        StrReturn = "Approval Matrix is not defined";
                        throwException = true;
                    }
                }

                if (throwException == true)
                {
                    throw new Exception();
                }

                var DepotMaster = context.SP_GetDepotList().Where(x => x.DepotId == obj.DepotId).FirstOrDefault();

                var AssintoEmp = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == DepotMaster.DepotName).FirstOrDefault();

                var StatusCheck = context.TblFutureStatus.Where(x => x.Request_ID == obj.RequestHeader_Id && x.IsActive == true).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (StatusCheck != null)
                {
                    if (StatusCheck.Status == 17 || StatusCheck.Status == 10017)
                    {
                        isNew = false;
                    }
                }
                var DealerDtl = context.sp_GetDealerDtlBy_DealerRepositoryId(obj.DealerId).FirstOrDefault();
                string AssignedTo = "";
                int FurtureStatus = 0;
                // When the Reason is Quality
                if (obj.ReasonForReturn_Id == 1)
                {
                    int ActiveRole = FindDepot != null ? 10 : 1;
                    if (AssintoEmp == null || AssintoEmp.ComplaintHandler == null)
                    {
                        StrReturn = "Approval Matrix Not Defined, Kindly Contact Admin";
                        throw new Exception();
                    }
                    else
                    {
                        ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, AssintoEmp.ComplaintHandler, ActiveRole, 2, 1, 2, obj.Remarks);
                    }
                    if (obj.EmployeeCode == AssintoEmp.ComplaintHandler)
                    {
                        RecommendRequestObj recommendRequestObj = new RecommendRequestObj
                        {
                            Active_Role = 2,
                            Requested_Role = 3,
                            CurrentStatus_Id = 10,
                            FutureStatus_Id = 3,
                            EmployeeCode =obj.EmployeeCode,
                            Remarks="Auto Approve",
                            RequestType_Id=obj.ReasonForReturn_Id,
                            Request_Id=Convert.ToInt32(RequestHeaderId),
                            //Country=obj.cou,
                        };
                        PendingRequestDAL.RecommendRequest(recommendRequestObj);
                    }
                }
                // When the Reason is Logistic
                else if (obj.ReasonForReturn_Id == 2)
                {
                    if (AssintoEmp == null)
                    {   
                        StrReturn = "Approval Matrix Not Defined, Kindly Contact Admin";
                        throw new Exception();
                    }
                    else
                    {
                        if (AssintoEmp.LogisticsHead == null)
                        {
                            StrReturn = "Approval Matrix Not Defined, Kindly Contact Admin";
                            throw new Exception();
                        }
                        else
                        {
                            int ActiveRole = FindDepot != null ? 10 : 1;
                            ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, AssintoEmp.LogisticsHead, 1, 4, 1, 4, "");
                            if (obj.EmployeeCode == AssintoEmp.LogisticsHead)
                            {
                                RecommendRequestObj recommendRequestObj = new RecommendRequestObj
                                {
                                    Active_Role = 4,
                                    Requested_Role = 5,
                                    CurrentStatus_Id = 11,
                                    FutureStatus_Id = 5,
                                    EmployeeCode = obj.EmployeeCode,
                                    Remarks = "Auto Approve",
                                    RequestType_Id = obj.ReasonForReturn_Id,
                                    Request_Id = Convert.ToInt32(RequestHeaderId),
                                    //Country=obj.cou,
                                };
                                PendingRequestDAL.RecommendRequest(recommendRequestObj);
                            }
                            
                        }
                    }
                }
                // When the Reason is Commercial
                else if (obj.ReasonForReturn_Id == 3)
                {
                    if (ParentRequestDtl != null)
                    {
                        UserDetail = context.SP_LFGDetails(ParentRequestDtl.CreatedBy).FirstOrDefault();
                    }
                    int ActiveRole = FindDepot != null ? 10 : 1;
                    int RequestedRole = 0;
                    if (FindDepot != null && ParentRequestDtl == null)
                    {
                        var DepotDtl = context.sp_CheckIfDepotPerson(obj.EmployeeCode).FirstOrDefault();
                        if (obj.EmployeeCode == FindDepot.Depotcode)
                        {
                            //amit
                            //if (DealerDtl.RMCode == null)
                            if(UserDetail.Regional_Head!="NA" && UserDetail.Regional_Head != "" && UserDetail.Regional_Head != null)
                            {
                               // AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE;
                                RequestedRole = 8;
                                FurtureStatus = 7;
                                if (UserDetail.segmentHead!=null && UserDetail.segmentHead != "NA")
                                {
                                    AssignedTo = UserDetail.segmentHead;
                                }
                                else
                                {
                                    StrReturn = "Segment Head is not Present in LFG";
                                    throw new Exception();
                                }
                                
                            }
                            else
                            {
                                if (UserDetail.Regional_Head != null && UserDetail.Regional_Head != "" && UserDetail.Regional_Head != "NA")
                                {
                                    AssignedTo = UserDetail.Regional_Head;
                                    FurtureStatus = 6;
                                    RequestedRole = 7;
                                }
                                else
                                {
                                    StrReturn = "Regional Head is not Present in LFG";
                                    throw new Exception();
                                }
                                //AssignedTo = DealerDtl.RMCode;
                                
                             
                            }
                            //amit
                            
                            
                        }
                      //  else if (obj.EmployeeCode == DealerDtl.RMCode)
                       else if (obj.EmployeeCode == UserDetail.Regional_Head)
                        {
                           // AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE;
                            RequestedRole = 8;
                            FurtureStatus = 7;

                            if (UserDetail.segmentHead != null && UserDetail.segmentHead != "NA" && UserDetail.segmentHead !=null)
                            {
                                AssignedTo = UserDetail.segmentHead;
                            }
                            else
                            {
                                StrReturn = "Segment Head is not Present in LFG";
                                throw new Exception();
                            }
                           
                        }
                        //else if (obj.EmployeeCode == context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE)
                        else if(obj.EmployeeCode== UserDetail.segmentHead)
                        {
                           // AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().VPHead;
                            RequestedRole = 5;
                            FurtureStatus = 5;
                            if (UserDetail.VPHead != null && UserDetail.VPHead != "NA" && UserDetail.VPHead != null)
                            {
                                AssignedTo = UserDetail.VPHead;
                            }
                            else
                            {
                                StrReturn = "VP head is not Present in LFG";
                                throw new Exception();
                            }
                               
                        }
                        else {
                            //AssignedTo = DealerDtl.RMCode;
                            if (UserDetail.Regional_Head != null && UserDetail.Regional_Head != "" && UserDetail.Regional_Head != "NA")
                            {
                                AssignedTo = UserDetail.Regional_Head;
                                FurtureStatus = 6;
                                RequestedRole = 7;
                            }
                            else
                            {
                                StrReturn = "Regional Head is not Present in LFG";
                                throw new Exception();
                            }
                            FurtureStatus = 6;
                            RequestedRole = 7;
                        }
                        ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, AssignedTo, ActiveRole, RequestedRole, 1, FurtureStatus, "");

                    }
                    else if (UserDetail == null || UserDetail.Regional_Head == null)
                    {
                        StrReturn = "Approval Matrix Not Defined, Kindly Contact Admin";
                        throw new Exception();
                    }
                    else
                    {
                        AssignedTo = UserDetail.Regional_Head;
                        RequestedRole = 7;
                        FurtureStatus = 6;

                        ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, AssignedTo, ActiveRole, RequestedRole, 1, FurtureStatus, "");
                        if (obj.EmployeeCode == AssignedTo)
                        {
                            RecommendRequestObj Obj = new RecommendRequestObj
                            {
                                Active_Role = 7,
                                Country = "India",
                                CurrentStatus_Id = 14,
                                EmployeeCode = obj.EmployeeCode,
                                FutureStatus_Id = 0,
                                Remarks = "Auto Approval",
                                Requested_Role = 0,
                                RequestType_Id = obj.ReasonForReturn_Id,
                                Request_Id = Convert.ToInt32(RequestHeaderId)
                            };
                            PendingRequestDAL.RecommendRequest(Obj);
                        }
                        //if (obj.EmployeeCode == UserDetail.Regional_Head)
                        //{
                        //    if (DealerDtl.RMCode == null)
                        //    {
                        //        AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE;
                        //        RequestedRole = 8;
                        //        FurtureStatus = 7;
                        //    }
                        //    else
                        //    {
                        //        AssignedTo = DealerDtl.RMCode;
                        //        FurtureStatus = 6;
                        //        RequestedRole = 7;
                        //    }
                        //}
                        //if (obj.EmployeeCode == DealerDtl.RMCode)
                        //{
                        //    AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().EMP_CODE;
                        //    RequestedRole = 8;
                        //    FurtureStatus = 7;
                        //}
                        //if (obj.EmployeeCode == context.SP_LFGDetails(DealerDtl.RMCode).FirstOrDefault().EMP_CODE)
                        //{
                        //    AssignedTo = context.SP_LFGDetailsBasedOnName(DealerDtl.Segment).FirstOrDefault().VPHead;
                        //    RequestedRole = 5;
                        //    FurtureStatus = 5;
                        //}

                        //ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, AssignedTo, ActiveRole, RequestedRole, 1, FurtureStatus, "");

                        //if (obj.EmployeeCode== DealerDtl.RMCode)
                        //{
                        //    RecommendRequestObj Obj = new RecommendRequestObj {
                        //        Active_Role=7,
                        //        Country="India",
                        //        CurrentStatus_Id=14,
                        //        EmployeeCode= obj.EmployeeCode,
                        //        FutureStatus_Id=0,
                        //        Remarks="Auto Approval",
                        //        Requested_Role=0,
                        //        RequestType_Id=obj.ReasonForReturn_Id,
                        //        Request_Id= Convert.ToInt32(RequestHeaderId)
                        //    };
                        //    PendingRequestDAL.RecommendRequest(Obj);
                        //}
                    }
                    //if (TotalSRV_Value <= AssintoValue.RH && InvoiceAge <= AssintoPeriod.RH)
                    //{
                    //     ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.Regional_Head, 1, 7, 1, 6, "");
                    //}
                    //else if (TotalSRV_Value <= AssintoValue.SegmentHead && InvoiceAge <= AssintoPeriod.SegmentHead)
                    //{
                    //    ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.segmentHead, 1, 8, 1, 7, "");
                    //}
                    //else if (TotalSRV_Value <= AssintoValue.VP && InvoiceAge <= AssintoPeriod.VP)
                    //{
                    //    ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.VPHead, 1, 9, 1, 8, "");
                    //}
                    //else
                    //{
                    //    ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.President_Code, 1, 6, 1, 9, "");
                    //}
                }
                return AssignedTo;
            }
        }

        public static void DisableAllAlreadySavedImages(long? Detail_Id, string CreatedBy)
        {
            using (var context = new SalesReturndbEntities())
            {
                var image = context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == Detail_Id && x.IsActive == true).FirstOrDefault();

                if (image != null)
                {
                    image.IsActive = false;
                    image.ModifiedBy = CreatedBy;
                    image.ModifiedDate = DateTime.Now;

                    context.Entry(image).State = EntityState.Modified;
                    context.SaveChanges();
                }


                // }
            }


        }

        public static void UpdateAllAlreadySavedImages(long? Detail_Id_Old, long? Detail_Id_new, string CreatedBy)
        {
            using (var context = new SalesReturndbEntities())
            {
                var image = context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == Detail_Id_Old && x.IsActive == true).FirstOrDefault();

                if (image != null)
                {
                    image.RequestDetail_Id = Detail_Id_new;
                    image.ModifiedBy = CreatedBy;
                    image.ModifiedDate = DateTime.Now;

                    context.Entry(image).State = EntityState.Modified;
                    context.SaveChanges();
                }


                // }
            }


        }

        public static bool DisableAllAlreadySavedRequestDetail(RequestDetailObj obj, long? RequestHeader_Id, string CreatedBy, ref string StrReturn)
        {
            using (var context = new SalesReturndbEntities())
            {

                //var UserDetail = context.SP_LFGDetails(obj.EmployeeCode).FirstOrDefault();



                //if (obj.ReasonForReturn_Id == 3 && UserDetail.Country == null)
                //{
                //    StrReturn = "country is not defined for this user in PMS";
                //    return false;
                //}
                //else if (obj.ReasonForReturn_Id == 3 && UserDetail.Regional_Head.ToUpper().Trim().Equals("NA"))
                //{

                //    StrReturn = "Region Head is not defined for this user in PMS";
                //    return false;
                //}
                //var AssignedTo = context.TblApprovalMatrices.Where(x => x.Country.Equals(UserDetail.Country) && x.RequestType == obj.ReasonForReturn_Id && x.BUType.Equals(UserDetail.SBU_Name) && x.Division.Equals(UserDetail.Dept_name) && x.IsActive == true).FirstOrDefault();

                //// When the Reason is Quality
                //if (obj.ReasonForReturn_Id == 1)
                //{

                //    if (AssignedTo == null || AssignedTo.ComplaintHandler == null)
                //    {

                //        StrReturn = "Complaint Handler is not defined for this user in Approval Matrix";
                //        return false;

                //    }
                //}
                //// When the Reason is Logistic
                //else if (obj.ReasonForReturn_Id == 2)
                //{
                //    if (AssignedTo == null || AssignedTo.LogisticsManager == null)
                //    {
                //        StrReturn = "Logistics Manager is not defined for this user in Approval Matrix";
                //        return false;
                //    }
                //}
                //// When the Reason is Commercial
                //else if (obj.ReasonForReturn_Id == 3)
                //{
                //    if (AssignedTo == null || AssignedTo.LogisticsManager == null)
                //    {

                //        StrReturn = "Regional Head is not defined for this user in Approval Matrix";
                //        return false;

                //    }

                //}


                var detailList = context.tblRequestDtls.Where(x => x.RequestHeaderId == RequestHeader_Id && x.IsActive == true).ToList();
                foreach (var pt in detailList)
                {
                    pt.IsActive = false;
                    pt.ModifiedBy = pt.CreatedBy;
                    pt.ModifiedDate = DateTime.Now;

                    context.Entry(pt).State = EntityState.Modified;
                    context.SaveChanges();
                }



                return true;
            }
        }

        public static bool DisableAllAlreadySavedRequestDetail_SavedAsDraft(RequestDetailObj obj, long? RequestHeader_Id, string CreatedBy, ref string StrReturn)
        {
            using (var context = new SalesReturndbEntities())
            {
                var UserDetail = context.SP_LFGDetails(obj.EmployeeCode).FirstOrDefault();

                var detailList = context.tblRequestDtls.Where(x => x.RequestHeaderId == RequestHeader_Id && x.IsActive == true).ToList();
                foreach (var pt in detailList)
                {
                    pt.IsActive = false;
                    pt.ModifiedBy = pt.CreatedBy;
                    pt.ModifiedDate = DateTime.Now;

                    context.Entry(pt).State = EntityState.Modified;
                    context.SaveChanges();
                }

                return true;
            }
        }

        public static List<PendingRequestModel> GetSavedAsDraftRequestList(string EmpCode)
        {
            List<PendingRequestModel> dataList = null; 
            using (var context = new SalesReturndbEntities())
            {
                var Requests = context.SP_GetSavedAsDraftRequestIds(EmpCode).ToList();

                if (Requests.Count!=0)
                {
                    dataList = Requests.Select(x => new PendingRequestModel()
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
                        TotalSRV = context.tblRequestDtls.Where(o => o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p => p.SRVValue),
                        RequestTypeOption = x.SKUCode==null?"T": x.SKUCode,// for access data behalf on skucode
                        
                    }).ToList();

                    return dataList;
                }
                return null;
            }
        }

        public static string ReconsiderByCommercial(RecommendRequestObj data)
        {
            using (var context = new SalesReturndbEntities())
            {
                var RequestDetail = context.tblRequestDtls.Where(x => x.RequestHeaderId == data.Request_Id).ToList();
                var approvalHeader = context.TblApproverHeaders.Where(x => x.Request_Id == data.Request_Id).FirstOrDefault();
                var ReqHdr = context.TblRequestHeaders.Where(x=>x.RequestHeaderId==data.Request_Id).FirstOrDefault();
                var currentstatus = context.SP_GetRequestDetail(Convert.ToInt32(data.Request_Id)).FirstOrDefault();
                var EmployeeMaster = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == currentstatus.DepotName).FirstOrDefault();

                ApproverDAL.UpdateRequestStatus(false, Convert.ToInt32(data.Request_Id), data.EmployeeCode, EmployeeMaster.Depotcode, data.Active_Role, data.Requested_Role, data.CurrentStatus_Id, data.FutureStatus_Id, data.Remarks);

                return "Success : "+ ReqHdr.RequestTypeOption+"-" + data.Request_Id + " Requested has been reconsidered successfully.";
            }
        }
    }
}
