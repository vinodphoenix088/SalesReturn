using SalesReturnBLL.BLL;
using SalesReturnDAL.DAL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesReturn.Controllers
{
    public class CommonController : ApiController
    {
        [HttpGet]
        public List<CountryModal> GetCountry()
        {
            return CommonDAL.GetCountry();
        }

        [HttpGet]
        public IList<StatusdetailInfo> GetRequestStatusDetails(long RequestId)
        {
            return CommonDAL.GetRequestStatusDetails(RequestId);
        }
        [HttpGet]
        public EmployeeDetailsInfo GetEmployeeRequestDetails(long RequestId)
        {
            return CommonDAL.GetEmployeeRequestDetails(RequestId);
        }
        [HttpGet]
        public IList<FutureStatus> GetFutureStatus(long RequestId)
        {
            return CommonDAL.GetFutureStatus(RequestId);
        }
        [HttpGet]
        public IList<StatusdetailInfo> GetCurrentStatus(long RequestId)
        {
            return CommonDAL.GetCurrentStatus(RequestId);
        }
        [HttpGet]
        public RequestDetailObj_Render GetRequestDetails(int RequestId, int CurrentStatus_Id, int FutureStatus_Id)
        {
            return CommonDAL.GetRequestDetails(RequestId, CurrentStatus_Id, FutureStatus_Id);
        }

        public RequestDetailObj_Render GetSavedRequestDetails(int RequestId, int CurrentStatus_Id, int FutureStatus_Id)
        {
            return CommonDAL.GetSavedRequestDetails(RequestId, CurrentStatus_Id, FutureStatus_Id);
        }

        [HttpGet]
        public RequestDetailObj_Render GetRequestDetailsforNextStage(int RequestId, int CurrentStatus_Id, int FutureStatus_Id)
        {
            return CommonDAL.GetRequestDetailsforNextStage(RequestId, CurrentStatus_Id, FutureStatus_Id);
        }

        [HttpGet]
        public List<spGetMasterReportData_Result> GetMasterReport(DateTime datefrom, DateTime dateTo)
        {
            return CommonDAL.GetMasterReport(datefrom, dateTo);
        }
    }
}