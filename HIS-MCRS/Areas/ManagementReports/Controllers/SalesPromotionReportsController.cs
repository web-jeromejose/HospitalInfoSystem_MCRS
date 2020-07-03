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
    public class SalesPromotionReportsController : Controller
    {
        //
        // GET: /ManagementReports/SalesPromotionReports/
   
        PatientStatisticsDB patientstatsDB = new PatientStatisticsDB();
        DepartmentDB departmentDB = new DepartmentDB();
        EmployeeDB employeeDB = new EmployeeDB();
        CategoryDB categoryDB = new CategoryDB();
        CompanyDB companyDB = new CompanyDB();
        MohDB MohDB = new MohDB();
        SalesPromotionDB SalesPromoDB = new SalesPromotionDB();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CensusAndGraphs()
        {
            var viewModel = new SPCensusGraphs()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ReportTypeList = new List<KeyValuePair<ReportType, string>>()
                {
                    new KeyValuePair<ReportType, string>(ReportType.DEFAULT, "Default"),
                    new KeyValuePair<ReportType, string>(ReportType.BARGRAPH, "Bar Graph"),
                    new KeyValuePair<ReportType, string>(ReportType.LINEGRAPH, "Line Graph")
                },
                ReportType = ReportType.DEFAULT,
                CategoryList = MohDB.getMOHCategories()


                
            };
            return View(viewModel);
        }

       
        


        [HttpPost]
        public ActionResult CensusAndGraphs(SPCensusGraphs viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {

                    //var data = _clPolyClinicDB.getOPProcedureStatistics(viewModel.StartDate, viewModel.EndDate.AddDays(1));
                    var data = SalesPromoDB.getSPCensusGraph(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.CategoryId);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewerVm reportVM = new ReportViewerVm();
                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if (viewModel.ReportType == ReportType.BARGRAPH)
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPCensusGraphs_BarGraph.rdl";

                        }
                        else if (viewModel.ReportType == ReportType.LINEGRAPH)
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPCensusGraphs_LineGraph.rdl";

                        }
                        else
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPCensusGraphs_Table.rdl";

                        }

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("categoryname", viewModel.CategoryName));
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

        public ActionResult DailyIncomeDetails()
        {

            var viewModel = new SPDailyIncome()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ReportTypeList = new List<KeyValuePair<ReportCategoryType, string>>()
                {
                    new KeyValuePair<ReportCategoryType, string>(ReportCategoryType.ALLPATIENT, "By All Patient"),
                    new KeyValuePair<ReportCategoryType, string>(ReportCategoryType.GOVOFFICE, "By Government Office"),
                    new KeyValuePair<ReportCategoryType, string>(ReportCategoryType.DEPARTMENT, "By Department")
                },
                ReportType = ReportCategoryType.ALLPATIENT,
                CategoryList = MohDB.getMOHCategories()



            };
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult DailyIncomeDetails(SPDailyIncome viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    ReportViewerVm reportVM = new ReportViewerVm();
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.ProcessingMode = ProcessingMode.Local;


                    //var data = _clPolyClinicDB.getOPProcedureStatistics(viewModel.StartDate, viewModel.EndDate.AddDays(1));
                    string reportid = viewModel.ReportType.ToString();
                    var data = SalesPromoDB.getSPDailyIncome(viewModel.StartDate, viewModel.EndDate.AddDays(1), reportid);
                    if (data.Rows.Count > 0)
                    {
                     

                        if (viewModel.ReportType == ReportCategoryType.DEPARTMENT)
                        {
                            return Content(Errors.ReportContent("NO WHCIT.VW_SalesPromotion   "));
                           // reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPDailyIncome_Dept.rdl";

                        }
                        else if (viewModel.ReportType == ReportCategoryType.GOVOFFICE)
                        {

                            return Content(Errors.ReportContent("NO WHCIT.VW_SalesPromotion   "));
                            //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPDailyIncome_Gov.rdl";

                        }
                        else
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPDailyIncome.rdl";

                        }

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToShortDateString()));
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

        public ActionResult DailyCensus()
        {
            var viewModel = new SPCensusGraphs()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                //EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                //ReportTypeList = new List<KeyValuePair<ReportType, string>>()
                //{
                //    new KeyValuePair<ReportType, string>(ReportType.DEFAULT, "Default"),
                //    new KeyValuePair<ReportType, string>(ReportType.BARGRAPH, "Bar Graph"),
                //    new KeyValuePair<ReportType, string>(ReportType.LINEGRAPH, "Line Graph")
                //},
                //ReportType = ReportType.DEFAULT,
                //CategoryList = MohDB.getMOHCategories()



            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DailyCensus(SPCensusGraphs viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {

                    //var data = _clPolyClinicDB.getOPProcedureStatistics(viewModel.StartDate, viewModel.EndDate.AddDays(1));
                    var data = SalesPromoDB.getDailyCensus(viewModel.StartDate, viewModel.StartDate.AddDays(1));
                    if (data.Rows.Count > 0)
                    {
                        ReportViewerVm reportVM = new ReportViewerVm();
                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPDailyCensus.rdl";

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.StartDate.AddDays(1).ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                                    
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

      
        public ActionResult IncomeYearlySummary()
        {
            var viewModel = new SPIncomeYearlySummary()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ReportTypeList = new List<KeyValuePair<ReportTypeByDoctor, string>>()
                {
                    new KeyValuePair<ReportTypeByDoctor, string>(ReportTypeByDoctor.BYOFFICE, "By Office"),
                    new KeyValuePair<ReportTypeByDoctor, string>(ReportTypeByDoctor.BYDOCTOR, "By Doctor"),
                    new KeyValuePair<ReportTypeByDoctor, string>(ReportTypeByDoctor.BYDEPARTMENT, "By Department")
                },
                ReportType = ReportTypeByDoctor.BYOFFICE,
                //CategoryList = MohDB.getMOHCategories()



            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IncomeYearlySummary(SPIncomeYearlySummary viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    //var data = _clPolyClinicDB.getOPProcedureStatistics(viewModel.StartDate, viewModel.EndDate.AddDays(1));
                    string reportid = viewModel.ReportType.ToString();
                    var data = SalesPromoDB.getSPDailyIncomeHistoricalReport(viewModel.StartDate, viewModel.StartDate.AddDays(1), reportid);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewerVm reportVM = new ReportViewerVm();
                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        //BYOFFICE = 0,
                        //BYDEPARTMENT = 1,
                        //BYDOCTOR = 2

                        if (reportid == "BYOFFICE")//Report_SalesPromotionIncome_Historical
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPIncomeHostorical.rdl";

                        }
                        else if (reportid == "BYDEPARTMENT") //Report_SalesPromotionIncome_Historical_Dept
                        {

                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPIncomeHostorical_Dept.rdl";

                        }
                        else //Report_SalesPromotionIncome_Historical_Doc
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPIncomeHostorical_Doc.rdl";


                        }

                       
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.StartDate.AddDays(1).ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));

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

        public ActionResult DailyPerformanceActualVsBudget()
        {

            var viewModel = new SalesPromotionDailyPerformanceActBudget()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ReportTypeList = new List<KeyValuePair<SalesPromotion_ReportType, string>>()
                {
                    new KeyValuePair<SalesPromotion_ReportType, string>(SalesPromotion_ReportType.BYDEPT, "BYDEPT"),
                    new KeyValuePair<SalesPromotion_ReportType, string>(SalesPromotion_ReportType.BYDOCTOR, "BYDOCTOR"),
                    //new KeyValuePair<ReportType, string>(SalesPromotion_ReportType.LINEGRAPH, "Line Graph")
                },
                ReportType = SalesPromotion_ReportType.BYDEPT,
                 BranchId = Bi_Site.SGH_JEDDAH.ToString(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DailyPerformanceActualVsBudget(SalesPromotionDailyPerformanceActBudget viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {

                    //var data = _clPolyClinicDB.getOPProcedureStatistics(viewModel.StartDate, viewModel.EndDate.AddDays(1));

                    // connncection in  ReportDoc.SetDatabaseLogon("sghit", "SGHIT", "130.1.2.223", "BI")
                    var data = SalesPromoDB.getDailyActualvsBudget(Request.Form["startdate"], viewModel.EndDate.AddDays(1), viewModel.BranchId);
                    if (data.Rows.Count > 0)
                    {
                        ReportViewerVm reportVM = new ReportViewerVm();
                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if (viewModel.BranchId == "SGH_JEDDAH")
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPIncomeHostorical.rdl";                            
                        }
                        else if (viewModel.BranchId == "BYDEPARTMENT")
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPIncomeHostorical_Dept.rdl";                            
                        
                        }
                        else
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\SalesPromotion\SPIncomeHostorical_Doc.rdl";                            
                       
                        }

                       
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.StartDate.AddDays(1).ToShortDateString()));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));

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



    }
}
