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
using OfficeOpenXml;
using OfficeOpenXml.Style;
 


namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class ARReportsController : BaseController
    {
        // GET: /MCRS/ARReports/
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
        DepartmentDB departmentDB = new DepartmentDB();
        EmployeeDB employeeDB = new EmployeeDB();
        OPRevenueDB opRevenueDB = new OPRevenueDB();



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UCAF()
        {
            var viewModel = new UCAFVM();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UCAF(UCAFVM ucafVM)
        {
            if (Request.IsAjaxRequest())
            {
                if (ucafVM.VisitId > 0)
                {
                    ReportViewerVm reportVM = new ReportViewerVm();
                    var reportViewer = this.generateUCAFPrintReportViewer(ucafVM.VisitId);

                    reportViewer.SizeToReportContent = true;
                    reportViewer.ShowPrintButton = true;

                    reportVM.ReportViewer = reportViewer;

                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                    return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                }
                else
                {
                    return Content(Errors.ReportContent("NO DOCTORS ENTRY FOUND"));
                }
            }

            return View(ucafVM);
        }

        public ActionResult StatementSummaryByCategory()
        {
            var vm = new StatementSummaryByCategoryVM();
            vm.FromDate = DateTime.Now;
            vm.ToDate = DateTime.Now;
            vm.Categories = categoryDb.getCategories();

            return View(vm);
        }

        [HttpPost]
        public ActionResult StatementSummaryByCategory(StatementSummaryByCategoryVM statementByCategory)
        {
            if (ModelState.IsValid)
            {
                var vm = statementByCategory;

                var fromDate = new DateTime(vm.FromDate.Year, vm.FromDate.Month, vm.FromDate.Day);
                var toDate = new DateTime(vm.ToDate.Year, vm.ToDate.Month, vm.ToDate.Day);

                if (Request.IsAjaxRequest())
                {
                    var statementReportDT = new DataTable();



                    ReportViewerVm reportVM = new ReportViewerVm();
                    ReportViewer reportViewer = new ReportViewer();


                    if (vm.ReportStatement == Enumerations.ARReportStatement.BYSUMMARY)
                    {
                        statementReportDT = arReportDb.getStatementByCategory(fromDate, toDate, vm.Category);
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_StatementSummarybyCategory.rdlc";
                    }
                    else if (vm.ReportStatement == Enumerations.ARReportStatement.BYCOMPANYWITHPOLICY)
                    {
                        statementReportDT = arReportDb.getStatementByCompany(fromDate, toDate, vm.Category);
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_StatementSummarybyCompany.rdlc";
                    }
                    else
                    {
                        statementReportDT = arReportDb.getStatementByCompanyCategory(fromDate, toDate, vm.Category);
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_StatementSummarybyCompanyCategory.rdlc";
                    }
                    ReportDataSource reportDataSource = new ReportDataSource("StatementSummary", statementReportDT);

                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    reportViewer.LocalReport.DataSources.Add(reportDataSource);

                    reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");


                    reportViewer.LocalReport.SetParameters(new ReportParameter("Company", Global.OrganizationDetails.Name));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("Branch", Global.OrganizationDetails.City));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", fromDate.ToString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", toDate.ToString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("Category", categoryDb.getCategory(vm.Category).Name));
                    reportViewer.Height = Unit.Percentage(100);
                    reportViewer.Width = Unit.Percentage(100);
                    reportViewer.SizeToReportContent = true;
                    reportVM.ReportViewer = reportViewer;




                    if (statementReportDT.Rows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORD FOUND"));
                    }
                }


            }
            return View();
        }

        public ActionResult StatementSummaryForAllCategory()
        {
            var vm = new StatementSummaryByAllCategoryVM();
            vm.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
            vm.EndDate = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day);
            return View(vm);
        }

        [HttpPost]
        public ActionResult StatementSummaryForAllCategory(StatementSummaryByAllCategoryVM statementsummary)
        {
            var vm = statementsummary;

            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    ReportViewerVm reportVM = new ReportViewerVm();
                    ReportViewer reportViewer = new ReportViewer();
                    DataTable statementReportDT = arReportDb.getStatementAllCategory(vm.StartDate, vm.EndDate.AddDays(1));

                    if (statementReportDT.Rows.Count > 0)
                    {
                        ReportDataSource reportDataSource = new ReportDataSource("StatementSummary", statementReportDT);
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_StatementSummaryAllCategory.rdlc";
                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.DataSources.Add(reportDataSource);
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("Company", Global.OrganizationDetails.Name));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("Branch", Global.OrganizationDetails.City));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToString()));

                        reportViewer.SizeToReportContent = true;
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORD FOUND"));
                    }
                }
            }

            return View();
        }

        public ActionResult UCAFBatchPrinting()
        {
            var vm = new UCAFBatchPrintingVM();
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;
            vm.ARUCAFRecords = new List<ARUCAFBatchPrintRecord>();

            vm.Doctors = (from doctor in empDb.getEmployeeByCategory((int)Enumerations.EmployeeCategory.DOCTOR)
                          select new SelectListItem
                          {
                              Value = doctor.OperatorId.ToString(),
                              Text = doctor.EmpCode + " - " + doctor.FullName
                          }).ToList();
            vm.Doctors.Insert(0, new SelectListItem { Value = "0", Text = "All" });
            vm.Categories = categoryDb.getCategories();
            return View(vm);
        }

        [HttpPost]
        public ActionResult UCAFBatchPrinting(UCAFBatchPrintingVM ucafBatchPrintingVM)
        {

            var reportViewer = this.generateUCAFPrintReportViewer(ucafBatchPrintingVM.ListOfSelectedVisitIds);
            var memoryStream = Common.Helper.createFileMemoryStream(reportViewer, "PDF");

            return new FileStreamResult(memoryStream, "application/pdf");
        }

        [HttpPost]
        public ActionResult GetUCAFRecord(int categoryId, int doctorId, DateTime startDate, DateTime endDate, string selectedCompaniesJson, string sortBy, string sortMode = "asc", int page = 1)
        {
            var selectedCompanies = new JavaScriptSerializer().Deserialize<List<CompanyModel>>(selectedCompaniesJson);

            var ucafRecords = arReportDb.getUCAFRecords(categoryId, doctorId, startDate, endDate, selectedCompanies);

            switch (sortBy)
            {
                case "patientname":
                    if (sortMode == "desc")
                        ucafRecords = ucafRecords.OrderByDescending(i => i.PatientName).ToList();

                    else
                        ucafRecords = ucafRecords.OrderBy(i => i.PatientName).ToList();

                    break;

                case "date":
                    if (sortMode == "desc")
                        ucafRecords = ucafRecords.OrderByDescending(i => i.DateTime).ToList();

                    else
                        ucafRecords = ucafRecords.OrderBy(i => i.DateTime).ToList();

                    break;

                case "type":
                    if (sortMode == "desc")
                        ucafRecords = ucafRecords.OrderByDescending(i => i.VisitType).ToList();

                    else
                        ucafRecords = ucafRecords.OrderBy(i => i.VisitType).ToList();

                    break;

                case "doctor":
                    if (sortMode == "desc")
                        ucafRecords = ucafRecords.OrderByDescending(i => i.DoctorName).ToList();

                    else
                        ucafRecords = ucafRecords.OrderBy(i => i.DoctorName).ToList();

                    break;

                case "code":
                    if (sortMode == "desc")
                        ucafRecords = ucafRecords.OrderByDescending(i => i.CompanyCode).ToList();

                    else
                        ucafRecords = ucafRecords.OrderBy(i => i.CompanyCode).ToList();

                    break;

                case "pin":
                    if (sortMode == "desc")
                        ucafRecords = ucafRecords.OrderByDescending(i => i.CompanyCode).ToList();

                    else
                        ucafRecords = ucafRecords.OrderBy(i => i.Pin).ToList();

                    break;


            };

            int recordCount = ucafRecords.Count();

            int pageSize = 25;
            int totalPageCount = (int)Math.Ceiling((double)recordCount / pageSize);

            int start = (page - 1) * pageSize;


            var pagedUcafRecord = ucafRecords.Skip(start).Take(pageSize).ToList();

            var visitIdList = ucafRecords.Where(t => t.VisitId != 0).Select(i => i.VisitId).ToList();


            return Json(new { records = pagedUcafRecord, totalPage = totalPageCount, pageSize = pageSize, currentPage = page, visitIdList = visitIdList, recordCount = recordCount }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteARDiagnosis(int visitId)
        {
            var success = arReportDb.deleteARDiagnosis(visitId);

            return Json(new { deleted = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updateARDiagnosis(ARDiagnosisModel arDiagnosis)
        {
            var success = arReportDb.updateARConsultationICDDetail(arDiagnosis.ICDId, arDiagnosis.VisitId, arDiagnosis.ICDCode, arDiagnosis.ICDDescription, base.OperatorId);

            return Json(new { updated = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveClinicalVisit(InsertUpdateARClinicalVisitVM clinicalvisit)
        {

            var success = arReportDb.insertOrUpdateARConsultationVisitDetails(clinicalvisit.VisitId, base.OperatorId, clinicalvisit.TreatmentPlan, clinicalvisit.ChiefComplaints);

            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListOfExpiredCompany()
        {
            var viewModel = new ARListOfExpiredCompany()
            {
                ExpiryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
                CompanyStatus = Enumerations.CompanyStatus.BLOCKED

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ListOfExpiredCompany(ARListOfExpiredCompany viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var companies = companyDb.getCompanyByValidityDate(viewModel.ExpiryDate, viewModel.CategoryId, (int)viewModel.CompanyStatus);
                    if (companies.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\CompanyByValidityDate.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CompanyList", companies));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("validUntilDate", viewModel.ExpiryDate.ToString()));



                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return Content("");
        }

        public ActionResult ListOfCompanyWithTransactions()
        {
            var viewModel = new ARListOfCompanyWithTransactions()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),


            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ListOfCompanyWithTransactions(ARListOfCompanyWithTransactions viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var companies = companyDb.getCompanyWithTransactions(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);
                    if (companies.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ListOfCompanyWithTransactions.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Companies", companies));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString()));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return Content("");
        }

        public ActionResult BillingEfficiencyReport()
        {
            var viewModel = new ARBillingEfficiencyReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult BillingEfficiencyReport(ARBillingEfficiencyReport viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = companyDb.getCompanyBillingEfficiency(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\BillingEfficiency.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GetBillingEfficiency", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString()));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return Content("");
        }

        public ActionResult CompanyTotalPatientVisit()
        {
            var viewModel = new ARCompanyTotalPatientVisit()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                CreationType = 0

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CompanyTotalPatientVisit(ARCompanyTotalPatientVisit viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {

                    var data = arReportDb.getARCompanyTotalVisit(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.CreationType);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARCompanyPatientVisit.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PatientVisit", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString()));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return View();
        }

        public ActionResult ARAMCOPatientList()
        {
            var viewModel = new ARReportsARAMCOPatientList()
            {
                RelationshipId = 0,
                PatientStatus = Enumerations.PatientStatus.ALL,

                PatientStatusList = new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>((int)Enumerations.PatientStatus.ALL,Enumerations.PatientStatus.ALL.ToString()),
                    new KeyValuePair<int, string>((int)Enumerations.PatientStatus.ACTIVE,Enumerations.PatientStatus.ACTIVE.ToString()),
                    new KeyValuePair<int, string>((int)Enumerations.PatientStatus.NONACTIVE,Enumerations.PatientStatus.NONACTIVE.ToString())
                },
                RelationshipList = relationshipDB.getRelationship()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ARAMCOPatientList(ARReportsARAMCOPatientList viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getARAMCOPatientList((int)viewModel.PatientStatus, viewModel.RelationshipId);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARAMCOPatientList.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PatientList", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("PatientRelation", viewModel.Relationship.Trim()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("PatientStatus", viewModel.PatientStatus.ToString()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult ARPackageAndInvoices()
        {
            var viewModel = new ARPackageAndInvoices()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                Deal = 0,
                ReportOption = 0
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ARPackageAndInvoices(ARPackageAndInvoices viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getARPakageAndNonPackageInvoice(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.Deal, viewModel.ReportOption);

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";
                        var reportHeader = viewModel.Deal == 0 ? "AR Company Package Deal Invoices Report" : "AR Company Non-Package Deal Invoices Report";
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if (viewModel.ReportOption == 0)
                            reportDocPath += "PackageDealAndNon-PackageDealInvoices.rdlc";

                        else if (viewModel.ReportOption == 1)
                            reportDocPath += "PackageDealAndNon-PackageDealInvoices(summary).rdlc";

                        else
                            reportDocPath += "PackageDealAndNon-PackageDealInvoices(categorysummary).rdlc";

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Invoices", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("Branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("ReportHeader", reportHeader));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return Content("");
        }

        public ActionResult ARCompanyAuditTrail()
        {

            var viewModel = new ARCompanyAuditTrail()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ARCompanyAuditTrail(ARCompanyAuditTrail viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = companyDb.getCompanyAuditTrail(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARCompanyAuditTrail.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ARCompanyAuditTrail", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewModel.CategoryText));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult ARPackageDeal()
        {

            var viewModel = new ARPackageDeal()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentId = 0,
                DepartmentList = departmentDb.getDepartmentByCategory(1),
                ReportOption = 0,
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ARPackageDeal(ARPackageDeal viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getARPatientPackage(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.DepartmentId);
                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if (viewModel.ReportOption == 0)
                            reportDocPath += "ARPackageByDoctor.rdlc";
                        else
                            reportDocPath += "ARPackageByDepartment.rdlc";

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Package", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("deptName", viewModel.Department));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult PINTransactionSummary()
        {

            var viewModel = new ARPINTransactionSummary()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                ReportOption = 1,
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PINTransactionSummary(ARPINTransactionSummary viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getARTransactionDetails(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.ReportOption);
                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\AROPTransactionDetails.rdlc";
                        var department = viewModel.ReportOption == 1 ? "ALL DEPARTMENT" : "DNT/OPT/OBGN";
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;



                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Transaction", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("department", department));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewModel.CategoryText));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult ARPackageDealDaily()
        {

            var viewModel = new ARPackageDealDaily()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                PackageBillList = arReportDb.getARPackageBills(null)
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ARPackageDealDaily(ARPackageDealDaily viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var packages = viewModel.Packages.Replace("&#39;", "''");
                    var data = arReportDb.getARPackageDealDailyCases(viewModel.StartDate, viewModel.EndDate.AddDays(1), packages);
                    if (data.Rows.Count > 0)
                    {

                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ARPDDayCases.rdlc";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Data", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult CompanyCharges()
        {

            var viewModel = new ARCompanyCharges()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                OPBServiceId = 0,
                CategoryList = categoryDb.getCategories(),
                ServiceList = serviceDb.getALLOPBServices()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CompanyCharges(ARCompanyCharges viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    var data = companyDb.getCompanyCharges(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.OPBServiceId, viewModel.HasBaseCharge, viewModel.BaseCharge);
                    var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ARServiceCharges.rdlc";

                    ReportViewer reportViewer = new ReportViewer();
                    ReportViewerVm reportVM = new ReportViewerVm();
                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ServiceCharges", data));
                    reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                    reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewModel.CategoryText));
                    if (viewModel.HasBaseCharge)
                    {
                        reportViewer.LocalReport.SetParameters(new ReportParameter("charge", "MORE THAN " + viewModel.BaseCharge.ToString("0.00") + " SAR"));
                    }

                    reportViewer.SizeToReportContent = true;
                    reportViewer.Width = Unit.Percentage(100);
                    reportViewer.Height = Unit.Percentage(100);
                    reportVM.ReportViewer = reportViewer;

                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                    return PartialView("~/Areas/ManagementReports/Views/Shared/_dataTableView.cshtml", data);

                }
            }
            return Content("");
        }

        public ActionResult ARAverageClaim()
        {
            var viewModel = new ARAverageClaim()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories()

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ARAverageClaim(ARAverageClaim viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = companyDb.getAverageClaim(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);
                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\CompanyAverageClaim.rdlc";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;



                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Claims", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult PhysiotherapyBilling()
        {
            var viewModel = new ARPhysiotherapyBilling()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories()

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PhysiotherapyBilling(ARPhysiotherapyBilling viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getPhysiotherapyBilling(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);
                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\PhysiotherapyBilling.rdlc";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Billing", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewModel.CategoryText.ToString()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult IPServiceRequiredApproval()
        {
            var viewModel = new ARIPServiceRequiredApproval()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                CategoryList = categoryDb.getCategories()

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult IPServiceRequiredApproval(ARIPServiceRequiredApproval viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var searchString = viewModel.ServiceName != null ? viewModel.ServiceName : String.Empty;
                    var data = arReportDb.getIPServicesRequiredForApproval(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, searchString);
                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ServicesRequiredApproval(IP).rdlc";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Services", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewModel.CategoryText.ToString()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult ActiveServiceItem()
        {
            var viewModel = new ARActiveServiceItem()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),


            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ActiveServiceItem(ARActiveServiceItem viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    var data = itemDb.getActiveServiceItems(viewModel.StartDate, viewModel.EndDate.AddDays(1));

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ActiveServiceItems.rdlc";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Services", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult InPatientIPCR()
        {

            var viewModel = new ARInPatientIPCR()
            {
                SearchByPin = true,
                InvoiceTypeList = new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>(0, "Non-Package Deal"),
                    new KeyValuePair<int, string>(1, "Package Deal")
                }
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult InPatientIPCR(ARInPatientIPCR viewModel)
        {
            if (Request.IsAjaxRequest())
            {


                var header = invoiceDb.getInvoiceHeaderByBillNo(viewModel.BillNo);
                var data = viewModel.InvoiceType == 0 ? arReportDb.getNonPackageIPCR(viewModel.BillNo) : arReportDb.getPackageIPCR(viewModel.BillNo);

                if (data.Rows.Count > 0)
                {
                    var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                    ReportViewer reportViewer = new ReportViewer();
                    ReportViewerVm reportVM = new ReportViewerVm();

                    reportViewer.ProcessingMode = ProcessingMode.Local;

                    if (viewModel.InvoiceType == 0)
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "NonPackageIPCR.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Discounts", arReportDb.getIPCRDiscount(viewModel.BillNo)));
                    }
                    else
                    {
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "PackageIPCR.rdl";
                    }

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IPCR", data));
                    reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");


                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvNo", header.InvoiceNo.ToString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvPin", Global.OrganizationDetails.IssueAuthorityCode + "." + header.RegistrationNumber.ToString("0000000000")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvAdmitDate", header.AdmitDateTime.ToString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvName", header.Name));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvAddress", header.CompanyAddress));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvDischargeDate", header.DischargeDateTime.ToString()));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvCompanyName", header.CompanyName));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvCompanyCode", header.CompanyCode));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvGrade", header.GradeName));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("InvMedId", header.MedIdNumber));

                    reportViewer.SizeToReportContent = true;
                    reportViewer.Width = Unit.Percentage(100);
                    reportViewer.Height = Unit.Percentage(100);
                    reportVM.ReportViewer = reportViewer;

                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                    return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                }
                else
                {
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));
                }

            }
            return Content(Errors.ReportContent("NO RECORDS FOUND"));
        }

        public ActionResult CompanyTariffCodes()
        {

            var viewModel = new ARCompanyTariffCodes()
            {
                CategoryList = categoryDb.getCategories()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyTariffCodes(ARCompanyTariffCodes viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    var data = companyDb.getCompanyTariffByCategory(viewModel.CategoryId);

                    if (data.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "CompanyTariffCodes.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Company", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewModel.CategoryText));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
            }
            return Content(Errors.ReportContent("NO RECORDS FOUND"));
        }

        public ActionResult BillingByCategory()
        {

            var viewModel = new ARBillingByCategory()
            {
                CategoryList = categoryDb.getCategories(),
                PatientTypeList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("A", "ALL"),
                    new KeyValuePair<string, string>("I", "IN-PATIENT"),
                    new KeyValuePair<string, string>("O", "OUT-PATIENT")
                },
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult BillingByCategory(ARBillingByCategory viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    var data = String.IsNullOrEmpty(viewModel.JsonStrCategoryIds) ? new DataTable() : arReportDb.getSummarizedBillingByCategory(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.PatientType, viewModel.JsonStrCategoryIds);

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();

                        var patientTypeHeader = "";

                        if (viewModel.PatientType == "I")
                        {
                            patientTypeHeader = "In-Patient";
                        }
                        else if (viewModel.PatientType == "O")
                        {
                            patientTypeHeader = "In-Patient";
                        }
                        else
                        {
                            patientTypeHeader = "In-Patient and Out-Patient";
                        }

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "SummarizedBillingReportByCategory.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Bills", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("PatientType", patientTypeHeader));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
            }
            return Content(Errors.ReportContent("RECORDS NOT FOUND", "style='color:white; padding-top:13%;color:#E6E7E8'"));
        }

        public ActionResult ServiceFrequency()
        {

            var viewModel = new ARServiceFrequency()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ServiceFrequency(ARServiceFrequency viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    var data = arReportDb.getActiveServiceFrequency(viewModel.StartDate, viewModel.EndDate.AddDays(1));

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "ActiveServiceFrequency.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Service", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString()));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
            }
            return Content(Errors.ReportContent("NO RECORDS FOUND"));
        }

        public ActionResult OPFrontLineBillingTracker()
        {

            var viewModel = new AROPFrontLineBillingTracker()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OPFrontLineBillingTracker(AROPFrontLineBillingTracker viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var strCriteria = "";

                    if (viewModel.FilterReceiptNo && viewModel.FilterBillDate)
                        strCriteria = "3";
                    else if (!viewModel.FilterReceiptNo && viewModel.FilterBillDate)
                        strCriteria = "1";
                    else if (viewModel.FilterReceiptNo && !viewModel.FilterBillDate)
                    {
                        strCriteria = "2";
                        //supply any valid date
                        viewModel.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        viewModel.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    }
                    else
                        strCriteria = "0";


                    var data = arReportDb.getOPFrontlineTrack(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.BillNo, strCriteria);

                    if (data.Rows.Count > 0)
                    {

                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "OPFrontlineTracker.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Tracker", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString()));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
            }
            return Content(Errors.ReportContent("NO RECORDS FOUND"));
        }

        public ActionResult PinWiseOPBilling()
        {

            var viewModel = new ARCancelledServices()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
                //PatientTypeList = new List<KeyValuePair<string, string>>() { 
                // new KeyValuePair<string, string>("ALL", "ALL"),
                // new KeyValuePair<string, string>("IP", "In-Patient"),
                // new KeyValuePair<string, string>("OP", "Out-Patient"),
                //}
            };
            return View(viewModel);
        }
        public ActionResult CancelledServices()
        {

            var viewModel = new ARCancelledServices()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
                PatientTypeList = new List<KeyValuePair<string, string>>() { 
                 new KeyValuePair<string, string>("ALL", "ALL"),
                 new KeyValuePair<string, string>("IP", "In-Patient"),
                 new KeyValuePair<string, string>("OP", "Out-Patient"),
                }
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CancelledServices(ARCancelledServices viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getCancelledServices(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.CompanyId, viewModel.PatientType);

                    if (data.Rows.Count > 0)
                    {

                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "CancelledServices.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Services", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", viewModel.EndDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("caption", viewModel.CategoryText + " - (" + (viewModel.PatientType == "ALL" ? "In-Patient & Out-Patient" : viewModel.PatientTypeText) + ")"));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
            }
            return Content(Errors.ReportContent("NO RECORDS FOUND"));
        }

        public ActionResult OPBillPinWise()
        {

            var viewModel = new AROPBillPinWise()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
                PinId = "0"

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OPBillPinWise(AROPBillPinWise viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    string choice = "0";
                    string pinstr = "0";
                    //i replicate the old sp please dont get mad at me im not the one XD


                    if (viewModel.PinId == "" || viewModel.PinId == "0" || viewModel.PinId == null && viewModel.CategoryId == 0 && viewModel.CompanyId == 0)
                    { choice = "1"; }
                    if (viewModel.PinId != "" && viewModel.PinId != "0" && viewModel.PinId != null && viewModel.PinId.Length > 5)
                    {
                        choice = "2";
                        pinstr = Global.OrganizationDetails.IssueAuthorityCode + "." + viewModel.PinId.Trim();
                    }
                    if (viewModel.CategoryId > 0)
                    { choice = "3"; }
                    if (viewModel.CompanyId > 0)
                    { choice = "4"; }

                    var data = arReportDb.getOPBillPinWise(viewModel.StartDate, viewModel.EndDate.AddDays(1), pinstr, choice, viewModel.CategoryId, viewModel.CompanyId);

                    if (data.Rows.Count > 0)
                    {

                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath + "ArReports_PinWiseOPBill.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString()));
                        //  reportViewer.LocalReport.SetParameters(new ReportParameter("caption", viewModel.CategoryText + " - (" + (viewModel.PatientType == "ALL" ? "In-Patient & Out-Patient" : viewModel.PatientTypeText) + ")"));


                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
            }
            return Content(Errors.ReportContent("NO RECORDS FOUND"));
        }

        public JsonResult getAdmissionByPin(int pin)
        {

            return Json(arReportDb.getAdmissionByPin(pin), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAdmissionBillNo(int billNo)
        {

            return Json(arReportDb.getAdmissionBillNo(billNo), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getARPackageBills(string searchString)
        {

            return Json(arReportDb.getARPackageBills(searchString), JsonRequestBehavior.AllowGet);
        }

        public JsonResult isUserARDoctor()
        {
            var employee = empDb.getEmployeeByOperatorId(base.OperatorId);
            var users = userDb.getUser(employee.EmployeeId.ToString());

            //return Json(new { isARDoctor = users.Any(i => i.GroupId == (int)Enumerations.MCRSGroupCategory.ARDOCTORS) }, JsonRequestBehavior.AllowGet);
            return Json(new { isARDoctor = true }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getICD10Codes(string description, int page = 1)
        {

            int recordCount = icd10codeDb.countICD10Code(description);
            int pageSize = 10;
            int totalPageCount = (int)Math.Ceiling((double)recordCount / pageSize);

            int start = (page - 1) * pageSize;
            int end = start + pageSize;

            var codes = icd10codeDb.getICD10CodesTakeAndSkip(description, start, end);

            return Json(new { ICD10 = codes, TotalPage = totalPageCount, PageSize = pageSize, CurrentPage = (page) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getVisitDates(int regNumber)
        {
            var visidates = arReportDb.getVisitDates(regNumber);
            return Json(JsonConvert.SerializeObject(visidates), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getConsultationDetail(int visitId)
        {
            var consultaTionDetail = arReportDb.getARConsultationVisitDetail(visitId);
            consultaTionDetail = consultaTionDetail.VisitId > 0 ? consultaTionDetail : arReportDb.getClinicVisitDetails(visitId);

            return Json(consultaTionDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDiagnosis(int visitId)
        {

            return Json(this.getARDiagnosis(visitId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCompanyByCategory(int categoryId)
        {

            return Json(companyDb.getCompanyByCategory(categoryId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getGradeByCompanyId(int companyId)
        {

            return Json(gradeDb.getGradeByCompanyId(companyId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTestNameAndCode(string searchString)
        {

            return Json(testDb.getTestNameAndCode(searchString), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCompaniesWithOPBillDetail(DateTime startDate, DateTime endDate, int id)
        {

            return Json(companyDb.getCompaniesWithOpBillDetail(startDate, endDate, id), JsonRequestBehavior.AllowGet);
        }

        private ReportViewer generateUCAFPrintReportViewer(int visitId)
        {
            var hdrUCAF = arReportDb.getHDRUCAF(visitId);
            var consultaTionDetail = arReportDb.getARConsultationVisitDetail(visitId);
            consultaTionDetail = consultaTionDetail.VisitId > 0 ? consultaTionDetail : arReportDb.getClinicVisitDetails(visitId);
            var arExaminations = arReportDb.getARExamination(visitId);
            var arDiagnosis = this.getARDiagnosis(visitId);
            var clinicalDetail = new ClinicalDetail(arReportDb.getClinicalDetail(visitId));
            var illnessType = arReportDb.getIllnessType(visitId);
            var testOrder = arReportDb.getClinicalTestOrdetUCAF(visitId);


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            var deptcode = arReportDb.getClinicalVisit(visitId).DepartmentCode;

            if (deptcode != "DNT")
            {

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_UCAF1.rdlc";

                //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_UCAF.rdlc";

            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_DCAF.rdl";

            }


            ReportDataSource datasourceItem = new ReportDataSource("UCAFTestOrder", testOrder);
            reportViewer.LocalReport.DataSources.Add(datasourceItem);
            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");


            reportViewer.LocalReport.SetParameters(new ReportParameter("InsuCode", hdrUCAF.InsuCode));
            reportViewer.LocalReport.SetParameters(new ReportParameter("PIN", hdrUCAF.PIN));
            reportViewer.LocalReport.SetParameters(new ReportParameter("VisitDept", hdrUCAF.VisitDept));
            reportViewer.LocalReport.SetParameters(new ReportParameter("DateVisit", hdrUCAF.VisitDate.ToString("dd-MMM-yyyy hh:mm tt")));
            reportViewer.LocalReport.SetParameters(new ReportParameter("PTName", hdrUCAF.PTName));
            reportViewer.LocalReport.SetParameters(new ReportParameter("VisitType", hdrUCAF.VisitType));
            reportViewer.LocalReport.SetParameters(new ReportParameter("IDCard", hdrUCAF.IDCard));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Sex", hdrUCAF.Sex));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Age", hdrUCAF.Age.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ExpiryDate", hdrUCAF.ExpiryDate.ToString("dd-MMM-yyyy")));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Class", hdrUCAF.Class));
            reportViewer.LocalReport.SetParameters(new ReportParameter("DocName", hdrUCAF.Doctorname));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Policyno", hdrUCAF.Policyno));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Company", hdrUCAF.company));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ChiefComplaints", consultaTionDetail.ChiefComplaints));
            reportViewer.LocalReport.SetParameters(new ReportParameter("SignificantSigns", this.parseARExamination(arExaminations)));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Diagnosis", this.parseARDiagnosis(arDiagnosis)));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Temp", clinicalDetail.Temperature));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Pulse", clinicalDetail.Pulse));
            reportViewer.LocalReport.SetParameters(new ReportParameter("BP", clinicalDetail.BloodPressure));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Illness", this.parseARIllnessType(illnessType)));
            reportViewer.LocalReport.SetParameters(new ReportParameter("TreatmentPlan", consultaTionDetail.TreatmentPlan));
            reportViewer.LocalReport.SetParameters(new ReportParameter("SGHBranch", Global.OrganizationDetails.Name + "-" + Global.OrganizationDetails.City));

            reportViewer.SizeToReportContent = true;
            reportViewer.ShowPrintButton = true;
            return reportViewer;
        }

        private ReportViewer generateUCAFPrintReportViewer(string visitIdJsonArray)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_UCAF_jfj.rdl";

            var records = arReportDb.getUCAFRecordForPrinting(visitIdJsonArray);

            ReportDataSource datasourceItem = new ReportDataSource("UCAFRecords", records);
            reportViewer.LocalReport.DataSources.Add(datasourceItem);

            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");


            reportViewer.SizeToReportContent = true;
            reportViewer.ShowPrintButton = true;
            return reportViewer;
        }

        private string parseARExamination(List<ARExaminationModel> arExaminations)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var exam in arExaminations)
            {
                builder.Append(exam.MainSymptom);
                builder.Append("    ");
                builder.Append(exam.SubSymptom);
                builder.Append(" : ");
                builder.Append(exam.Description);
                builder.Append("\n");
            }

            return builder.ToString();
        }

        private string parseARDiagnosis(List<ARDiagnosisModel> arDiagnosisModel)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var diagnosis in arDiagnosisModel)
            {
                builder.Append(arDiagnosisModel.IndexOf(diagnosis) + 1);
                builder.Append(".)");
                builder.Append(diagnosis.ICDDescription);
                builder.Append(" (" + diagnosis.ICDCode + ")");

                if (arDiagnosisModel.IndexOf(diagnosis) < arDiagnosisModel.Count() - 1)
                {
                    builder.Append(", ");
                }
            }

            return builder.ToString();

        }

        private string parseARIllnessType(List<ARIllnessTypeModel> illnessType)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var illness in illnessType)
            {

                builder.Append(illness.Name);
                builder.Append(" : ");
                builder.Append(illness.Condition);
                builder.Append("     ");

            }

            return builder.Length > 0 ? builder.ToString() : "Nothing";
        }

        private List<ARDiagnosisModel> getARDiagnosis(int visitId)
        {
            var arDiagnosis = arReportDb.getARDiagnosis(visitId);

            if (arDiagnosis.Count() == 0)
            {
                arDiagnosis = arReportDb.getICDDiagnosis(visitId);
            }

            return arDiagnosis;
        }


        public ActionResult BillingReportDeptWise()
        {
            var viewModel = new ARBillingReportDeptWise()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = 0,
                PatientTypeId = 0,
                CategoryList = categoryDb.getCategories(),
                PatientTypeList = new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int,string>(0,"ALL"),
                    new KeyValuePair<int,string>(1,"IN-PATIENT"),
                    new KeyValuePair<int,string>(2,"OUT-PATIENT")
                }

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult BillingReportDeptWise(ARBillingReportDeptWise viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = arReportDb.getBillingDepartmentWise(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.PatientTypeId);

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ArReports_BillingWise.rdl";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("catname", viewModel.CategoryText.ToString()));
                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult PatientUnderReferral()
        {

            var viewModel = new AROPBillPinWise()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PatientUnderReferral(ARBillingReportDeptWise viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    //        [MCRS].[ARReport_ReferralPatients]
                    //@StartDate Date,@EndDate Date,@CatId bigint


                    var data = arReportDb.getReferralPatients(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ArReports_ReferralPatients.rdl";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        // reportViewer.LocalReport.SetParameters(new ReportParameter("catname", viewModel.CategoryText.ToString()));
                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult ExpiringPolicies()
        {

            var viewModel = new AROPBillPinWise()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ExpiringPolicies(AROPBillPinWise viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    string days = Request.Form["days"];
                    int IntDays = Convert.ToInt32(days);
                    var data = arReportDb.getExpiringAccounts(viewModel.StartDate, IntDays);

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ArReports_ExpiringAccount.rdl";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        // reportViewer.LocalReport.SetParameters(new ReportParameter("catname", viewModel.CategoryText.ToString()));
                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult PinWiseOPInvoicePrinting()
        {
            //frmInvoicePrinting

            var viewModel = new AROPBillPinWise()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDb.getCategories(),
                PinId = "0"
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult PinWiseOPInvoicePrinting(AROPBillPinWise viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    int pin = Convert.ToInt32(viewModel.PinId);

                    var data = arReportDb.getOPInvoiceBillPriting(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, pin);

                    if (data.Rows.Count > 0)
                    {
                        var reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ARReport_getInvoicePrinting.rdl";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));
                        // reportViewer.LocalReport.SetParameters(new ReportParameter("catname", viewModel.CategoryText.ToString()));
                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }
            return Content("");
        }

        public ActionResult SummaryOfAccounts()
        {
            var viewModel = new ARSummaryOfAccounts()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                SubCategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                SubCategoryList = categoryDb.getSubCategories(),
                BankDetails = true,
                Type = 1
            };
            return View(viewModel);
        }

        public ActionResult YearlySummaryOfAccounts()
        {
            var viewModel = new ARSummaryOfAccounts()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                SubCategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                SubCategoryList = categoryDb.getSubCategories(),
                BankDetails = true,
                AfterCoveringLetter = true,
                isAfterCoveringLetter = 0,
                Type = 1
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult YearlySummaryOfAccounts(ARSummaryOfAccounts viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    string deptidArray = Request.Form["SelectedCategoryIdsList"];


                    var CAtId = deptidArray;

                    var data = new DataTable();

                    utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDate);


                    string SubCategoryName = viewModel.SubCategory == "ALL" ? "0" : viewModel.SubCategory;
                    data = arReportDb.getMonthlyIncomeCredit(viewModel.StartDate, viewModel.EndDate, viewModel.StartDate, CAtId, viewModel.CompanyId, Request.Form["reportdetails"], viewModel.GradeId);

                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARReport_getmonthlycreditincome.rdl";

                        var stdate = "";
                        var endate = "";
                        if (Request.Form["reportdetails"] == "yearly")
                        {
                            stdate = " " + viewModel.StartDate.ToString("yyyy") + " - Jan";
                            endate = " " + viewModel.StartDate.ToString("yyyy") + " - Dec";
                        }
                        else
                        {
                            stdate = viewModel.StartDate.ToString("dd-MMM-yyyy");
                            endate = viewModel.EndDate.ToString("dd-MMM-yyyy");
                        }



                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("parastartdate", stdate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enddate", endate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return Content("");
        }



        public ActionResult MonthlyCreditSubCatWise()
        {
            var viewModel = new ARSummaryOfAccounts()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                SubCategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                SubCategoryList = categoryDb.getSubCategories(),
                BankDetails = true,
                Type = 1
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MonthlyCreditSubCatWise(ARSummaryOfAccounts viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {


                    var data = new DataTable();

                    utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDate);


                    string SubCategoryName = viewModel.SubCategory == "ALL" ? "0" : viewModel.SubCategory;
                    data = arReportDb.getMonthlyIncomeCreditSubCategoryWise(viewModel.StartDate, viewModel.EndDate, viewModel.StartDate, viewModel.CategoryId, viewModel.CompanyId, Request.Form["reportdetails"], viewModel.GradeId);

                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        //if (Request.Form["reportdetails"] == "yearly")
                        //{
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARReport_getmonthlycreditincome-new.rdl";//ARReport_getSubCatMonthlyCreditIncome.rdl";

                        //}

                        //else {
                        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARReport_getSubCatMonthlyCreditIncomePerMonth.rdl";

                        //}

                        var stdate = "";
                        var endate = "";
                        if (Request.Form["reportdetails"] == "yearly")
                        {
                            stdate = " " + viewModel.StartDate.ToString("yyyy") + " - Jan";
                            endate = " " + viewModel.StartDate.ToString("yyyy") + " - Dec";
                        }
                        else
                        {
                            stdate = viewModel.StartDate.ToString("dd-MMM-yyyy");
                            endate = viewModel.EndDate.ToString("dd-MMM-yyyy");
                        }

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", stdate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("endddate", endate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Common.Global.OrganizationDetails.Name + " - " + Common.Global.OrganizationDetails.City.ToUpper()));

                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }
                }
            }

            return Content("");
        }


        public ActionResult DoctorRevenueOP()
        {
            var vieModel = new ARReportsDoctorRevenueOP()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

                PatientBillTypes = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, Enumerations.PatientBillType.CHARGE.ToString())
                                },

                DepartmentList = departmentDB.getAllDepartment(),

            };

            return View(vieModel);
        }



        [HttpPost]
        public ActionResult DoctorRevenueOP(ARReportsDoctorRevenueOP financeReportsOPRevenue)
        {

            var vm = financeReportsOPRevenue;
            var filters = "Bill Type : " + vm.PatientBillType.ToString();

            if (vm.DoctorId > 0)
            {
                var doc = empDb.getEmployeeByOperatorId(vm.DoctorId);

                filters += " , Doctors Code : " + doc.EmpCode;
            }

            if (vm.DepartmentId > 0)
            {
                var dept = departmentDB.getById(vm.DepartmentId);

                filters += " , Department : " + dept.Name;
            }



            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\Report_DoctorRevenueOP.rdl";

                DataTable reportData = opRevenueDB.getDoctorRevenueOP(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.DepartmentId, vm.DoctorId);


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("OPRevenue", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", vm.StartDate.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", vm.EndDate.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("filters", filters));
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

        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctors(searchString), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SummaryOfAccounts(ARSummaryOfAccounts viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = new DataTable();
                    var data2 = new DataTable();
                    //
                    utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDate);
                    string SubCategoryName = viewModel.SubCategory == "ALL" ? "0" : viewModel.SubCategory;

                    if (viewModel.AfterCoveringLetter == true)
                    {
                        viewModel.isAfterCoveringLetter = 1;
                    }

                    if ((viewModel.CategoryId == 23 || viewModel.CategoryId == 70) && viewModel.Type == 0) //MEDNET
                    {
                        data = arReportDb.getSummaryOfAccountsORWithSubCategoryName_MedNet(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, SubCategoryName);
                        data2 = arReportDb.getSummaryOfAccountsORWithSubCategoryName_MedNet_green(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, SubCategoryName);

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_MEDNET_Green.rdl";

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StatementSummary", data));
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Green", data2));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToString()));


                        if (viewModel.BankDetails == true)
                        {

                            var bankHeader = "";
                            var coName = "";
                            var hospital = "";
                            var accno = "";
                            var bankName = "";
                            var address = "";
                            var subcategory = "";

                            var arGenInfo = genInfoDB.getARGeneralInformation();

                            if (!String.IsNullOrEmpty(arGenInfo.BankAccountName) && !String.IsNullOrWhiteSpace(arGenInfo.BankAccountName))
                            {
                                bankHeader = "Please send the payments to the following Bank Accounts:";
                                coName = arGenInfo.BankAccountName;
                                hospital = "SAUDI GERMAN HOSPITAL";
                                accno = "Account No. : " + arGenInfo.BankAccountNo;
                                bankName = "Bank Name   : " + arGenInfo.BankName;
                                address = arGenInfo.BankBranchName + " " + arGenInfo.BankAddress;
                                subcategory = viewModel.SubCategory;
                            }

                            reportViewer.LocalReport.SetParameters(new ReportParameter("bankHeader", bankHeader));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("coName", coName));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("hospital", hospital));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("accNo", accno));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("bankName", bankName));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("address", address));

                        }

                        reportViewer.LocalReport.SetParameters(new ReportParameter("Category", viewModel.Category));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("SubCategory", viewModel.SubCategory));
                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);




                    }
                    else
                    {

                        data = arReportDb.getSummaryOfAccountsORWithSubCategoryName(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, SubCategoryName, viewModel.isAfterCoveringLetter);

                        if (data.Rows.Count > 0)
                        {
                            ReportViewer reportViewer = new ReportViewer();
                            ReportViewerVm reportVM = new ReportViewerVm();
                            reportViewer.ProcessingMode = ProcessingMode.Local;


                           // if ((viewModel.CategoryId == 70 || viewModel.CategoryId == 78 || viewModel.CategoryId == 25) && viewModel.Type == 0)
                         if (( viewModel.CategoryId == 78 || viewModel.CategoryId == 25) && viewModel.Type == 0)
                            {
                                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_MEDNET.rdl";
                            }
                            else

                             if (viewModel.CategoryId == 23 && viewModel.Type == 1)
                             {
                                 reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_MEDNET.rdl";
                             }
                             else if (viewModel.CategoryId == 24 && viewModel.Type == 1)
                                {
                                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_NCCI.rdl";
                                }
                                else
                                {
                                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_GroupBySubCategory.rdlc";
                                }

                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StatementSummary", data));
                            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToString()));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToString()));


                            if (viewModel.BankDetails == true)
                            {

                                var bankHeader = "";
                                var coName = "";
                                var hospital = "";
                                var accno = "";
                                var bankName = "";
                                var address = "";
                                var subcategory = "";

                                var arGenInfo = genInfoDB.getARGeneralInformation();

                                if (!String.IsNullOrEmpty(arGenInfo.BankAccountName) && !String.IsNullOrWhiteSpace(arGenInfo.BankAccountName))
                                {
                                    bankHeader = "Please send the payments to the following Bank Accounts:";
                                    coName = arGenInfo.BankAccountName;
                                    hospital = "SAUDI GERMAN HOSPITAL";
                                    accno = "Account No. : " + arGenInfo.BankAccountNo;
                                    bankName = "Bank Name   : " + arGenInfo.BankName;
                                    address = arGenInfo.BankBranchName + " " + arGenInfo.BankAddress;
                                    subcategory = viewModel.SubCategory;
                                }

                                reportViewer.LocalReport.SetParameters(new ReportParameter("bankHeader", bankHeader));
                                reportViewer.LocalReport.SetParameters(new ReportParameter("coName", coName));
                                reportViewer.LocalReport.SetParameters(new ReportParameter("hospital", hospital));
                                reportViewer.LocalReport.SetParameters(new ReportParameter("accNo", accno));
                                reportViewer.LocalReport.SetParameters(new ReportParameter("bankName", bankName));
                                reportViewer.LocalReport.SetParameters(new ReportParameter("address", address));

                            }
                            reportViewer.LocalReport.SetParameters(new ReportParameter("Category", viewModel.Category));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("SubCategory", viewModel.SubCategory));
                            reportViewer.SizeToReportContent = true;
                            reportViewer.Width = Unit.Percentage(100);
                            reportViewer.Height = Unit.Percentage(100);
                            reportVM.ReportViewer = reportViewer;

                            System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                            System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                            return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                        }
                        else
                        {
                            return Content(Errors.ReportContent("NO RECORDS FOUND"));
                        }
                    }
                }
            }

            return Content("");
        }



        public ActionResult SummaryOfAccountswithVAT()
        {
            var viewModel = new ARSummaryOfAccounts()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                SubCategoryId = 0,
                CategoryList = categoryDb.getCategories(),
                SubCategoryList = categoryDb.getSubCategories(),
                BankDetails = true,
                AfterCoveringLetter = true,
                Type = 1 
             
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SummaryOfAccountswithVAT(ARSummaryOfAccounts viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = new DataTable();
                    var data2 = new DataTable();
                    //
                   // utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDate);
                    string SubCategoryName = viewModel.SubCategory == "ALL" ? "0" : viewModel.SubCategory;

                    if (viewModel.AfterCoveringLetter == true)
                    {
                        viewModel.isAfterCoveringLetter = 1;
                    }

                    if (viewModel.Type == 0)
                    {//summary 
                        data = arReportDb.getSummaryOfAccountsORWithSubCategoryNameWithVAT_Summary(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, SubCategoryName, viewModel.isAfterCoveringLetter, viewModel.SubCategoryId);

                    }
                    else
                    {
                        //detailed
                        data = arReportDb.getSummaryOfAccountsORWithSubCategoryNameWithVAT(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, SubCategoryName, viewModel.isAfterCoveringLetter);

                    }






                    if (data.Rows.Count > 0)
                    {

                        var bankHeader = "";
                        var coName = "";
                        var hospital = "";
                        var accno = "";
                        var bankName = "";
                        var address = "";
                        var subcategory = "";

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if (viewModel.Type == 0)
                        {//summary 
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_VAT_GroupbySummary.rdl";
                        }
                        else
                        {
                            //detailed
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_GroupBySubCategorywithVAT.rdl";

                        }

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StatementSummary", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToShortDateString()));

                        reportViewer.LocalReport.SetParameters(new ReportParameter("Category", viewModel.Category));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("SubCategory", viewModel.SubCategory));



                        if (viewModel.BankDetails == true)
                        {
                            var arGenInfo = genInfoDB.getARGeneralInformation();

                            if (!String.IsNullOrEmpty(arGenInfo.BankAccountName) && !String.IsNullOrWhiteSpace(arGenInfo.BankAccountName))
                            {
                                bankHeader = "Please send the payments to the following Bank Accounts:";
                                coName = arGenInfo.BankAccountName;
                                hospital = "SAUDI GERMAN HOSPITAL";
                                accno = "Account No. : " + arGenInfo.BankAccountNo;
                                bankName = "Bank Name   : " + arGenInfo.BankName;
                                address = arGenInfo.BankBranchName + " " + arGenInfo.BankAddress;
                                subcategory = viewModel.SubCategory;
                            }

                            reportViewer.LocalReport.SetParameters(new ReportParameter("bankHeader", bankHeader));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("coName", coName));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("hospital", hospital));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("accNo", accno));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("bankName", bankName));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("address", address));

                        }



                        reportViewer.SizeToReportContent = true;
                        reportViewer.Width = Unit.Percentage(100);
                        reportViewer.Height = Unit.Percentage(100);
                        reportVM.ReportViewer = reportViewer;

                        System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                        System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                        return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }
                    else
                    {
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                    }

                }
            }

            return Content("");
        }


 

        public ActionResult SummaryOfAccountswithVATEXCEL(ARSummaryOfAccounts viewModel)
        {
           
            List<SummaryOfAccounts_DatatableLIST> list = arReportDb.SummaryOfAccount_EXCEL(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, 0, 0, viewModel.Type, "0", (viewModel.AfterCoveringLetter == true ? 1 : 0));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            var result = new ContentResult
            {
                Content = serializer.Serialize(new { list = list ?? new List<SummaryOfAccounts_DatatableLIST>() }),
                ContentType = "application/json"
            };
            return result;
        }


        //[HttpPost]
        //public ActionResult SummaryOfAccountswithVATEXCEL(ARSummaryOfAccounts viewModel)
        //{


        //    var data = new DataTable();
        //    var data2 = new DataTable();

        //    utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDateExcel);
        //    string SubCategoryNameExcel = viewModel.SubCategoryExcel == "ALL" ? "0" : viewModel.SubCategoryExcel;
        //    SubCategoryNameExcel = SubCategoryNameExcel == null ? "0" : SubCategoryNameExcel;
        //    if (viewModel.AfterCoveringLetterExcel == true)
        //    {
        //        viewModel.isAfterCoveringLetterExcel = 1;
        //    }

        //    if (viewModel.TypeExcel == 0)
        //    {//summary 
        //        List<SummaryOfAccounts_VATSummaryEXCEL> list = arReportDb.getSummaryOfAccountsORWithSubCategoryNameWithVAT_SummaryEXCEL(viewModel.StartDateExcel, viewModel.EndDateExcel.AddDays(1), viewModel.CategoryIdExcel, viewModel.GradeIdExcel, viewModel.CompanyIdExcel, viewModel.TypeExcel, SubCategoryNameExcel, viewModel.isAfterCoveringLetterExcel, viewModel.SubCategoryIdExcel);

        //        //summary 
        //        //  reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_VAT_GroupbySummary.rdl";




        //        using (ExcelPackage pck = new ExcelPackage())
        //        {
        //            ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Sheet1");


        //            worksheet.Cells[1, 1].Value = "Insurance Company";
        //            worksheet.Cells[1, 2].Value = "IP Count";
        //            worksheet.Cells[1, 3].Value = "OP Count";
        //            worksheet.Cells[1, 4].Value = "Total Count";
        //            worksheet.Cells[1, 5].Value = "IP Gross Amt";
        //            worksheet.Cells[1, 6].Value = "IP Net Amt";
        //            worksheet.Cells[1, 7].Value = "IP VAT";
        //            worksheet.Cells[1, 8].Value = "OP Gross Amt";
        //            worksheet.Cells[1, 9].Value = "OP Net Amt";
        //            worksheet.Cells[1, 10].Value = "OP VAT";
        //            worksheet.Cells[1, 11].Value = "TOTAL Gross Amt";
        //            worksheet.Cells[1, 12].Value = "TOTAL Net Amt";
        //            worksheet.Cells[1, 13].Value = "TOTAL VAT";


        //            int InvCount = 2;


        //            int CurrSeq = 1;

        //            foreach (var item in list)
        //            {

        //                worksheet.Cells[InvCount, 1].Value = item.InsuranceName;// "Insurance Company";
        //                worksheet.Cells[InvCount, 2].Value = item.IPCount;// "IP Count";
        //                worksheet.Cells[InvCount, 3].Value = item.OPCount;// "OP Count";
        //                worksheet.Cells[InvCount, 4].Value = item.TCount;// "Total Count";
        //                worksheet.Cells[InvCount, 5].Value = item.IPGross;// "IP Gross Amt";
        //                worksheet.Cells[InvCount, 6].Value = item.IPNet;// "IP Net Amt";
        //                worksheet.Cells[InvCount, 7].Value = item.IPVAT;// "IP VAT";
        //                worksheet.Cells[InvCount, 8].Value = item.OPGross;// "OP Gross Amt";
        //                worksheet.Cells[InvCount, 9].Value = item.OPNet;// "OP Net Amt";
        //                worksheet.Cells[InvCount, 10].Value = item.OPVat;// "OP VAT";
        //                worksheet.Cells[InvCount, 11].Value = item.TGross;// "TOTAL Gross Amt";
        //                worksheet.Cells[InvCount, 12].Value = item.TNet;// "TOTAL Net Amt";
        //                worksheet.Cells[InvCount, 13].Value = item.TNetVAT;// "TOTAL VAT";

        //                CurrSeq += 1;

        //                InvCount += 1;
        //            }


        //            Byte[] fileBytes = pck.GetAsByteArray();
        //            Response.Clear();
        //            Response.Buffer = true;

        //            Response.AddHeader("content-disposition", "attachment;filename=ARReport_SummaryOfAcctsWithVAT_summary_" + viewModel.StartDateExcel.ToShortDateString() + "_" + viewModel.EndDateExcel.ToShortDateString() + ".xlsx");



        //            /*
        //             *            new KeyValuePair<int, string>(1,"DISCHARGED"),
        //                                                     new KeyValuePair<int, string>(2,"UNDISCHARGED")
        //                                if (viewModel.PatientBillType == Enumerations.PatientBillType.CASH)
        //                                {
        //                                    // reportViewer.LocalReport.SetParameters(new ReportParameter("PinNo", viewModel.PIN.ToString() ));
        //                                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCash.rdl";
        //                                }
        //                                else
        //                                {
        //                                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCharge.rdl";
        //                                }
        //                                */

        //            // Replace filename with your custom Excel-sheet name.

        //            Response.Charset = "";
        //            Response.ContentType = "application/vnd.ms-excel";
        //            StringWriter sw = new StringWriter();
        //            Response.BinaryWrite(fileBytes);
        //            Response.End();
        //        }









        //    }
        //    else
        //    {
        //        //detailed
        //        List<SummaryOfAccounts_DetailVATEXCEL> list = arReportDb.getSummaryOfAccountsORWithSubCategoryNameWithVATEXCEL(viewModel.StartDateExcel, viewModel.EndDateExcel.AddDays(1), viewModel.CategoryIdExcel, viewModel.GradeIdExcel, viewModel.CompanyIdExcel, viewModel.TypeExcel, SubCategoryNameExcel, viewModel.isAfterCoveringLetterExcel);


        //        //detailed
        //        //   reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_GroupBySubCategorywithVAT.rdl";



        //        using (ExcelPackage pck = new ExcelPackage())
        //        {
        //            ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Sheet1");


        //            worksheet.Cells[1, 1].Value = "Insurance Company";
        //            worksheet.Cells[1, 2].Value = "IP Count";
        //            worksheet.Cells[1, 3].Value = "OP Count";
        //            worksheet.Cells[1, 4].Value = "Total Count";
        //            worksheet.Cells[1, 5].Value = "IP Gross Amt";
        //            worksheet.Cells[1, 6].Value = "IP Discount Amt";
        //            worksheet.Cells[1, 7].Value = "IP Deduct Amt";
        //            worksheet.Cells[1, 8].Value = "IP Net";
        //            worksheet.Cells[1, 9].Value = "IP VAT";
        //            worksheet.Cells[1, 10].Value = "OP Gross Amt";
        //            worksheet.Cells[1, 11].Value = "OP Discount Amt";
        //            worksheet.Cells[1, 12].Value = "OP Deduct Amt";
        //            worksheet.Cells[1, 13].Value = "OP Net";
        //            worksheet.Cells[1, 14].Value = "OP VAT";
        //            worksheet.Cells[1, 15].Value = "Total Gross";
        //            worksheet.Cells[1, 16].Value = "Total Discount";
        //            worksheet.Cells[1, 17].Value = "Total Deduct";
        //            worksheet.Cells[1, 18].Value = "Total NET";
        //            worksheet.Cells[1, 19].Value = "Total VAT";
        //            worksheet.Cells[1, 20].Value = "Total Due";

        //            int InvCount = 2;

        //            int CurrSeq = 1;

        //            foreach (var item in list)
        //            {


        //                worksheet.Cells[InvCount, 1].Value = item.CCode+"-"+item.CompanyName;//"Insurance Company";
        //                worksheet.Cells[InvCount, 2].Value = item.IPCount;//"IP Count";
        //                worksheet.Cells[InvCount, 3].Value = item.OPCount;//"OP Count";
        //                worksheet.Cells[InvCount, 4].Value = item.TCount;//"Total Count";
        //                worksheet.Cells[InvCount, 5].Value = item.IPGross;//"IP Gross Amt";
        //                worksheet.Cells[InvCount, 6].Value = item.IPDiscount;//"IP Discount Amt";
        //                worksheet.Cells[InvCount, 7].Value = item.IPDeductable;//"IP Deduct Amt";
        //                worksheet.Cells[InvCount, 8].Value = item.IPNet;//"IP Net";
        //                worksheet.Cells[InvCount, 9].Value = item.IPVat;//"IP VAT";
        //                worksheet.Cells[InvCount, 10].Value = item.OPGross;//"OP Gross Amt";
        //                worksheet.Cells[InvCount, 11].Value = item.OPDiscount;//"OP Discount Amt";
        //                worksheet.Cells[InvCount, 12].Value = item.OPDeductable;//"OP Deduct Amt";
        //                worksheet.Cells[InvCount, 13].Value = item.OPNet;//"OP Net";
        //                worksheet.Cells[InvCount, 14].Value = item.OPVat;//"OP VAT";
        //                worksheet.Cells[InvCount, 15].Value = item.TGross;//"Total Gross";
        //                worksheet.Cells[InvCount, 16].Value = item.TDisc;//"Total Discount";
        //                worksheet.Cells[InvCount, 17].Value = item.TDeduc;//"Total Deduct";
        //                worksheet.Cells[InvCount, 18].Value = item.TNet;//"Total NET";
        //                worksheet.Cells[InvCount, 19].Value = item.NetVat;//"Total VAT";
        //                worksheet.Cells[InvCount, 20].Value = item.TNetVat;//"Total Due";

        //                CurrSeq += 1;

        //                InvCount += 1;
        //            }


        //            Byte[] fileBytes = pck.GetAsByteArray();
        //            Response.Clear();
        //            Response.Buffer = true;

        //            Response.AddHeader("content-disposition", "attachment;filename=ARReport_SummaryOfAcctsWithVAT_detailed_" + viewModel.StartDateExcel.ToShortDateString() + "_" + viewModel.EndDateExcel.ToShortDateString() + ".xlsx");



        //            /*
        //    *            new KeyValuePair<int, string>(1,"DISCHARGED"),
        //                                            new KeyValuePair<int, string>(2,"UNDISCHARGED")
        //                    if (viewModel.PatientBillType == Enumerations.PatientBillType.CASH)
        //                    {
        //                        // reportViewer.LocalReport.SetParameters(new ReportParameter("PinNo", viewModel.PIN.ToString() ));
        //                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCash.rdl";
        //                    }
        //                    else
        //                    {
        //                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCharge.rdl";
        //                    }
        //                    */

        //            // Replace filename with your custom Excel-sheet name.

        //            Response.Charset = "";
        //            Response.ContentType = "application/vnd.ms-excel";
        //            StringWriter sw = new StringWriter();
        //            Response.BinaryWrite(fileBytes);
        //            Response.End();
        //        }

        //    }





        //    return RedirectToAction("Index");



        //}




        #region UCAFversion2

        public ActionResult UCAFv2()
        {
            var viewModel = new UCAFVM();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UCAFv2(UCAFVM ucafVM)
        {
            if (Request.IsAjaxRequest())
            {
                if (ucafVM.VisitId > 0)
                {
                    ReportViewerVm reportVM = new ReportViewerVm();
                    var reportViewer = this.generateUCAFPrintReportViewer_V2(ucafVM.VisitId);

                    reportViewer.SizeToReportContent = true;
                    reportViewer.ShowPrintButton = true;

                    reportVM.ReportViewer = reportViewer;

                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                    return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                }
                else
                {
                    return Content(Errors.ReportContent("NO DOCTORS ENTRY FOUND"));
                }
            }

            return View(ucafVM);
        }


        private ReportViewer generateUCAFPrintReportViewer_V2(int visitId)
        {
            var hdrUCAF = arReportDb.getHDRUCAF(visitId);
            var consultaTionDetail = arReportDb.getARConsultationVisitDetail(visitId);
            consultaTionDetail = consultaTionDetail.VisitId > 0 ? consultaTionDetail : arReportDb.getClinicVisitDetails(visitId);
            var arExaminations = arReportDb.getARExamination(visitId);
            var arDiagnosis = this.getARDiagnosis(visitId);
            var clinicalDetail = new ClinicalDetail(arReportDb.getClinicalDetail(visitId));
            var illnessType = arReportDb.getIllnessType(visitId);
            var testOrder = arReportDb.getClinicalTestOrdetUCAF_version2(visitId);


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            var deptcode = arReportDb.getClinicalVisit(visitId).DepartmentCode;

            if (deptcode != "DNT")
            {

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_UCAF1.rdlc";

                //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_UCAF.rdlc";

            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\Report_DCAF.rdlc";

            }


            ReportDataSource datasourceItem = new ReportDataSource("UCAFTestOrder", testOrder);
            reportViewer.LocalReport.DataSources.Add(datasourceItem);
            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");


            reportViewer.LocalReport.SetParameters(new ReportParameter("InsuCode", hdrUCAF.InsuCode));
            reportViewer.LocalReport.SetParameters(new ReportParameter("PIN", hdrUCAF.PIN));
            reportViewer.LocalReport.SetParameters(new ReportParameter("VisitDept", hdrUCAF.VisitDept));
            reportViewer.LocalReport.SetParameters(new ReportParameter("DateVisit", hdrUCAF.VisitDate.ToString("dd-MMM-yyyy hh:mm tt")));
            reportViewer.LocalReport.SetParameters(new ReportParameter("PTName", hdrUCAF.PTName));
            reportViewer.LocalReport.SetParameters(new ReportParameter("VisitType", hdrUCAF.VisitType));
            reportViewer.LocalReport.SetParameters(new ReportParameter("IDCard", hdrUCAF.IDCard));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Sex", hdrUCAF.Sex));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Age", hdrUCAF.Age.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ExpiryDate", hdrUCAF.ExpiryDate.ToString("dd-MMM-yyyy")));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Class", hdrUCAF.Class));
            reportViewer.LocalReport.SetParameters(new ReportParameter("DocName", hdrUCAF.Doctorname));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Policyno", hdrUCAF.Policyno));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Company", hdrUCAF.company));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ChiefComplaints", consultaTionDetail.ChiefComplaints));
            reportViewer.LocalReport.SetParameters(new ReportParameter("SignificantSigns", this.parseARExamination(arExaminations)));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Diagnosis", this.parseARDiagnosis(arDiagnosis)));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Temp", clinicalDetail.Temperature));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Pulse", clinicalDetail.Pulse));
            reportViewer.LocalReport.SetParameters(new ReportParameter("BP", clinicalDetail.BloodPressure));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Illness", this.parseARIllnessType(illnessType)));
            reportViewer.LocalReport.SetParameters(new ReportParameter("TreatmentPlan", consultaTionDetail.TreatmentPlan));
            reportViewer.LocalReport.SetParameters(new ReportParameter("SGHBranch", Global.OrganizationDetails.Name + "-" + Global.OrganizationDetails.City));

            reportViewer.SizeToReportContent = true;
            reportViewer.ShowPrintButton = true;
            return reportViewer;
        }


        #endregion



        #region IPBILLFinalized
        public ActionResult IpBillARFinalize()
        {
           return View();
        }

        [HttpPost]
        public ActionResult IpBillARFinalize(OtherReportsDateTimeOnly param)
       {
            var vm = param;
            OtherReportsDB OtherReportDB = new OtherReportsDB();
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[ArReport_IPBillARFinalization]");
                reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\AR_REPORTS_IpBillARFinalize.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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



        #endregion

        #region mcrs2019

        public ActionResult ReportDoctorListPatientRecord()
        {
             var viewModel = new ReportDoctorListPatientRecordVM()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day),
                Doctors = employeeDB.getAllDoctors().OrderBy(i => i.FullName).ToList(),
            };

            return View(viewModel);

        }
 
        [HttpPost]
        public ActionResult ReportDoctorListPatientRecord(ReportDoctorListPatientRecordVM qpsMedTower)
        {
            var vm = qpsMedTower;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                  reportData = arReportDb.ReportDoctorListPatientRecord(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId);

               // reportData = qpsreportDb.getMedicalTowerCases(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ARReport_ClinicalVisitLog.rdl";
                //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARReport_ClinicalVisitLog.rdl";
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                //  reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                // reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
               // reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.City));
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", vm.StartDate.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", vm.EndDate.ToString()));

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

        public ActionResult ReportIPUninvoice()
        {
            var viewModel = new ReportDoctorListPatientRecordVM()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day),
                Doctors = employeeDB.getAllDoctors().OrderBy(i => i.FullName).ToList(),
            };

            return View(viewModel);

        }

        [HttpPost]
        public ActionResult ReportIPUninvoice(ReportDoctorListPatientRecordVM qpsMedTower)
        {
            var vm = qpsMedTower;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = arReportDb.ReportIPUninvoice(vm.StartDate, vm.EndDate.AddDays(1));

                // reportData = qpsreportDb.getMedicalTowerCases(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\ARReports\ARReport_IPUnInvoiceReport.rdl";
                //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\ARReport_ClinicalVisitLog.rdl";
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                //  reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                // reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                // reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.City));
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", vm.StartDate.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", vm.EndDate.ToString()));

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


        #endregion

    }
}
