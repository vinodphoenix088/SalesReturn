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
    public class ApprovedRequestController : ApiController
    {
        [HttpGet]
        public List<PendingRequestModel> getApprovedRequest(string EmployeeCode)
        {
            return ApprovedRequestDAL.getApprovedRequest(EmployeeCode);

        }
    }
}