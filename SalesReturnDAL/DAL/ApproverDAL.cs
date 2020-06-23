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
    public class ApproverDAL
    {
        public static void UpdateRequestStatus(bool isNew, int Request_Id, string CreatedBy, string AssignedTo, int Active_Role, int Requested_Role, int CurrentStatus_Id, int FutureStatus_Id, string Remark)
        {
            using (var context = new SalesReturndbEntities())
            {
                using (TransactionScope T2 = new TransactionScope())
                {
                    int Approver_Id = 0;

                    if (isNew == true)
                    {
                        TblApproverHeader header = new TblApproverHeader()
                        {
                            Active_Role = Active_Role,
                            AssignedTo = AssignedTo,
                            Requested_Role = Requested_Role,
                            Request_Id = Request_Id ,
                            Status_Id = CurrentStatus_Id,
                            IsActive = true,
                            CreatedBy = CreatedBy,
                            CreatedDate = DateTime.Now,
                        };

                        context.Entry(header).State = EntityState.Added;
                        context.SaveChanges();

                        Approver_Id = header.Approver_Id;
                    }
                    else
                    {
                        var detail = context.TblApproverHeaders.Where(x => x.Request_Id == Request_Id).FirstOrDefault();

                        detail.Active_Role = Active_Role;
                        detail.AssignedTo = AssignedTo;
                        detail.Status_Id = CurrentStatus_Id;
                        detail.Requested_Role = Requested_Role;

                        detail.ModifiedBy = CreatedBy;
                        detail.ModifiedDate = DateTime.Now;
                        detail.IsActive = true;

                        context.Entry(detail).State = EntityState.Modified;
                        context.SaveChanges();

                        Approver_Id = detail.Approver_Id;
                    }

                    TblApproverDetail Appdetail = new TblApproverDetail()
                    {
                        Approver_Id = Approver_Id,
                        AssignedTo = AssignedTo,
                        Remark = Remark,
                        Role_Id = Active_Role,
                        Status_Id = CurrentStatus_Id,
                        CreatedBy = CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    context.Entry(Appdetail).State = EntityState.Added;
                    context.SaveChanges();

                    TblFutureStatu statu = new TblFutureStatu()
                    {
                        Request_ID = Request_Id,
                        EmployeeCode = AssignedTo,
                        Role = Requested_Role,
                        Status = FutureStatus_Id,
                        CreatedBy = CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };

                    context.Entry(statu).State = EntityState.Added;
                    context.SaveChanges();

                    T2.Complete();
                    T2.Dispose();
                }
            }
        }
    }
}
