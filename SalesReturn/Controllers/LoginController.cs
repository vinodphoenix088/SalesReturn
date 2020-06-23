using AssetMgmtSys.DAL;
using EPMSBAL.GenricLib;
using SalesReturnBLL.BLL;
using SalesReturnDAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesReturn.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        public LoginModel GetUserDetails(string id)
        {
            DecryptFunction de = new DecryptFunction();
            //string uid = de.Decrypt(id);
            string uid = "";
            //var con = ConfigurationManager.ConnectionStrings["LFGEntities"].ToString();
            var AllowEncryption = System.Configuration.ConfigurationSettings.AppSettings["AllowEncryption"];
            if (AllowEncryption.Equals("true"))
            {
               // uid = DECRYPTPP.Decryptmy(id);
                uid = de.Decrypt(id);  //enable for live
            }
            else
            {
                uid = id;
            }
            return UserDAL.GetEmployeeDetails(uid, "");
            //  return UserDAL.GetEmployeeDetails(uid, con);
        }

        //[HttpGet]
        //public static TblEmployeeMaster CheckIfDepotPerson(string Emp_Code) {
        //    return UserDAL.CheckIfDepotPerson{ ();
        //}
    }
}