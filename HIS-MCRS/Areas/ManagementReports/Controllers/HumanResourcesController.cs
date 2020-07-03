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
    public class HumanResourcesController : Controller
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
        PersonnelDB personneldb = new PersonnelDB();
        PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult StaffContractDetailsReport()
        {

            var viewModel = new HRDStaffContractDetails()
            {
                DepartmentList = departmentDb.getAllDepartment(),
                PositionList = empDb.getDesignation()

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult StaffContractDetailsReport(HRDStaffContractDetails param)
        {
            //SP_GET_EMPCONTRACT_INFO  " & xDept & "," & xPos & "")
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = humanresourcesdb.getStaffContractDetails(vm.DepartmentId, vm.PositionId);
                reportDocPath = @"\Areas\ManagementReports\Reports\HumanResourcesReports\HRD_GetStaffContractDetails.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                //reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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

        public ActionResult EvaluationMonitor()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult get_data_evaluation_monitor(DateTime fdate, DateTime tdate)
        {

            List<HRDEvaluationMonitorModel> _RE = OtherReportDB.get_evaluation_monitor(fdate, tdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<HRDEvaluationMonitorModel>(), rcode = OtherReportDB.ret, rmsg = OtherReportDB.retmsg }),
                ContentType = "application/json"
            };
            return result;
        }




        [HttpPost]
        public ActionResult EvaluationMonitor(OtherReportsDateTimeOnly param)
        {
            //SP_OP_GeneralExpensesReport Report_EmployeeIndividualEvaluation
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                var clickEmpId = Request.Form["ClickEmployeeId"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                if (clickEmpId == "0")
                {
                    reportData = OtherReportDB.getEvalMonitr(vm.StartDate, vm.EndDate.AddDays(1));
                    reportDocPath = @"\Areas\ManagementReports\Reports\HumanResourcesReports\HRD_GetEvaluationMonitor.rdl";

                    if (reportData.Rows.Count == 0)
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));

                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                    ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                    reportViewer.LocalReport.DataSources.Add(datasourceItem);

                    reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.StartDate.ToShortDateString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.EndDate.ToShortDateString()));
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
                else
                {

                    reportData = OtherReportDB.getIndividualEmplEvaluation(clickEmpId);
                    reportDocPath = @"\Areas\ManagementReports\Reports\HumanResourcesReports\HRD_GetEvaluationMonitorIndividual.rdl";

                    if (reportData.Rows.Count == 0)
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));

                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                    ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                    reportViewer.LocalReport.DataSources.Add(datasourceItem);

                    reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.StartDate.ToShortDateString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.EndDate.ToShortDateString()));
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

            }
            return View();

        }

        public ActionResult EmployeeWise()
        {

            var viewModel = new HRDStaffContractDetails()
            {
                EmployeeList = empDb.getEmployeedetails(),


            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EmployeeWise(HRDStaffContractDetails param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                string fromtext = Request.Form["SelectedCategoryText"];
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = humanresourcesdb.getIndividualEmplEval(vm.EvaluationDate);
                reportDocPath = @"\Areas\ManagementReports\Reports\HumanResourcesReports\HRD_GetIndivEmpEval.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("fromtext", fromtext.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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


        public ActionResult getPerformanceEval(int EmpId)
        {
            return Json(empDb.getEvalByEmpId(EmpId), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListEmployeeByCategory()
        {

            var viewModel = new PRListOfEmployees()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                HrList = personneldb.getAllHrCategory(),
                NationalityList = _clPatientStatisticsDB.GetNationality(),
                //GenderList = sexdb.getSex()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ListEmployeeByCategory(PRListOfEmployees param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                if (vm.HrId == 0)
                {
                    reportData = personneldb.getEmployeebyCatALL(vm.CategoryId, vm.DeptId, vm.StartDate, vm.EndDate.AddDays(1), vm.GenderId, vm.NationalityId);
                    if (vm.IsWithSalary)
                    {
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_EmployeeByCatALL.rdl";
                    }
                    else
                    {
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_EmployeeByCatALL_NoSalary.rdl";
                    }


                }
                else
                {
                    reportData = personneldb.getEmployeebyCat(vm.HrId, vm.DeptId, vm.StartDate, vm.EndDate.AddDays(1), vm.GenderId, vm.NationalityId);
                    if (vm.IsWithSalary)
                    {
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_EmployeeByCat.rdl";
                    }
                    else
                    {
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_EmployeeByCat_NoSalary.rdl";
                    }


                }

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
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

        public ActionResult VaccinationMaster()
        {

            var viewModel = new HRvaccinationFile()
            {
                VaccinationList = humanresourcesdb.getAllVaccinationFile()
            };

            return View(viewModel);
        }



        [HttpPost]
        public bool saveVaccination()
        {
            var data = Request.Form["SelectedVaccination"];
            string IsInsert = Request.Form["IsInsert"];
            // string test = "{\"Id\":1,\"ItemCode\":\"1\",\"ItemName\":\"MMR\",\"Serology\":true,\"Dose1\":1,\"Dose2\":true,\"Dose3\":true,\"Dose4\":true,\"Deleted\":0}";
            JavaScriptSerializer j = new JavaScriptSerializer();
            dynamic a = j.Deserialize(data, typeof(object));
            string Id = "";
            string ItemCode = "";
            string ItemName = "";
            string Serology = "";
            string Dose1 = "";
            string Dose2 = "";
            string Dose3 = "";
            string Dose4 = "";
            string Deleted = "";

            foreach (var obj in a)
            {

                if (obj.Key == "Id")
                {
                    Id = obj.Value.ToString();
                }
                if (obj.Key == "ItemCode")
                {
                    ItemCode = obj.Value.ToString();
                }
                if (obj.Key == "ItemName")
                {
                    ItemName = obj.Value.ToString();
                }
                if (obj.Key == "Serology")
                {
                    Serology = obj.Value.ToString();
                }
                if (obj.Key == "Dose1")
                {
                    Dose1 = obj.Value.ToString();
                }
                if (obj.Key == "Dose2")
                {
                    Dose2 = obj.Value.ToString();
                }
                if (obj.Key == "Dose3")
                {
                    Dose3 = obj.Value.ToString();
                }
                if (obj.Key == "Dose4")
                {
                    Dose4 = obj.Value.ToString();
                }
                if (obj.Key == "Deleted")
                {
                    Deleted = obj.Value.ToString();
                }

            }

            if (IsInsert.ToString() == "1")
            {
                humanresourcesdb.saveOrUpdateVaccinationMasterFile(Id, ItemCode, ItemName, Serology, Dose1, Dose2, Dose3, Dose4, Deleted, "1");
            }
            else
            {
                humanresourcesdb.saveOrUpdateVaccinationMasterFile(Id, ItemCode, ItemName, Serology, Dose1, Dose2, Dose3, Dose4, Deleted, "0");

            }

            return true;
        }



        public ActionResult PerformanceEvaluationMapping()
        {

            var viewModel = new PRListOfEmployees()
            {
                EmployeeList = empDb.getHREmployeeDetails(),
                DepartmentList = empDb.getHRdepartmentDetails()
            };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult PerformanceEvaluationMapping(PRListOfEmployees param)
        {
            var vm = param;
            var json = Request.Form["SelectedEmpId"];
            if (Request.IsAjaxRequest())
            {
                humanresourcesdb.getSavePerformanceEvalMapping(json, vm.DeptId);
            }
          
            return Json("success", JsonRequestBehavior.AllowGet);
        }




        public ActionResult getEmployeebyDeptId(int DeptId)
        {
            return Json(empDb.getEmployebyDeptId(DeptId), JsonRequestBehavior.AllowGet);
        }


    }
}
