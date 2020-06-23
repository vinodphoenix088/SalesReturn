using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class ReasonMasterModal
    {
        public int? ReasonMaster_Id { get; set; }
        public string Reason { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class SalesReasonMasterModel
    {
        public int SalesReason_Id { get; set; }
        public int RequestTypeId { get; set; }
        public string SubReason { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public  class SAPReasonModel
    {
        public int SAPReasonID { get; set; }
        public int RequestTypeId { get; set; }
        public Nullable<int> SubReasonID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string SAPSubReasons { get; set; }
    }

    public  class SAPSMasterList
    {
        public int SAPID { get; set; }
        public string SAPCode { get; set; }
        public string SubReasons { get; set; }
        public string Process { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
