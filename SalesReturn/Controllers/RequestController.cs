using Newtonsoft.Json;
using SalesReturnBLL.BLL;
using SalesReturnDAL.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace SalesReturn.Controllers
{
    public class RequestController : ApiController
    {
        [HttpGet]
        public Dummy_RequestDetailObj getDummyRequestDetailObj()
        {
            Dummy_RequestDetailObj Obj = new Dummy_RequestDetailObj();
            Obj.BatchList = new List<BatchList>();
            Obj.RequestDetail = new List<RequestDetailArray>();
            Obj.RequestDetail.Add(new RequestDetailArray
            {
                BatchNo = null,
                InvoiceNo = null,
                InvoiceDate = null,
                InvoiceQuantity = null,
                PackSize = null,
                Remarks = null,
                SRVValue = null
            });
            return Obj;
        }

        [HttpGet]
        public List<DepotModal> GetDepotList()
        {
            return RequestDal.GetDepotList();
        }

        [HttpGet]
        public List<DepotModal> GetDepotListBasedOnEmpCode(string EmpCode)
        {
            return RequestDal.GetDepotList(EmpCode);
        }

        [HttpGet]
        public List<DealerModal> GetDealerList(long Depot_Id)
        {
            return RequestDal.GetDealerList(Depot_Id);
        }
        [HttpGet]

        public List<SKUClass> GetSKUCode()
        {
            return RequestDal.GetSKUCode();
        }
        [HttpGet]
        public List<CustomerComplaintModal> GetCustomerComplaintList()
        {
            return RequestDal.GetCustomerComplaintList();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveAsDraft()
        {
            //string[] location = new string[2];
            string fileName = "";
            RequestDetailObj Obj;
            string StrReturn = "";
            long RequestHeader_Id = 0;
            // Check if the request contains multipart/form-data.
            string strUniqueId = System.Guid.NewGuid().ToString();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["RootImages"]);


            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            if (result.FormData["data"] == null)
            { }
            var model = result.FormData["data"];
            Obj = JsonConvert.DeserializeObject<RequestDetailObj>(model);


            var IsDisabled = true;
            if (Obj.RequestHeader_Id != null)
            {
                IsDisabled = RequestDal.DisableAllAlreadySavedRequestDetail_SavedAsDraft(Obj, Obj.RequestHeader_Id, Obj.EmployeeCode, ref StrReturn);
            }

            List<int> List_Detail_Id = null;

            if (IsDisabled == true)
            {
                List_Detail_Id = RequestDal.saveRequestAsDraft(Obj, ref StrReturn, ref RequestHeader_Id);

            }

            if (List_Detail_Id != null && List_Detail_Id.Count > 0)
            {
                string[] location = new string[result.FileData.Count];
                List<string> FilePath = new List<string>();
                try
                {
                    for (int i = 0; i < result.FileData.Count; i++)
                    {

                        fileName = result.FileData[i].Headers.ContentDisposition.FileName;
                        string fileserial = result.FileData[i].Headers.ContentDisposition.Name.Trim('"');
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);

                            Match regex = Regex.Match(root, @"(.+) \((\d+)\)\.\w+");

                            if (regex.Success)
                            {
                                fileName = regex.Groups[1].Value;

                            }
                        }
                        fileName = RemoveSpecialCharacters(fileName);
                        fileName = NewNumber().ToString() + fileName;
                        //Storing file to temporary location in p   roject
                        try
                        {
                            //string fileType = Path.GetExtension(fileName);

                            string filename = System.Configuration.ConfigurationManager.AppSettings["RootImages"] + "/" + fileName;

                            File.Move(result.FileData[i].LocalFileName, Path.Combine(root, fileName));

                            location[i] = filename;

                            RequestDal.UploadInvoicesIntoTable(List_Detail_Id[i], filename, Obj.EmployeeCode);

                        }

                        catch (Exception e)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, e.Message);
                        }

                    }

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Error : " + e.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, RequestHeader_Id);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> saveRequest()
        {
            string fileName = "";
            RequestDetailObj Obj;
            string StrReturn = "";
            string strUniqueId = System.Guid.NewGuid().ToString();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["RootImages"]);

            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                if (result.FormData["data"] == null)
                { }
                var model = result.FormData["data"];
                Obj = JsonConvert.DeserializeObject<RequestDetailObj>(model);

                var IsDisabled = true;
                if (Obj.RequestHeader_Id != null)
                {
                    IsDisabled = RequestDal.DisableAllAlreadySavedRequestDetail(Obj, Obj.RequestHeader_Id, Obj.EmployeeCode, ref StrReturn);
                }

                List<int> List_Detail_Id = null;

                if (IsDisabled == true)
                {
                    List_Detail_Id = RequestDal.saveRequest(Obj, ref StrReturn);
                }

                if (List_Detail_Id != null)
                {
                    string[] location = new string[result.FileData.Count];
                    List<string> FilePath = new List<string>();
                    try
                    {
                        for (int i = 0; i < result.FileData.Count; i++)
                        {
                            fileName = result.FileData[i].Headers.ContentDisposition.FileName;
                            string fileserial = result.FileData[i].Headers.ContentDisposition.Name.Trim('"');
                            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                            {
                                fileName = fileName.Trim('"');
                            }
                            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                            {
                                fileName = Path.GetFileName(fileName);

                                Match regex = Regex.Match(root, @"(.+) \((\d+)\)\.\w+");

                                if (regex.Success)
                                {
                                    fileName = regex.Groups[1].Value;
                                }
                            }
                            fileName = RemoveSpecialCharacters(fileName);
                            fileName = NewNumber().ToString() + fileName;
                            //Storing file to temporary location in p   roject
                            try
                            {
                                //string fileType = Path.GetExtension(fileName);

                                string filename = System.Configuration.ConfigurationManager.AppSettings["RootImages"] + fileName;

                                File.Move(result.FileData[i].LocalFileName, Path.Combine(root, fileName));

                                location[i] = filename;

                                RequestDal.UploadInvoicesIntoTable(List_Detail_Id[i], filename, Obj.EmployeeCode);

                            }
                            catch (Exception e)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, e.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Error : " + e.Message);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.Accepted, StrReturn);
            }
            catch (Exception Ex) { return Request.CreateResponse(HttpStatusCode.ExpectationFailed, Ex.Message.ToString()); }
        }

        public string RemoveSpecialCharacters(string fileName)
        {
            fileName = fileName.Replace("&", "");
            fileName = fileName.Replace("#", "");
            fileName = fileName.Replace("%", "");
            fileName = fileName.Replace("?", "");
            fileName = fileName.Replace(" ", "");
            return fileName;

        }

        private int NewNumber()
        {
            Random a = new Random(Guid.NewGuid().GetHashCode());
            return a.Next(10000, 500000);
        }

        [HttpGet]
        public RequestDetailObj_Render GetSavedAsDraftRequest(string EmpCode, int Depot_Id, int Dealer_Id, int Reason_Id)
        {
            return RequestDal.GetSavedAsDraftRequest(EmpCode, Depot_Id, Dealer_Id, Reason_Id);

        }

        [HttpGet]
        public List<PendingRequestModel> GetSavedAsDraftRequestList(string EmpCode)
        {
            return RequestDal.GetSavedAsDraftRequestList(EmpCode);
        }

        [HttpGet]
        public RequestDetailObj_Render getSRVCloserList(string EmpCode, int Depot_Id, int Dealer_Id, int Reason_Id)
        {
            return RequestDal.GetSavedAsDraftRequest(EmpCode, Depot_Id, Dealer_Id, Reason_Id);

        }

        [HttpGet]
        public List<PendingRequestModel> getRequestforDepot(string EmpCode)
        {
            return RequestDal.getRequestforDepot(EmpCode);
        }

        [HttpPost]
        public string AcknowledgeRequestbyDepot(RequestDetailObjbyDepot Obj)
        {
            return RequestDal.AcknowledgeRequestbyDepot(Obj);
        }

        [HttpPost]
        public string UpdateRequestbyCSO(RequestDetailObjbyDepot Obj)
        {
            return RequestDal.UpdateRequestbyCSO(Obj);
        }
        [HttpPost]
        public string UpdateDONo(RequestDetailObjbyDepot Obj)
        {
            return RequestDal.UpdateDONo(Obj);
        }
        [HttpPost]
        public string AckbyCommercial(RequestDetailObjbyDepot Obj)
        {
            return RequestDal.AckbyCommercial(Obj);

        }

        [HttpPost]
        public string ApprovebyDepot(RequestDetailObjbyDepot Obj)
        {
            return RequestDal.ApprovebyDepot(Obj);

        }

        [HttpPost]
        public string ReconsiderByCommercial([FromBody]RecommendRequestObj data)
        {
            return RequestDal.ReconsiderByCommercial(data);
        }
    }
}
