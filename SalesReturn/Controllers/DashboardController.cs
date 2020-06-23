using SalesReturnDAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesReturn.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpGet]
        public int GetPendingRequestCount(string empCode)
        {
            return DashboardDAL.GetPendingRequestCount(empCode);
        }

        [HttpGet]
        public int GetInprocessRequestCount(string empCode)
        {
            return DashboardDAL.GetInprocessRequestCount(empCode);
        }

        [HttpGet]
        public int GetRejectRequestCount(string empCode)
        {
            return DashboardDAL.GetRejectRequestCount(empCode);
        }

        [HttpGet]
        public int GetClosedRequestCount(string empCode)
        {
            return DashboardDAL.GetClosedRequestCount(empCode);
        }

        [HttpGet]
        public int GetApprovedRequestCount(string empCode)
        {
            return DashboardDAL.GetApprovedRequestCount(empCode);
        }

        [HttpGet]
        public int GetSalesTotalRequestCount(string empCode)
        {
            return DashboardDAL.GetSalesTotalRequestCount(empCode);
        }

        [HttpGet]
        public int GetTotalRequestCount(string empCode)
        {
            return DashboardDAL.GetTotalRequestCount(empCode);
        }

        [HttpGet]
        public int GetPendingBillingClosureRequestCount(string empCode)
        {
            return DashboardDAL.GetPendingBillingClosureRequestCount(empCode);
        }

        [HttpGet]
        public int GetPendingSRVBillingClosureRequestCount(string empCode)
        {
            return DashboardDAL.GetPendingSRVBillingClosureRequestCount(empCode);
        }
    }
}
