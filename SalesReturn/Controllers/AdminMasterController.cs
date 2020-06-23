using SalesReturnBLL.BLL;
using SalesReturnDAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesReturn.Controllers
{
    public class AdminMasterController : ApiController
    {
        [HttpGet]
        public List<AdminMasterModal> getAdminList()
        {
            return AdminMasterDAL.getAdminList();

        }

        [HttpPost]
        public string UpdateAdminList([FromBody]List<AdminMasterModal> Obj)
        {
            return AdminMasterDAL.UpdateAdminList(Obj);
        }


        [HttpGet]
        public string DeleteAdminList(int Id, string EmployeeCode)
        {
            return AdminMasterDAL.DeleteAdminList(Id, EmployeeCode);
        }

        [HttpGet] 
        public List<EmployeeMasterModel> getEmployeeList()
        {
            return AdminMasterDAL.getEmployeeList();
        }

        [HttpGet] 
        public List<CCStackHolder> GetCCStackHolderDetail()
        {
            return AdminMasterDAL.GetCCStackHolderDetail();
        }

        [HttpPost]
        public string UpdateEmployeeList([FromBody]List<EmployeeMasterModel> Obj)
        {
            return AdminMasterDAL.UpdateEmployeeList(Obj);
        }

        [HttpGet]
        public string DeleteList(int Id, string EmployeeCode)
        {
            return AdminMasterDAL.DeleteList(Id, EmployeeCode);
        }
    }
}