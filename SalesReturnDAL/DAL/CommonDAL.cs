using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnDAL.DAL
{
    public class CommonDAL
    {
        public static List<CountryModal> GetCountry()
        {
            using (var context = new SalesReturndbEntities())
            {
                var list = context.SP_GetCountry().ToList();
                List<CountryModal> cityList = null;
                if (list != null)
                {
                    cityList = list.Select(x => new CountryModal()
                    {
                        CountryName = x.Country
                    }).ToList();

                    return cityList;
                }
                return cityList;
            }
        }


        public static bool CheckIfEmployeeExist(string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                var check = context.SP_LFGDetails(EmployeeCode).FirstOrDefault();
                if (check != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        public static EmployeeDetailsInfo GetEmployeeRequestDetails(long RequestId)
        {
            using (var context = new SalesReturndbEntities())
            {
                var item = context.SP_GetEmployeeDetailsForRequest(RequestId).FirstOrDefault();
                EmployeeDetailsInfo list = new EmployeeDetailsInfo();

                list.EMP_CODE = item.EMP_CODE;
                list.Emp_First_name = item.Emp_First_name;
                list.Added_Date = Convert.ToDateTime(item.CreatedDate);
                list.SBU_Name = item.Seg_Name;
                list.Dept_name = item.Dept_name;

                return list;
            }
        }

        public static List<spGetMasterReportData_Result> GetMasterReport(DateTime datefrom, DateTime dateTo)
        {
            using (var context = new SalesReturndbEntities())
            {
                var dataobj = context.spGetMasterReportData(datefrom, dateTo).ToList();
                return dataobj;
            }
        }

        public static RequestDetailObj_Render GetRequestDetailsforNextStage(int RequestId, int CurrentStatus_Id, int FutureStatus_Id)
        {
            using (var context = new SalesReturndbEntities())
            {
                RequestDetailObj_Render obj = new RequestDetailObj_Render();
                var ReqHeaderdetail = context.SP_GetRequestDetail(RequestId).FirstOrDefault();
                var UserDetail = context.SP_LFGDetails(ReqHeaderdetail.CreatedBy).FirstOrDefault();
                var AppMatrixValue = context.TblFlowMatrices.Where(x => x.RequestType == (ReqHeaderdetail.RequestType_Id) && x.IsActive == true && x.Options == "Value").FirstOrDefault();
                var AppMatrixPeriod = context.TblFlowMatrices.Where(x => x.RequestType == (ReqHeaderdetail.RequestType_Id) && x.IsActive == true && x.Options == "Period").FirstOrDefault();

                if (ReqHeaderdetail != null)
                {
                    var ReqHDR = context.TblRequestHeaders.Where(x => x.RequestHeaderId == RequestId && x.IsActive == true).FirstOrDefault();

                    obj.RequestHeader_Id = ReqHeaderdetail.RequestHeaderId;
                    obj.DealerName = ReqHeaderdetail.DealerName + ":" + ReqHeaderdetail.DealerCode;
                    obj.DealerId = ReqHeaderdetail.DealerId;

                    obj.DepotName =  ReqHeaderdetail.DepotName + " : " + ReqHeaderdetail.DealerCode;
                    obj.DepotId = ReqHeaderdetail.DepotId;

                    obj.ReasonForReturn = ReqHeaderdetail.RequestType;
                    obj.ReasonForReturn_Id = ReqHeaderdetail.RequestType_Id;
                    obj.RequestTypeOption = ReqHDR.RequestTypeOption;
                    obj.RequestDetail = new List<RequestDetailArray_Render>();

                    var ReqDetailList = context.tblRequestDtls.Where(x => x.RequestHeaderId == RequestId && x.IsActive == true).ToList();

                    int InvoiceAge = 0;
                    decimal? TotalSRV_Value = 0;
                    DateTime date = DateTime.Now;

                    foreach (var pt in ReqDetailList)
                    {
                        if (pt.ReleaseByCM == false) { continue; }
                        RequestDetailArray_Render detailArray = new RequestDetailArray_Render()
                        {
                            Detail_Id = pt.Id,
                            InvoiceDate = pt.InvoiceDate,
                            InvoiceNo = pt.InvoiceNumber,
                            InvoiceQuantity = pt.InvoiceQuantity,
                            PackSize = pt.PackSize,
                            ProvideGST_Yes = pt.ReadyToProvideGST,
                            ProvideGST_No = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? false : true,
                            Remarks = pt.Remarks,
                            SRVQuantity = pt.SRVQuantity,
                            SRVValue = pt.SRVValue,
                            Unit = pt.Unit,
                            Volume = (pt.SRVQuantity * pt.PackSize),
                            SKUCode = pt.SKUCode,
                            SKUName = pt.SKUName,
                            Acknowledge = pt.Acknowledge,
                            Damaged = pt.DamagedQTY,
                            Excess = pt.ExccessQTY,
                            ReceivedQuantity = pt.ReceivedQTY,
                            SAPsubReasonID = pt.ReasonforSAP,
                            Short = pt.ShortQTY,
                            Manufacturing_Date = context.TblBarCodeDetails.Where(x => x.SKU_Code == pt.SKUCode).Select(y => y.Manufacturing_Date).FirstOrDefault(),
                            Shelf_Life = context.spGetShelfLifeData(pt.SKUCode).FirstOrDefault().Shelf_Life.ToString(),
                            BatchNo = pt.BatchNo,
                            ComplaintNumber = obj.ReasonForReturn_Id == 1 ? context.SP_GetCCNumber(pt.CCNo).FirstOrDefault().ComplaintNumber : 0,
                            SubReasonName = context.TblSalesReasonMasters.Where(x => x.SalesReason_Id == pt.SubReason).Select(y => y.SubReason).FirstOrDefault(),
                            SubReason = pt.SubReason,
                            UploadedInvoice = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == pt.Id && x.IsActive == true).FirstOrDefault().ImageUploaded : "",
                            UploadedInvoice_Id = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == pt.Id && x.IsActive == true).FirstOrDefault().Id : 0,
                            DONo = pt.DONo,
                            SRVInvoiceNo = pt.SRVInvoiceNo,
                            ReleaseByCM=pt.ReleaseByCM,
                            ReleaseByCM_Date=pt.ReleaseByCM_Date
                        };

                        obj.RequestDetail.Add(detailArray);

                        TotalSRV_Value = TotalSRV_Value + detailArray.SRVValue;

                        // getting invoice age from the oldest invoice date.
                        if (pt.InvoiceDate != null)
                        {
                            int InvoiceDateAge = ((date.Year - pt.InvoiceDate.Value.Year) * 12) + date.Month - pt.InvoiceDate.Value.Month;

                            if (InvoiceAge <= InvoiceDateAge)
                            {
                                InvoiceAge = InvoiceDateAge;
                            }
                        }
                    }

                    // if request is pending to user.
                    if (FutureStatus_Id == 17)
                    {
                        foreach (var item in obj.RequestDetail)
                        {
                            if (ReqHeaderdetail.RequestType_Id == 1)
                            {
                                var ccDetail = context.SP_GetCCNumber(item.ComplaintNumber).FirstOrDefault();

                                item.selectedComplaint = new ComplaintDetail_Render();
                                item.selectedComplaint.Complaint_ID = ccDetail.Complaint_ID;
                                item.selectedComplaint.ComplaintDesc = ccDetail.ComplaintDesc;
                                item.selectedComplaint.ComplaintNumber = ccDetail.ComplaintNumber;
                            }
                            var SKUDetail = context.SP_GetSKUCode(item.SKUCode).FirstOrDefault();

                            if (SKUDetail != null)
                            {
                                //item.selectedSKU = new SKUClass();
                                item.selectedSKU = new SKUClass();
                                item.selectedSKU.SKUCode = SKUDetail.SKUCode;
                                item.selectedSKU.SKUName = SKUDetail.SKUName;
                                item.selectedSKU.SKUDescription = SKUDetail.SKUDescription;
                            }
                        }


                    }

                    if (CurrentStatus_Id == 10 || CurrentStatus_Id == 11)
                    {

                        //if (TotalSRV_Value < AppMatrixValue.ComplaintHandler && InvoiceAge < AppMatrix.InvoiceAge)
                        if (TotalSRV_Value < AppMatrixValue.ComplaintHandler)
                        {
                            obj.ShowApproveButton = false;
                        }
                        else
                        {
                            obj.ShowApproveButton = true;
                        }

                    }
                    else if (CurrentStatus_Id == 12 || CurrentStatus_Id == 13 || CurrentStatus_Id == 14 || FutureStatus_Id == 6 || FutureStatus_Id == 7 || FutureStatus_Id == 8)
                    {
                        obj.ShowApproveButton = true;
                    }
                    return obj;
                }
                return obj;
            }
        }

        public static IList<StatusdetailInfo> GetRequestStatusDetails(long RequestId)
        {
            using (var context = new SalesReturndbEntities())
            {
                var item = context.SP_GetRequestStatusDetails(RequestId).ToList();
                IList<StatusdetailInfo> list = null;
                list = item.Select(x => new StatusdetailInfo()
                {
                    RequestId = x.RequestHeaderId,
                    Department = x.Department,
                    Status = x.Status,
                    Remark = x.Remark,
                    Added_By = x.CreatedBy,
                    Added_Date = x.CreatedDate,
                    EMP_CODE = x.EMP_CODE,
                    Emp_First_name = x.Emp_First_name

                }).ToList();




                return list;
            }
        }

        public static IList<FutureStatus> GetFutureStatus(long RequestId)
        {
            using (var context = new SalesReturndbEntities())
            {
                var fut = context.SP_GetFutureStatusDetails(RequestId).ToList();

                IList<FutureStatus> list = null;

                list = fut.Select(s => new FutureStatus()
                {
                    Status = s.Status,
                    EmployeeCode = s.EMP_CODE
                }).ToList();

                //var reqdata = context.TblRequestHeaders.Where(x => x.RequestHeaderId == RequestId).FirstOrDefault();

                //var CheckIF = context.TblApproverHeaders.Where(x => x.Request_Id == RequestId).FirstOrDefault();

                //var checkReqStatus = context.TblFutureStatus.Where(x => x.Request_ID == RequestId).FirstOrDefault();

                return list;
            }

        }
        public static IList<StatusdetailInfo> GetCurrentStatus(long RequestId)
        {
            using (var context = new SalesReturndbEntities())
            {
                var fut = context.SP_GetCurrentStatusDetails(RequestId).ToList();

                IList<StatusdetailInfo> list = null;

                list = fut.Select(x => new StatusdetailInfo()
                {
                    RequestId = Convert.ToInt32(x.RequestHeaderId),
                    Department = x.Department,
                    Status = x.Status,
                    Remark = x.Remark,
                    Added_By = x.CreatedBy,
                    Added_Date = x.CreatedDate,
                    EMP_CODE = x.EMP_CODE,
                    Emp_First_name = x.Emp_First_name

                }).ToList();

                return list;
            }

        }

        public static RequestDetailObj_Render GetRequestDetails(int? RequestId, int CurrentStatus_Id, int FutureStatus_Id)
        {
            using (var context = new SalesReturndbEntities())
            {
                RequestDetailObj_Render obj = new RequestDetailObj_Render();
                var ReqHeaderdetail = context.SP_GetRequestDetail(RequestId).FirstOrDefault();
                var UserDetail = context.SP_LFGDetails(ReqHeaderdetail.CreatedBy).FirstOrDefault();
                var AppMatrixValue = context.TblFlowMatrices.Where(x => x.RequestType == (ReqHeaderdetail.RequestType_Id) && x.IsActive == true && x.Options == "Value").FirstOrDefault();
                var AppMatrixPeriod = context.TblFlowMatrices.Where(x => x.RequestType == (ReqHeaderdetail.RequestType_Id) && x.IsActive == true && x.Options == "Period").FirstOrDefault();

                if (ReqHeaderdetail != null)
                {
                    var ReqHDR = context.TblRequestHeaders.Where(x => x.RequestHeaderId == RequestId && x.IsActive == true).FirstOrDefault();

                    obj.RequestHeader_Id = ReqHeaderdetail.RequestHeaderId;
                    obj.DealerName = ReqHeaderdetail.DealerName + " : " + ReqHeaderdetail.DealerCode;
                    obj.DealerId = ReqHeaderdetail.DealerId;
                    obj.DepotName = ReqHeaderdetail.DepotName + " : " + ReqHeaderdetail.DepotCode;
                    obj.DepotId = ReqHeaderdetail.DepotId;
                    obj.ReasonForReturn = ReqHeaderdetail.RequestType;
                    obj.ReasonForReturn_Id = ReqHeaderdetail.RequestType_Id;
                    obj.RequestTypeOption = ReqHDR.RequestTypeOption;
                    obj.DocketNumber = ReqHDR.DocketNumber;
                    obj.IsCommercialSettlement = ReqHDR.IsCommercialSettlement;
                    obj.MaterialWillGoToDealer = ReqHDR.MaterialWillGoToDealer;
                    obj.ParentRequest = ReqHDR.ParentRequest;
                    obj.ReasonForCommercialSettlement = ReqHDR.ReasonForCommercialSettlement;
                    obj.DetailsForMaterialGoToDealer = ReqHDR.DetailsForMaterialGoToDealer;
                    obj.EPNo = ReqHDR.EPNo;
                    obj.DealerNameForMail = ReqHeaderdetail.DealerName;
                    obj.DealerCodeForMail= ReqHeaderdetail.DealerCode;
                    obj.RequestDetail = new List<RequestDetailArray_Render>();
                    obj.DepotNameForMail = ReqHeaderdetail.DepotName;
                    var ReqDetailList = context.tblRequestDtls.Where(x => x.RequestHeaderId == RequestId && x.IsActive == true).ToList();

                    double InvoiceAge = 0;
                    decimal? TotalSRV_Value = 0;
                    DateTime date = DateTime.Now;

                    foreach (var pt in ReqDetailList)
                    {
                        var BarCodeDetails = context.TblBarCodeDetails.Where(x => x.SKU_Code == pt.SKUCode).FirstOrDefault();
                        var GetCCNumber = context.SP_GetCCNumber(pt.CCNo).FirstOrDefault();
                        var SalesReasonMasters = context.TblSalesReasonMasters.Where(x => x.SalesReason_Id == pt.SubReason).FirstOrDefault();
                        RequestDetailArray_Render detailArray = new RequestDetailArray_Render()
                        {
                            Detail_Id = pt.Id,
                            InvoiceDate = pt.InvoiceDate,
                            InvoiceNo = pt.InvoiceNumber,
                            InvoiceQuantity = pt.InvoiceQuantity,
                            PackSize = pt.PackSize,
                            ProvideGST_Yes = pt.ReadyToProvideGST,
                            ProvideGST_No = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? false : true,
                            Remarks = pt.Remarks,
                            SRVQuantity = pt.SRVQuantity,
                            SRVValue = pt.SRVValue,
                            Unit = pt.Unit,
                            Volume = (pt.SRVQuantity * pt.PackSize),
                            SKUCode = pt.SKUCode,
                            SKUName = pt.SKUName,
                            Damaged = pt.DamagedQTY,
                            DONo = pt.DONo,
                            Acknowledge = pt.Acknowledge,
                            Excess = pt.ExccessQTY,
                            ReceivedQuantity = pt.ReceivedQTY,
                            Short = pt.ShortQTY,
                            SRVInvoiceNo = pt.SRVInvoiceNo,
                            // Manufacturing_Date = BarCodeDetails != null ? BarCodeDetails.Manufacturing_Date : null,
                            //Shelf_Life = context.spGetShelfLifeData(pt.SKUCode).FirstOrDefault().Shelf_Life.ToString(),
                            Manufacturing_Date=pt.MFG_Date,
                            Shelf_Life=pt.Shelf_Life,
                            BatchNo = pt.BatchNo,
                            ComplaintNumber = obj.ReasonForReturn_Id == 1 ? GetCCNumber != null ? GetCCNumber.ComplaintNumber : 0 : 0,
                            SubReasonName = SalesReasonMasters != null ? SalesReasonMasters.SubReason : "",
                            SubReason = pt.SubReason,
                            UploadedInvoice = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == pt.Id && x.IsActive == true).FirstOrDefault().ImageUploaded : "",
                            UploadedInvoice_Id = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == pt.Id && x.IsActive == true).FirstOrDefault().Id : 0,
                            selectedSKU = new SKUClass
                            {
                                SKUCode = pt.SKUCode,
                                SKUName = pt.SKUName
                            }
                        };

                        obj.RequestDetail.Add(detailArray);

                        TotalSRV_Value = TotalSRV_Value + detailArray.SRVValue;

                        // getting invoice age from the oldest invoice date.
                        if (pt.InvoiceDate != null)
                        {
                            double InvoiceDateAge = DateTime.Now.Subtract(pt.InvoiceDate.Value).Days / (365.25 / 12);
                            //int InvoiceDateAge = ((date.Year - pt.InvoiceDate.Value.Year) * 12) + date.Month - pt.InvoiceDate.Value.Month;

                            if (InvoiceAge <= InvoiceDateAge)
                            {
                                InvoiceAge = InvoiceDateAge;
                            }
                        }
                    }

                    // if request is pending to user.
                    if (ReqHeaderdetail.RequestType_Id != 3)
                    {
                        if (FutureStatus_Id == 17)
                        {
                            foreach (var item in obj.RequestDetail)
                            {
                                if (ReqHeaderdetail.RequestType_Id == 1)
                                {
                                    var ccDetail = context.SP_GetCCNumber(item.ComplaintNumber).FirstOrDefault();

                                    item.selectedComplaint = new ComplaintDetail_Render();
                                    item.selectedComplaint.Complaint_ID = ccDetail.Complaint_ID;
                                    item.selectedComplaint.ComplaintDesc = ccDetail.ComplaintDesc;
                                    item.selectedComplaint.ComplaintNumber = ccDetail.ComplaintNumber;

                                }
                                var SKUDetail = context.SP_GetSKUCode(item.SKUCode).FirstOrDefault();

                                if (SKUDetail != null)
                                {
                                    //item.selectedSKU = new SKUClass();
                                    item.selectedSKU = new SKUClass();
                                    item.selectedSKU.SKUCode = SKUDetail.SKUCode;
                                    item.selectedSKU.SKUName = SKUDetail.SKUName;
                                    item.selectedSKU.SKUDescription = SKUDetail.SKUDescription;
                                }
                            }
                        }

                        if (CurrentStatus_Id == 10)
                        {
                            if (ReqHeaderdetail.RequestType_Id == 1)
                            {
                                if (TotalSRV_Value <= AppMatrixValue.ComplaintHandler && InvoiceAge <= AppMatrixPeriod.ComplaintHandler && FutureStatus_Id != 9)
                                {
                                    obj.ShowApproveButton = true;
                                }
                                else if (FutureStatus_Id == 9)
                                {
                                    obj.ShowApproveButton = true;
                                }
                                else
                                {
                                    obj.ShowApproveButton = false;
                                }
                            }
                        }
                        else if (CurrentStatus_Id == 11)
                        {
                            if (ReqHeaderdetail.RequestType_Id == 2)
                            {
                                if (TotalSRV_Value < AppMatrixValue.LogisticsHead && InvoiceAge < AppMatrixPeriod.LogisticsHead)
                                {
                                    obj.ShowApproveButton = true;
                                }
                                else if (FutureStatus_Id == 9)
                                {
                                    obj.ShowApproveButton = true;
                                }
                                else
                                {
                                    obj.ShowApproveButton = false;
                                }
                            }
                        }
                        else if (CurrentStatus_Id == 12 || CurrentStatus_Id == 13 || CurrentStatus_Id == 14 || FutureStatus_Id == 6 || FutureStatus_Id == 7 || FutureStatus_Id == 8)
                        {
                            obj.ShowApproveButton = true;
                        }
                    }
                    else
                    {
                        if (FutureStatus_Id == 6)
                        {
                            if (TotalSRV_Value <= AppMatrixValue.RH && InvoiceAge <= AppMatrixPeriod.RH)
                            { obj.ShowApproveButton = true; }
                            else { obj.ShowApproveButton = false; }
                        }
                        else if (FutureStatus_Id == 7)
                        {
                            if (TotalSRV_Value <= AppMatrixValue.SegmentHead && InvoiceAge <= AppMatrixPeriod.SegmentHead)
                            { obj.ShowApproveButton = true; }
                            else { obj.ShowApproveButton = false; }
                        }
                        else if (FutureStatus_Id == 8)
                        {
                            if (TotalSRV_Value <= AppMatrixValue.VP && InvoiceAge <= AppMatrixPeriod.VP)
                            { obj.ShowApproveButton = true; }
                            else { obj.ShowApproveButton = false; }
                        }
                        else if (FutureStatus_Id == 9)
                        {
                            obj.ShowApproveButton = true;
                        }
                        //if (TotalSRV_Value <= AppMatrixValue.RH && InvoiceAge <= AppMatrixPeriod.RH)
                        //{
                        //     ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.Regional_Head, 1, 7, 1, 6, "");
                        //}
                        //else if (TotalSRV_Value <= AppMatrixValue.SegmentHead && InvoiceAge <= AppMatrixPeriod.SegmentHead)
                        //{
                        //    ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.segmentHead, 1, 8, 1, 7, "");
                        //}
                        //else if (TotalSRV_Value <= AppMatrixValue.VP && InvoiceAge <= AppMatrixPeriod.VP)
                        //{
                        //    ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.VPHead, 1, 9, 1, 8, "");
                        //}
                        //else
                        //{
                        //    ApproverDAL.UpdateRequestStatus(isNew, Convert.ToInt32(RequestHeaderId), obj.EmployeeCode, UserDetail.President_Code, 1, 6, 1, 9, "");
                        //}
                    }
                    return obj;
                }
                return obj;
            }
        }

        public static RequestDetailObj_Render GetSavedRequestDetails(int? RequestId, int CurrentStatus_Id, int FutureStatus_Id)
        {
            using (var context = new SalesReturndbEntities())
            {
                RequestDetailObj_Render obj = new RequestDetailObj_Render();
                var ReqHeaderdetail = context.SP_GetRequestDetail(RequestId).FirstOrDefault();
                var UserDetail = context.SP_LFGDetails(ReqHeaderdetail.CreatedBy).FirstOrDefault();
                var AppMatrixValue = context.TblFlowMatrices.Where(x => x.RequestType == (ReqHeaderdetail.RequestType_Id) && x.IsActive == true && x.Options == "Value").FirstOrDefault();
                var AppMatrixPeriod = context.TblFlowMatrices.Where(x => x.RequestType == (ReqHeaderdetail.RequestType_Id) && x.IsActive == true && x.Options == "Period").FirstOrDefault();

                if (ReqHeaderdetail != null)
                {
                    var ReqHDR = context.TblRequestHeaders.Where(x => x.RequestHeaderId == RequestId && x.IsActive == true).FirstOrDefault();

                    obj.RequestHeader_Id = ReqHeaderdetail.RequestHeaderId;
                    obj.DealerName = ReqHeaderdetail.DealerName + " : " + ReqHeaderdetail.DealerCode;
                    obj.DealerId = ReqHeaderdetail.DealerId;
                    obj.DepotName = ReqHeaderdetail.DepotCode + " : " + ReqHeaderdetail.DepotName;
                    obj.DepotId = ReqHeaderdetail.DepotId;
                    obj.ReasonForReturn = ReqHeaderdetail.RequestType;
                    obj.ReasonForReturn_Id = ReqHeaderdetail.RequestType_Id;
                    obj.RequestTypeOption = ReqHDR.RequestTypeOption;
                    obj.RequestDetail = new List<RequestDetailArray_Render>();

                    var ReqDetailList = context.tblRequestDtls.Where(x => x.RequestHeaderId == RequestId && x.IsActive == true).ToList();

                    int InvoiceAge = 0;
                    decimal? TotalSRV_Value = 0;
                    DateTime date = DateTime.Now;

                    foreach (var pt in ReqDetailList)
                    {
                        var TblUploadedInvoicesDtl = context.TblUploadedInvoices.Where(x => x.RequestDetail_Id == pt.Id && x.IsActive == true).FirstOrDefault();
                        var TblBarCodeDetails = context.TblBarCodeDetails.Where(x => x.SKU_Code == pt.SKUCode).FirstOrDefault();
                        var spGetShelfLifeData = context.spGetShelfLifeData(pt.SKUCode).FirstOrDefault();
                        var TblSalesReasonMasters = context.TblSalesReasonMasters.Where(x => x.SalesReason_Id == pt.SubReason).FirstOrDefault();
                        var GetCCNumber = context.SP_GetCCNumber(pt.CCNo).FirstOrDefault();
                        RequestDetailArray_Render detailArray = new RequestDetailArray_Render()
                        {
                            Detail_Id = pt.Id,
                            InvoiceDate = pt.InvoiceDate,
                            InvoiceNo = pt.InvoiceNumber,
                            InvoiceQuantity = pt.InvoiceQuantity,
                            PackSize = pt.PackSize,
                            ProvideGST_Yes = pt.ReadyToProvideGST,
                            ProvideGST_No = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST.Value == true) ? false : true,
                            Remarks = pt.Remarks,
                            SRVQuantity = pt.SRVQuantity,
                            SRVValue = pt.SRVValue,
                            Unit = pt.Unit,
                            Volume = (pt.SRVQuantity * pt.PackSize),
                            SKUCode = pt.SKUCode,
                            SKUName = pt.SKUName,
                            Manufacturing_Date = TblBarCodeDetails != null ? TblBarCodeDetails.Manufacturing_Date : null,
                            Shelf_Life = spGetShelfLifeData != null ? spGetShelfLifeData.Shelf_Life.ToString() : "",
                            BatchNo = pt.BatchNo,
                            ComplaintNumber = obj.ReasonForReturn_Id == 1 ? GetCCNumber != null ? GetCCNumber.ComplaintNumber : 0 : 0,
                            SubReasonName = TblSalesReasonMasters != null ? TblSalesReasonMasters.SubReason : "",
                            SubReason = pt.SubReason,
                            UploadedInvoice = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST == true) ? TblUploadedInvoicesDtl != null ? TblUploadedInvoicesDtl.ImageUploaded : "" : "",
                            UploadedInvoice_Id = (pt.ReadyToProvideGST != null && pt.ReadyToProvideGST == true) ? TblUploadedInvoicesDtl != null ? TblUploadedInvoicesDtl.Id : 0 : 0,
                            selectedComplaint = new ComplaintDetail_Render
                            {
                                ComplaintNumber = pt.CCNo,
                            },
                            selectedSKU = new SKUClass
                            {
                                Batch_No = pt.BatchNo,
                                SKUCode = pt.SKUCode,
                                SKUName = pt.SKUName,
                                //SKUDescription=
                            }
                        };

                        obj.RequestDetail.Add(detailArray);

                        TotalSRV_Value = TotalSRV_Value + detailArray.SRVValue;

                        // getting invoice age from the oldest invoice date.
                        if (pt.InvoiceDate != null)
                        {
                            int InvoiceDateAge = ((date.Year - pt.InvoiceDate.Value.Year) * 12) + date.Month - pt.InvoiceDate.Value.Month;

                            if (InvoiceAge <= InvoiceDateAge)
                            {
                                InvoiceAge = InvoiceDateAge;
                            }
                        }
                    }
                    // if request is pending to user.
                    if (FutureStatus_Id == 17)
                    {
                        foreach (var item in obj.RequestDetail)
                        {
                            if (ReqHeaderdetail.RequestType_Id == 1)
                            {
                                var ccDetail = context.SP_GetCCNumber(item.ComplaintNumber).FirstOrDefault();

                                item.selectedComplaint = new ComplaintDetail_Render();
                                item.selectedComplaint.Complaint_ID = ccDetail.Complaint_ID;
                                item.selectedComplaint.ComplaintDesc = ccDetail.ComplaintDesc;
                                item.selectedComplaint.ComplaintNumber = ccDetail.ComplaintNumber;

                            }
                            var SKUDetail = context.SP_GetSKUCode(item.SKUCode).FirstOrDefault();

                            if (SKUDetail != null)
                            {
                                //item.selectedSKU = new SKUClass();
                                item.selectedSKU = new SKUClass();
                                item.selectedSKU.SKUCode = SKUDetail.SKUCode;
                                item.selectedSKU.SKUName = SKUDetail.SKUName;
                                item.selectedSKU.SKUDescription = SKUDetail.SKUDescription;
                            }
                        }
                    }

                    if (CurrentStatus_Id == 10 || CurrentStatus_Id == 11)
                    {

                        //if (TotalSRV_Value < AppMatrixValue.ComplaintHandler && InvoiceAge < AppMatrix.InvoiceAge)
                        if (TotalSRV_Value < AppMatrixValue.ComplaintHandler)
                        {
                            obj.ShowApproveButton = false;
                        }
                        else
                        {
                            obj.ShowApproveButton = true;
                        }
                    }
                    else if (CurrentStatus_Id == 12 || CurrentStatus_Id == 13 || CurrentStatus_Id == 14 || FutureStatus_Id == 6 || FutureStatus_Id == 7 || FutureStatus_Id == 8)
                    {
                        obj.ShowApproveButton = true;
                    }
                    return obj;
                }
                return obj;
            }

        }
    }
}
