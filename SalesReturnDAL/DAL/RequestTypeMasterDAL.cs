using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SalesReturnDAL.DAL
{
    public class RequestTypeMasterDAL
    {

        public static List<RequestTypeModal> GetRequestType()
        {
            using (var context = new SalesReturndbEntities())
            {

                int i;
                i = 0;
                List<RequestTypeModal> dataList = null;
                var objList = context.TblRequestTypes.Where(x => x.IsActive == true).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new RequestTypeModal()
                    {
                        RequestType = x.RequestType,
                        RequestType_Id = x.RequestType_Id


                    }).ToList();


                    return dataList;
                }
                return dataList;
            }


        }

        public static List<SP_GetBatchNo_Result> GetBatchNO(string SkuCode)
        {
            try
            {

            
            using (var context = new SalesReturndbEntities())
            {

                    List<SP_GetBatchNo_Result> objList = context.SP_GetBatchNo(SkuCode).ToList();

                    objList.Add(new SP_GetBatchNo_Result
                    {
                        Product_ID = null,
                        SKUCode = "Other",
                        SKUDescription = "Other",
                        Batch_No = "Other"
                    });
                    

                return objList;
            }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<SAPReasonModel> GetSAPReasonDetailByReason(int id)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<SAPReasonModel> dataList = new List<SAPReasonModel>();
                var objList = context.TblSAPReasonMasters.Where(x => x.IsActive == true && x.RequestTypeId == id).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new SAPReasonModel()
                    {



                        RequestTypeId = x.RequestTypeId,
                        SAPReasonID = x.SAPReasonID,
                        SubReasonID = x.SubReasonID,
                        SAPSubReasons = context.TblSAPSMasters.Where(y => y.IsActive == true && y.SAPID == x.SubReasonID).FirstOrDefault().SubReasons

                    }).ToList();


                    return dataList;
                }
                return dataList;
            }
        }

        public static InvoiceDetailListModel GetInvoiceDataRequest(InvoiceDataModel obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                InvoiceDetailListModel inv = new InvoiceDetailListModel();
                var data = context.SP_GetInvoiceDetailList(obj.InvoiceNo, obj.SKUCode, obj.BatchNo).FirstOrDefault();
                if(data== null)
                {
                    return null;
                }else
                {
                    inv.InvoiceNumber = data.InvoiceNumber;
                    inv.InvoiceQuantity = data.InvoiceQuantity;
                    inv.ReceivedQTY = data.ReceivedQTY;
                    inv.SRVQuantity = data.SRVQuantity;
                    inv.Status_Id = data.Status_Id;
                    inv.RemainingQTY = data.RemainingQTY;

                    return inv;

                }

               

            }
        }

        public static DateTime GetLastThreeMonthDate()
        {
            using (var context = new SalesReturndbEntities())
            {
                var dat = DateTime.Now.AddMonths(-3);

                return dat;

            }
        }

        public static List<PendingRequestModel> getRequestforBillingClosure(string employeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<PendingRequestModel> dataList = null;
                var PendingRequestList = context.SP_GetPendingSRVClosureRequest(employeeCode).ToList();
                if (PendingRequestList != null)
                {

                    dataList = PendingRequestList.Select(x => new PendingRequestModel()
                    {

                        BatchNo = x.BatchNo,
                        DealerAddress = x.DealerAddress,
                        DealerCode = x.DealerCode,
                        DealerName = x.DealerName,
                        DepotName = x.DepotName,
                        DepotAddress = x.DepotAddress,
                        DepotCode = x.DepotCode,
                        FutureStatus = x.FutureStatus,
                        RequestHeaderId = x.RequestHeaderId,
                        FutureStatus_Id = x.FutureStatus_Id,
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

            };
        }

        public static List<RequestTypeModal> GetSubReasonList(int id)
        {
            using (var context = new SalesReturndbEntities())
            {
                List<RequestTypeModal> dataList = null;
                var objList = context.TblSalesReasonMasters.Where(x => x.IsActive == true && x.RequestTypeId == id).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new RequestTypeModal()
                    {

                        RequestType_Id = x.RequestTypeId,
                        salesReasonId = x.SalesReason_Id,
                        SubReason = x.SubReason


                    }).ToList();


                    return dataList;
                }
                return dataList;
            }
        }

        public static string SaveRequestType(List<RequestTypeModal> list)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    foreach (var obj in list)
                    {
                        var duplicateCheck = context.TblRequestTypes.Where(x => x.IsActive == true && x.RequestType_Id == obj.RequestType_Id).FirstOrDefault();

                        if (duplicateCheck != null)
                        {
                            var duplicateValueCheck = context.TblRequestTypes.Where(x => x.RequestType.Equals(obj.RequestType) && x.IsActive == true && x.RequestType_Id != obj.RequestType_Id).FirstOrDefault();

                            if (duplicateValueCheck == null)
                            {
                                //update
                                duplicateCheck.RequestType = obj.RequestType;
                                duplicateCheck.ModifiedBy = obj.EmployeeCode;
                                duplicateCheck.ModifiedDate = DateTime.Now;


                                context.Entry(duplicateCheck).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            var duplicateValueCheck = context.TblRequestTypes.Where(x => x.RequestType.Equals(obj.RequestType) && x.IsActive == true).FirstOrDefault();

                            if (duplicateValueCheck == null)
                            {
                                //new entry
                                TblRequestType RequestType = new TblRequestType()
                                {
                                    RequestType = obj.RequestType,
                                    IsActive = true,
                                    CreatedBy = obj.EmployeeCode,
                                    CreatedDate = DateTime.Now,

                                };
                                context.Entry(RequestType).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();
                            }
                        }



                    }
                    transaction.Complete();
                    transaction.Dispose();
                }
                return "Success : Request types succssfully saved";
            }


        }



        public static string DeleteRequestType(int Id, string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblRequestTypes.Where(x => x.IsActive == true && x.RequestType_Id == Id).FirstOrDefault();
                    if (Check != null)
                    {
                        //update
                        Check.IsActive = false;
                        Check.ModifiedBy = EmployeeCode;
                        Check.ModifiedDate = DateTime.Now;


                        context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        transaction.Complete();
                        transaction.Dispose();

                    }

                }
                return "Success : Request types succssfully deleted.";
            }


        }
    }
}
