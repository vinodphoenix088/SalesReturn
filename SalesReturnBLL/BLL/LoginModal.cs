using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{


    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string RoleName { get; set; }

        public string DesignationName { get; set; }
        public Nullable<int> DesignationID { get; set; }
        public Nullable<int> Level { get; set; }
        public string Token { get; set; }
        public Nullable<int> ContractorID { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public string EmployeeName { get; set; }
        public string RegionHead { get; set; }
        public string SegmentHead { get; set; }
        public string ContractorName { get; set; }
        public int User_ID { get; set; }
        public string Oth_key { get; set; }

        public string EMP_CODE { get; set; }
        public string Country { get; set; }
        public string Emp_First_name { get; set; }
        public string Emp_Middle_name { get; set; }
        public string Emp_Last_name { get; set; }
        public string Desg_Desc { get; set; }
        public long DepotId { get; set; }
        public string Dept_name { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }
        public bool IsDepotPerson { get; set; }
        //public int Dept_Id { get; set; }
        //public int Seg_Id { get; set; }
        public string SBU_Name { get; set; }
        public int Department_Id { get; set; }

        public string Seg_Name { get; set; }
        public int Seg_Id { get; set; }
        public DateTime doj { get; set; }

        public string SubDepartment_Name { get; set; }
        public string NewCost_Center { get; set; }
        public string Emp_Status { get; set; }

        public string Zone { get; set; }
        public string Region_Name { get; set; }
        public string email_id { get; set; }
        public string Mobile_no { get; set; }
        public string RM1 { get; set; }
        public string RM2 { get; set; }
        public bool isApprover { get; set; }
        public bool isAdmin { get; set; }
        public bool isFinance { get; set; }
        public decimal? amount { get; set; }

        public string AppSeg_Head { get; set; }
        public string AppRegion_Head { get; set; }
        public string AppCentral_Planer { get; set; }
    }
}
