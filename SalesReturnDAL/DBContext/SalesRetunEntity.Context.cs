﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesReturnDAL.DBContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SalesReturndbEntities : DbContext
    {
        public SalesReturndbEntities()
            : base("name=SalesReturndbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TblRequestType> TblRequestTypes { get; set; }
        public virtual DbSet<TblApprovalMatrix> TblApprovalMatrices { get; set; }
        public virtual DbSet<TblReasonMaster> TblReasonMasters { get; set; }
        public virtual DbSet<TblUploadedInvoice> TblUploadedInvoices { get; set; }
        public virtual DbSet<TblApproverDetail> TblApproverDetails { get; set; }
        public virtual DbSet<TblApproverHeader> TblApproverHeaders { get; set; }
        public virtual DbSet<TblFutureStatu> TblFutureStatus { get; set; }
        public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; }
        public virtual DbSet<TblAdminMaster> TblAdminMasters { get; set; }
        public virtual DbSet<TblSalesReasonMaster> TblSalesReasonMasters { get; set; }
        public virtual DbSet<TblStatu> TblStatus { get; set; }
        public virtual DbSet<TblFlowMatrix> TblFlowMatrices { get; set; }
        public virtual DbSet<TblMailTemplate> TblMailTemplates { get; set; }
        public virtual DbSet<TblSAPReasonMaster> TblSAPReasonMasters { get; set; }
        public virtual DbSet<TblSAPSMaster> TblSAPSMasters { get; set; }
        public virtual DbSet<TblEmployeeMaster> TblEmployeeMasters { get; set; }
        public virtual DbSet<TblBarCodeDetail> TblBarCodeDetails { get; set; }
        public virtual DbSet<TblRequestHeader> TblRequestHeaders { get; set; }
        public virtual DbSet<tblRequestDtl> tblRequestDtls { get; set; }
    
        public virtual ObjectResult<SP_GetCountry_Result> SP_GetCountry()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetCountry_Result>("SP_GetCountry");
        }
    
        public virtual ObjectResult<SP_Get_Division_For_BU_Result> SP_Get_Division_For_BU(string bU)
        {
            var bUParameter = bU != null ?
                new ObjectParameter("BU", bU) :
                new ObjectParameter("BU", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_Division_For_BU_Result>("SP_Get_Division_For_BU", bUParameter);
        }
    
        public virtual ObjectResult<sp_GetBUList_Result> sp_GetBUList(string country)
        {
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetBUList_Result>("sp_GetBUList", countryParameter);
        }
    
        public virtual ObjectResult<SP_LFGDetails_Result> SP_LFGDetails(string eMP_CODE)
        {
            var eMP_CODEParameter = eMP_CODE != null ?
                new ObjectParameter("EMP_CODE", eMP_CODE) :
                new ObjectParameter("EMP_CODE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_LFGDetails_Result>("SP_LFGDetails", eMP_CODEParameter);
        }
    
        public virtual ObjectResult<SP_GetEmployeeForBU_Division_Country_Result> SP_GetEmployeeForBU_Division_Country(string bUName, string divisionName, string country)
        {
            var bUNameParameter = bUName != null ?
                new ObjectParameter("BUName", bUName) :
                new ObjectParameter("BUName", typeof(string));
    
            var divisionNameParameter = divisionName != null ?
                new ObjectParameter("DivisionName", divisionName) :
                new ObjectParameter("DivisionName", typeof(string));
    
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetEmployeeForBU_Division_Country_Result>("SP_GetEmployeeForBU_Division_Country", bUNameParameter, divisionNameParameter, countryParameter);
        }
    
        public virtual ObjectResult<SP_GetDepotList_Result> SP_GetDepotList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetDepotList_Result>("SP_GetDepotList");
        }
    
        public virtual ObjectResult<SP_GetDealerList_Result> SP_GetDealerList(Nullable<long> depot_Id)
        {
            var depot_IdParameter = depot_Id.HasValue ?
                new ObjectParameter("Depot_Id", depot_Id) :
                new ObjectParameter("Depot_Id", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetDealerList_Result>("SP_GetDealerList", depot_IdParameter);
        }
    
        public virtual ObjectResult<string> SP_Get_Segment_For_BU_And_Division(string bU, string division)
        {
            var bUParameter = bU != null ?
                new ObjectParameter("BU", bU) :
                new ObjectParameter("BU", typeof(string));
    
            var divisionParameter = division != null ?
                new ObjectParameter("Division", division) :
                new ObjectParameter("Division", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SP_Get_Segment_For_BU_And_Division", bUParameter, divisionParameter);
        }
    
        public virtual ObjectResult<SP_GetPendingRequest_Result> SP_GetPendingRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetPendingRequest_Result>("SP_GetPendingRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetEmployeeDetailsForRequest_Result> SP_GetEmployeeDetailsForRequest(Nullable<long> requestID)
        {
            var requestIDParameter = requestID.HasValue ?
                new ObjectParameter("RequestID", requestID) :
                new ObjectParameter("RequestID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetEmployeeDetailsForRequest_Result>("SP_GetEmployeeDetailsForRequest", requestIDParameter);
        }
    
        public virtual ObjectResult<SP_GetFutureStatusDetails_Result> SP_GetFutureStatusDetails(Nullable<long> requestID)
        {
            var requestIDParameter = requestID.HasValue ?
                new ObjectParameter("RequestID", requestID) :
                new ObjectParameter("RequestID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetFutureStatusDetails_Result>("SP_GetFutureStatusDetails", requestIDParameter);
        }
    
        public virtual ObjectResult<SP_GetRequestStatusDetails_Result> SP_GetRequestStatusDetails(Nullable<long> requestID)
        {
            var requestIDParameter = requestID.HasValue ?
                new ObjectParameter("RequestID", requestID) :
                new ObjectParameter("RequestID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetRequestStatusDetails_Result>("SP_GetRequestStatusDetails", requestIDParameter);
        }
    
        public virtual ObjectResult<SP_GetCurrentStatusDetails_Result> SP_GetCurrentStatusDetails(Nullable<long> requestId)
        {
            var requestIdParameter = requestId.HasValue ?
                new ObjectParameter("RequestId", requestId) :
                new ObjectParameter("RequestId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetCurrentStatusDetails_Result>("SP_GetCurrentStatusDetails", requestIdParameter);
        }
    
        public virtual ObjectResult<SP_GetRequestDetail_Result> SP_GetRequestDetail(Nullable<int> request_Id)
        {
            var request_IdParameter = request_Id.HasValue ?
                new ObjectParameter("Request_Id", request_Id) :
                new ObjectParameter("Request_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetRequestDetail_Result>("SP_GetRequestDetail", request_IdParameter);
        }
    
        public virtual ObjectResult<SP_GetCCNumber_Result> SP_GetCCNumber(Nullable<int> cC_Id)
        {
            var cC_IdParameter = cC_Id.HasValue ?
                new ObjectParameter("CC_Id", cC_Id) :
                new ObjectParameter("CC_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetCCNumber_Result>("SP_GetCCNumber", cC_IdParameter);
        }
    
        public virtual ObjectResult<SP_GetRejectedRequest_Result> SP_GetRejectedRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetRejectedRequest_Result>("SP_GetRejectedRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetInprocessRequest_Result> SP_GetInprocessRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetInprocessRequest_Result>("SP_GetInprocessRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetApprovedRequest_Result> SP_GetApprovedRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetApprovedRequest_Result>("SP_GetApprovedRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_GetSavedAsDraftRequest(string empCode, Nullable<int> depot_Id, Nullable<int> dealer_Id, Nullable<int> reason_Id)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            var depot_IdParameter = depot_Id.HasValue ?
                new ObjectParameter("Depot_Id", depot_Id) :
                new ObjectParameter("Depot_Id", typeof(int));
    
            var dealer_IdParameter = dealer_Id.HasValue ?
                new ObjectParameter("Dealer_Id", dealer_Id) :
                new ObjectParameter("Dealer_Id", typeof(int));
    
            var reason_IdParameter = reason_Id.HasValue ?
                new ObjectParameter("Reason_Id", reason_Id) :
                new ObjectParameter("Reason_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_GetSavedAsDraftRequest", empCodeParameter, depot_IdParameter, dealer_IdParameter, reason_IdParameter);
        }
    
        public virtual ObjectResult<spGetCCStackHolderDetail_Result> spGetCCStackHolderDetail()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCCStackHolderDetail_Result>("spGetCCStackHolderDetail");
        }
    
        public virtual ObjectResult<string> spGetCSOList(string depot)
        {
            var depotParameter = depot != null ?
                new ObjectParameter("depot", depot) :
                new ObjectParameter("depot", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("spGetCSOList", depotParameter);
        }
    
        public virtual ObjectResult<SP_GetClosedRequest_Result> SP_GetClosedRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetClosedRequest_Result>("SP_GetClosedRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetOpenRequest_Result> SP_GetOpenRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetOpenRequest_Result>("SP_GetOpenRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetPendingRequestForDepot_Result> SP_GetPendingRequestForDepot(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetPendingRequestForDepot_Result>("SP_GetPendingRequestForDepot", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetPendingSRVClosureRequest_Result> SP_GetPendingSRVClosureRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetPendingSRVClosureRequest_Result>("SP_GetPendingSRVClosureRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetTotalRequest_Result> SP_GetTotalRequest(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetTotalRequest_Result>("SP_GetTotalRequest", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetDepotList1_Result> SP_GetDepotList1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetDepotList1_Result>("SP_GetDepotList1");
        }
    
        public virtual int spGetMasterReport(Nullable<System.DateTime> dateForm, Nullable<System.DateTime> dateTo)
        {
            var dateFormParameter = dateForm.HasValue ?
                new ObjectParameter("dateForm", dateForm) :
                new ObjectParameter("dateForm", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("DateTo", dateTo) :
                new ObjectParameter("DateTo", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spGetMasterReport", dateFormParameter, dateToParameter);
        }
    
        public virtual ObjectResult<spGetMasterReportData_Result> spGetMasterReportData(Nullable<System.DateTime> dateForm, Nullable<System.DateTime> dateTo)
        {
            var dateFormParameter = dateForm.HasValue ?
                new ObjectParameter("dateForm", dateForm) :
                new ObjectParameter("dateForm", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("DateTo", dateTo) :
                new ObjectParameter("DateTo", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetMasterReportData_Result>("spGetMasterReportData", dateFormParameter, dateToParameter);
        }
    
        public virtual ObjectResult<spGetShelfLifecount_Result> spGetShelfLifecount(string skuCode)
        {
            var skuCodeParameter = skuCode != null ?
                new ObjectParameter("skuCode", skuCode) :
                new ObjectParameter("skuCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetShelfLifecount_Result>("spGetShelfLifecount", skuCodeParameter);
        }
    
        public virtual ObjectResult<spGetShelfLifeData_Result> spGetShelfLifeData(string skuCode)
        {
            var skuCodeParameter = skuCode != null ?
                new ObjectParameter("skuCode", skuCode) :
                new ObjectParameter("skuCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetShelfLifeData_Result>("spGetShelfLifeData", skuCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetBatchNoData_Result> SP_GetBatchNoData(string sKUCode)
        {
            var sKUCodeParameter = sKUCode != null ?
                new ObjectParameter("SKUCode", sKUCode) :
                new ObjectParameter("SKUCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetBatchNoData_Result>("SP_GetBatchNoData", sKUCodeParameter);
        }
    
        [DbFunction("SalesReturndbEntities", "SplitString")]
        public virtual IQueryable<string> SplitString(string input, string character)
        {
            var inputParameter = input != null ?
                new ObjectParameter("Input", input) :
                new ObjectParameter("Input", typeof(string));
    
            var characterParameter = character != null ?
                new ObjectParameter("Character", character) :
                new ObjectParameter("Character", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<string>("[SalesReturndbEntities].[SplitString](@Input, @Character)", inputParameter, characterParameter);
        }
    
        public virtual ObjectResult<SP_GetBatchNo_Result> SP_GetBatchNo(string sKUCode)
        {
            var sKUCodeParameter = sKUCode != null ?
                new ObjectParameter("SKUCode", sKUCode) :
                new ObjectParameter("SKUCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetBatchNo_Result>("SP_GetBatchNo", sKUCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetInvoiceDetailList_Result> SP_GetInvoiceDetailList(string invoiceNo, string skucode, string batchno)
        {
            var invoiceNoParameter = invoiceNo != null ?
                new ObjectParameter("InvoiceNo", invoiceNo) :
                new ObjectParameter("InvoiceNo", typeof(string));
    
            var skucodeParameter = skucode != null ?
                new ObjectParameter("Skucode", skucode) :
                new ObjectParameter("Skucode", typeof(string));
    
            var batchnoParameter = batchno != null ?
                new ObjectParameter("Batchno", batchno) :
                new ObjectParameter("Batchno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetInvoiceDetailList_Result>("SP_GetInvoiceDetailList", invoiceNoParameter, skucodeParameter, batchnoParameter);
        }
    
        public virtual ObjectResult<SP_GetSKUCode_Result> SP_GetSKUCode(string sKUCode)
        {
            var sKUCodeParameter = sKUCode != null ?
                new ObjectParameter("SKUCode", sKUCode) :
                new ObjectParameter("SKUCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetSKUCode_Result>("SP_GetSKUCode", sKUCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetSavedAsDraftRequestIds_Result> SP_GetSavedAsDraftRequestIds(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetSavedAsDraftRequestIds_Result>("SP_GetSavedAsDraftRequestIds", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_GetSavedRequests_Result> SP_GetSavedRequests(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetSavedRequests_Result>("SP_GetSavedRequests", empCodeParameter);
        }
    
        public virtual ObjectResult<sp_CheckIfDepotPerson_Result> sp_CheckIfDepotPerson(string emp_Code)
        {
            var emp_CodeParameter = emp_Code != null ?
                new ObjectParameter("Emp_Code", emp_Code) :
                new ObjectParameter("Emp_Code", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_CheckIfDepotPerson_Result>("sp_CheckIfDepotPerson", emp_CodeParameter);
        }
    
        public virtual ObjectResult<SP_GetDepotBasedOnEmpCode_Result> SP_GetDepotBasedOnEmpCode(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetDepotBasedOnEmpCode_Result>("SP_GetDepotBasedOnEmpCode", empCodeParameter);
        }
    
        public virtual ObjectResult<SP_LFGDetailsBasedOnName_Result> SP_LFGDetailsBasedOnName(string eMP_CODE)
        {
            var eMP_CODEParameter = eMP_CODE != null ?
                new ObjectParameter("EMP_CODE", eMP_CODE) :
                new ObjectParameter("EMP_CODE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_LFGDetailsBasedOnName_Result>("SP_LFGDetailsBasedOnName", eMP_CODEParameter);
        }
    
        public virtual ObjectResult<sp_CheckEPNumber_Result> sp_CheckEPNumber(string eP_No)
        {
            var eP_NoParameter = eP_No != null ?
                new ObjectParameter("EP_No", eP_No) :
                new ObjectParameter("EP_No", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_CheckEPNumber_Result>("sp_CheckEPNumber", eP_NoParameter);
        }
    
        public virtual ObjectResult<SP_Find_if_ExistIn_EmployeeMsater_Result> SP_Find_if_ExistIn_EmployeeMsater(string empCode)
        {
            var empCodeParameter = empCode != null ?
                new ObjectParameter("EmpCode", empCode) :
                new ObjectParameter("EmpCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Find_if_ExistIn_EmployeeMsater_Result>("SP_Find_if_ExistIn_EmployeeMsater", empCodeParameter);
        }
    
        public virtual ObjectResult<sp_GetuserDetailsFromLFG_Result> sp_GetuserDetailsFromLFG(string eMP_CODE)
        {
            var eMP_CODEParameter = eMP_CODE != null ?
                new ObjectParameter("EMP_CODE", eMP_CODE) :
                new ObjectParameter("EMP_CODE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetuserDetailsFromLFG_Result>("sp_GetuserDetailsFromLFG", eMP_CODEParameter);
        }
    
        public virtual ObjectResult<string> Sp_GetDepotNameById(Nullable<long> depotId)
        {
            var depotIdParameter = depotId.HasValue ?
                new ObjectParameter("depotId", depotId) :
                new ObjectParameter("depotId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Sp_GetDepotNameById", depotIdParameter);
        }
    
        public virtual ObjectResult<sp_GetDealerDtlBy_DealerRepositoryId_Result> sp_GetDealerDtlBy_DealerRepositoryId(Nullable<long> dealerRepositoryId)
        {
            var dealerRepositoryIdParameter = dealerRepositoryId.HasValue ?
                new ObjectParameter("DealerRepositoryId", dealerRepositoryId) :
                new ObjectParameter("DealerRepositoryId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetDealerDtlBy_DealerRepositoryId_Result>("sp_GetDealerDtlBy_DealerRepositoryId", dealerRepositoryIdParameter);
        }
    }
}