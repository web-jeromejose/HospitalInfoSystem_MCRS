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

using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class HelpDeskReportsController : Controller
    {
        ARReportsDB arReportDb = new ARReportsDB();
        MCRSUserDB userDb = new MCRSUserDB();
        EmployeeDB empDb = new EmployeeDB();
        ICD10CodesDB icd10codeDb = new ICD10CodesDB();
        CategoryDB categoryDb = new CategoryDB();
        CompanyDB companyDb = new CompanyDB();
        GradeDB gradeDb = new GradeDB();
        UtilitiesDB utilitiesDB = new UtilitiesDB();
        GeneralInformationDB genInfoDB = new GeneralInformationDB();
        RelationshipDB relationshipDB = new RelationshipDB();
        DepartmentDB departmentDb = new DepartmentDB();
        ServicesDB serviceDb = new ServicesDB();
        TestDB testDb = new TestDB();
        ItemDB itemDb = new ItemDB();
        InvoiceDB invoiceDb = new InvoiceDB();
        OtherReportsDB OtherReportDB = new OtherReportsDB();
        StationDB stationdb = new StationDB();
        HumanResourcesDepartmentDB humanresourcesdb = new HumanResourcesDepartmentDB();
        HelpDeskDB helpdeskDB = new HelpDeskDB();

        // GET: /ManagementReports/HelpDesk/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MagcardEmployee()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDb.getAllHRCategory()      
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MagcardEmployee(OtherReportsDateTimeOnly param)
        {
             
            var vm = param;
            string deptidArray = Request.Form["DepartmentIdArray"];
            string DepartmentIdArraytext = Request.Form["DepartmentIdArraytext"];
           DepartmentIdArraytext =  DepartmentIdArraytext.Substring(1);

            var DeptId = "0";
            if (deptidArray.Contains("0") || deptidArray == null || deptidArray == "")
            {
                DeptId = "0";
            }
            else
            {
                DeptId = deptidArray;
            }
            if (Request.IsAjaxRequest())
            {


                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = helpdeskDB.getMagcard(vm.StartDate, vm.EndDate.AddDays(1), DeptId, "[MCRS].[Helpdesk_DutyRosterMissingWithHRCategory]");
                reportDocPath = @"\Areas\ManagementReports\Reports\HelpDeskReports\HelpDeskMissingDutyRosterReport.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("sdate", vm.StartDate.ToString("MMMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("MMMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("catname", DepartmentIdArraytext));
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

        public ActionResult HRPayrollEmployee()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
            return View(viewModel);
        }
        


    }
}
