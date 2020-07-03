using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Globalization;
using DataLayer;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.IO;
using System.Security.Permissions;
using Microsoft.Reporting.WebForms;
using DataLayer.Data;

namespace HIS.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private int? _operatorId;
        private int _departmentId;
        private string _operatorName;
        private int _divisionId;
        private string _version;
        ConstantModel cons = new ConstantModel();
        ApplicationVersionModel apps = new ApplicationVersionModel();

        string connString = "";
        public string SqlConnectionString { get; private set; }

        public BaseController()
        {
        }

        public int OperatorId
        {
            get
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                var d = ticket.UserData.Split('|');
                return Convert.ToInt32(d[0].ToString());
            }
            set {
               // Response.Cookies.Add(new HttpCookie("ARIPBILLING_HIS_OPEID", value.ToString()));
                _operatorId = value;
                
            }
        }

        public int StationId
        {
            get
            {
                if (Request.Cookies["ARIPBILLING_HIS_STATION"] != null)
                {
                    var stationId = 0;
                    try
                    {
                        stationId = Convert.ToInt32(Request.Cookies["ARIPBILLING_HIS_STATION"].Value);
                    }
                    catch
                    {
                        stationId = 0;
                    }

                    return stationId;
                }
                else
                    return 0;
            }
            set 
            {
                Response.Cookies.Add(new HttpCookie("ARIPBILLING_HIS_STATION", value.ToString()));

            }
        }

        public int DepartmentId
        {
            get
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                var d = ticket.UserData.Split('|');
                return Convert.ToInt32(d[4].ToString());
            }

            set { _departmentId = value; }
        }

        public void _setLogInfo() {

            EncryptDecrypt ENC = new EncryptDecrypt();

            Response.Cookies.Add(new HttpCookie("ELOG_PAR1", ENC.Encrypt(OperatorId.ToString(), true)));
            Response.Cookies.Add(new HttpCookie("ELOG_PAR2", ENC.Encrypt(LocalIPAddress().ToString(), true)));
            Response.Cookies.Add(new HttpCookie("ELOG_PAR3", ENC.Encrypt(StationId.ToString(), true)));
        }
        
        public int DivisionId
        {
            get
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                var d = ticket.UserData.Split('|');
                return Convert.ToInt32(d[3].ToString());
            }

            set { _divisionId = value; }
        }

        public string OperatorName
        {
            get
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                var d = ticket.UserData.Split('|');
                return d[2].ToString();
            }

            set { _operatorName = value; }
        }

        public string FeatureID
        {
            get
            {
                if (Request.Cookies["HIS_FUNC"] != null)
                    return Request.Cookies["HIS_FUNC"].Value;
                else
                    return "0";
            }
            set
            {
                Response.Cookies.Add(new HttpCookie("HIS_FUNC", value.ToString()));
            }
        }


        [HttpGet]
        public void GetApplicationMenu()
        {
            if (Request.Cookies["MenuModel"] == null)
            {
                ApplicationGlobal app = new ApplicationGlobal();
                ApplicationVersionModel model = app.GetApplicationDetail();
                ControllerContext.StoreModelToCookies("MenuModel", model);
            }
            else
            {
                var d = JsonConvert.DeserializeObject<ApplicationVersionModel>(Request.Cookies["AppModel"].Value);
                _version =  d.MajorVersion + "." + d.MinorVersion;
            }
        }

        [HttpGet]
        public void GetApplicationDetails()
        {
           
        }

        [HttpGet]
        public JsonResult SecurityFeature()
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            var d = ticket.UserData.Split('|');
            HISSecurity cs = new HISSecurity();
            cs.UserID = d[0].ToString();
            cs.ModuleID = cons.cModuleID;
            var li = cs.SecuritryFeatureCS();
            return Json(li, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SecurityFunction()
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            var d = ticket.UserData.Split('|');
            HISSecurity cs = new HISSecurity();
            cs.UserID = d[0].ToString();
            cs.ModuleID = cons.cModuleID;
            cs.FeatureID = this.FeatureID ;
            var li = cs.SecuritryFunctionCS();
          //0 FeatureID = no access
            return Json(li, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult NotAuthorizedpage()
        {
             
            return PartialView("~/Views/Shared/NotAuthorized.cshtml" );
        }



        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            EncryptDecrypt enc = new EncryptDecrypt();
            if (Request.Cookies["AppModel" + cons.cModuleID] != null)
            {
                apps = JsonConvert.DeserializeObject<ApplicationVersionModel>(Request.Cookies["AppModel" + cons.cModuleID].Value);

                connString = enc.Decrypt(ConfigurationManager.ConnectionStrings["SghDbContextConnString"].ConnectionString, true);
                SqlConnectionString = connString + ";Application Name=WebUtilities";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(SqlConnectionString); 
              
                string IPAddress = builder.DataSource;
                ViewBag.VersionID = apps.MajorVersion + "." + apps.MinorVersion;
                ViewBag.Title = apps.ModuleName;
                ViewBag.BranchCode = apps.ModuleName;
                ViewBag.DateDeployed = apps.DateDeployed;
                ViewBag.BranchName = apps.Name;
                ViewBag.Address = apps.Address;
                ViewBag.PhoneNo = apps.PhoneNo;
                ViewBag.FaxNo = apps.FaxNo;
                ViewBag.IssueAuthorityCode = apps.IssueAuthorityCode;
                ViewBag.Developer = apps.Developer;
                ViewBag.ConServer = "HIS"; // TBA
                ViewBag.LocalIPAddress = LocalIPAddress();
                ViewBag.CopyRight = apps.CopyRight;                
            }

            base.OnActionExecuting(filterContext);
            _setLogInfo();
            
        }

        public string LocalIPAddress()
        {
            string stringIpAddress;
            stringIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
                stringIpAddress = Request.ServerVariables["REMOTE_ADDR"]; //we can use REMOTE_ADDR
            else if (stringIpAddress == null)
                stringIpAddress = GetLanIPAddress();

            return stringIpAddress;
        }

        public string GetLanIPAddress()
        {
            //Get the Host Name
            string stringHostName = Dns.GetHostName();
            //Get The Ip Host Entry
            IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
            //Get The Ip Address From The Ip Host Entry Address List
            System.Net.IPAddress[] arrIpAddress = ipHostEntries.AddressList;
            return arrIpAddress[arrIpAddress.Length - 1].ToString();
        }

        protected string SaveFilestreamtoPDF(ReportViewer reportViewer)
        {
            FileIOPermission myPerm = new FileIOPermission(PermissionState.Unrestricted);
            myPerm.Demand();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            //string pdfpath = System.IO.Path.GetTempPath();
            //string pdfpath = "D:\\Temp";
            //string pdfpath = Server.MapPath("~/Temp");
            //pdfpath += "/" + base.OperatorId + "_" + filename + ".pdf";

            string sAuthority = System.Web.HttpContext.Current.Request.Url.Authority;
            string sApplicationPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            string pdffilename = Guid.NewGuid().ToString() + ".pdf";
            string pdfpath = Server.MapPath("~/Temp") + "\\" + pdffilename;

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            sApplicationPath = sApplicationPath == "/" ? "" : sApplicationPath;
            System.Web.HttpContext.Current.Session["pdffile"] = sAuthority + sApplicationPath + "/Temp/" + pdffilename; ;

            using (FileStream fs = new FileStream(pdfpath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return pdfpath;
            //return pdfpath.Replace("\\", "/");
            //return "Temp/" + base.OperatorId + "_" + filename + ".pdf";

        }


        public ReportViewer DynamicReportHeader(ReportViewer reportViewer,string reportdataSetName) 
        {
       //For Dynamic Report Header 
                OtherReportsDB otherReprtDB = new OtherReportsDB();
                DataTable reportHeader = new DataTable();
                reportHeader = otherReprtDB.getReportHeader2018();
                ReportDataSource datareportheaderitem = new ReportDataSource(reportdataSetName, reportHeader);
                reportViewer.LocalReport.DataSources.Add(datareportheaderitem);
                return reportViewer;
        }
    }
}
