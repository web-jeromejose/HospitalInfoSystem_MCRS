using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;
using Microsoft.Reporting.WebForms;


namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class DeficiencyFilesController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            DeficiencyFilesModel m = new DeficiencyFilesModel();
            //m.ReportType = 0;
            //m.StartDate = DateTime.Now.ToString("dd/MM/yyyy");
            //m.EndDate = DateTime.Now.ToString("dd/MM/yyyy");
            m.Floors = "0";
            m.Group = "D";
            return View(m);
        }

        [HttpPost]
        public ActionResult Index(DeficiencyFilesModel param)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var deficiencyModel = new System.Data.DataTable();

            if (param.ReportType == 0)
            {
                deficiencyModel =  _clPatientStatisticsDB.Deficiency(param.StartDate, param.EndDate, param.Group, param.Floors == "0" ? "A" : param.Floors, param.IncludeStandards); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\Deficiency.rdl";
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Group", param.Group));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Floors", param.Floors == "0" ? "A" : param.Floors));
                reportViewer.LocalReport.SetParameters(new ReportParameter("IncludeNew", param.IncludeStandards));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("Label", string.Empty));
            }
            else
            {
                if (param.GraphType == 0)
                {
                    deficiencyModel = _clPatientStatisticsDB.DeficiencyDepartment(param.StartDate, param.EndDate, param.Group, param.Floors == "0" ? "A" : param.Floors, param.IncludeStandards); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyDepartment.rdl";
                    reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("Group", param.Group));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("Floors", param.Floors == "0" ? "A" : param.Floors));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("IncludeNew", param.IncludeStandards));
                    //reportViewer.LocalReport.SetParameters(new ReportParameter("Label", String.Empty));
                    
                }
                else if (param.GraphType == 2)
                {
                    var startdate = DateTime.Parse(param.StartDate);
                    var enddate = DateTime.Parse(param.EndDate);
                    var monthcount = Math.Abs((startdate.Month - enddate.Month) + 12 * (startdate.Year - enddate.Year)) + 1;

                    if (param.IncludeStandards == "N")
                    {
                        deficiencyModel = _clPatientStatisticsDB.DeficiencyMonthGraph(param.StartDate, param.EndDate, monthcount.ToString(), param.Group); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyDepartmentPercentMonthly.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("Group", param.Group));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("Label", String.Empty));
                    }
                    else
                    {
                        deficiencyModel = _clPatientStatisticsDB.DeficiencyMonthGraphWithNew(param.StartDate, param.EndDate, monthcount.ToString(), param.Group); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyDepartmentPercentMonthly.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("Group", param.Group));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("Label", String.Empty));
                    }
                }
                else
                {
                    if (param.IncludeStandards == "N")
                    {
                        deficiencyModel = _clPatientStatisticsDB.DeficiencyDepartmentGraph(param.StartDate, param.EndDate, param.Group); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyDepartmentPercent.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("Group", param.Group));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("Label", String.Empty));
                    }
                    else
                    {
                        deficiencyModel = _clPatientStatisticsDB.DeficiencyDepartmentGraphWithNew(param.StartDate, param.EndDate, param.Group); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyDepartmentPercent.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("Group", param.Group));
                        //reportViewer.LocalReport.SetParameters(new ReportParameter("Label", String.Empty));
                    }
                }
            }

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsDeficiency", deficiencyModel));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param);
        }

        public JsonResult GetFloors()
        {
            var floors = new List<GenericListModel>
            {
                new GenericListModel { id = 0, name = "ALL", text = "ALL" }, 
                new GenericListModel { id = 1, name = "1ST", text = "1ST" },
                new GenericListModel { id = 2, name = "2ND", text = "2ND" },
                new GenericListModel { id = 3, name = "3RD", text = "3RD" },
                new GenericListModel { id = 4, name = "4TH", text = "4TH" },
                new GenericListModel { id = 5, name = "5TH", text = "5TH" },
                new GenericListModel { id = 6, name = "6TH", text = "6TH" },
                new GenericListModel { id = 7, name = "7TH", text = "7TH" },
                new GenericListModel { id = 8, name = "8TH", text = "8TH" }
            };

            var json = floors.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
