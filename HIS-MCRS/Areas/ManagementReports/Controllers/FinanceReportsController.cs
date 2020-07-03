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
using System.Web.Script.Serialization;


using System.IO;
using System.Security.Permissions;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;



namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class FinanceReportsController : BaseController
    {
        OPRevenueDB opRevenueDB = new OPRevenueDB();
        IPDischargeDB ipDischargeDB = new IPDischargeDB();
        CompanyDB companyDB = new CompanyDB();
        EmployeeDB employeeDB = new EmployeeDB();
        DepartmentDB departmentDB = new DepartmentDB();
        IPRevenueDB ipRevenueDB = new IPRevenueDB();
        CategoryDB categoryDB = new CategoryDB();
        AdjustmentsDB adjustmentDB = new AdjustmentsDB();
        MCRSAllInPatientByDischargeDateTimeDB McrsAllPatiendDB = new MCRSAllInPatientByDischargeDateTimeDB();
        OtherReportsDB OtherReportDB = new OtherReportsDB();
        FinanceReportDB financereportDB
            = new FinanceReportDB();
        StationDB stationdb = new StationDB();

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
        IPBServiceDB ipBServiceDB = new IPBServiceDB();
        CSTAndPhamarcyCostPriceLogic cst = new CSTAndPhamarcyCostPriceLogic();

        public ActionResult Index()
        {
            FeatureID = "1985";
            return View();
        }

        public ActionResult OPRevenue()
        {
            FeatureID = "1985";
            var vieModel = new FinanceReportsOPRevenue()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StartDate2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                BillTypes = new List<KeyValuePair<Enumerations.BillType, string>>{
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.ALL,Enumerations.BillType.ALL.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.NOTCANCELLED,Enumerations.BillType.NOTCANCELLED.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.CANCELLED,Enumerations.BillType.CANCELLED.ToString())
                                },
                PatientBillTypes = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                      //  new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                                },
                PatientBillTypes2 = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                         new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                                },
                DepartmentList = departmentDB.getAllDepartment(),

                SortByCancellationDate = false
            };

            return View(vieModel);
        }

        [HttpPost]
        public ActionResult OPRevenue(FinanceReportsOPRevenue financeReportsOPRevenue)
        {
            var EmpId = Request.Form["EmpId"];
            var ModeofPayment = Request.Form["ModeofPayment"];
            var vm = financeReportsOPRevenue;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                //if (financeReportsOPRevenue.BillType == Enumerations.BillType.NOTCANCELLED)
                //{
                //    reportData = opRevenueDB.getOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                //    if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue.rdl";

                //    else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue_Company.rdl";
                //    else
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllNotCancelledOPRevenue.rdl";

                //}
                //else if (financeReportsOPRevenue.BillType == Enumerations.BillType.CANCELLED)
                //{
                //    reportData = opRevenueDB.getCancelledOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                //    if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCancelled.rdl";

                //    else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCancelled_Company.rdl";
                //    else
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllCancelledOPRevenue.rdl";
                //}
                //else
                //{

                //     reportData = opRevenueDB.getAllOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                //if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue_CancelledAndNotCancelled.rdl";

                //else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCharge_CancelledAndNotCancelled.rdl";

                //else
                //        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllOPRevenue.rdl";
                // }


                if (financeReportsOPRevenue.BillType == Enumerations.BillType.NOTCANCELLED)
                {
                    reportData = opRevenueDB.getOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());
                }
                else if (financeReportsOPRevenue.BillType == Enumerations.BillType.CANCELLED)
                {
                    reportData = opRevenueDB.getCancelledOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());
                }
                else
                {
                    reportData = opRevenueDB.getAllOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                }
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllOPRevenue.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("OPRevenue", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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


        public ActionResult IPRevenue()
        {
            FeatureID = "1985";
            var viewModel = new FinanceReportsIPRevenue()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                BillTypes = new List<KeyValuePair<int, string>>{
                                        // new KeyValuePair<int, string>(0,"ALL"),
                                         new KeyValuePair<int, string>(1,"DISCHARGED"),
                                         new KeyValuePair<int, string>(2,"UNDISCHARGED")
                 },
                PatientBillTypes = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                       // new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                },


                StartDateExcel = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDateExcel = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

                PatientBillTypesExcel = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                        //new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                },
                BillTypesExcel = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(1,"DISCHARGED"),
                                         new KeyValuePair<int, string>(2,"UNDISCHARGED")
                 },

                DepartmentList = departmentDB.getAllDepartment(),
                Services = ipBServiceDB.getServices(),
                ServiceId = 0,
                IsRevenue = false
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult OPRevenueDataTAble(FinanceReportsOPRevenueDataTable financeReportsOPRevenue)
        {
            var EmpId = financeReportsOPRevenue.EmpId;
            var ModeofPayment = financeReportsOPRevenue.ModeofPayment;
            var vm = financeReportsOPRevenue;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;


            if (financeReportsOPRevenue.BillType == Enumerations.BillType.NOTCANCELLED)
            {
                List<OPRevenueDataTAbleTAbleResult> list = opRevenueDB.getOPRevenueDataTAble(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.PIN, EmpId, ModeofPayment.ToString());

                var result = new ContentResult
                {
                    Content = serializer.Serialize(new { list = list ?? new List<OPRevenueDataTAbleTAbleResult>() }),
                    ContentType = "application/json"
                };
                return result;
            }
            else if (financeReportsOPRevenue.BillType == Enumerations.BillType.CANCELLED)
            {
                List<getCancelledOPRevenueDataTAbleTAbleResult> list = opRevenueDB.getCancelledOPRevenueDataTAble(vm.StartDate, vm.EndDate.AddDays(1)
                    , (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.ToString()
                    , EmpId.ToString(), ModeofPayment.ToString());

                var result = new ContentResult
                {
                    Content = serializer.Serialize(new { list = list ?? new List<getCancelledOPRevenueDataTAbleTAbleResult>() }),
                    ContentType = "application/json"
                };
                return result;
            }
            else
            {

                List<getAllOPRevenueDataTAbleTAbleResult> list = opRevenueDB.getAllOPRevenueDataTAble(vm.StartDate, vm.EndDate.AddDays(1)
                    , (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId
                    , vm.SortByCancellationDate, vm.PIN
                     , EmpId, ModeofPayment
                    );
                var result = new ContentResult
                {
                    Content = serializer.Serialize(new { list = list ?? new List<getAllOPRevenueDataTAbleTAbleResult>() }),
                    ContentType = "application/json"
                };
                return result;
            }



        }


        [HttpPost]
        public ActionResult IPRevenue(FinanceReportsIPRevenue viewModel)
        {
            var EmpId = Request.Form["EmpId"];

            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = ipRevenueDB.getIPRevenue(viewModel.StartDate
                                                                , viewModel.EndDate.AddDays(1)
                                                                , (int)viewModel.PatientBillType
                                                                , viewModel.BillType
                                                                , viewModel.CompanyId
                                                                , viewModel.DepartmentId
                                                                , viewModel.DoctorId
                                                                , viewModel.PIN != null ? (int)viewModel.PIN : 0
                                                                , Convert.ToInt16(viewModel.IsRevenue)
                                                                , viewModel.ServiceId
                                                                , EmpId.ToString()
                                                                );

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                if (viewModel.PatientBillType == Enumerations.PatientBillType.CASH)
                {
                    // reportViewer.LocalReport.SetParameters(new ReportParameter("PinNo", viewModel.PIN.ToString() ));
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCash.rdl";
                }
                else
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCharge.rdl";
                }
                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("IPRevenue", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                //  reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("patientTypeText", viewModel.PatientTypeText));
                reportViewer.LocalReport.SetParameters(new ReportParameter("billTypeText", viewModel.BillTypeText));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));


                //if (viewModel.PatientBillType == Enumerations.PatientBillType.CASH)
                //{
                //    reportViewer.LocalReport.SetParameters(new ReportParameter("PinNo", viewModel.PIN.ToString()));
                //}



                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();
        }


        public ActionResult RevenueAdjustments()
        {
            FeatureID = "1985";
            var category = categoryDB.getCategories();
            var viewModel = new FinanceRevenueAdjustments()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                BillTypes = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(1,"OP"),
                                         new KeyValuePair<int, string>(2,"IP")
                            },
                Categories = category,
                Companies = companyDB.getCompanyByCategory(category.First().Id)
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RevenueAdjustments(FinanceRevenueAdjustments viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = adjustmentDB.getRevenueAdjustments(viewModel.StartDate
                                                                        , viewModel.EndDate.AddDays(1)
                                                                        , viewModel.BillType
                                                                        , viewModel.CategoryId.HasValue ? viewModel.CategoryId.Value : 0
                                                                        , viewModel.CompanyId.HasValue ? viewModel.CompanyId.Value : 0);

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_RevenueAdjustments.rdl";

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();
        }

        public ActionResult SearchCompanies(string searchString)
        {
            return Json(companyDB.findCompanies(searchString), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctors(searchString), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompanyByCategory(int categoryId)
        {
            return Json(companyDB.getCompanyByCategory(categoryId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CashInPatientDischargeReport()
        {
            FeatureID = "1985";
            var viewModel = new McrsAllInPatient()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CashInPatientDischargeReport(McrsAllInPatient McrsAllinpatient)
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
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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

        public ActionResult SummaryReportAdvance()
        {
            FeatureID = "1985";
            //SP_Get_OPDADV_Payment_Summary  Report_OP_ADVPayment_Summary
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SummaryReportAdvance(OtherReportsDateTimeOnly param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_OPdavPaymentSummary]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\SummaryReportAdvance.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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


        public ActionResult ListAdvancePayment()
        {
            FeatureID = "1985";
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        public ActionResult DepartmentIssuanceReport()
        {
            //SP_Get_DepartmentIssueance Report_DepartmentIssueance
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult DepartmentIssuanceReport(OtherReportsDateTimeOnly param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                DataTable reportData2 = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_DeptIssuance]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_DeptIssuance.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                //ReportDataSource datasourceItemReportHeader2018 = new ReportDataSource("ReportHeader2018", reportData2);

                //reportViewer.LocalReport.DataSources.Add(datasourceItemReportHeader2018);
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



        public ActionResult MealSummary()
        {
            FeatureID = "1985";
            //SP_GetMealSummary  Report_MealSummary
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult MealSummary(OtherReportsDateTimeOnly param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_MealSummary]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_MealSummary.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult PayrollSummaryDept()
        {
            FeatureID = "1985";
            //SP_GetPayrollSummary Report_PayrollSummarybyDepartment
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult PayrollSummaryDept(OtherReportsDateTimeOnly param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_PayrollSummary]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_PayrollSummary.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult IPOPReceipts()
        {
            FeatureID = "1985";
            //SP_IP_OP_Receipts Report_IpOp_Receipts
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IPOPReceipts(OtherReportsDateTimeOnly param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IpOpReceipts]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_IpOpReceipts.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult DeptWiseLossDiscount()
        {
            FeatureID = "1985";
            //strSQL = "select xxx.department dep,dept_name,sum(pd) pd,sum(discount) discount, " & _
            // "sum(discount_sghins) discount_sghins, " & _
            // "sum(neg_adj) neg_adj, " & _
            // "sum(pos_adj) pos_adj, " & _
            // "sum(total) total from " & _
            // "(select fin_ipop_discount.department,pd,decode(type,'SGHINS',0,DISCOUNT) discount, " & _
            // "decode(type,'SGHINS',discount,0) discount_sghins,neg_adj,pos_adj,total " & _
            // "from fin_ipop_discount " & _
            // "where yyyymm between '" & FormatDatePlus(stDate.Value, "YYYYMM") & "' and '" & FormatDatePlus(enDate.Value, "YYYYMM") & "' " & _
            // ") xxx,depart_ment " & _
            // "where xxx.department = depart_ment.department " & _
            // "group by xxx.department,dept_name " & _
            // "order by xxx.department "

            //Report_DiscountPackage.rpt
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeptWiseLossDiscount(OtherReportsDateTimeOnly param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                //different connection dead end
                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IpOpReceipts]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_IpOpReceipts.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult StaffBonus()
        {
            FeatureID = "1985";
            //strSQL = "SELECT " & _
            //   "decode(FIN_BONUS.DEPARTMENT,'AR','FIN','CHW','COM','COR','PRS','END','INT','ERP','PRS', " & _
            //   "'HEM','NEP','INC','INT','MAX','DNT','MSO','MKT','NCU','PDT',FIN_BONUS.DEPARTMENT) DEP, " & _
            //   "DEPT_NAME,SUBSTR(YYYYMM,1,4) YEAR, " & _
            //   "SUM(DECODE(SUBSTR(YYYYMM,5,2),'03',BONUS,0)) Q1, " & _
            //   "SUM(DECODE(SUBSTR(YYYYMM,5,2),'06',BONUS,0)) Q2, " & _
            //   "SUM(DECODE(SUBSTR(YYYYMM,5,2),'09',BONUS,0)) Q3, " & _
            //   "SUM(DECODE(SUBSTR(YYYYMM,5,2),'12',BONUS,0)) Q4, " & _
            //   "SUM(BONUS) TOTAL " & _
            //   "FROM FIN_BONUS,DEPART_MENT " & _
            //   "WHERE decode(FIN_BONUS.DEPARTMENT, " & _
            //   "'AR','FIN','CHW','COM','COR','PRS','END','INT','ERP','PRS','HEM','NEP','INC','INT', " & _
            //   "'MAX','DNT','MSO','MKT','NCU','PDT',FIN_BONUS.DEPARTMENT) = DEPART_MENT.DEPARTMENT AND " & _
            //   "YYYYMM BETWEEN '" & FormatDatePlus(stDate.Value, "YYYYMM") & "' AND '" & FormatDatePlus(enDate.Value, "YYYYMM") & "'  " & _
            //   "GROUP BY decode(FIN_BONUS.DEPARTMENT, " & _
            //   "'AR','FIN','CHW','COM','COR','PRS','END','INT','ERP','PRS','HEM','NEP','INC','INT', " & _
            //   "'MAX','DNT','MSO','MKT','NCU','PDT',FIN_BONUS.DEPARTMENT),DEPT_NAME,SUBSTR(YYYYMM,1,4) " & _
            //   "ORDER BY decode(FIN_BONUS.DEPARTMENT, " & _
            //   "'AR','FIN','CHW','COM','COR','PRS','END','INT','ERP','PRS','HEM','NEP','INC','INT', " & _
            //   "'MAX','DNT','MSO','MKT','NCU','PDT',FIN_BONUS.DEPARTMENT),DEPT_NAME,SUBSTR(YYYYMM,1,4) "

            //\Report_StaffBonus.rpt

            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }


        public ActionResult IPRefundableReport()
        {
            FeatureID = "1985";
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IPRefundableReport(OtherReportsDateTimeOnly param)
        {
            //SP_Get_IPRevokeDischarges Report_RevokeDischarges.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPRefundable]");
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_IpRefundable.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult ARAdjustmentDetailsOp()
        {
            FeatureID = "1985";
            var viewModel = new FROPAdjustment()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDB.getCategoriesWithId()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ARAdjustmentDetailsOp(FROPAdjustment param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                //[MCRS].[FinanceReport_ARAdjustmentOP]

                reportData = OtherReportDB.getARAdjustmentDetailsOp(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_ARAdjustmentDetails.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("categoryname", Request.Form["CategoryName"]));
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



        public ActionResult OPDCashCollection()
        {
            FeatureID = "1985";
            var viewModel = new FROPAdjustment()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult OPDCashCollection(FROPAdjustment param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                string isgroup = Request.Form["isGroupByReceptionist"];

                if (isgroup == "false")
                {
                    reportData = financereportDB.getOPDCashCollectionByReceptionist(vm.StartDate);
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_OPDCash.rdl";
                }
                else
                {
                    reportData = financereportDB.getOPDCashCollectionByReceptionistIsGroup(vm.StartDate);
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_OPDCashIsGroup.rdl";
                }


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
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


        public ActionResult BillingEffReport()
        {
            FeatureID = "1985";
            var viewModel = new FROPAdjustment()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDB.getCategoriesWithId()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult BillingEffReport(FROPAdjustment param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                //[MCRS].[FinanceReport_ARAdjustmentOP]

                reportData = financereportDB.getBillEff(vm.StartDate, vm.EndDate.AddDays(1), vm.CategoryId);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_BillingEfficiencyReport.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.StartDate.ToShortDateString()));
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


        public ActionResult ArPackageNPInvoice()
        {
            FeatureID = "1985";
            var viewModel = new FROPAdjustment()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryList = categoryDB.getCategoriesWithId()
            };
            return View(viewModel);
        }



        [HttpPost]
        public ActionResult ArPackageNPInvoice(FROPAdjustment param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                if (Request.Form["Summary"] == "PIN")
                {
                    //    ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_AR_PD_NPD.rpt")
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AR_PD_NPD.rdl";

                    if (vm.CategoryId != 34)
                    {
                        if (Request.Form["Package"] == "Package")
                        {
                            reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "PD", vm.CategoryId, "[MCRS].[FinanceReport_GetIpCompanyARBill]");

                        }
                        else
                        {
                            reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "NPD", vm.CategoryId, "[MCRS].[FinanceReport_GetIpCompanyARBill]");

                        }

                    }
                    else
                    {

                        if (Request.Form["Package"] == "Package")
                        {
                            reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "PD", vm.CategoryId, "[MCRS].[FinanceReport_GetIPCompanyARBillCharity]");

                        }
                        else
                        {
                            reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "NPD", vm.CategoryId, "[MCRS].[FinanceReport_GetIPCompanyARBillCharity]");
                        }

                    }
                }

                if (Request.Form["Summary"] == "Company")
                {
                    //ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_AR_PD_NPD(Summary).rpt")
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_AR_PD_NPD(Summary).rdl";
                    if (Request.Form["Package"] == "Package")
                    {
                        reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "PD", vm.CategoryId, "[MCRS].[FinanceReport_GetIPCompanyARBillSummary]");

                    }
                    else
                    {
                        reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "NPD", vm.CategoryId, "[MCRS].[FinanceReport_GetIPCompanyARBillSummary]");
                    }

                }

                if (Request.Form["Summary"] == "Category")
                {
                    //    ReportDoc.Load(Configuration.ConfigurationSettings.AppSettings("ReportMapString") & "\Report_AR_PD_NPD(SummaryCategory).rpt")
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_AR_PD_NPDSummaryCategory.rdl";
                    if (Request.Form["Package"] == "Package")
                    {

                        reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "PD", vm.CategoryId, "[MCRS].[FinanceReport_GetIPCompanyARBillSummaryCategory]");
                    }
                    else
                    {
                        reportData = financereportDB.ARPackageNonPacakgewithSP(vm.StartDate, vm.EndDate.AddDays(1), "NPD", vm.CategoryId, "[MCRS].[FinanceReport_GetIPCompanyARBillSummaryCategory]");

                    }

                }


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                if (Request.Form["Package"] == "Package")
                {
                    reportViewer.LocalReport.SetParameters(new ReportParameter("header", "AR Company Package Deal Invoices Report"));
                }
                else { reportViewer.LocalReport.SetParameters(new ReportParameter("header", "AR Company Non-Package Deal Invoices Report")); }
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToString("dd-MMM-yyyy")));
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


        public ActionResult WarehousetoStoreSummary()
        {
            FeatureID = "1985";
            var viewModel = new FinanceReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getStations(),
                ItemGroupList = stationdb.getItemGroup()
            };
            return View(viewModel);

        }


        [HttpPost]
        public ActionResult WarehousetoStoreSummary(FinanceReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {



                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                //[MCRS].[FinanceReport_ARAdjustmentOP]

                reportData = financereportDB.getWareHouseIssueancetoStore(vm.ItemGroupId, vm.StartDate, vm.EndDate.AddDays(1), Request.Form["StationName"]);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_WarehousetoStore.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("categoryname", Request.Form["CategoryName"]));
                reportViewer.LocalReport.SetParameters(new ReportParameter("AreaStore", Request.Form["StationName"]));
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

        public ActionResult IPDischargesStatement()
        {
            FeatureID = "1985";
            var viewModel = new FinanceReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getStations(),
                ItemGroupList = stationdb.getItemGroup()
            };
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult IPDischargesStatement(FinanceReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                string billtype = Request.Form["BillTypeId"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                string option = "0";
                //Report_IPDischargesStatement.rpt

                if (billtype == "Cash")
                {

                    option = "1";
                    //reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPDischargeStatement]");

                }
                else
                {
                    //  reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPDischargeStatementCharge]");

                }

                reportData = OtherReportDB.getIPDischarge(vm.StartDate, vm.EndDate.AddDays(1), option);



                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_IPDischargesStatement.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToString("dd-MMM-yyyy")));

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




        public ActionResult SummaryAccounts()
        {
            FeatureID = "1985";
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



        public JsonResult getGradeByCompanyId(int companyId)
        {
            return Json(gradeDb.getGradeByCompanyId(companyId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SummaryAccounts(ARSummaryOfAccounts viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    var data = new DataTable();

                    utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDate);

                    if (viewModel.SubCategoryId == 0)
                    {
                        data = arReportDb.getSummaryOfAccounts(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, viewModel.SubCategoryId);
                    }
                    else
                    {
                        data = arReportDb.getSummaryOfAccountsBySubCategory(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId, viewModel.Type, viewModel.SubCategoryId);
                    }

                    if (data.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if ((viewModel.CategoryId == 23 || viewModel.CategoryId == 70) && viewModel.Type == 0)
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_MEDNET.rdlc";
                        }
                        else if (viewModel.CategoryId == 24 && viewModel.Type == 1)
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_NCCI.rdlc";
                        }
                        else
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\ARReports\StatementSummary_GroupBySubCategory.rdlc";
                        }

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("StatementSummary", data));
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

            return Content("");
        }




        public ActionResult NewOpAdvancePayment()
        {
            FeatureID = "1985";
            var viewModel = new FinanceReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult NewOpAdvancePayment(FinanceReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                //from frmOPDAdvPaymentTranJuly2013 skip the Oracle part
                reportData = financereportDB.getOPDAdvPayment(vm.StartDate, vm.EndDate.AddDays(1), vm.PinId == null ? "0" : vm.PinId, vm.ReceiptNo == null ? "0" : vm.ReceiptNo);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_OP_ADVPayment_TRX.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.EndDate.ToString("dd-MMM-yyyy")));

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


        public ActionResult IPDischargesStatementPinWise()
        {
            FeatureID = "1985";
            var viewModel = new FinanceReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getStations(),
                ItemGroupList = stationdb.getItemGroup(),
                EmployeeList = empDb.getHREmployeeDetails()
            };
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult IPDischargesStatementPinWise(FinanceReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                string billtype = Request.Form["BillTypeId"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                string option = "0";
                //Report_IPDischargesStatement.rpt

                if (billtype == "Cash")
                {

                    option = "1";
                    //reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPDischargeStatement]");

                }
                else
                {
                    //  reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPDischargeStatementCharge]");

                }

                reportData = OtherReportDB.getIPDischarge(vm.StartDate, vm.EndDate.AddDays(1), option);



                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_IPDischargesStatement.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToString("dd-MMM-yyyy")));

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

        public JsonResult IPDischargesStatementPinWise_GetPIN(string doctorid, string fromdate, string todate)
        {

            PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

            //if (String.IsNullOrEmpty(doctorid)) return null;
            var getpinModel = new List<GenericListModel>();
            getpinModel = !string.IsNullOrEmpty(doctorid) ? _clPatientStatisticsDB.GetPIN(int.Parse(doctorid), fromdate, todate) : getpinModel;
            getpinModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });

            var json = getpinModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PendingServices()
        {
            FeatureID = "1985";
            var category = categoryDB.getCategories();
            var viewModel = new PendingServices()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

                PatientTypes = new List<KeyValuePair<Enumerations.FinanceReport_PendingServicesPatientType, string>>{
                                        new KeyValuePair<Enumerations.FinanceReport_PendingServicesPatientType, string>(Enumerations.FinanceReport_PendingServicesPatientType.ALL, Enumerations.FinanceReport_PendingServicesPatientType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.FinanceReport_PendingServicesPatientType, string>(Enumerations.FinanceReport_PendingServicesPatientType.INPATIENT, Enumerations.FinanceReport_PendingServicesPatientType.INPATIENT.ToString()),
                                        new KeyValuePair<Enumerations.FinanceReport_PendingServicesPatientType, string>(Enumerations.FinanceReport_PendingServicesPatientType.OUTPATIENT, Enumerations.FinanceReport_PendingServicesPatientType.OUTPATIENT.ToString()),
                                 },
                CoveringLetterTypes = new List<KeyValuePair<Enumerations.FinanceReport_CoveringLetterType, string>>{
                                        new KeyValuePair<Enumerations.FinanceReport_CoveringLetterType, string>(Enumerations.FinanceReport_CoveringLetterType.ALL, Enumerations.FinanceReport_CoveringLetterType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.FinanceReport_CoveringLetterType, string>(Enumerations.FinanceReport_CoveringLetterType.BEFORE, Enumerations.FinanceReport_CoveringLetterType.BEFORE.ToString()),
                                        new KeyValuePair<Enumerations.FinanceReport_CoveringLetterType, string>(Enumerations.FinanceReport_CoveringLetterType.AFTER, Enumerations.FinanceReport_CoveringLetterType.AFTER.ToString()),
                                 },

                DepartmentList = departmentDB.getAllDepartment(),
                CategoryList = category,

                CompanyList = companyDB.getCompanyByCategory(category.First().Id)


            };
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult PendingServices(PendingServices param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                //[MCRS].[FinanceReport_ARAdjustmentOP]

                string PatientType = Request.Form["PatientType"];
                reportData = financereportDB.PendingServices(vm.StartDate, vm.EndDate.AddDays(1), PatientType, vm.DepartmentId, vm.CategoryId, vm.CompanyId, vm.PIN);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_PendingServices.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.StartDate.ToShortDateString()));
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


 /*      REDIRECT to AR for one report only
  *      public ActionResult SummaryOfAccountswithVAT()
        {
            FeatureID = "1985";
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
                    utilitiesDB.fixedHeaderTotalForMCRS(viewModel.StartDate);
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
*/


        public ActionResult OPRevenuewithVAT()
        {
            FeatureID = "1985";
            var vieModel = new FinanceReportsOPRevenue()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                BillTypes = new List<KeyValuePair<Enumerations.BillType, string>>{
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.ALL,Enumerations.BillType.ALL.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.NOTCANCELLED,Enumerations.BillType.NOTCANCELLED.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.CANCELLED,Enumerations.BillType.CANCELLED.ToString())
                                },
                PatientBillTypes = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                                },

                DepartmentList = departmentDB.getAllDepartment(),

                SortByCancellationDate = false
            };

            return View(vieModel);
        }

        [HttpPost]
        public ActionResult OPRevenuewithVAT(FinanceReportsOPRevenue financeReportsOPRevenue)
        {
            var EmpId = Request.Form["EmpId"];
            var ModeofPayment = Request.Form["ModeofPayment"];
            var vm = financeReportsOPRevenue;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                if (financeReportsOPRevenue.BillType == Enumerations.BillType.NOTCANCELLED)
                {
                    reportData = opRevenueDB.getOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                    if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue.rdl";

                    else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue_Company.rdl";
                    else
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllNotCancelledOPRevenue.rdl";

                }
                else if (financeReportsOPRevenue.BillType == Enumerations.BillType.CANCELLED)
                {
                    reportData = opRevenueDB.getCancelledOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                    if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCancelled.rdl";

                    else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCancelled_Company.rdl";
                    else
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllCancelledOPRevenue.rdl";


                }
                else
                {

                    reportData = opRevenueDB.getAllOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                    if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue_CancelledAndNotCancelled.rdl";

                    else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCharge_CancelledAndNotCancelled.rdl";

                    else
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllOPRevenue.rdl";
                }


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("OPRevenue", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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


        public ActionResult FinanceReport_SalesToSaudiCitizen_NoPharmacy()
        {
            FeatureID = "1985";
            var vieModel = new FinanceReportsVatDetails()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };

            return View(vieModel);
        }

        [HttpPost]
        public ActionResult FinanceReport_SalesToSaudiCitizen_NoPharmacy(FinanceReportsVatDetails FinanceReportsVatDetails)
        {

            var vm = FinanceReportsVatDetails;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = opRevenueDB.SalesToSaudiCitizen(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_SalesToSaudiCitizen_NoPharmacy.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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


        public ActionResult FinanceReport_ZeroRatedSales_AllPharmacy()
        {
            FeatureID = "1985";
            var vieModel = new FinanceReportsVatDetails()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };

            return View(vieModel);
        }

        [HttpPost]
        public ActionResult FinanceReport_ZeroRatedSales_AllPharmacy(FinanceReportsVatDetails FinanceReportsVatDetails)
        {

            var vm = FinanceReportsVatDetails;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = opRevenueDB.FinanceReport_ZeroRatedSales_AllPharmacy(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_ZeroRatedSales_AllPharmacy.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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


        public ActionResult OPRevenue2018()
        {
            FeatureID = "1985";
            var vieModel = new FinanceReportsOPRevenue()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                BillTypes = new List<KeyValuePair<Enumerations.BillType, string>>{
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.ALL,Enumerations.BillType.ALL.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.NOTCANCELLED,Enumerations.BillType.NOTCANCELLED.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.CANCELLED,Enumerations.BillType.CANCELLED.ToString())
                                },
                PatientBillTypes = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                                },

                DepartmentList = departmentDB.getAllDepartment(),

                SortByCancellationDate = false
            };

            return View(vieModel);
        }

        /*[HttpPost]
        public ActionResult OPRevenue2018(FinanceReportsOPRevenue financeReportsOPRevenue)
        {
            var EmpId = Request.Form["EmpId"];
            var ModeofPayment = Request.Form["ModeofPayment"];
            var vm = financeReportsOPRevenue;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                if (financeReportsOPRevenue.BillType == Enumerations.BillType.NOTCANCELLED)
                {
                    reportData = opRevenueDB.getOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                    //if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue.rdl";

                    //else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue_Company.rdl";
                    //else
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllNotCancelledOPRevenue.rdl";

                }
                else if (financeReportsOPRevenue.BillType == Enumerations.BillType.CANCELLED)
                {
                    reportData = opRevenueDB.getCancelledOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                    //if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCancelled.rdl";

                    //else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCancelled_Company.rdl";
                    //else
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllCancelledOPRevenue.rdl";
                }
                else
                {

                    reportData = opRevenueDB.getAllOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                    //if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenue_CancelledAndNotCancelled.rdl";

                    //else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                    //    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_OPRevenueCharge_CancelledAndNotCancelled.rdl";

                    //else
                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllOPRevenue_OptimizeReport.rdl";
                }


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("OPRevenue", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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
        */


        [HttpPost]
        public ActionResult OPRevenue2018(FinanceReportsOPRevenue financeReportsOPRevenue)
        {
            //sorry sa name rush d ko na pinalitan kse alam mo na. .. For Excel download only
            var EmpId = Request.Form["EmpId"];
            var ModeofPayment = Request.Form["ModeofPayment"];
            var vm = financeReportsOPRevenue;




            List<getAllOPRevenueDataTAbleTAbleResult> _Res = opRevenueDB.getAllOPRevenueDataTAble(vm.StartDate2, vm.EndDate2.AddDays(1), (int)vm.PatientBillType2, 0, 0, 0, true, "0", "0", "0");





            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Sheet1");


                //worksheet.Cells[1, 1].Value = "example");
                worksheet.Cells[1, 1].Value = "OPID";
                worksheet.Cells[1, 2].Value = "BILL_NUMBER";
                worksheet.Cells[1, 3].Value = "BILL_DATE";
                worksheet.Cells[1, 4].Value = "CANCEL_DATE";
                worksheet.Cells[1, 5].Value = "TRANSACTIONMONTH";
                worksheet.Cells[1, 6].Value = "PIN_NUMBER";
                worksheet.Cells[1, 7].Value = "NATIONALITY";
                worksheet.Cells[1, 8].Value = "BILL_TYPE";
                worksheet.Cells[1, 9].Value = "DOCTOR_CODE";
                worksheet.Cells[1, 10].Value = "COMPANY_CODE";
                worksheet.Cells[1, 11].Value = "COMPANY_NAME";
                worksheet.Cells[1, 12].Value = "MEDICAL DEPART";
                worksheet.Cells[1, 13].Value = "MODE OF PAYMENT";
                worksheet.Cells[1, 14].Value = "RECEIPT_NO";
                worksheet.Cells[1, 15].Value = "EMPLOYEE_ID";
                worksheet.Cells[1, 16].Value = "EMPLOYEE_NAME";
                worksheet.Cells[1, 17].Value = "SERVICE_CODE";
                worksheet.Cells[1, 18].Value = "SERVICE DESC";
                worksheet.Cells[1, 19].Value = "QUANTITY";
                worksheet.Cells[1, 20].Value = "RATE";
                worksheet.Cells[1, 21].Value = "RECEIPT_NO";
                worksheet.Cells[1, 22].Value = "HIS_REVENUE";
                worksheet.Cells[1, 23].Value = "RECEIPT_NO";
                worksheet.Cells[1, 24].Value = "DISCOUNT PERCENTAGE";
                worksheet.Cells[1, 25].Value = "HIS_DISCOUNT";
                worksheet.Cells[1, 26].Value = "HIS_DEDUCTICBLE";
                worksheet.Cells[1, 27].Value = "HIS_CHARGE_REVENUE";
                worksheet.Cells[1, 28].Value = "HIS_CASH_REVENUE";
                worksheet.Cells[1, 29].Value = "HIS_RECIEVABLE";

                //worksheet.Cells[1, 15].Value = "INV_DEDUCT";
                //worksheet.Cells[1, 16].Value = "INV_DISC";
                //worksheet.Cells[1, 17].Value = "SERVICE_CODE";
                //worksheet.Cells[1, 18].Value = "SERVICE DESCRIPTION";

                //worksheet.Cells[1, 19].Value = "TREATMENT_DATE";
                //worksheet.Cells[1, 20].Value = "CHARGE_QUANTITY";
                //worksheet.Cells[1, 21].Value = "BILLED_AMT";
                //worksheet.Cells[1, 22].Value = "LINE_DISC";

                //worksheet.Cells[1, 23].Value = "INHOUSE_CLIN_REF";
                //worksheet.Cells[1, 24].Value = "EMERGENCY_IND";
                //worksheet.Cells[1, 25].Value = "MEDICAL_INFO";
                //worksheet.Cells[1, 26].Value = "TYPE";

                //worksheet.Cells[1, 27].Value = "TEMPERATURE";
                //worksheet.Cells[1, 28].Value = "RESPIRATORY RATE";
                //worksheet.Cells[1, 29].Value = "BLOOD PRESSURE";
                //worksheet.Cells[1, 30].Value = "HEIGHT";
                //worksheet.Cells[1, 31].Value = "WEIGHT";


                //worksheet.Cells[1, 33].Value = "RAD_COMCODE";
                //worksheet.Cells[1, 34].Value = "INVOICE_NO";
                //worksheet.Cells[1, 35].Value = "MR_NO";
                //worksheet.Cells[1, 36].Value = "RADIOLOGY DATE";
                //worksheet.Cells[1, 37].Value = "TEST CODE";
                //worksheet.Cells[1, 38].Value = "TEST NAME";
                //worksheet.Cells[1, 39].Value = "RADIOLOGY CLINIC DATA";
                //worksheet.Cells[1, 40].Value = "RADIOLOGY REPORT TEXT";

                //worksheet.Cells[1, 42].Value = "LAB_COMCODE";
                //worksheet.Cells[1, 43].Value = "INVOICE_NO";
                //worksheet.Cells[1, 44].Value = "MRNO";
                //worksheet.Cells[1, 45].Value = "LAB_VISIT_DATE";
                //worksheet.Cells[1, 46].Value = "LAB_PROFILE";
                //worksheet.Cells[1, 47].Value = "LAB_TEST_NAME";
                //worksheet.Cells[1, 48].Value = "LAB_RESULT";
                //worksheet.Cells[1, 49].Value = "LAB_UNITS";
                //worksheet.Cells[1, 50].Value = "LAB_LOW";
                //worksheet.Cells[1, 51].Value = "LAB_HIGH";
                //worksheet.Cells[1, 52].Value = "LAB_SECTION";




                int InvCount = 2;
                int InvCount2 = 2;
                int InvCount3 = 2;

                int CurrSeq = 1;
                int ClaimSeqNo = 1;
                var PrevInvNo = "";
                foreach (var item in _Res)
                {

                    //string[] PTTemp = item.BillType.Split('-');
                    //string[] PTResp OPDCashCollection= item.DepartmentList.Split('-');
                    //string[] PTBP = item.BillTypes.Split('-');



                    worksheet.Cells[InvCount, 1].Value = item.OPID;
                    worksheet.Cells[InvCount, 2].Value = item.BillNumber;
                    worksheet.Cells[InvCount, 3].Value = item.BillDate; //"BILL_DATE";
                    worksheet.Cells[InvCount, 4].Value = item.CancelDate;// "CANCEL_DATE";
                    worksheet.Cells[InvCount, 5].Value = item.TransactionMonth;// "TRANSACTIONMONTH";
                    worksheet.Cells[InvCount, 6].Value = item.PinNumber;// "PIN_NUMBER";
                    worksheet.Cells[InvCount, 7].Value = item.Nationality; //"NATIONALITY";
                    worksheet.Cells[InvCount, 8].Value = item.BillType;// "BILL_TYPE";
                    worksheet.Cells[InvCount, 9].Value = item.DoctorCode; //"DOCTOR_CODE";
                    worksheet.Cells[InvCount, 10].Value = item.CompanyCode;// "COMPANY_CODE";
                    worksheet.Cells[InvCount, 11].Value = item.CompanyName;// "COMPANY_NAME";
                    worksheet.Cells[InvCount, 12].Value = item.DepartmentName; //"MEDICAL DEPART";
                    worksheet.Cells[InvCount, 13].Value = item.ModeOfPayment; //"MODE OF PAYMENT";
                    worksheet.Cells[InvCount, 14].Value = item.ReceiptNo; //"RECEIPT_NO";
                    worksheet.Cells[InvCount, 15].Value = item.EmployeeID; //"EMPLOYEE_ID";
                    worksheet.Cells[InvCount, 16].Value = item.Name; //"EMPLOYEE_NAME";
                    worksheet.Cells[InvCount, 17].Value = item.ItemCode; //"SERVICE_CODE";
                    worksheet.Cells[InvCount, 18].Value = item.ItemName; //"SERVICE DESC";
                    worksheet.Cells[InvCount, 19].Value = item.Quantity; //"QUANTITY";
                    worksheet.Cells[InvCount, 20].Value = item.Rate; //"RATE";
                    worksheet.Cells[InvCount, 21].Value = item.ReceiptNo; //"RECEIPT_NO";
                    worksheet.Cells[InvCount, 22].Value = item.Billamount; //"HIS_REVENUE";
                    worksheet.Cells[InvCount, 23].Value = item.ReceiptNo; //"RECEIPT_NO";
                    worksheet.Cells[InvCount, 24].Value = item.DiscountPercentage; //"DISCOUNT PERCENTAGE";
                    worksheet.Cells[InvCount, 25].Value = item.DiscountAmount; //"HIS_DISCOUNT";
                    worksheet.Cells[InvCount, 26].Value = item.DeductablePaid; //"HIS_DEDUCTICBLE";
                    worksheet.Cells[InvCount, 27].Value = item.ChargeRevenue; // "HIS_CHARGE_REVENUE";
                    worksheet.Cells[InvCount, 28].Value = item.HISCashRevenue; //"HIS_CASH_REVENUE";
                    worksheet.Cells[InvCount, 29].Value = item.Recievable; //"HIS_RECIEVABLE";


                    //worksheet.Cells[InvCount, 3].Value = CurrSeq;
                    //worksheet.Cells[InvCount, 4].Value = item.MEMB_NO;
                    //worksheet.Cells[InvCount, 5].Value = item.INVOICE_NO.Trim();
                    //worksheet.Cells[InvCount, 6].Value = item.INVOICE_DATE;
                    //worksheet.Cells[InvCount, 7].Value = item.PT_FILE_NO;
                    //worksheet.Cells[InvCount, 8].Value = item.ICD10_CODE.Trim();
                    //worksheet.Cells[InvCount, 9].Value = item.ICD10_DESCRIPTION.Trim();
                    //worksheet.Cells[InvCount, 10].Value = item.CLAIM_TYPE.Trim();
                    //worksheet.Cells[InvCount, 11].Value = item.PRE_AUTH_NO;
                    //worksheet.Cells[InvCount, 12].Value = item.DOCTOR_NAME;
                    //worksheet.Cells[InvCount, 13].Value = item.SPECIALITY;
                    //worksheet.Cells[InvCount, 14].Value = item.CLINICAL_DATA.Trim();

                    //worksheet.Cells[InvCount, 15].Value = item.INV_DEDUCT;
                    //worksheet.Cells[InvCount, 16].Value = item.INV_DISC;
                    //worksheet.Cells[InvCount, 17].Value = item.SERVICE_CODE.Trim();
                    //worksheet.Cells[InvCount, 18].Value = item.SERVICE_DESCRIPTION.Trim();

                    //worksheet.Cells[InvCount, 19].Value = item.TREATMENT_DATE.Trim();
                    //worksheet.Cells[InvCount, 20].Value = item.CHARGE_QUANTITY;
                    //worksheet.Cells[InvCount, 21].Value = item.BILLED_AMOUNT;
                    //worksheet.Cells[InvCount, 22].Value = item.LINE_DISC;
                    //worksheet.Cells[InvCount, 23].Value = item.INHOUSE_CLIN_REF.Trim();
                    //worksheet.Cells[InvCount, 24].Value = item.EMERGENCY_IND.Trim();
                    //worksheet.Cells[InvCount, 25].Value = item.MEDICAL_INFO.Trim();
                    //worksheet.Cells[InvCount, 26].Value = item.TYPE.Trim();




                    //worksheet.Cells[InvCount, 27].Value = PTTemp.Length > 0 ? item.TEMPERATURE.Trim().Split('-')[0] : " ";
                    //worksheet.Cells[InvCount, 28].Value = (PTResp.Length >= 2 ? item.RESPIRATORY_RATE.Split('-')[1] : " ")
                    //    + " - " + (PTResp.Length >= 3 ? item.RESPIRATORY_RATE.Split('-')[2] : " ")
                    //    + " - " + (PTResp.Length >= 4 ? item.RESPIRATORY_RATE.Split('-')[3] : " ");
                    //worksheet.Cells[InvCount, 29].Value = (PTBP.Length >= 5 ? item.BLOOD_PRESSURE.Split('-')[4] : " ")
                    //    + " - " + (PTBP.Length >= 6 ? item.RESPIRATORY_RATE.Split('-')[5] : " ");


                    //worksheet.Cells[InvCount, 30].Value = item.HEIGHT.Trim();
                    //worksheet.Cells[InvCount, 31].Value = item.WEIGHT.Trim();


                    CurrSeq += 1;

                    InvCount += 1;
                }


                //foreach (var item2 in _ResRad)
                //{

                //        worksheet.Cells[InvCount2, 33].Value = item2.COMCODE.Trim();
                //        worksheet.Cells[InvCount2, 34].Value = item2.RAD_INVOICE_NO.Trim();
                //        worksheet.Cells[InvCount2, 35].Value = item2.MR_NO.Trim();
                //        worksheet.Cells[InvCount2, 36].Value = item2.RADIOLOGY_DATE.Trim();
                //        worksheet.Cells[InvCount2, 37].Value = item2.TEXT_CODE.Trim();
                //        worksheet.Cells[InvCount2, 38].Value = item2.TEXT_NAME.Trim();
                //        worksheet.Cells[InvCount2, 39].Value = item2.RADIOLOGY_CLINIC_DATA.Trim();
                //        worksheet.Cells[InvCount2, 40].Value = item2.RADIOLOGY_REPORT_TEXT.Trim();

                //    InvCount2 += 1;
                //}


                //foreach (var item3 in _ResLab)
                //{

                //        worksheet.Cells[InvCount3, 42].Value = item3.COMCODE.Trim();
                //        worksheet.Cells[InvCount3, 43].Value = item3.LAB_INVOICE_NO.Trim();
                //        worksheet.Cells[InvCount3, 44].Value = item3.LAB_MRNO.Trim();
                //        worksheet.Cells[InvCount3, 45].Value = item3.LAB_VISIT_DATE.Trim();
                //        worksheet.Cells[InvCount3, 46].Value = item3.LAB_PROFILE.Trim();
                //        worksheet.Cells[InvCount3, 47].Value = item3.LAB_TEST_NAME.Trim();
                //        worksheet.Cells[InvCount3, 48].Value = item3.LAB_RESULT.Trim();
                //        worksheet.Cells[InvCount3, 49].Value = item3.LAB_UNITS.Trim();
                //        worksheet.Cells[InvCount3, 50].Value = item3.LAB_LOW.Trim();
                //        worksheet.Cells[InvCount3, 51].Value = item3.LAB_HIGH.Trim();
                //        worksheet.Cells[InvCount3, 52].Value = item3.LAB_SECTION.Trim();

                //    InvCount3 += 1;
                //}

                Byte[] fileBytes = pck.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;



                if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CASH)
                    Response.AddHeader("content-disposition", "attachment;filename=OPRevenueReport_cash.xlsx");
                else if (financeReportsOPRevenue.PatientBillType == Enumerations.PatientBillType.CHARGE)
                    Response.AddHeader("content-disposition", "attachment;filename=OPRevenueReport_charge.xlsx");
                else
                    Response.AddHeader("content-disposition", "attachment;filename=OPRevenueReport_all.xlsx");

                // Replace filename with your custom Excel-sheet name.

                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }

            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult IPRevenueDownloadExcel(FinanceReportsIPRevenue viewModel)
        {

            var EmpId = Request.Form["EmpId"];

            List<FinanceReportsIPRevenueDataTAbleResult> list = ipRevenueDB.getIPRevenueDataTAble(viewModel.StartDateExcel, viewModel.EndDateExcel.AddDays(1)
                    , (int)viewModel.PatientBillTypeExcel, viewModel.BillTypeExcel);

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Sheet1");


                worksheet.Cells[1, 1].Value = "IPID";
                worksheet.Cells[1, 2].Value = "Invoice No";
                worksheet.Cells[1, 3].Value = "Admission Date";
                worksheet.Cells[1, 4].Value = "Discharge Date";
                worksheet.Cells[1, 5].Value = "Discharge Month";
                worksheet.Cells[1, 6].Value = "Pin Number";
                worksheet.Cells[1, 7].Value = "Bill Type";
                worksheet.Cells[1, 8].Value = "Package Deal Patient";
                worksheet.Cells[1, 9].Value = "Gain/Loss Patient";
                worksheet.Cells[1, 10].Value = "Room No";
                worksheet.Cells[1, 11].Value = "Doctor Code";
                worksheet.Cells[1, 12].Value = "Medical Department";
                worksheet.Cells[1, 13].Value = "Company Code";
                worksheet.Cells[1, 14].Value = "Company Name";
                worksheet.Cells[1, 15].Value = "Service Category";
                worksheet.Cells[1, 16].Value = "Service Code";
                worksheet.Cells[1, 17].Value = "Service Description";
                worksheet.Cells[1, 18].Value = "Service Date";
                worksheet.Cells[1, 19].Value = "Service Month";
                worksheet.Cells[1, 20].Value = "Quantity";
                worksheet.Cells[1, 21].Value = "Rate";
                worksheet.Cells[1, 22].Value = "HIS Revenue";
                worksheet.Cells[1, 23].Value = "HIS Discount";
                worksheet.Cells[1, 24].Value = "Discount Amount";
                worksheet.Cells[1, 25].Value = "Package Deal Amount";
                worksheet.Cells[1, 26].Value = "HIS (Gain)/ Loss ";
                worksheet.Cells[1, 27].Value = "HIS Receivable";
                worksheet.Cells[1, 28].Value = "Nationality";




                int InvCount = 2;
                int InvCount2 = 2;
                int InvCount3 = 2;

                int CurrSeq = 1;
                int ClaimSeqNo = 1;
                var PrevInvNo = "";
                foreach (var item in list)
                {


                    // worksheet.Cells[InvCount,1].Value = item.BillType;// "BILL_TYPE";
                    worksheet.Cells[InvCount, 1].Value = item.IPID;// "IPID";
                    worksheet.Cells[InvCount, 2].Value = item.InvoiceNo;//"Invoice No";
                    worksheet.Cells[InvCount, 3].Value = item.AdmissionDate;//"Admission Date";
                    worksheet.Cells[InvCount, 4].Value = item.DischargeDate;//"Discharge Date";
                    worksheet.Cells[InvCount, 5].Value = item.DischargeMonth;//"Discharge Month";
                    worksheet.Cells[InvCount, 6].Value = item.PIN;//"Pin Number";
                    worksheet.Cells[InvCount, 7].Value = item.BillType;//"Bill Type";
                    worksheet.Cells[InvCount, 8].Value = item.PackageDealPatient;//"Package Deal Patient";
                    worksheet.Cells[InvCount, 9].Value = item.GainLossPatient;//"Gain/Loss Patient";
                    worksheet.Cells[InvCount, 10].Value = item.RoomNo;//"Room No";
                    worksheet.Cells[InvCount, 11].Value = item.DoctorCode;//"Doctor Code";
                    worksheet.Cells[InvCount, 12].Value = item.MedicalDept;//"Medical Department";
                    worksheet.Cells[InvCount, 13].Value = item.CompanyCode;//"Company Code";
                    worksheet.Cells[InvCount, 14].Value = item.CompanyName;//"Company Name";
                    worksheet.Cells[InvCount, 15].Value = item.ServiceCategory;//"Service Category";
                    worksheet.Cells[InvCount, 16].Value = item.ServiceCode;//"Service Code";
                    worksheet.Cells[InvCount, 17].Value = item.ServiceDesc;//"Service Description";
                    worksheet.Cells[InvCount, 18].Value = item.ServiceDate;//"Service Date";
                    worksheet.Cells[InvCount, 19].Value = item.ServiceMonth;//"Service Month";
                    worksheet.Cells[InvCount, 20].Value = item.Quantity;//"Quantity";
                    worksheet.Cells[InvCount, 21].Value = item.Rate;//"Rate";
                    worksheet.Cells[InvCount, 22].Value = item.HISRevenue;//"HIS Revenue";
                    worksheet.Cells[InvCount, 23].Value = item.DiscountPercentage;//"HIS Discount";
                    worksheet.Cells[InvCount, 24].Value = item.DiscountAmount;//"Discount Amount";
                    worksheet.Cells[InvCount, 25].Value = item.PackageAmount;//"Package Deal Amount";
                    worksheet.Cells[InvCount, 26].Value = item.HISGainLoss;//"HIS (Gain)/ Loss ";
                    worksheet.Cells[InvCount, 27].Value = item.HISRecievable;//"HIS Receivable";
                    worksheet.Cells[InvCount, 28].Value = item.Nationality;//"HIS Receivable";


                    CurrSeq += 1;

                    InvCount += 1;
                }


                Byte[] fileBytes = pck.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;


                var dischargeornot = "UNDISCHARGED";
                if (viewModel.BillTypeExcel == 1) //discharged
                { dischargeornot = "DISCHARGED"; }

                if (viewModel.PatientBillTypeExcel == Enumerations.PatientBillType.CASH)
                {
                    Response.AddHeader("content-disposition", "attachment;filename=IPRevenueReport_" + dischargeornot + "_CASH_" + viewModel.StartDateExcel.ToShortDateString() + "__TO__" + viewModel.EndDateExcel.ToShortDateString() + ".xlsx");
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment;filename=IPRevenueReport_" + dischargeornot + "_CHARGE_" + viewModel.StartDateExcel.ToShortDateString() + "__TO__" + viewModel.EndDateExcel.ToShortDateString() + ".xlsx");
                }



                /*
                 *            new KeyValuePair<int, string>(1,"DISCHARGED"),
                                                         new KeyValuePair<int, string>(2,"UNDISCHARGED")
                                    if (viewModel.PatientBillType == Enumerations.PatientBillType.CASH)
                                    {
                                        // reportViewer.LocalReport.SetParameters(new ReportParameter("PinNo", viewModel.PIN.ToString() ));
                                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCash.rdl";
                                    }
                                    else
                                    {
                                        reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_IPRevenueCharge.rdl";
                                    }
                                    */

                // Replace filename with your custom Excel-sheet name.

                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }

            return RedirectToAction("Index");




        }



        #region IPdischarge Version 2
        public ActionResult IPDischargesStatementVersion2()
        {
            FeatureID = "1985";
            var viewModel = new FinanceReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getStations(),
                ItemGroupList = stationdb.getItemGroup()
            };
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult IPDischargesStatementVersion2(FinanceReport param)
        {

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                string billtype = Request.Form["BillTypeId"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                string option = "0";
                //Report_IPDischargesStatement.rpt

                if (billtype == "Cash")
                {

                    option = "1";
                    //reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPDischargeStatement]");

                }
                else
                {
                    //  reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[FinanceReport_IPDischargeStatementCharge]");

                }

                reportData = OtherReportDB.getIPDischargeVersion2(vm.StartDate, vm.EndDate.AddDays(1), option);



                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReport_IPDischargesStatement_version2.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToString("dd-MMM-yyyy")));

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

        public ActionResult PDNPDReportAfterCL()
        {
            FeatureID = "1985";
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
        public ActionResult PDNPDReportAfterCL(ARSummaryOfAccounts viewModel)
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


                    data = arReportDb.PDNPDReportAfterCL(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId, viewModel.GradeId, viewModel.CompanyId);


                    if (data.Rows.Count > 0)
                    {


                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;


                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReports_PDNPDReportAfterCL.rdl";


                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IPRevenue", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.StartDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.EndDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("Category", viewModel.Category));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("SubCategory", viewModel.SubCategory));

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

        #region MCRS2019

        public ActionResult NetRevenue()
        {
            FeatureID = "1985";
            var viewModel = new NetRevenueVM()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day)
                ,PatientTypeList = new List<KeyValuePair<Enumerations.FinanceReport_NetRevenue, string>>
                {
                new KeyValuePair<Enumerations.FinanceReport_NetRevenue,string>(Enumerations.FinanceReport_NetRevenue.ALL,"ALL")
                ,new KeyValuePair<Enumerations.FinanceReport_NetRevenue,string>(Enumerations.FinanceReport_NetRevenue.IP,"INPATIENT")
                ,new KeyValuePair<Enumerations.FinanceReport_NetRevenue,string>(Enumerations.FinanceReport_NetRevenue.OP,"OUTPATIENT")
                }
                ,BillTypeList = new List<KeyValuePair<Enumerations.FinanceReport_NetRevenueBillType, string>>
                {
                new KeyValuePair<Enumerations.FinanceReport_NetRevenueBillType,string>(Enumerations.FinanceReport_NetRevenueBillType.ALL,"ALL")
                ,new KeyValuePair<Enumerations.FinanceReport_NetRevenueBillType,string>(Enumerations.FinanceReport_NetRevenueBillType.CHARGE,"CHARGE")
                ,new KeyValuePair<Enumerations.FinanceReport_NetRevenueBillType,string>(Enumerations.FinanceReport_NetRevenueBillType.CASH,"CASH")
                }
                ,BillFinalizeList = new List<KeyValuePair<Enumerations.FinanceReport_NetRevenueBillFinalize, string>>
                {
                new KeyValuePair<Enumerations.FinanceReport_NetRevenueBillFinalize,string>(Enumerations.FinanceReport_NetRevenueBillFinalize.YES,Enumerations.FinanceReport_NetRevenueBillFinalize.YES.ToString())
                ,new KeyValuePair<Enumerations.FinanceReport_NetRevenueBillFinalize,string>(Enumerations.FinanceReport_NetRevenueBillFinalize.NO,Enumerations.FinanceReport_NetRevenueBillFinalize.NO.ToString())
                }
                ,Categories = categoryDb.getAllCategories()
                ,
                GroupByCategoryList = new List<KeyValuePair<Enumerations.FinanceReport_NetRevenueGroupByCategory, string>>
                {
                new KeyValuePair<Enumerations.FinanceReport_NetRevenueGroupByCategory,string>(Enumerations.FinanceReport_NetRevenueGroupByCategory.YES,Enumerations.FinanceReport_NetRevenueBillFinalize.YES.ToString())
                ,new KeyValuePair<Enumerations.FinanceReport_NetRevenueGroupByCategory,string>(Enumerations.FinanceReport_NetRevenueGroupByCategory.NO,Enumerations.FinanceReport_NetRevenueBillFinalize.NO.ToString())
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult NetRevenue(NetRevenueVM vm)
        {

          
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = arReportDb.NetRevenue(vm.StartDate, vm.EndDate, vm.PatientType, vm.BillType, vm.BillFinalize, vm.Category, vm.IPID,vm.RegistrationNo,vm.CancelledOnly);


                
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReports_NetRevenue_No_Group.rdl";

                if (vm.GroupByCategory.ToString() == "YES") {
                    reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReports_NetRevenue.rdl";
                }

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
                reportViewer.LocalReport.SetParameters(new ReportParameter("Category", vm.CategoryText == null ? "ALL" : vm.CategoryText.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("BillType", vm.BillTypeText == null ? "ALL" : vm.BillTypeText.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("PatientType", vm.PatientTypeText == null ? "ALL" : vm.PatientTypeText.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("from", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("to", vm.EndDate.ToShortDateString()));

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
        public ActionResult Monthly_CIP_REVENUE()
        {

            var viewModel = new NetRevenueVM()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Monthly_CIP_REVENUE(NetRevenueVM qpsMedTower)
        {

            var vm = qpsMedTower;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = arReportDb.Monthly_CIP_REVENUE(vm.StartDate);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReports_Monthly-CIP-Revenue.rdl";
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                 reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("DateFrom", vm.StartDate.ToString("MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("Category", vm.CategoryText == null ? "ALL" : vm.CategoryText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("BillType", vm.BillTypeText == null ? "ALL" : vm.BillTypeText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("PatientType", vm.PatientTypeText == null ? "ALL" : vm.PatientTypeText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DateFrom", vm.StartDate.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DateTo", vm.EndDate.ToString()));

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

        #region EODREPORTS

        public ActionResult Eod_DailyDoctorsTarget()
        {
            FeatureID = "1985";
            var viewModel = new NetRevenueVM()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Eod_DailyDoctorsTarget(NetRevenueVM qpsMedTower)
        {

            var vm = qpsMedTower;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = arReportDb.Eod_DailyDoctorsTarget(vm.StartDate);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReports_EOD_DailyDoctorsTarget.rdl";
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                //reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                //  reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Address", "Daily Doctors Target - EOD"));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Company", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("Category", vm.CategoryText == null ? "ALL" : vm.CategoryText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("BillType", vm.BillTypeText == null ? "ALL" : vm.BillTypeText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("PatientType", vm.PatientTypeText == null ? "ALL" : vm.PatientTypeText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DateFrom", vm.StartDate.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DateTo", vm.EndDate.ToString()));

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


        public ActionResult Eod_DailyRevenueDetail()
        {
            FeatureID = "1985";
            var viewModel = new NetRevenueVM()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Eod_DailyRevenueDetail(NetRevenueVM qpsMedTower)
        {

            var vm = qpsMedTower;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = arReportDb.Eod_DailyRevenueDetail(vm.StartDate);
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\FinanceReports_EOD_DailyRevenueDetail.rdl";
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                //reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                //  reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Address", "Daily Revenue Detail - EOD"));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Company", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("Category", vm.CategoryText == null ? "ALL" : vm.CategoryText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("BillType", vm.BillTypeText == null ? "ALL" : vm.BillTypeText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("PatientType", vm.PatientTypeText == null ? "ALL" : vm.PatientTypeText.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DateFrom", vm.StartDate.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DateTo", vm.EndDate.ToString()));

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

        #region BEVERLY
        public ActionResult Beverly_OPRevenue()
        {
            var vieModel = new FinanceReportsOPRevenue()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StartDate2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                BillTypes = new List<KeyValuePair<Enumerations.BillType, string>>{
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.ALL,Enumerations.BillType.ALL.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.NOTCANCELLED,Enumerations.BillType.NOTCANCELLED.ToString()),
                                         new KeyValuePair<Enumerations.BillType, string>(Enumerations.BillType.CANCELLED,Enumerations.BillType.CANCELLED.ToString())
                                },
                PatientBillTypes = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                      //  new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.ALL, Enumerations.PatientBillType.ALL.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                                },
                PatientBillTypes2 = new List<KeyValuePair<Enumerations.PatientBillType, string>>{
                                         new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CASH, Enumerations.PatientBillType.CASH.ToString()),
                                        new KeyValuePair<Enumerations.PatientBillType, string>(Enumerations.PatientBillType.CHARGE, "COMPANY")
                                },
                DepartmentList = departmentDB.getAllDepartment(),

                SortByCancellationDate = false
            };

            return View(vieModel);
        }

        [HttpPost]
        public ActionResult Beverly_OPRevenue(FinanceReportsOPRevenue financeReportsOPRevenue)
        {
            var EmpId = Request.Form["EmpId"];
            var ModeofPayment = Request.Form["ModeofPayment"];
            var vm = financeReportsOPRevenue;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();



                if (financeReportsOPRevenue.BillType == Enumerations.BillType.NOTCANCELLED)
                {
                    reportData = opRevenueDB.getOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());
                }
                else if (financeReportsOPRevenue.BillType == Enumerations.BillType.CANCELLED)
                {
                    reportData = opRevenueDB.getCancelledOPRevenue(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());
                }
                else
                {
                    reportData = opRevenueDB.getAllOPRevenue_BEVERLY(vm.StartDate, vm.EndDate.AddDays(1), (int)vm.PatientBillType, vm.CompanyId, vm.DepartmentId, vm.DoctorId, vm.SortByCancellationDate, vm.PIN.HasValue ? vm.PIN.Value : 0, EmpId.ToString(), ModeofPayment.ToString());

                }
                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\Report_AllOPRevenue_BEVERLY.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("OPRevenue", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

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


        #region CST and Pharmacy Cost Price
        public ActionResult CSTAndPharmacyCostPrice()
        {
            FeatureID = "1985";
            var viewModel = new VMCSTAndPharmacyCostPrice();
            //{
            //    PIN = cst.Generic_S2AllPIN(""),
            //    AdmissionDate = new List<Select2ListModel>(), // cst.Generic_S2AllPIN(""),
            //    ReportType = false
            //};
            return View(viewModel);
        }

        public JsonResult Generic_S2AllPIN(string search)
        {
            return Json(cst.Generic_S2AllPIN(search), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Generic_S2AdmissionDate(string search)
        {
            return Json(cst.Generic_S2AdmissionDate(search), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CSTAndPharmacyCostPrice(VMCSTAndPharmacyCostPrice viewModel)
        {

            if (Request.IsAjaxRequest())
            {
                //if (ModelState.IsValid)
                //{
                    var data = new DataTable();
                    if (viewModel.ReportType == 1) //summary
                        data = cst.RptCSTAndPharmacyCostPrice(viewModel.AdmissionDate.ToString("dd-MMM-yyyy"), viewModel.AdmissionDate.ToString("dd-MMM-yyyy"), viewModel.PIN);
                    else
                        data = cst.RptCSTCostPriceItemLevel(viewModel.BillNo.ToString());

                    if (data.Rows.Count > 0)
                    {


                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;



                        //reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        if (viewModel.ReportType == 1) //summary
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\FinanceReports\RptCSTAndPharmacyCostPrice.rdl";
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.AdmissionDate.ToString("dd-MMM-yyyy")));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.AdmissionDate.ToString("dd-MMM-yyyy")));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("ListOfRegistrationNo", viewModel.PIN));
                        }
                        else
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\FinanceReports\RptCSTAndPharmacyCostPriceDetail.rdl";
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                            //reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                            reportViewer.LocalReport.SetParameters(new ReportParameter("BillNo", viewModel.BillNo.ToString()));
                            reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
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

                //}
            }

            return Content("");
        }
        #endregion

        #region CL Detailed report  And CL Revenue
        public ActionResult CLDetailedReport()
        {
             FeatureID = "1985";
            var viewModel = new CLDetailedReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Date = DateTime.Now.Month+"-" +DateTime.Now.Year+"",
                BillTypes = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(0,"ALL"),
                                         new KeyValuePair<int, string>(1,"OP"),
                                         new KeyValuePair<int, string>(2,"IP")
                            },
              };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CLDetailedReport(CLDetailedReport viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = financereportDB.getCLDetailedReport(viewModel.Date,viewModel.BillType);

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\rptCLDetails2.rdl";

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
              //  reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();
        }


        public ActionResult CLRevenueRecon()
        {
             FeatureID = "1985";
            var viewModel = new CLRevenueRecon()
            {
                StartDate =  DateTime.Now.Month + "-" + DateTime.Now.Day + "-"+DateTime.Now.Year,
                EndDate =  DateTime.Now.Month + "-" + DateTime.Now.Day + "-"+DateTime.Now.Year,
                 BillTypes = new List<KeyValuePair<int, string>>{
                                         new KeyValuePair<int, string>(0,"All"),
                                         new KeyValuePair<int, string>(1,"Reconciled"),
                                         new KeyValuePair<int, string>(2,"Not Reconciled"),
                                         new KeyValuePair<int, string>(3,"No CL Generated"),
                                         new KeyValuePair<int, string>(4,"No Revenue Generated") 
                            },
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CLRevenueRecon(CLRevenueRecon viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = financereportDB.CLRevenueRecon(viewModel.StartDate,viewModel.EndDate,viewModel.BillType );


                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportDocPath = @"\Areas\ManagementReports\Reports\FinanceReports\rptCLvsRevenue2.rdl";

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
               // reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.SizeToReportContent = true;
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
