using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class BUList
    {
        public int BU_Id { get; set; }
        public string BUName { get; set; }
    }
    public class DivisionList
    {
        public long Division_Id { get; set; }
        public string DivisionName { get; set; }

    }
    public class SearchModal
    {
        public string CountryName { get; set; }
        public string BUName { get; set; }
        public string DivisionName { get; set; }
    }

    public class EmployeeCommanModal
    {

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

    }

    public class FlowDataModel
    {

        public int RequestType { get; set; }
        public List<FlowModel> FlowListModel { get; set; }

    }

    public class FlowModel
    {

        public string Options { get; set; }
        public FlowMatrixModel FlowObjModel { get; set; }

    }



    public class FlowMatrixModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public int RequestType { get; set; }
        public string Options { get; set; }
        public Nullable<long> ComplaintHandler { get; set; }
        public Nullable<long> ComplaintManager { get; set; }
        public Nullable<long> LogisticsHead { get; set; }
        public Nullable<long> ISC { get; set; }
        public Nullable<long> RH { get; set; }
        public Nullable<long> SegmentHead { get; set; }
        public Nullable<long> VP { get; set; }
        public Nullable<long> President { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ApprovalMatrixModal
    {
        public int? Matrix_Id { get; set; }
        public string Country { get; set; }
        public string BUType { get; set; }
        public string Division { get; set; }
        public int RequestType { get; set; }
        public Nullable<decimal> SRV_Value { get; set; }
        public Nullable<decimal> InvoiceAge { get; set; }
        public string ComplaintHandler { get; set; }
        public string ComplaintManager { get; set; }
        public string LogisticsManager { get; set; }
        public string LogisticsHead { get; set; }
        public Nullable<decimal> SegmentHeadHRV { get; set; }
        public Nullable<decimal> SegmentInvoiceAge { get; set; }
        public string VPName { get; set; }
        public string President { get; set; }
        public string CreatedBy { get; set; }

    }
}

