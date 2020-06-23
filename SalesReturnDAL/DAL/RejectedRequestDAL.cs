using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnDAL.DAL
{
    public class RejectedRequestDAL
    {


        public static List<PendingRequestModel> getRejectedRequest(string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {

                List<PendingRequestModel> dataList = null;
                var RejectedRequestList = context.SP_GetRejectedRequest(EmployeeCode).ToList();
                if (RejectedRequestList != null)
                {

                    dataList = RejectedRequestList.Select(x => new PendingRequestModel()
                    {

                        BatchNo = x.BatchNo,
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerName = x.DealerName,
                        DepotName = x.DepotName,
                        DepotAddress = x.DepotAddress,
                        DepotCode = x.DepotCode,
                        RequestHeaderId = x.RequestHeaderId,
                        CurrentStatus = x.CurrentStatus,
                        CurrentStatus_Id = x.CurrentStatus_Id,
                        SKUCode = x.SKUCode,
                        SKUName = x.SKUName,
                        CreatedBy_EMP_CODE = x.CreatedBy,
                        CreatedBy = context.SP_LFGDetails(x.CreatedBy).FirstOrDefault().Emp_First_name,
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
