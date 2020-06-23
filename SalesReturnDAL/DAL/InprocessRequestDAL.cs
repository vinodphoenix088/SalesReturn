using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnDAL.DAL
{
    public class InprocessRequestDAL
    {

        public static List<PendingRequestModel> getInprocessRequest(string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {

                List<PendingRequestModel> dataList = new List<PendingRequestModel>();
                var InprocessRequest = context.SP_GetInprocessRequest(EmployeeCode).ToList();
                if (InprocessRequest != null)
                {
                    dataList = InprocessRequest.Select(x => new PendingRequestModel()
                    {
                        BatchNo = x.BatchNo,
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerName = x.DealerName,
                        DepotName = x.DepotName,
                        DepotAddress = x.DepotAddress,
                        DepotCode = x.DepotCode,
                        RequestHeaderId = x.RequestHeaderId,
                        FutureStatus_Id = x.FutureStatus_Id,
                        FutureStatus = x.FutureStatus,
                        CurrentStatus_Id = x.CurrentStatus_Id,
                        SKUCode = x.SKUCode,
                        SKUName = x.SKUName,
                        CreatedBy = context.SP_LFGDetails(x.CreatedBy).FirstOrDefault().Emp_First_name,
                        CreatedBy_EMP_CODE = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        TotalSRV = context.tblRequestDtls.Where(o => o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p => p.SRVValue).Value,
                        RequestTypeOption = x.SKUCode,// for access data behalf on skucode
                    }).ToList();


                    return dataList;
                }
                return dataList;
            }


        }
    }
}
