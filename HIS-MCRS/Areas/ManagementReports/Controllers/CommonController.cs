using DataLayer.Data.Common;
using DataLayer.Model.Common;
using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class CommonController : BaseController
    {
        //
        // GET: /MCRS/Common/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult get_common_list(int id, int ctype)
        {
            CommonDB _CO = new CommonDB();
            List<CommonDropdownModel> _CL = _CO.get_common_list(id, ctype);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _CL ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_common_list_server(int id, int ctype, string terms)
        {
            CommonDB _CO = new CommonDB();
            List<CommonDropdownServerModel> _CL = _CO.get_common_list_server(id, ctype, terms);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CLS = _CL ?? new List<CommonDropdownServerModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_common_list_selected(int id, int ctype)
        {
            CommonDB _CO = new CommonDB();
            List<CommonDropdownModel> _CL = _CO.get_common_list_selected(id, ctype);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CLSEL = _CL ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }
        public ActionResult get_category_info(int id)
        {
            CommonDB _CO = new CommonDB();
            CommonDropdownModel _CL = _CO.get_cat_list(id);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _CL ?? new CommonDropdownModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult get_grade_info(int cid, int catid)
        {
            CommonDB _CO = new CommonDB();
            CommonDropdownModel _CL = _CO.get_gra_list(cid, catid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _CL ?? new CommonDropdownModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult get_com_PTName(long pin)
        {
            CommonDB _DB = new CommonDB();
            PTCommonInfoModel _RE = _DB.get_com_PTName(pin);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new PTCommonInfoModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult get_inv_accountpin(int reqtype, int invtype, string fdate, string tdate, int catid, long comid)
        {
            CommonDB _DB = new CommonDB();
            List<CommonDropdownModel> _RE = _DB.get_inv_accountpin(reqtype, invtype, fdate, tdate, OperatorId, catid, comid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _RE ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult get_inv_accountpin_ubf(int reqtype, int invtype, string fdate, string tdate, int catid, long comid)
        {
            CommonDB _DB = new CommonDB();
            List<CommonDropdownModel> _RE = _DB.get_inv_accountpin_ubf(reqtype, invtype, fdate, tdate, OperatorId, catid, comid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _RE ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult get_cl_company(int catid, int btype, string fdate, string tdate)
        {
            CommonDB _DB = new CommonDB();
            List<CommonDropdownModel> _RE = _DB.get_cl_company(catid, btype, fdate, tdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _RE ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult get_cl_refno(int catid, long comid, string fdate, string tdate)
        {
            CommonDB _DB = new CommonDB();
            List<CommonDropdownModel> _RE = _DB.get_cl_refno(catid, comid, fdate, tdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { CL = _RE ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }


        
    }
}
