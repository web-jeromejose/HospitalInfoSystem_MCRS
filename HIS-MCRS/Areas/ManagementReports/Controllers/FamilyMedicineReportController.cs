using DataLayer;
using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using HIS_MCRS.Common;
using HIS_MCRS.Enumerations;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class FamilyMedicineReportController : Controller
    {
        PatientStatisticsDB patientstatsDB = new PatientStatisticsDB();
        DepartmentDB departmentDB = new DepartmentDB();
        EmployeeDB employeeDB = new EmployeeDB();
        CategoryDB categoryDB = new CategoryDB();
        CompanyDB companyDB = new CompanyDB();
        MohDB MohDB = new MohDB();
        SalesPromotionDB SalesPromoDB = new SalesPromotionDB();
        HumanResourcesDepartmentDB humanresourcesdb = new HumanResourcesDepartmentDB();
        FamilyMedicineDB familymedDb = new FamilyMedicineDB();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult VaccinationEntryForm()
        {
            var viewModel = new FMRVaccinationReport()
            {
                    DepartmentList = departmentDB.getAllDepartment()               
            };
            return View(viewModel);
        }

            [HttpPost]
        public ActionResult VaccinationEntryForm(FMRVaccinationReport viewModel)
        {

                string postype = Request.Form["PostType"];
               
                



                if (Request.IsAjaxRequest())
                {
                    ReportViewerVm reportVM = new ReportViewerVm();
                    ReportViewer reportViewer = new ReportViewer();
                    string reportDocPath = "";

                    DataTable reportData = new DataTable();
                  

                    if (postype == "PrintAllPending")
                    {
                         reportData = familymedDb.getPrintByPending();
                         reportDocPath = @"\Areas\ManagementReports\Reports\FamilyMedicineReports\Familymed_GetPending.rdl";
                    }

                    if (postype == "PrintAllDone")
                    {
                         reportData = familymedDb.getPrintByAllDone();
                         reportDocPath = @"\Areas\ManagementReports\Reports\FamilyMedicineReports\Familymed_GetALL.rdl";
                    }

                    if (postype == "PrintDepartment")
                    {
                         reportData = familymedDb.getPrintByDepartment(viewModel.DepartmentId);
                         reportDocPath = @"\Areas\ManagementReports\Reports\FamilyMedicineReports\Familymed_GetByDepartment.rdl";
                     
                    }



                    if (reportData.Rows.Count == 0)
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));

                    reportViewer.ProcessingMode = ProcessingMode.Local;

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                    ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                    reportViewer.LocalReport.DataSources.Add(datasourceItem);
                    //reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                    //reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                    //reportViewer.LocalReport.SetParameters(new ReportParameter("patientTypeText", viewModel.PatientTypeText));
                    //reportViewer.LocalReport.SetParameters(new ReportParameter("billTypeText", viewModel.BillTypeText));
                    if (postype == "PrintDepartment")
                    {
                        string DeptName = Request.Form["DeptName"];
                        reportViewer.LocalReport.SetParameters(new ReportParameter("department", DeptName));
                    }

                    reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                    reportViewer.SizeToReportContent = true;
                    reportViewer.ShowPrintButton = true;
                    reportVM.ReportViewer = reportViewer;

                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                    return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                }

                return View();


        }

        


        [HttpPost]
        public ActionResult get_data_vaccinationEntryForm(int EmployeeId, int DepartmentId)
        {

            List<HRDEvaluationMonitorModel> _RE = humanresourcesdb.get_VaccinationEntryForm(EmployeeId, DepartmentId);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<HRDEvaluationMonitorModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

         [HttpPost]
        public ActionResult get_data_VaccinationTest(int xvID)
        {

            List<FMReport_GetVaccinationPendingModel> _RE = humanresourcesdb.get_VaccinationSelectList(xvID);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<FMReport_GetVaccinationPendingModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        


        
        [HttpPost]
        public ActionResult get_data_vaccinationPerEmployee(int EmployeeId, int DepartmentId)
        {

            List<FMReport_VaccinationEntryFormModel> _RE = humanresourcesdb.get_vaccinationperEmployee(EmployeeId);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<FMReport_VaccinationEntryFormModel>() }),
                ContentType = "application/json"
            };
            return result;
        }


        
    }
}
