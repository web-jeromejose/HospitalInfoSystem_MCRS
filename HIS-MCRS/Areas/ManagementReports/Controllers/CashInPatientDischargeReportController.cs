//using HIS_MCRS.Areas.ManagementReports.Models;

//using DataLayer;
//using DataLayer.Data;
//using HIS_MCRS.Areas.ManagementReports.ViewModels;
//using HIS_MCRS.Common;
//using HIS_MCRS.Models;
//using Microsoft.Reporting.WebForms;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.UI.WebControls;

using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS_MCRS.Areas.ManagementReports.Models;
using DataLayer.Data;
using Newtonsoft.Json;
using Microsoft.Reporting.WebForms;
using HIS_MCRS.Models;
using DataLayer.Model;
using System.Text;
using System.Data;
using HIS_MCRS.Common;
using HIS_MCRS;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using DataLayer;
using HIS_MCRS.Extension;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using HIS_MCRS.Areas.ManagementReports.ViewModels;


namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class CashInPatientDischargeReportController : Controller
    {
        //
        // GET: /ManagementReports/CashInPatientDischargeReport/

        MCRSAllInPatientByDischargeDateTimeDB McrsAllPatiendDB = new MCRSAllInPatientByDischargeDateTimeDB();


        public ActionResult Index()
        {
            var viewModel = new McrsAllInPatient()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(McrsAllInPatient McrsAllinpatient)
        {
            var vm = McrsAllinpatient;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = McrsAllPatiendDB.getAllinPatientByDischargeTime(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\Mcrs_AllPatientByDischargeDateReports\Report_McrsAllPatientByDischargeDate.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", vm.EndDate.ToString("dd-MMM-yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("reptype", vm.RepType.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.SizeToReportContent = true;
                reportViewer.Height = Unit.Percentage(100);
                reportViewer.Width = Unit.Percentage(100);
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;
                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);

            }

            return View();
        }

    }
}
