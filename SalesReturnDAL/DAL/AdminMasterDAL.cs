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
    public class AdminMasterDAL
    {
        public static List<AdminMasterModal> getAdminList()
        {
            using (var context = new SalesReturndbEntities())
            {
                var dataList = context.TblAdminMasters.Where(x => x.IsActive == true).ToList();

                List<AdminMasterModal> objList = null;
                if (dataList != null)
                {
                    objList = dataList.Select(x => new AdminMasterModal()
                    {
                        Admin_Id = x.Admin_Id,
                        EmployeeCode = x.EmployeeCode

                    }).ToList();


                    return objList;

                }
                return objList;
            }
        }

        public static List<EmployeeMasterModel> getEmployeeList()
        {
            using (var context = new SalesReturndbEntities())
            {
                var dataList = context.TblEmployeeMasters.Where(x => x.IsActive == true).ToList();

                List<EmployeeMasterModel> objList = new List<EmployeeMasterModel>();
                if (dataList != null)
                {
                    objList = dataList.Select(x => new EmployeeMasterModel()
                    {
                        CommercialCode = x.CommercialCode,
                        CommercialHead=x.CommercialHead,
                        ComplaintHandler = x.ComplaintHandler,
                        ComplaintManager = x.ComplaintManager,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        CSO = x.CSO,
                        Depotcode = x.Depotcode,
                        DepotName = x.DepotName,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        ISC = x.ISC,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        LogisticsHead = x.LogisticsHead,
                    }).ToList();


                    return objList;

                }
                return objList;
            }
        }

        public static string DeleteList(int id, string employeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.Id == id).FirstOrDefault();
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
                return "Success : succssfully deleted.";
            }
        }

        public static string UpdateEmployeeList(List<EmployeeMasterModel> list)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    List<EmployeeMasterModel> lst = new List<EmployeeMasterModel>();

                    foreach (var obj in list)
                    {

                        var duplicateCheck = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.Id == obj.Id).FirstOrDefault();
                        if (duplicateCheck != null)
                        {
                            var check = lst.Where(x => x.IsActive == true && x.DepotName == obj.DepotName).FirstOrDefault();

                            if (check == null)
                            {
                                //update
                                duplicateCheck.DepotName = obj.DepotName;
                                duplicateCheck.CommercialCode = obj.CommercialCode;
                                duplicateCheck.CommercialHead = obj.CommercialHead;
                                duplicateCheck.ComplaintHandler = obj.ComplaintHandler;
                                duplicateCheck.ComplaintManager = obj.ComplaintManager;
                                duplicateCheck.CSO = obj.CSO;
                                duplicateCheck.Depotcode = obj.Depotcode;
                                duplicateCheck.ISC = obj.ISC;
                                duplicateCheck.LogisticsHead = obj.LogisticsHead;
                                duplicateCheck.ModifiedBy = obj.CreatedBy;
                                duplicateCheck.ModifiedDate = DateTime.Now;


                                context.Entry(duplicateCheck).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                lst.Add(obj);

                            }
                            else
                            {
                                return "Duplicate value not allowed";
                            }
                        }
                        else
                        {
                            var duplicateCheck1 = context.TblEmployeeMasters.Where(x => x.IsActive == true && x.DepotName == obj.DepotName).FirstOrDefault();
                            if (duplicateCheck1 == null)
                            {
                                //new entry
                                TblEmployeeMaster ReasonMaster = new TblEmployeeMaster()
                                {
                                    DepotName = obj.DepotName,
                                    CommercialCode = obj.CommercialCode,
                                    CommercialHead = obj.CommercialHead,
                                    ComplaintHandler = obj.ComplaintHandler,
                                    ComplaintManager = obj.ComplaintManager,
                                    CSO = obj.CSO,
                                    Depotcode = obj.Depotcode,
                                    ISC = obj.ISC,
                                    LogisticsHead = obj.LogisticsHead,
                                    IsActive = true,
                                    CreatedBy = obj.CreatedBy,
                                    CreatedDate = DateTime.Now,

                                };
                                context.Entry(ReasonMaster).State = System.Data.Entity.EntityState.Added;
                                context.SaveChanges();

                                lst.Add(obj);

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
                return "Success : succssfully saved";
            }
        }

        public static List<CCStackHolder> GetCCStackHolderDetail()
        {
            using (var context = new SalesReturndbEntities())
            {
                var dataList = context.spGetCCStackHolderDetail().Where(x => x.IsDeleted == false).ToList();

                List<CCStackHolder> objList = new List<CCStackHolder>();
                if (dataList != null)
                {
                    objList = dataList.Select(x => new CCStackHolder()
                    {
                        Catalyst = x.Catalyst,
                        ComplaintHandler = x.ComplaintHandler,
                        ComplaintManager = x.ComplaintManager,
                        CreatedBy = x.CreatedBy,
                        IsDeleted = x.IsDeleted,
                        ComplaintStakeHolders_ID = x.ComplaintStakeHolders_ID,
                        CreationDate = x.CreationDate,
                        LocalTechnical = x.LocalTechnical,
                        Manager = x.Manager,
                        SBU_Name = x.SBU_Name,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,

                    }).ToList();


                    return objList;

                }
                return objList;
            }
        }

        public static string UpdateAdminList(List<AdminMasterModal> data)
        {
            using (var context = new SalesReturndbEntities())
            {

                using (TransactionScope transaction = new TransactionScope())
                {

                    foreach (var Obj in data)
                    {
                        //CommonDAL.CheckIfEmployeeExist(Obj.VPName);



                        var duplicateCheck = context.TblAdminMasters.Where(x => x.Admin_Id == Obj.Admin_Id && x.IsActive == true).FirstOrDefault();

                        if (duplicateCheck != null)
                        {
                            var duplicateCheckValue = context.TblAdminMasters.Where(x => x.EmployeeCode == Obj.EmployeeCode && x.Admin_Id != Obj.Admin_Id && x.IsActive == true).FirstOrDefault();
                            if (duplicateCheckValue == null)
                            {
                                duplicateCheck.EmployeeCode = Obj.EmployeeCode;

                                duplicateCheck.ModifiedBy = Obj.CreatedBy;
                                duplicateCheck.ModifiedDate = DateTime.Now;



                                context.Entry(duplicateCheck).State = EntityState.Modified;
                                context.SaveChanges();

                            }
                        }
                        else
                        {
                            var duplicateCheckValue = context.TblAdminMasters.Where(x => x.EmployeeCode == Obj.EmployeeCode && x.IsActive == true).FirstOrDefault();
                            if (duplicateCheckValue == null)
                            {
                                TblAdminMaster marix = new TblAdminMaster()
                                {
                                    EmployeeCode = Obj.EmployeeCode,
                                    IsActive = true,
                                    CreatedBy = Obj.CreatedBy,
                                    CreatedDate = DateTime.Now,

                                };

                                context.Entry(marix).State = EntityState.Added;
                                context.SaveChanges();
                            }

                        }
                    }

                    transaction.Complete();
                    transaction.Dispose();

                }

                return "Success : Admin list successfully updated.";
            }
        }

        public static string DeleteAdminList(int Id, string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblAdminMasters.Where(x => x.IsActive == true && x.Admin_Id == Id).FirstOrDefault();
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
                return "Success : admin succssfully deleted.";
            }


        }
    }
}

