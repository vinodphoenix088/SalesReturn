using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class RequestTypeModal
    {
        public int RequestType_Id { get; set; }
        public string RequestType { get; set; }
        public string EmployeeCode { get; set; }
        public string SubReason { get; set; }

        public int salesReasonId { get; set; }

    }

    public class InvoiceDataModel
    {
        public string InvoiceNo { get; set; }
        public string SKUCode { get; set; }
        public string BatchNo { get; set; }


    }

    public partial class InvoiceDetailListModel
    {
        public string InvoiceNumber { get; set; }
        public Nullable<decimal> InvoiceQuantity { get; set; }
        public Nullable<decimal> SRVQuantity { get; set; }
        public Nullable<int> ReceivedQTY { get; set; }
        public Nullable<decimal> RemainingQTY { get; set; }
        public int Status_Id { get; set; }
    }
}
