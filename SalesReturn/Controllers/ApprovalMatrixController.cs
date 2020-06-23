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
    public class ApprovalMatrixController : ApiController
    {
        [HttpGet]
        public List<ApprovalMatrixModal> getApprovalMatrixData()
        {
            return ApprovalMatrixDAL.getApprovalMatrixData();

        }


        [HttpGet]
        public List<BUList> getBUForCountry(string Country)
        {
            return ApprovalMatrixDAL.getBUForCountry(Country);
        }

        [HttpGet]
        public List<DivisionList> getDivisionForBU(string BU)
        {
            return ApprovalMatrixDAL.getDivisionForBU(BU);
        }
        [HttpPost]
        public List<EmployeeCommanModal> GetSalesDirector([FromBody]SearchModal Obj)
        {
            return ApprovalMatrixDAL.GetSalesDirector(Obj);
        }
        [HttpPost]
        public string UpdateApprovalMatrix([FromBody]List<ApprovalMatrixModal> Obj)
        {
            return ApprovalMatrixDAL.UpdateApprovalMatrix(Obj);
        }


        [HttpGet]
        public string DeleteApprovalMatrixRow(int Id, string EmployeeCode)
        {
            return ApprovalMatrixDAL.DeleteApprovalMatrixRow(Id, EmployeeCode);
        }
        
        [HttpGet]
        public List<FlowMatrixModel> getFlowMatrixList()
        {
            return ApprovalMatrixDAL.getFlowMatrixList();

        }

        [HttpPost]
        public string UpdateFlowList(List<FlowMatrixModel> Obj)
        {
            return ApprovalMatrixDAL.UpdateFlowList(Obj);
        }
        


    }
}