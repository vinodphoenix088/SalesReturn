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
    public class ApprovalMatrixDAL
    {



        public static List<ApprovalMatrixModal> getApprovalMatrixData()
        {

            using (var context = new SalesReturndbEntities())
            {

                List<ApprovalMatrixModal> objList = null;
                var dataList = context.TblApprovalMatrices.Where(x => x.IsActive == true).ToList();

                if (dataList != null)
                {
                    objList = dataList.Select(x => new ApprovalMatrixModal() {

                        Country = x.Country,
                        BUType = x.BUType,
                        Division = x.Division,
                        RequestType = x.RequestType,
                        SRV_Value = x.SRV_Value,
                        InvoiceAge = x.InvoiceAge,
                        ComplaintHandler = x.ComplaintHandler,
                        ComplaintManager = x.ComplaintManager,
                        LogisticsManager = x.LogisticsManager,
                        LogisticsHead = x.LogisticsHead,
                        SegmentHeadHRV = x.SegmentHeadHRV,
                        SegmentInvoiceAge = x.SegmentInvoiceAge,
                        Matrix_Id = x.Matrix_Id , 
                        VPName = x.VP,
                        President = x.President,

                    }).ToList();


                    return objList;

                }
                return objList;
            }
        }

        public static string UpdateFlowList(List<FlowMatrixModel> obj)
        {
            using (var context = new SalesReturndbEntities())
            {

              foreach(var ob in obj)
                {
                    try
                    {
                        var InsrtData = context.TblFlowMatrices.Where(x => x.IsActive == true && x.Id == ob.Id).FirstOrDefault();

                        InsrtData.Id = ob.Id;
                        InsrtData.ISC = ob.ISC;
                        InsrtData.LogisticsHead = ob.LogisticsHead;
                        InsrtData.President = ob.President;
                        InsrtData.RH = ob.RH;
                        InsrtData.SegmentHead = ob.SegmentHead;
                        InsrtData.VP = ob.VP;
                        InsrtData.ComplaintHandler = ob.ComplaintHandler;
                        InsrtData.ComplaintManager = ob.ComplaintManager;
                        InsrtData.ModifiedBy = ob.ModifiedBy;

                        context.Entry(InsrtData).State = EntityState.Modified;
                        context.SaveChanges();

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);

                    }
                    


                }

                
                return "Success";
            }
        }

        public static List<FlowMatrixModel> getFlowMatrixList()
        {
            using (var context = new SalesReturndbEntities())
            {

                var list = context.TblFlowMatrices.Where(x=>x.IsActive==true).ToList();
                //  List<FlowDataModel> FlowList = new List<FlowDataModel>();
                List<FlowMatrixModel> FlowList = new List<FlowMatrixModel>();
                var Reqtype = context.TblRequestTypes.Where(x => x.IsActive == true).ToList();
                if (list != null)
                {

                    FlowList = list.Select(x => new FlowMatrixModel()
                    {

                        Country = x.Country,
                        ComplaintHandler = x.ComplaintHandler,
                        ComplaintManager = x.ComplaintManager,
                        LogisticsHead = x.LogisticsHead,
                        IsActive = x.IsActive,
                        ISC = x.ISC,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        Options = x.Options,
                        President = x.President,
                        RequestType = x.RequestType,
                        RH = x.RH,
                        SegmentHead = x.SegmentHead,
                        VP = x.VP


                    }).ToList();

                    //foreach(var rt in Reqtype)
                    //{
                    //    var dataObj = list.Where(y => y.RequestType == rt.RequestType_Id).ToList();

                    //    FlowDataModel objm = new FlowDataModel();

                    //    objm.RequestType = rt.RequestType_Id;

                    //    FlowModel Fm = new FlowModel();
                    //    foreach (var dt in dataObj)
                    //    {


                    //        if(dt.Options == "Value")
                    //        {
                    //            Fm.Options = dt.Options;
                    //            Fm.FlowObjModel= dataObj.Where(y=>y.IsActive==true && y.Options== "Value").Select(x=> new FlowMatrixModel()
                    //            {

                    //                Country = x.Country,
                    //                ComplaintHandler = x.ComplaintHandler,
                    //                ComplaintManager = x.ComplaintManager,
                    //                LogisticsHead = x.LogisticsHead,
                    //                IsActive = x.IsActive,
                    //                ISC = x.ISC,
                    //                CreatedBy = x.CreatedBy,
                    //                CreatedDate = x.CreatedDate,
                    //                Id = x.Id,
                    //                ModifiedBy = x.ModifiedBy,
                    //                ModifiedDate = x.ModifiedDate,
                    //                Options = x.Options,
                    //                President = x.President,
                    //                RequestType = x.RequestType,
                    //                RH = x.RH,
                    //                SegmentHead = x.SegmentHead,
                    //                VP = x.VP


                    //            }).FirstOrDefault();
                    //        }
                    //        if (dt.Options == "Period")

                    //            Fm.Options = dt.Options;
                    //        Fm.FlowObjModel = dataObj.Where(y => y.IsActive == true && y.Options == "Period").Select(x => new FlowMatrixModel()
                    //        {

                    //            Country = x.Country,
                    //            ComplaintHandler = x.ComplaintHandler,
                    //            ComplaintManager = x.ComplaintManager,
                    //            LogisticsHead = x.LogisticsHead,
                    //            IsActive = x.IsActive,
                    //            ISC = x.ISC,
                    //            CreatedBy = x.CreatedBy,
                    //            CreatedDate = x.CreatedDate,
                    //            Id = x.Id,
                    //            ModifiedBy = x.ModifiedBy,
                    //            ModifiedDate = x.ModifiedDate,
                    //            Options = x.Options,
                    //            President = x.President,
                    //            RequestType = x.RequestType,
                    //            RH = x.RH,
                    //            SegmentHead = x.SegmentHead,
                    //            VP = x.VP


                    //        }).FirstOrDefault();

                    //        objm.FlowListModel.Add(Fm);
                    //    }



                    //    FlowList.Add(objm);


                    //}





                }

                return FlowList;
            }
        }

        public static List<BUList> getBUForCountry(string Country)
        {

            using (var context = new SalesReturndbEntities())
            {

                var list = context.sp_GetBUList(Country).ToList();
                List<BUList> bUList = null;
                if (list != null)
                {
                    bUList = list.Select(x => new BUList()
                    {
                        BUName = x.SBU_Name


                    }).ToList();



                    return bUList;
                }

                return bUList;
            }

        }

        public static List<DivisionList> getDivisionForBU(string BU)
        {

            using (var context = new SalesReturndbEntities())
            {

                var list = context.SP_Get_Division_For_BU(BU).ToList();
                List<DivisionList> DivisionList = null;
                if (list != null)
                {
                    DivisionList = list.Select(x => new DivisionList()
                    {
                        DivisionName = x.Dept_name



                    }).ToList();



                    return DivisionList;
                }

                return DivisionList;
            }

        }


        public static List<EmployeeCommanModal> GetSalesDirector(SearchModal Obj)
        {
            using (var context = new SalesReturndbEntities())
            {
                var list = context.SP_GetEmployeeForBU_Division_Country(Obj.BUName, Obj.DivisionName, Obj.CountryName).ToList();
                List<EmployeeCommanModal> empList = null;
                if (list != null)
                {
                    empList = list.Select(x => new EmployeeCommanModal()
                    {
                        EmployeeCode = x.EMP_CODE,
                        EmployeeName = x.Emp_First_name,



                    }).ToList();


                    return empList;
                }

                return empList;
            }
        }


        public static string UpdateApprovalMatrix(List<ApprovalMatrixModal> data)
        {
            using (var context = new SalesReturndbEntities())
            {

                using (TransactionScope transaction = new TransactionScope())
                {

                    foreach (var Obj in data)
                    {
                        //CommonDAL.CheckIfEmployeeExist(Obj.VPName);

                        var duplicateCheck = context.TblApprovalMatrices.Where(x => x.Matrix_Id == Obj.Matrix_Id && x.IsActive == true).FirstOrDefault();

                        if (duplicateCheck != null)
                        {
                            duplicateCheck.Country = Obj.Country;

                            duplicateCheck.BUType = Obj.BUType;
                            duplicateCheck.Division = Obj.Division;
                            duplicateCheck.RequestType = Obj.RequestType;

                            duplicateCheck.SRV_Value = Obj.SRV_Value;
                            duplicateCheck.InvoiceAge = Obj.InvoiceAge;

                            duplicateCheck.ComplaintHandler = Obj.ComplaintHandler;
                            duplicateCheck.ComplaintManager = Obj.ComplaintManager;

                            duplicateCheck.LogisticsManager = Obj.LogisticsManager;
                            duplicateCheck.LogisticsHead = Obj.LogisticsHead;

                            duplicateCheck.SegmentHeadHRV = Obj.SegmentHeadHRV;
                            duplicateCheck.SegmentInvoiceAge = Obj.SegmentInvoiceAge;

                            //duplicateCheck.VP = Obj.VPName;
                            //duplicateCheck.President = Obj.President;

                            duplicateCheck.ModifiedBy = Obj.CreatedBy;
                            duplicateCheck.ModifiedDate = DateTime.Now;



                            context.Entry(duplicateCheck).State = EntityState.Modified;
                            context.SaveChanges();

                        }
                        else
                        {
                            TblApprovalMatrix marix = new TblApprovalMatrix()
                            {
                                Country = Obj.Country,
                                BUType = Obj.BUType,
                                Division = Obj.Division,
                                RequestType = Obj.RequestType,
                                SRV_Value = Obj.SRV_Value,
                                InvoiceAge = Obj.InvoiceAge,
                                ComplaintHandler = Obj.ComplaintHandler,
                                ComplaintManager = Obj.ComplaintManager,
                                LogisticsManager = Obj.LogisticsManager,
                                LogisticsHead = Obj.LogisticsHead,
                                SegmentHeadHRV = Obj.SegmentHeadHRV,
                                SegmentInvoiceAge = Obj.SegmentInvoiceAge,

                                //VP = Obj.VPName,
                                //President = Obj.President,

                                IsActive = true,
                                CreatedBy = Obj.CreatedBy,
                                CreatedDate = DateTime.Now,

                            };

                            context.Entry(marix).State = EntityState.Added;
                            context.SaveChanges();

                        }
                    }

                    transaction.Complete();
                    transaction.Dispose();

                }

                return "Success : Approval matrix successfully updated.";
            }
        }

        public static string DeleteApprovalMatrixRow(int Id, string EmployeeCode)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {

                    var Check = context.TblApprovalMatrices.Where(x => x.IsActive == true && x.Matrix_Id == Id).FirstOrDefault();
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
                return "Success : Row succssfully deleted.";
            }


        }
    }
}

