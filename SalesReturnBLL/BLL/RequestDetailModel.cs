using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{

    public class RequestDetailObj
    {
        public long? RequestHeader_Id { get; set; }
        public int? RequestParentId { get; set; }
        public long DepotId { get; set; }
        public long DealerId { get; set; }
        public int ReasonForReturn_Id { get; set; }
        //public SKUClass selectedSKU { get; set; }
        public string EmployeeCode { get; set; }
        //public string BatchNo { get; set; }
        public string Remarks { get; set; }

        public List<RequestDetailArray> RequestDetail { get; set; }
    }

    public class RequestDetailObjbyDepot
    {
        public long? Request_Id { get; set; }
        public long DepotId { get; set; }
        public long DealerId { get; set; }
        public int ReasonForReturn_Id { get; set; }
        //public SKUClass selectedSKU { get; set; }
        public string EmployeeCode { get; set; }
        //public string BatchNo { get; set; }
        public string Remarks { get; set; }
        public int CurrentStatus_Id { get; set; }
        public int FutureStatus_Id { get; set; }
        public int Active_Role { get; set; }
        public int Requested_Role { get; set; }

        public List<RequestDetArrayforDepot> RequestDetail { get; set; }
    }

    public class RequestDetArrayforDepot
    {
        public long? Detail_Id { get; set; }
        public int? Damaged { get; set; }
        public int? Short { get; set; }
        public decimal? SRVQuantity { get; set; }
        public int? Excess { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? ReceivedQuantity { get; set; }
        public bool? Acknowledge { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        public int? SubReason { get; set; }
        public int? SAPsubReasonID { get; set; }
        public string SRVInvoiceNo { get; set; }
        public string DONo { get; set; }
        public Nullable<bool> ReleaseByCM { get; set; }
        public Nullable<System.DateTime> ReleaseByCM_Date { get; set; }
    }


    public class RequestDetailArray
    {
        public long? Detail_Id { get; set; }
        public string BatchNo { get; set; }
        public SKUClass selectedSKU { get; set; }
        public decimal? SRVQuantity { get; set; }
        public decimal? PackSize { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? ProvideGST_Yes { get; set; }
        public bool? ProvideGST_No { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        public string Unit { get; set; }
        public decimal? SRVValue { get; set; }
        public decimal? Volume { get; set; }
        public ComplaintDetail selectedComplaint { get; set; }
        public string Remarks { get; set; }

        public string UploadedInvoice { get; set; }
        public long? UploadedInvoice_Id { get; set; }

        public int? SubreasonId { get; set; }
        public int? SAPsubReasonID { get; set; }
        
    }

    public class SavedRequestDetail
    {
        public long? Detail_Id { get; set; }
        public string BatchNo { get; set; }
        public SKUClass selectedSKU { get; set; }
        public decimal? SRVQuantity { get; set; }
        public decimal? PackSize { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? ProvideGST_Yes { get; set; }
        public bool? ProvideGST_No { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        public string Unit { get; set; }
        public decimal? SRVValue { get; set; }
        public decimal? Volume { get; set; }
        public ComplaintDetail selectedComplaint { get; set; }
        public string Remarks { get; set; }

        public string UploadedInvoice { get; set; }
        public long? UploadedInvoice_Id { get; set; }

        public int? SubreasonId { get; set; }
        public int? SAPsubReasonID { get; set; }

        public List<RequestDetailArray_Render> RequestDetail { get; set; }

    }

    public class ComplaintDetail
    {

        public long? Complaint_ID { get; set; }
        public string ComplaintNumber { get; set; }
        public string ComplaintDesc { get; set; }
        public decimal? ComplaintQty { get; set; }

    }

    public class Dummy_RequestDetailObj
    {
        public long? RequestHeader_Id { get; set; }
        public int? RequestParentId { get; set; }
        public long DepotId { get; set; }
        public long DealerId { get; set; }
        public int ReasonForReturn_Id { get; set; }
        //public SKUClass selectedSKU { get; set; }
        public string EmployeeCode { get; set; }
        //public string BatchNo { get; set; }
        public string Remarks { get; set; }
        public List<BatchList> BatchList { get; set; }
        public List<RequestDetailArray> RequestDetail { get; set; }
    }
    public partial class BatchList
    {
        public Nullable<long> Product_ID { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string SKUDescription { get; set; }
        public string Pack_Size { get; set; }
        public string Unit { get; set; }
        public string Batch_No { get; set; }
        public Nullable<System.DateTime> Manufacturing_Date { get; set; }
        public string Shelf_Life { get; set; }
    }
    /** Below Object is to Get 
     * Data From database
     */

    public class RequestDetailObj_Render
    {
        public long? RequestHeader_Id { get; set; }
        public long? DepotId { get; set; }
        public long? DealerId { get; set; }
        public string Country { get; set; }
        public string EmployeeCode { get; set; }
        public string RequestTypeOption { get; set; }
        public bool ShowApproveButton { get; set; }
        public List<RequestDetailArray_Render> RequestDetail { get; set; }
        //Used when display  data in grid
        public string DealerName { get; set; }
        public string DepotName { get; set; }
        public string ReasonForReturn { get; set; }
        public int ReasonForReturn_Id { get; set; }
        //For Stage-4 purpose
        public Nullable<int> ParentRequest { get; set; }
        public Nullable<bool> IsCommercialSettlement { get; set; }
        public Nullable<bool> MaterialWillGoToDealer { get; set; }
        public string ReasonForCommercialSettlement { get; set; }
        public string DetailsForMaterialGoToDealer { get; set; }
        public string DocketNumber { get; set; }
        public DateTime? DocketDate { get; set; }
        public string EPNo { get; set; }
        public string DealerNameForMail { get; set; }
        public string DealerCodeForMail { get; set; }
        public string DepotNameForMail { get; set; }
    }

    public class MasterReportDataModel
    {
        public Nullable<long> SRV_Request_Nos_ { get; set; }
        public Nullable<int> Parent_SRV_Request_Nos_ { get; set; }
        public string SRV_Request_Date { get; set; }
        public string Request_Raised_By { get; set; }
        public string Depot_Code { get; set; }
        public string Depot_Name { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string SKU_Code { get; set; }
        public string SKU_Name { get; set; }
        public string Batch { get; set; }
        public Nullable<System.DateTime> Manufacturing_Date { get; set; }
        public string Shelf_Liife { get; set; }
        public Nullable<decimal> SRV_Quantity { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public Nullable<decimal> SRV_Value { get; set; }
        public string Ref_Invoice_Number { get; set; }
        public Nullable<System.DateTime> Ref_Invoice_Date { get; set; }
        public string Primary_SRV_Reason { get; set; }
        public string Status { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Date_of_Materials_Receipt_in_Warehouse { get; set; }
        public Nullable<int> Received_Quantity { get; set; }
        public Nullable<int> SAP_Reason_Code { get; set; }
        public Nullable<int> SAP_Reason_Desc_ { get; set; }
        public Nullable<int> OBD_Number { get; set; }
        public Nullable<int> OBD_Date { get; set; }
        public Nullable<int> OBD_Relased_Date { get; set; }
        public Nullable<System.DateTime> Req_Closure_Date { get; set; }
        public Nullable<int> SAP_Ref_SRV_Inv__ { get; set; }
        public Nullable<int> SAP_Ref_SRV_Inv_Date { get; set; }
        public string Segement { get; set; }
        public string SO_Name { get; set; }
        public string ASM { get; set; }
        public string RM { get; set; }
        public string Segment_Head { get; set; }
    }



    public class RequestDetailArray_Render
    {
        public string DONo { get; set; }
        public string SRVInvoiceNo { get; set; }
        public int? Damaged { get; set; }
        public int? Short { get; set; }
        public int? Excess { get; set; }
        public int? ReceivedQuantity { get; set; }
        public bool? Acknowledge { get; set; }
        public int? SAPsubReasonID { get; set; }
        public long Detail_Id { get; set; }
        public string BatchNo { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public decimal? SRVQuantity { get; set; }
        public decimal? PackSize { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? ProvideGST_Yes { get; set; }
        public bool? ProvideGST_No { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        public string Unit { get; set; }
        public decimal? SRVValue { get; set; }
        public decimal? Volume { get; set; }
        public Nullable<int> SubReason { get; set; }
        public string SubReasonName { get; set; }
        public string UploadedInvoice { get; set; }
        public long? UploadedInvoice_Id { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> Manufacturing_Date { get; set; }
        public string Shelf_Life { get; set; }
        //Used when display  data in grid
        public int? ComplaintNumber { get; set; }
        public ComplaintDetail_Render selectedComplaint { get; set; }
        public SKUClass selectedSKU { get; set; }
        public Nullable<bool> ReleaseByCM { get; set; }
        public Nullable<System.DateTime> ReleaseByCM_Date { get; set; }
    }

    public class ComplaintDetail_Render
    {
        public long? Complaint_ID { get; set; }
        public int? ComplaintNumber { get; set; }
        public string ComplaintDesc { get; set; }
        public decimal? ComplaintQty { get; set; }
    }

}



