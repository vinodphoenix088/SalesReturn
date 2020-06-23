using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class StatusBLL
    {}

    public partial class StatusdetailInfo
    {
        public long RequestId { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Added_By { get; set; }
        public System.DateTime Added_Date { get; set; }
        public string EMP_CODE { get; set; }
        public string Emp_First_name { get; set; }
    }
    public class FutureStatus
    {
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class EmployeeDetailsInfo
    {
        public string RequestNumber { get; set; }
        public string EMP_CODE { get; set; }
        public string Emp_First_name { get; set; }
        public string Dept_name { get; set; }
        public string SBU_Name { get; set; }
        public System.DateTime Added_Date { get; set; }
    }
}
