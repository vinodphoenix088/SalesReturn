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
    public class CatalystStatusController : ApiController
    {
        [HttpGet]
        public List<PendingRequestModel> GetOpenRequest(string empCode)
        {
            return DashboardDAL.GetOpenRequest(empCode);
        }

        [HttpGet]
        public List<PendingRequestModel> GetClosedRequest(string empCode)
        {
            return DashboardDAL.GetClosedRequest(empCode);
        }
    }
}
