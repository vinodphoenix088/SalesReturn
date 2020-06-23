using SalesReturnBLL.BLL;
using SalesReturnDAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SalesReturnDAL.DAL
{
    public class ReasonMasterDAL
    {

        public static List<ReasonMasterModal> GetReasonMaster()
        {
            using (var context = new SalesReturndbEntities())
            {
                List<ReasonMasterModal> dataList = null;
                var objList = context.TblReasonMasters.Where(x => x.IsActive == true).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new ReasonMasterModal()
                    {
                        Reason = x.Reason,
                        ReasonMaster_Id = x.ReasonMaster_Id


                    }).ToList();


                    return dataList;
                }
                return dataList;
            }


        }

        public static List<SalesReasonMasterModel> GetReasonDetail()
        {
            using (var context = new SalesReturndbEntities())
            {
                List<SalesReasonMasterModel> dataList = new List<SalesReasonMasterModel>();
                var objList = context.TblSalesReasonMasters.Where(x => x.IsActive == true).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new SalesReasonMasterModel()
                    {
                        SalesReason_Id = x.SalesReason_Id,
                        RequestTypeId = x.RequestTypeId,
                        SubReason = x.SubReason,



                    }).ToList();


                    return dataList;
                }
                return dataList;
            }
        }

        public static string UpdateSAPSubReason(List<SAPReasonModel> list)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    List<TblSAPReasonMaster> lst = new List<TblSAPReasonMaster>();

                    foreach (var obj in list)
                    {

                        var duplicateCheck = context.TblSAPReasonMasters.Where(x => x.IsActive == true && x.SAPReasonID == obj.SAPReasonID).FirstOrDefault();
                        if (duplicateCheck != null)
                        {
                            var check = lst.Where(x => x.IsActive == true && x.RequestTypeId == obj.RequestTypeId
                                && x.SubReasonID == obj.SubReasonID).FirstOrDefault();

                            if (check == null)
                            {
                                //update
                                duplicateCheck.RequestTypeId = obj.RequestTypeId;
                                duplicateCheck.SubReasonID = obj.SubReasonID;
                                duplicateCheck.ModifiedBy = obj.CreatedBy;
                                duplicateCheck.ModifiedDate = DateTime.Now;


                                context.Entry(duplicateCheck).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                lst.Add(duplicateCheck);

                            }
                            else
                            {
                                return "Duplicate value not allowed";
                            }
                        }
                        else
                        {
                            var duplicateCheck1 = context.TblSAPReasonMasters.Where(x => x.IsActive == true && x.RequestTypeId == obj.RequestTypeId
                                && x.SubReasonID == obj.SubReasonID).FirstOrDefault();
                            if (duplicateCheck1 == null)
                            {
                                //new entry
                                TblSAPReasonMaster ReasonMaster = new TblSAPReasonMaster()
                                {
                                    RequestTypeId = obj.RequestTypeId,
                                    SubReasonID = obj.SubReasonID,
                                   
                                    IsActive = true,
                                    CreatedBy = obj.CreatedBy,
                                    CreatedDate = DateTime.Now,

                                };
                                context.Entry(ReasonMaster).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();

                                lst.Add(duplicateCheck);

                            }
                            else
                            {
                                return "Duplicate value not allowed";
                            }
                        }



                    }
                    transaction.Complete();
                    transaction.Dispose();
                }
                return "Success : Reasons succssfully saved";
            }
        }

        public static string DeleteSAPSubReason(int id, string employeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblSAPReasonMasters.Where(x => x.IsActive == true && x.SAPReasonID == id).FirstOrDefault();
                    if (Check != null)
                    {
                        //update
                        Check.IsActive = false;
                        Check.ModifiedBy = employeeCode;
                        Check.ModifiedDate = DateTime.Now;


                        context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        transaction.Complete();
                        transaction.Dispose();

                    }

                }
                return "Success : Reason succssfully deleted.";
            }
        }

        public static List<SAPSMasterList> GetSAPSubReasonMasterList()
        {
            using (var context = new SalesReturndbEntities())
            {
                List<SAPSMasterList> dataList = new List<SAPSMasterList>();
                var objList = context.TblSAPSMasters.Where(x => x.IsActive == true).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new SAPSMasterList()
                    {
                        SAPID = x.SAPID,
                        SubReasons = x.SubReasons,
                        SAPCode = x.SAPCode,
                        Process=x.Process



                    }).ToList();


                    return dataList;
                }
                return dataList;
            }
        }

        public static List<SAPReasonModel> GetSAPReasonDetail()
        {
            using (var context = new SalesReturndbEntities())
            {
                List<SAPReasonModel> dataList = new List<SAPReasonModel>();
                var objList = context.TblSAPReasonMasters.Where(x => x.IsActive == true).ToList();
                if (objList != null)
                {
                    dataList = objList.Select(x => new SAPReasonModel()
                    {
                        SAPReasonID= x.SAPReasonID,
                        RequestTypeId = x.RequestTypeId,
                        SubReasonID = x.SubReasonID,



                    }).ToList();


                    return dataList;
                }
                return dataList;
            }
        }

        public static string DeleteReasonRow(int id, string employeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblSalesReasonMasters.Where(x => x.IsActive == true && x.SalesReason_Id == id).FirstOrDefault();
                    if (Check != null)
                    {
                        //update
                        Check.IsActive = false;
                        Check.ModifiedBy = employeeCode;
                        Check.ModifiedDate = DateTime.Now;


                        context.Entry(Check).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        transaction.Complete();
                        transaction.Dispose();

                    }

                }
                return "Success : Reason succssfully deleted.";
            }
        }

        public static string UpdateReason(List<SalesReasonMasterModel> list)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    List<TblSalesReasonMaster> lst = new List<TblSalesReasonMaster>();

                    foreach (var obj in list)
                    {
                      
                        var duplicateCheck = context.TblSalesReasonMasters.Where(x => x.IsActive == true && x.SalesReason_Id == obj.SalesReason_Id).FirstOrDefault();
                        if (duplicateCheck != null)
                        {
                            var check = lst.Where(x => x.IsActive == true && x.RequestTypeId == obj.RequestTypeId
                                && x.SubReason == obj.SubReason).FirstOrDefault();
                         
                            if (check == null)
                            {
                                //update
                                duplicateCheck.RequestTypeId = obj.RequestTypeId;
                                duplicateCheck.SubReason = obj.SubReason;
                                duplicateCheck.ModifiedBy = obj.CreatedBy;
                                duplicateCheck.ModifiedDate = DateTime.Now;


                                context.Entry(duplicateCheck).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                lst.Add(duplicateCheck);

                            }
                            else
                            {
                                return "Duplicate value not allowed";
                            }
                        }
                        else
                        {
                            var duplicateCheck1 = context.TblSalesReasonMasters.Where(x => x.IsActive == true && x.RequestTypeId == obj.RequestTypeId
                                && x.SubReason == obj.SubReason).FirstOrDefault();
                            if (duplicateCheck1 == null)
                            {
                                //new entry
                                TblSalesReasonMaster ReasonMaster = new TblSalesReasonMaster()
                                {
                                    RequestTypeId = obj.RequestTypeId,
                                    SubReason = obj.SubReason,
                                    IsActive = true,
                                    CreatedBy = obj.CreatedBy,
                                    CreatedDate = DateTime.Now,

                                };
                                context.Entry(ReasonMaster).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();

                                lst.Add(duplicateCheck);

                            }
                            else
                            {
                                return "Duplicate value not allowed";
                            }
                        }

                        

                    }
                    transaction.Complete();
                    transaction.Dispose();
                }
                return "Success : Reasons succssfully saved";
            }
        }

        public static string SaveReason(List<ReasonMasterModal> list)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    foreach (var obj in list)
                    {
                        var duplicateCheck = context.TblReasonMasters.Where(x => x.IsActive == true && x.ReasonMaster_Id == obj.ReasonMaster_Id).FirstOrDefault();
                        
                        if (duplicateCheck != null)
                        {
                            //update
                            duplicateCheck.Reason = obj.Reason;
                            duplicateCheck.ModifiedBy = obj.EmployeeCode;
                            duplicateCheck.ModifiedDate = DateTime.Now;


                            context.Entry(duplicateCheck).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        else
                        {

                            //new entry
                            TblReasonMaster ReasonMaster = new TblReasonMaster()
                            {
                                Reason = obj.Reason,
                                IsActive = true,
                                CreatedBy = obj.EmployeeCode,
                                CreatedDate = DateTime.Now,

                            };
                            context.Entry(ReasonMaster).State = System.Data.Entity.EntityState.Added;
                            context.SaveChanges();
                        }



                    }
                    transaction.Complete();
                    transaction.Dispose();
                }
                return "Success : Reasons succssfully saved";
            }


        }



        public static string DeleteReason(int Id, string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblReasonMasters.Where(x => x.IsActive == true && x.ReasonMaster_Id == Id).FirstOrDefault();
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
                return "Success : Reason succssfully deleted.";
            }


        }

    }
}
