using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnDAL.DAL
{
    public class ApprovedRequestDAL
    {
        public static List<PendingRequestModel> getApprovedRequest(string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<PendingRequestModel> dataList = null;
                var ApprovedRequestList = context.SP_GetApprovedRequest(EmployeeCode).ToList();

                if (ApprovedRequestList != null)
                {
                    dataList = ApprovedRequestList.Select(x => new PendingRequestModel()
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
                        SKUName = x.SKUName ,
                        CreatedBy_EMP_CODE = x.CreatedBy,
                        //CreatedBy = context.SP_LFGDetails(x.CreatedBy).FirstOrDefault().Emp_First_name, //
                        CreatedBy = x.EmpName,
                        CreatedDate = x.CreatedDate ,
                        //TotalSRV = context.tblRequestDtls.Where(o=>o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p=>p.SRVValue).Value,
                        TotalSRV = x.SRVValue,
                        RequestTypeOption = x.SKUCode,// for access data behalf on skucode

                    }).ToList();

                    return dataList;
                }
                return dataList;

            }

        }
    }
}
