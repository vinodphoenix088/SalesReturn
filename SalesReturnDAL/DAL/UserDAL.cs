using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnDAL.DAL
{
    public class UserDAL
    {

        public static LoginModel GetEmployeeDetails(string id, string conn)
        {
            LoginModel user = null;
            using (var DbContext = new SalesReturndbEntities())
            {
                var source = DbContext.SP_LFGDetails(id).FirstOrDefault();
                var IfDepotPerson = DbContext.sp_CheckIfDepotPerson(id).FirstOrDefault();
                if (source != null)
                {
                    user = new LoginModel
                    {
                        EMP_CODE = source.EMP_CODE.ToString(),
                        Emp_First_name = source.Emp_First_name.ToString(),
                        Desg_Desc = source.Desg_Desc,
                        Dept_name = source.Dept_name,
                        IsDepotPerson= IfDepotPerson!=null?true:false,
                        DepotName = IfDepotPerson?.DepotName,
                        DepotCode= IfDepotPerson?.DepotCode,
                        Seg_Name = source.Seg_Name,
                        DepotId= IfDepotPerson!=null? IfDepotPerson.DepotId:0,
                        email_id = source.email_id,
                        Mobile_no = source.Mobile_no,
                        SBU_Name = source.SBU_Name,
                        Zone = source.Zone,
                        RegionHead = source.Regional_Head,
                        SegmentHead = source.Country_Head,
                        Oth_key = id,
                        Country = source.Country
                    };

                    var checkAdmin = DbContext.TblAdminMasters.Where(x => x.EmployeeCode.Equals(id) && x.IsActive == true).FirstOrDefault();

                    if (id.Equals("0") || checkAdmin != null)
                    {
                        user.RoleName = "Admin";
                    }
                }
            }
            return user;
        }

        public static TblEmployeeMaster CheckIfDepotPerson(string Emp_Code)
        {
            using (var DbContext = new SalesReturndbEntities())
            {
                var source = DbContext.TblEmployeeMasters.Where(x=>x.Depotcode== Emp_Code&& x.IsActive==true).FirstOrDefault();
                if (source != null)
                {
                    return source;
                }
                else
                    return null;
            }
        }
    }
}
