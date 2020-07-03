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
    public class AuditReportsController : Controller
    {
        //
        // GET: /ManagementReports/AuditReports/

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
        AuditReportsDB auditDB = new AuditReportsDB();
        OPRevenueDB opRevenueDB = new OPRevenueDB();
        IPDischargeDB ipDischargeDB = new IPDischargeDB();
        CompanyDB companyDB = new CompanyDB();
        EmployeeDB employeeDB = new EmployeeDB();
        DepartmentDB departmentDB = new DepartmentDB();
        IPRevenueDB ipRevenueDB = new IPRevenueDB();
        CategoryDB categoryDB = new CategoryDB();
        AdjustmentsDB adjustmentDB = new AdjustmentsDB();


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IPRevenue()
        {
            
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult IPRevenue(OtherReportsDateTimeOnly param)
        {
            //SP_GIA_Doctor_Revenue Report_RevenueSummary.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[AuditReports_GetRevenueDoctor]");
                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\AuditReports_GetRevenueDoctor.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

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

        public ActionResult IPOPCharge()
        {
            var viewModel = new AuditReportIPOPCharged()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                PackageId = Enumerations.AuditReport_PackageType.NONPACKAGEDEAL,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IPOPCharge(AuditReportIPOPCharged param)
        {
            //SP_GIA_Doctor_Revenue Report_RevenueSummary.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                int billtypeId;
                billtypeId = 1;
                 int chargetype;
                chargetype = 1;
                if (vm.ChargeType  == Enumerations.AuditReport_ChargeType.CHARGED)
                {
                    chargetype = 0;
                }

                if(vm.PackageId == Enumerations.AuditReport_PackageType.NONPACKAGEDEAL)
                { billtypeId = 0;
                }




                reportData = auditDB.getIPOPXrayCharge(vm.StartDate, vm.EndDate.AddDays(1), billtypeId, chargetype);

                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\AuditReports_IPOPXray.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

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

        
             public ActionResult OPCancelledBillByDept()
        {
            var viewModel = new AuditReportsIPCancelledByDept()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
               BillType = Enumerations.AuditReport_IpCancelledByDept.CASH
            };

            return View(viewModel);
        }
       

        
        [HttpPost]
        public ActionResult OPCancelledBillByDept(AuditReportsIPCancelledByDept param)
        {
   
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                int billtypeId;
                billtypeId = 0;
              
                if (vm.BillType == Enumerations.AuditReport_IpCancelledByDept.CHARGE)
                {
                    billtypeId = 1;
                }

                if(vm.BillType == Enumerations.AuditReport_IpCancelledByDept.BOTH)
                {
                    billtypeId = 2;
                }

                reportData = auditDB.getOPCancelledBillByDept(vm.StartDate, vm.EndDate.AddDays(1), billtypeId);

                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\getOPCancelledBillByDept.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
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



        public ActionResult ListSubAccount()
        {
            var viewModel = new AuditReportsListSubAccounts()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
               Categories =  categoryDB.getCategories()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ListSubAccount(AuditReportsListSubAccounts param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = auditDB.getSublist(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId);

                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\getSublist.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
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



        public ActionResult DischargeInfo()
        {

            var viewModel = new AuditReportDischarge()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DoctorsList = employeeDB.getAllDoctosbyID(),
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult DischargeInfo(AuditReportDischarge param)
        {
          
            var vm = param;
            if (Request.IsAjaxRequest())
            {


                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                int withDoc = 1;
                if (vm.DocOrNone == true)
                { withDoc = 0; }
                reportData = auditDB.getDischargeInfo(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId, withDoc );
                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\getDischargeInfo.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

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

        public ActionResult IPChargeBilledReport()
        {

            var viewModel = new AuditReportIPChargeBilledReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                 DoctorList = employeeDB.getAllDoctosbyID(),
                ServiceList = categoryDB.getIPBServices(),

                CategoryList = categoryDB.getCategoriesWithId(), // Account TYPE = CASH -> categoryDB.getCategoriesWithOutId 
                //CompanyList = getCompanyByCategory kapag meroin na category
               
                ChargeTypeList = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(0,"Currently-In"),
                                         new KeyValuePair<int, string>(1,"Discharged"),
                 },
                 AccountTypeList = new List<KeyValuePair<int,string>>{
                                        new KeyValuePair<int,string>(0,"Charge"),
                                        new KeyValuePair<int,string>(1,"Cash")
                 }
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IPChargeBilledReport(AuditReportIPChargeBilledReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                int withDoc = 1;
                //if (vm.DocOrNone == true)
                //{ withDoc = 0; }
                reportData = auditDB.getIPChargeBilledReport(vm.StartDate, vm.EndDate.AddDays(1), vm.ChargedORBilled, vm.ChargedType, vm.AccountType, vm.DoctorId, vm.ServiceId, vm.CategoryId, vm.CompanyId);
                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\Report_IPCharged.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

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


        public JsonResult getCompanyByCategory(int categoryId)
        {

            return Json(companyDb.getCompanyByCategory(categoryId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getCashCategory(int checker)
        {
            if(checker == 1)
            return Json(companyDb.getCashCategory(), JsonRequestBehavior.AllowGet);
            else
                return Json(categoryDB.getCategoriesWithId(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyProfLogs()
        {

            var viewModel = new AuditReportIPChargeBilledReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDB.getCategoriesWithId(),
                IsConsultation = false,
                TypeIPList = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(0,"In-Patient"),
                                         new KeyValuePair<int, string>(1,"Out-Patient"),
                 },
                 TypeGradeList = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(0,"Grade"),
                                         new KeyValuePair<int, string>(1,"Company"),
                 },
                 LevelList = new List<KeyValuePair<int,string>>{
                                        new KeyValuePair<int,string>(0,"Service"),
                                        new KeyValuePair<int,string>(1,"Department")
                 },
                LevelOPList = new List<KeyValuePair<int, string>>{
                                        new KeyValuePair<int,string>(0,"Grade"),
                                        new KeyValuePair<int,string>(1,"Service"),
                                        new KeyValuePair<int,string>(2,"Department")
                 },
                SubCategoryList = new List<KeyValuePair<int, string>>{
                                        new KeyValuePair<int,string>(0,"Deductible"),
                                        new KeyValuePair<int,string>(1,"Discount"),
                                        new KeyValuePair<int,string>(2,"Markup")
                 }
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CompanyProfLogs(AuditReportIPChargeBilledReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
             
                if (vm.IsConsultation)
                {
                    //Gia_Audit_GradeConsultation getGradeConsultation
                    reportData = auditDB.getGradeConsultation(vm.StartDate, vm.EndDate.AddDays(1),vm.CategoryId,vm.TypeGradeId);
                   
                }
                else
                {
                    if (vm.TypeIPId == 0)
                    {

                        if (vm.LevelId == 0)
                        {
                            reportData = auditDB.getCompanyProfileLogs(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.SubCategoryId, "[MCRS].[AuditReport_IpCompanyService]");
                            //ViewReport("Gia_Audit_IPCompanyServiceNew '" & FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEdate.Value), "DD_MMM_YYYY") & "','" & DirectCast(cboCategory.SelectedItem, ValueDescriptionPair).Value() & "'," & cboSubLevel.SelectedIndex)
                        }
                        else if (vm.LevelId == 1) {
                            reportData = auditDB.getCompanyProfileLogs(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.SubCategoryId, "[MCRS].[AuditReports_IpCompanyDepartment]");
                            //ViewReport("Gia_Audit_IPCompanyDepartmentNew '" & FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEdate.Value), "DD_MMM_YYYY") & "','" & DirectCast(cboCategory.SelectedItem, ValueDescriptionPair).Value() & "'," & cboSubLevel.SelectedIndex)
                        }
                        else if (vm.LevelId == 2)
                        {
                            reportData = auditDB.getCompanyProfileLogs(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.SubCategoryId, "[MCRS].[AuditReport_IpCompanyItem]");
                            //ViewReport("Gia_Audit_IPCompanyItemNew '" & FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEdate.Value), "DD_MMM_YYYY") & "','" & DirectCast(cboCategory.SelectedItem, ValueDescriptionPair).Value() & "'," & cboSubLevel.SelectedIndex)
                        }
                    }

                    if (vm.TypeIPId == 1)
                    {
                        if (vm.LevelOPId == 0)
                        {
                            reportData = auditDB.getCompanyProfileLogs(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.SubCategoryId, "[MCRS].[AuditReport_OPCompanyGrade]");
                            //ViewReport("Gia_Audit_OPCompanyGradeNew '" & FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEdate.Value), "DD_MMM_YYYY") & "','" & DirectCast(cboCategory.SelectedItem, ValueDescriptionPair).Value() & "'," & cboSubLevel.SelectedIndex)
                        }
                        else if (vm.LevelOPId == 1)
                        {
                            reportData = auditDB.getCompanyProfileLogs(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.SubCategoryId, "[MCRS].[AuditReports_OPCompanyDepartment]");
                        }
                        else if (vm.LevelOPId == 2)
                        {
                            reportData = auditDB.getCompanyProfileLogs(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.SubCategoryId, "[MCRS].[AuditReport_OPCompanyItem]");
                        }
                    }
                }

                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\AuditReport_CompanyProfLogs.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                if (vm.IsConsultation)
                {
                    reportViewer.LocalReport.SetParameters(new ReportParameter("label", Request.Form["TypeName"] + " Level Consultation (" + Request.Form["CategoryName"] + ") - "));
                }
                else
                {
                    reportViewer.LocalReport.SetParameters(new ReportParameter("label", Request.Form["TypeName"] + " Level " + Request.Form["SubCategoryName"] + " (" + Request.Form["CategoryName"] + ") - " + Request.Form["LevelName"])); 
                }

                reportViewer.LocalReport.SetParameters(new ReportParameter("startdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enddate", vm.EndDate.ToString("dd-MMM-yyyy")));
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



        public ActionResult OPBilledReport()
        {
            var viewModel = new AuditReportIPChargeBilledReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DoctorList = employeeDB.getAllDoctosbyID(),
                ServiceList = categoryDB.getOPBServices(),

                CategoryList = categoryDB.getCategoriesWithId(), 

                ChargeTypeList = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(0,"Currently-In"),
                                         new KeyValuePair<int, string>(1,"Discharged"),
                 },
                AccountTypeList = new List<KeyValuePair<int, string>>{
                                        new KeyValuePair<int,string>(0,"Charge"),
                                        new KeyValuePair<int,string>(1,"Cash")
                 }
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OPBilledReport(AuditReportIPChargeBilledReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                int withDoc = 1;
                //if (vm.DocOrNone == true)
                //{ withDoc = 0; }
                reportData = auditDB.getOPBillCharge(vm.StartDate, vm.EndDate.AddDays(1),vm.DoctorId,vm.ServiceId,vm.CategoryId,vm.CompanyId,vm.AccountType,vm.ChargedORBilled);
                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\AuditReport_OPBilledReport.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("startdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enddate", vm.EndDate.ToShortDateString()));
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

       
        public ActionResult ArOpBillMonthWise()
        {
            var viewModel = new AuditReportIPChargeBilledReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                //DoctorList = employeeDB.getAllDoctosbyID(),
                ServiceList = categoryDB.getIPBServices(),
                CategoryList = categoryDB.getCategoriesWithId(), 
 
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ArOpBillMonthWise(AuditReportIPChargeBilledReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
            
             
                reportData = auditDB.getArOpBillMonthWise(vm.StartDate, vm.EndDate.AddDays(1),vm.CategoryId, vm.CompanyId);
                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\AuditReport_ArOpBillMonthWise.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("startdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enddate", vm.EndDate.ToShortDateString()));
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

        public ActionResult SummaryBillPercentage()
        {
            var viewModel = new AuditReportIPChargeBilledReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DoctorList = employeeDB.getAllDoctosbyID(),
                ServiceList = categoryDB.getIPBServices(),
                CategoryList = categoryDB.getCategoriesWithId(),

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SummaryBillPercentage(AuditReportIPChargeBilledReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = auditDB.getSummaryBillPercentage(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId, vm.CompanyId,vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\AuditReports\AuditReport_getSummaryBillPercentage.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("startdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enddate", vm.EndDate.ToShortDateString()));
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
