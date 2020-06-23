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
    
    public partial class tblRequestDtl
    {
        public long Id { get; set; }
        public long RequestHeaderId { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<bool> ReadyToProvideGST { get; set; }
        public string InvoiceUpload { get; set; }
        public string SKUName { get; set; }
        public string SKUCode { get; set; }
        public string BatchNo { get; set; }
        public Nullable<decimal> InvoiceQuantity { get; set; }
        public Nullable<decimal> SRVQuantity { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> PackSize { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public Nullable<decimal> SRVValue { get; set; }
        public Nullable<int> CCNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
