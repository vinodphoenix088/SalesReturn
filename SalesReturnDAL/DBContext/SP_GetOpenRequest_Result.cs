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
    
    public partial class SP_GetOpenRequest_Result
    {
        public long RequestHeaderId { get; set; }
        public Nullable<long> DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string DealerAddress { get; set; }
        public Nullable<long> DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }
        public string DepotAddress { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string BatchNo { get; set; }
        public string CurrentStatus { get; set; }
        public int CurrentStatus_Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
