using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class AdminMasterModal
    {
        public int Admin_Id { get; set; }
        public string EmployeeCode { get; set; }
        public string CreatedBy { get; set; }
        
    }

    public class EmployeeMasterModel
    {
        public int Id { get; set; }
        public string DepotName { get; set; }
        public string CommercialCode { get; set; }
        public string CommercialHead { get; set; }
        
        public string Depotcode { get; set; }
        public string CSO { get; set; }
        public string ComplaintHandler { get; set; }
        public string ComplaintManager { get; set; }
        public string LogisticsHead { get; set; }
        public string ISC { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class CCStackHolder
    {
        public int ComplaintStakeHolders_ID { get; set; }
        public string SBU_Name { get; set; }
        public string Catalyst { get; set; }
        public string ComplaintHandler { get; set; }
        public string ComplaintManager { get; set; }
        public string LocalTechnical { get; set; }
        public string Manager { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
