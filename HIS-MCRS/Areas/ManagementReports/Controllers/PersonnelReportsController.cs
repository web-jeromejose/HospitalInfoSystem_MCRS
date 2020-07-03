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
    public class PersonnelReportsController : BaseController
    {
        //
        // GET: /ManagementReports/PersonnelReports/
       PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();
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
        PersonnelDB personneldb = new PersonnelDB();
        SexDB sexdb = new SexDB();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExpireIqama()
        {

            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ExpireIqama(OtherReportsDateTimeOnly param)
        {
            //SP_Get_Iquama_Epiry   Report_EmployeebyExpiringIquama.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[PersonnelReports_GetIqamaExpiry]");
                reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReports_GetIqamaExpiry.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToShortDateString()));
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
        public ActionResult SaudiSixMonth()
        {

            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult SaudiSixMonth(OtherReportsDateTimeOnly param)
        {
            //SP_SaudiStaffCompleted6Months Report_SaudiStaffCompleted6Months.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[PersonnelReports_GetSaudiStaffforSixMonths]");
                reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReports_GetSaudiStaffforSixMonths.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToShortDateString()));
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

        public ActionResult ListEmployeeByCategory()
        {

            var viewModel = new PRListOfEmployees()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                HrList = personneldb.getAllHrCategory(),
                NationalityList= _clPatientStatisticsDB.GetNationality(),
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
                    reportData = personneldb.getEmployeebyCatALL(vm.CategoryId,vm.DeptId,vm.StartDate,vm.EndDate.AddDays(1),vm.GenderId,vm.NationalityId);
                    if (vm.IsWithSalary)
                    {
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_EmployeeByCatALL.rdl";   
                    }
                    else
                    {
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_EmployeeByCatALL_NoSalary.rdl";                
                    }
             

                }
                else {
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
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult PassportDetailsReport()
        {

            var viewModel = new PRListOfEmployees()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PassportDetailsReport(PRListOfEmployees param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                //    ViewReport("SP_GetEmployeebyCategoryAllwithPassport  " & xcat & "," & xdeptcat & ",'" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "'," & CType(cboGender.SelectedItem, ValueDescriptionPair).Value())
                //Report_PassportDetailList

                reportData = personneldb.getEmployeePassportDetail( vm.DeptId, vm.StartDate, vm.EndDate.AddDays(1), vm.GenderId);

                reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\getEmployeePassportDetail.rdl";
                 

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        

        public ActionResult ProfLicenseReport()
        {

            var viewModel = new PRProfLicenseReport()
            {
        

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ProfLicenseReport(PRProfLicenseReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
  
                  ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = personneldb.getProfessionalLicense(vm.LicenseId, vm.GroupById);

             

                //If CType(cboDepartment.SelectedItem, ValueDescriptionPair).Value() = 1 Then
                //    ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_ProfessionalLicenseReport(MOH).rpt")
                //ElseIf CType(cboDepartment.SelectedItem, ValueDescriptionPair).Value() = 2 Then
                //    ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_ProfessionalLicenseReport(SaudiCouncil).rpt")
                //Else
                //    If opt1.Checked = True Then
                //        ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_ProfessionalLicenseReport(ALL)1.rpt")
                //    Else
                //        ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_ProfessionalLicenseReport(ALL)2.rpt")
                //    End If
                //End If


                if (vm.LicenseId == 1)
                {

                    reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReports_PRLicenseMOH.rdl";

                }
                else if (vm.LicenseId == 2)
                {

                    reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReports_ProfessionalLicense_SaudiCouncil.rdl";

                }
                else {

                    //if (vm.GroupById == 0)
                    //{
                        reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReport_ProfessionalLicenseALLByCat.rdl";
                    //}
                    //else {
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReport_ProfessionalLicenseALLByDept.rdl";
                    //}
                }


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                //reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToShortDateString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
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


        public ActionResult DependentSummaryList()
        {
                  
            return View();
        }
       
         [HttpPost]
        public ActionResult DependentSummaryList(PRProfLicenseReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
  
                  ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = personneldb.getDependentSummaryList();
                //Report_DependentList
                reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PR_GetDependentSummaryList.rdl";
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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


         [HttpPost]
         public ActionResult get_data_hrcategories(int xvID)
         {
             List<HRDEvaluationMonitorModel> _RE = personneldb.get_dataHrCategory(xvID);
             var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
             var result = new ContentResult
             {
                 Content = serializer.Serialize(new { Res = _RE ?? new List<HRDEvaluationMonitorModel>() }),
                 ContentType = "application/json"
             };
             return result;
         }



         public ActionResult EmployeeExclusionMaster()
        {

            var viewModel = new PRProfLicenseReport()
            {
             HrCategoryList = departmentDb.getAllHRCategory(),

            };

            return View(viewModel);
        }
         public ActionResult EmployeeDependentInfoSheet()
        {

            var viewModel = new PersonnelReportsVM()
            {
                DepartmentList = departmentDb.getAllDepartment(),
                ContractTypeList = departmentDb.getAllContractType()
            };
            return View(viewModel);
        }

        
            [HttpPost]
         public ActionResult EmployeeDependentInfoSheet(PersonnelReportsVM param)
         {
             string SelectedId = Request.Form["ListOfSelectedEmployees"];

             var reportViewer = this.generateFamilyDependentperBatchReportViewer(SelectedId);
             var memoryStream = Common.Helper.createFileMemoryStream(reportViewer, "PDF");

             return new FileStreamResult(memoryStream, "application/pdf");
        }


            private ReportViewer generateFamilyDependentperBatchReportViewer(string visitIdJsonArray)
            {
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReports_GetFamilyDependentSheet.rdl";

                var records = personneldb.getEmployeeDependeSheetBatch(visitIdJsonArray);

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", records);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
               
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                 


                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                return reportViewer;
            }

         [HttpPost]
            public ActionResult EPISBatchPrinting(int EmployeeId)
         {

             
             if (Request.IsAjaxRequest())
             {

                 ReportViewerVm reportVM = new ReportViewerVm();
                 ReportViewer reportViewer = new ReportViewer();
                 string reportDocPath = "";
                 DataTable reportData = new DataTable();

                 reportData = personneldb.getFamilyDependentList(EmployeeId);
                 //Report_DependentList
                 reportDocPath = @"\Areas\ManagementReports\Reports\PersonnelReports\PersonnelReport_EmpDepInformationSheet.rdl";

                 if (reportData.Rows.Count == 0)
                     return Content(Errors.ReportContent("NO RECORDS FOUND"));

                 var ReportDetails = personneldb.getEmpDetails(EmployeeId);

                 reportViewer.ProcessingMode = ProcessingMode.Local;
                 reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                 ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                 reportViewer.LocalReport.DataSources.Add(datasourceItem);
                 reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                 if (ReportDetails.Rows.Count != 0)
                 {
                     foreach (DataRow row in ReportDetails.Rows)
                     {   
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeeId", Convert.ToString(row["EmployeeID"])));
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeename", Convert.ToString(row["Name"])));
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeedepartment", Convert.ToString(row["Department"])));
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeecontracttype", Convert.ToString(row["ContractType"])));
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeeposition", Convert.ToString(row["Designation"])));
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeedatehired", Convert.ToString(row["DateHired"])));
                         reportViewer.LocalReport.SetParameters(new ReportParameter("employeepin", Convert.ToString(row["PIN"])));
                     }
                 }

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

         //[HttpPost]
         public ActionResult getEmployeeData(int department, int contractType)
         {

             List<PersonnelReportModel> _RE = personneldb.get_EmployeeInfoSheet(department,contractType);
             var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
             var result = new ContentResult
             {
                 Content = serializer.Serialize(new { Res = _RE ?? new List<PersonnelReportModel>() }),
                 ContentType = "application/json"
             };
             return result;
         }


         public ActionResult StaffDocMonitoring()
        {

            var viewModel = new PRListOfEmployees()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StaffDepartmentList = departmentDb.getAllDepartment(),

            };

            return View(viewModel);
        }

         public ActionResult get_StaffDocMonitoring(PRStaffDocMonitoring param)
         {
            var vm = param;
             
            DateTime StartDate =  Convert.ToDateTime(this.Request.QueryString["StartDate"]);
            DateTime EndDate =  Convert.ToDateTime(this.Request.QueryString["EndDate"]);
            string EmpId2 = this.Request.QueryString["EmpId"];
            string DeptId = this.Request.QueryString["DeptId"];

            int EmpId = 0;

            if (EmpId2 != "0" && EmpId2 != "" )
            {
                 EmpId = Convert.ToInt32(EmpId2);
            }



            List<PRStaffDocMonitoring> _RE = personneldb.getStaffDocMonitoring(StartDate, EndDate.AddDays(1), EmpId, DeptId);
             var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
             var result = new ContentResult
             {
                 Content = serializer.Serialize(new { Res = _RE ?? new List<PRStaffDocMonitoring>() }),
                 ContentType = "application/json"
             };
             return result;


             //return Json(result, JsonRequestBehavior.AllowGet);
        
         }

       [HttpPost]
         public ActionResult saveStaffDocMonitoring(string param)
        {
            var selectedemployees = Request.Form["SelectedEmployees"];

            var objResponse1 = JsonConvert.DeserializeObject<List<PRStaffDocModel>>(selectedemployees);

            foreach (var t in objResponse1)
            {
                 string fullname = t.fullname;
                 string employeeid  = t.employeeid;
                 string deptcode = t.deptcode;
                 string name = t.name;
                 string cv = (t.cv.ToLower() == "true" ? "1" : "0");
                 string orient_dept = (t.orient_dept.ToLower() == "true" ? "1" : "0");
                 string orient_gen = (t.orient_gen.ToLower() == "true" ? "1"  :  "0");
                 string jd = (t.jd.ToLower() == "true" ? "1" : "0");
                 string license = (t.license.ToLower() == "true" ? "1" : "0");
                 string educ_cert = (t.educ_cert.ToLower() == "true" ? "1" : "0");
                 string fs = (t.fs.ToLower() == "true" ? "1" : "0");
                 string ifc = (t.ifc.ToLower() == "true" ? "1" : "0");
                 string tqm = (t.tqm.ToLower() == "true" ? "1" : "0");
                 string bcls = (t.bcls.ToLower() == "true" ? "1" : "0");
                 string acls = (t.acls.ToLower() == "true" ? "1" : "0");
                 string eval_1 = (t.eval_1.ToLower() == "true" ? "1" : "0");
                 string eval_2 = (t.eval_2.ToLower() == "true" ? "1" : "0");
                 string eval_3 = (t.eval_3.ToLower() == "true" ? "1" : "0");
                 string eval_4 = (t.eval_4.ToLower() == "true" ? "1" : "0");
                 string confidentiality = (t.confidentiality.ToLower() == "true" ? "1" : "0");
                 string credentialing = (t.credentialing.ToLower() == "true" ? "1" : "0");
                 string previledging = (t.previledging.ToLower() == "true" ? "1" : "0");

                 var update = personneldb.updateStaffDocMonitoring(employeeid, fullname, deptcode, name, cv, orient_dept, orient_gen, jd, license, educ_cert, fs, ifc, tqm, bcls, acls, eval_1, eval_2, eval_3, eval_4, confidentiality, credentialing, previledging);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        
    }
}
