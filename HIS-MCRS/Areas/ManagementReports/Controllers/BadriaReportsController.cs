using DataLayer;
using DataLayer.Data;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using HIS_MCRS.Common;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


using System.IO;
using System.Security.Permissions;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class BadriaReportsController : BaseController
    {
        BadriaReportDal model = new BadriaReportDal();
      
        //
        // GET: /ManagementReports/BadriaReports/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Beverly_CreatedDailyReport()
        {    
            var viewModel = new Beverly_CreatedDailyReportVM()
            {
                txtFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                txtToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
             };
            return View(viewModel);
        }

        public ActionResult Beverly_CreatedDailyReportDetails()
        {
            var viewModel = new Beverly_CreatedDailyReportVM()
            {
                txtFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                txtToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
            return View(viewModel);
        }



        [HttpPost]
        public ActionResult Beverly_CreatedDailyReportDetails(Beverly_CreatedDailyReportVM param)
        {
            var docid = Extensions.HandleInt(param.sel2doctor);
            var headerDate = Extensions.ToString(param.txtFromDate, "MMMM-yyyy");
            var startdate = Extensions.ToString(param.txtFromDate, "dd-MMM-yyyy");
            var todate = Extensions.ToString(param.txtToDate, "dd-MMM-yyyy");

            var data = model.Beverly_CreatedDailyReportDetails(startdate, todate, docid);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var title = "Doctor Revenue Report - Doctor Commission Details";
            headerDate = startdate.ToString() + " - " + todate.ToString();
           

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\Badria\Bardia-DoctorRevenue.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", headerDate));

            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
               

            //reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", todate.ToString()));
            //reportViewer.LocalReport.SetParameters(new ReportParameter("options", param.options.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ReportTitle", title));

            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param);

        }


        public JsonResult GetDocList()
        {
            List<RoleModel> li = model.GetDocList();
            return Json(li.OrderBy(x => x.name), JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public ActionResult Beverly_CreatedDailyReport(Beverly_CreatedDailyReportVM param)
        {
            var docid = Extensions.HandleInt(param.sel2doctor);
            var headerDate = Extensions.ToString(param.txtFromDate, "MMMM-yyyy");
            var startdate = Extensions.ToString(param.txtFromDate, "dd-MMM-yyyy");
            var todate = Extensions.ToString(param.txtToDate, "dd-MMM-yyyy");

            var data = model.Beverly_CreatedDailyReport(startdate, todate, docid);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var title = "Doctor Revenue Report - Monthly After Clinical Process";

            if (param.options.ToString() == "beforesalary")
            {
                title = "Doctor Revenue Report - Daily ";
                headerDate = startdate.ToString() + " - " + todate.ToString();
            }
            else if (param.options.ToString() == "aftersalary")
            {
                title = "Doctor Revenue Report - Daily After Clinical Process";
                headerDate = startdate.ToString() + " - " + todate.ToString();
            }
            else
            {
                title = "Doctor Revenue Report - Daily ";
                headerDate = startdate.ToString() + " - " + todate.ToString();
            }
            
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\Badria\Bardia-DoctorRevenue.rdl";             
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
               
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", headerDate));
            //reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", todate.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("options", param.options.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ReportTitle", title));
             
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param); 
           
        }

        
    }
}
