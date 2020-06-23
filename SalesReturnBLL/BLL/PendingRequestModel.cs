using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesReturnBLL.BLL
{
    public class PendingRequestModel
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
        public string FutureStatus { get; set; }
        public string CurrentStatus { get; set; }
        public long? FutureStatus_Id { get; set; }
        public int? CurrentStatus_Id { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedBy_EMP_CODE { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? TotalSRV { get; set; }
        public string RequestTypeOption { get; set; }
    }

    public class RecommendRequestObj
    {
        public string EmployeeCode { get; set; }
        public string Country { get; set; }
        public int CurrentStatus_Id { get; set; }
        public int FutureStatus_Id { get; set; }
        public int Request_Id { get; set; }
        public int RequestType_Id { get; set; }

        public int Active_Role { get; set; }
        public int Requested_Role { get; set; }
        public string Remarks { get; set; }


    }
    public class CloseRequestObjForStg_4
    {
        public RequestDetailObj_Render Request { get; set; }

        public string EmployeeCode { get; set; }
        public string Country { get; set; }
        public int CurrentStatus_Id { get; set; }
        public int FutureStatus_Id { get; set; }
        public int Request_Id { get; set; }
        public int RequestType_Id { get; set; }

        public int Active_Role { get; set; }
        public int Requested_Role { get; set; }
        public string Remarks { get; set; }
    }
}
