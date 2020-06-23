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
    public class PendingRequestController : ApiController
    {
        [HttpGet]
        public List<PendingRequestModel> getPendingRequest(string EmployeeCode)
        {
            return PendingRequestDAL.getPendingRequest(EmployeeCode);

        }
        [HttpPost]
        public string RecommendRequest([FromBody]RecommendRequestObj data)
        {
            return PendingRequestDAL.RecommendRequest(data);
        }

        [HttpPost]
        public string RejectRequest([FromBody]RecommendRequestObj data)
        {
            return PendingRequestDAL.RejectRequest(data);
        }

        [HttpPost]
        public string ApproveRequest([FromBody]RecommendRequestObj data)
        {
            return PendingRequestDAL.ApproveRequest(data);
        }

        [HttpPost]
        public string ReconsiderRequest([FromBody]RecommendRequestObj data)
        {
            return PendingRequestDAL.ReconsiderRequest(data);
        }

        [HttpPost]
        public string CloseRequest([FromBody]CloseRequestObjForStg_4 data)
        {
            return PendingRequestDAL.CloseRequest(data);
        }
    }
}