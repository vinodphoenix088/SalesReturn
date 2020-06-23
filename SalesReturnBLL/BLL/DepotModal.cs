using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class DepotModal
    {
        public string DepotAddress { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public long DepotId { get; set; }
    }
    public class DealerModal
    {
        public string DealerAddress { get; set; }
        public string DealerCode { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class SKUClass
    {

        public long? Product_ID { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string SKUDescription { get; set; }
        public Nullable<decimal> PackSize { get; set; }
        public string Unit { get; set; }
        public string Batch_No { get; set; }

        public string BatchNoText { get; set; }
        public Nullable<System.DateTime> Manufacturing_Date { get; set; }
        public Nullable<System.DateTime> MGfDate { get; set; }
        public string Shelf_Life { get; set; }
    }

    public class CustomerComplaintModal
    {

        public long Complaint_ID { get; set; }
        public int ComplaintNumber { get; set; }
        public string ComplaintDesc { get; set; }
        public Nullable<decimal> ComplaintQty { get; set; }
    }
}
