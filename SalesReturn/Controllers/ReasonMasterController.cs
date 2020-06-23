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
    public class ReasonMasterController : ApiController
    {

        [HttpGet]
        public List<ReasonMasterModal> GetReasonMaster()
        {
            return ReasonMasterDAL.GetReasonMaster();
        }
        [HttpPost]
        public string SaveReason(List<ReasonMasterModal> list)
        {
            return ReasonMasterDAL.SaveReason(list);
        }
        [HttpGet]
        public string DeleteReason(int Id, string EmployeeCode)
        {
            return ReasonMasterDAL.DeleteReason(Id, EmployeeCode);
        }
        
        [HttpGet]
        public List<SalesReasonMasterModel> GetReasonDetail()
        {
            return ReasonMasterDAL.GetReasonDetail();
        }
        [HttpPost]
        public string UpdateReasonDetail(List<SalesReasonMasterModel> list)
        {
            return ReasonMasterDAL.UpdateReason(list);
        }
        [HttpGet]
        public string DeleteReasonRow(int Id,string EmployeeCode)
        {
            return ReasonMasterDAL.DeleteReasonRow(Id, EmployeeCode);
        }
        [HttpGet]
        public List<SAPReasonModel> GetSAPReasonDetail()
        {
            return ReasonMasterDAL.GetSAPReasonDetail();
        }

        [HttpGet]
        public List<SAPSMasterList> GetSAPSubReasonMasterList()
        {
            return ReasonMasterDAL.GetSAPSubReasonMasterList();
        }
        [HttpPost]
        public string UpdateSAPSubReason(List<SAPReasonModel> list)
        {
            return ReasonMasterDAL.UpdateSAPSubReason(list);
        }
        [HttpGet]
        public string DeleteSAPSubReason(int Id, string EmployeeCode)
        {
            return ReasonMasterDAL.DeleteSAPSubReason(Id, EmployeeCode);
        }
    }
}