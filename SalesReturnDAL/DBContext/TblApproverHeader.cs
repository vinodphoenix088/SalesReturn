//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesReturnDAL.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblApproverHeader
    {
        public int Approver_Id { get; set; }
        public int Request_Id { get; set; }
        public Nullable<int> Active_Role { get; set; }
        public string AssignedTo { get; set; }
        public int Status_Id { get; set; }
        public Nullable<int> Requested_Role { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
