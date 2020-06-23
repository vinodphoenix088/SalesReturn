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
    public class ClosedController : ApiController
    {
        [HttpGet]
        public List<PendingRequestModel> getClosedRequest(string EmployeeCode)
        {
            return Closed.getClosedRequest(EmployeeCode);
        }
    }
}
