using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;

namespace SalesReturnDAL.DAL
{
    public class DashboardDAL
    {
        public static int GetPendingRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetPendingRequest(empCode).Count();
                return Count;
            }
        }

        public static int GetInprocessRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetInprocessRequest(empCode).Count();
                return Count;
            }
        }

        public static int GetRejectRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetRejectedRequest(empCode).Count();
                return Count;
            }
        }

        public static int GetClosedRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetClosedRequest(empCode).Count();
                return Count;
            }
        }

        public static int GetSalesTotalRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetTotalRequest(empCode).Where(x => x.SKUCode == "S").Count();
                return Count;
            }
        }

        public static int GetTotalRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetTotalRequest(empCode).Count();
                return Count;
            }
        }

        public static int GetApprovedRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetApprovedRequest(empCode).Count();
                return Count;
            }
        }

        public static List<PendingRequestModel> GetOpenRequest(string empCode)
        {
            List<PendingRequestModel> List = new List<PendingRequestModel>();
            using (var context = new SalesReturndbEntities())
            {
                var data = context.SP_GetOpenRequest(empCode).ToList();
                if (data.Count != 0)
                {
                    List = data.Select(x => new PendingRequestModel()
                    {
                        BatchNo = x.BatchNo,
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerName = x.DealerName,
                        DepotName = x.DealerName,
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

                        TotalSRV = context.tblRequestDtls.Where(o => o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p => p.SRVValue),
                    }).ToList();
                }
                return List;
            }
        }

        public static List<PendingRequestModel> GetClosedRequest(string empCode)
        {
            List<PendingRequestModel> List = new List<PendingRequestModel>();
            using (var context = new SalesReturndbEntities())
            {
                var data = context.SP_GetClosedRequest(empCode).ToList();
                if (data.Count != 0)
                {
                    List = data.Select(x => new PendingRequestModel()
                    {
                        BatchNo = x.BatchNo,
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerName = x.DealerName,
                        DepotName = x.DealerName,
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
                        TotalSRV = context.tblRequestDtls.Where(o => o.RequestHeaderId == x.RequestHeaderId && o.IsActive == true).Sum(p => p.SRVValue).Value
                    }).ToList();
                }
                return List;
            }
        }

        public static int GetPendingBillingClosureRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetPendingSRVClosureRequest(empCode).Count();
                return Count;
            }
        }

        public static int GetPendingSRVBillingClosureRequestCount(string empCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                int Count = context.SP_GetPendingRequestForDepot(empCode).Count();
                return Count;
            }
        }
    }
}
