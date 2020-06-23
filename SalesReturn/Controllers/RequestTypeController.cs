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
    public class RequestTypeController : ApiController
    {
        [HttpGet]
        public List<RequestTypeModal> GetRequestType()
        {
            return RequestTypeMasterDAL.GetRequestType();
        }
        [HttpPost]
        public string SaveRequestType(List<RequestTypeModal> list)
        {
            return RequestTypeMasterDAL.SaveRequestType(list);
        }
        [HttpGet]
        public string DeleteRequestType(int Id, string EmployeeCode)
        {
            return RequestTypeMasterDAL.DeleteRequestType(Id, EmployeeCode);
        }

        [HttpGet]
        public List<RequestTypeModal> GetSubReasonList(int Id)
        {
            return RequestTypeMasterDAL.GetSubReasonList(Id);
        }


        [HttpGet]
        public List<SP_GetBatchNo_Result> GetBatchNO(string SkuCode)
        {
            return RequestTypeMasterDAL.GetBatchNO(SkuCode);
        }


        [HttpGet]
        public List<SAPReasonModel> GetSAPReasonDetailByReason(int ReqType)
        {
            return RequestTypeMasterDAL.GetSAPReasonDetailByReason(ReqType);
        }
        

        [HttpGet]
        public List<PendingRequestModel> getRequestforBillingClosure(string EmpCode)
        {
            return RequestTypeMasterDAL.getRequestforBillingClosure(EmpCode);
        }
        
         [HttpGet]
        public DateTime GetLastThreeMonthDate()
        {
            return RequestTypeMasterDAL.GetLastThreeMonthDate();
        }

        [HttpPost]
        public InvoiceDetailListModel GetInvoiceDataRequest(InvoiceDataModel obj)
        {
            return RequestTypeMasterDAL.GetInvoiceDataRequest(obj);
        }

    }
}