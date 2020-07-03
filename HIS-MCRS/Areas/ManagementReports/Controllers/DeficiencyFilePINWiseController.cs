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
    public class DeficiencyFilesPINWiseController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            DeficiencyFilesPINWiseModel m = new DeficiencyFilesPINWiseModel();
            List<DeficiencyFilesPINWiseModel.YearDate> yeardate = new List<DeficiencyFilesPINWiseModel.YearDate>();
            DateTime yearToday = DateTime.Now;
            for (int i = 2006; i <= yearToday.Year; i++)
            {
                yeardate.Add(new DeficiencyFilesPINWiseModel.YearDate { Id = i, Value = i.ToString() });
            }
            m.YearOptions = yeardate;
            m.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            m.EndDate = DateTime.Now.ToString("MM/dd/yyyy");
            m.IncludeStandards = true;
            m.Speciality = "0";
            m.Doctor = "0";
            m.PIN = "0";

            return View(m);
        }

        [HttpPost]
        public ActionResult Index(DeficiencyFilesPINWiseModel param)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var deficiencypinwiseModel = new System.Data.DataTable();

            List<DeficiencyFilesPINWiseModel.YearDate> yeardate = new List<DeficiencyFilesPINWiseModel.YearDate>();
            DateTime yearToday = DateTime.Now;
            for (int i = 2006; i <= yearToday.Year; i++)
            {
                yeardate.Add(new DeficiencyFilesPINWiseModel.YearDate { Id = i, Value = i.ToString() });
            }
            param.YearOptions = yeardate;

            if (!param.IncludeParameter)
            {
                if (param.PIN == "0")
                {
                    if (param.ExcludeAdmission)
                    {
                        deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWiseAll(param.StartDate, param.EndDate, param.ExcludeAdmission);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWise.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                    }
                    else
                    {
                        deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWiseAll(param.StartDate, param.EndDate, param.ExcludeAdmission);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWise.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                    }
                }
                else
                {
                    if (param.ExcludeAdmission)
                    {
                        deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWise(param.StartDate, param.EndDate, param.PIN, param.ExcludeAdmission);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWise.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                    }
                    else
                    {
                        deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWise(param.StartDate, param.EndDate, param.PIN, param.ExcludeAdmission);

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWise.rdl";
                        reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                    }
                }
            }
            else
            {
                if ((param.Speciality != "0") && (param.Doctor == "0"))
                {
                    deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWiseComparisonAll(param.ExcludeAdmission, param.MonthFromDate, param.YearFromDate, param.MonthToDate, param.YearToDate, param.Standard, param.Speciality, param.Doctor);

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWiseComparison.rdl";
                    reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                }
                else if ((param.Speciality != "0") && (param.Doctor != "0"))
                {
                    deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWiseComparison(param.ExcludeAdmission, param.MonthFromDate, param.YearFromDate, param.MonthToDate, param.YearToDate, param.Doctor, param.Standard);

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWiseComparison.rdl";
                    reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                }
                else
                {
                    deficiencypinwiseModel = _clPatientStatisticsDB.DeficiencyPINWiseComparisonSpeciality(param.ExcludeAdmission, param.MonthFromDate, param.YearFromDate, param.MonthToDate, param.YearToDate, param.Standard);

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\DeficiencyPINWiseComparison.rdl";
                    reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                }

            }

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsDeficiencyPINWise", deficiencypinwiseModel));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param);
        }

        public JsonResult GetSpeciality()
        {
            var getspecialityModel = _clPatientStatisticsDB.GetSpeciality();
            getspecialityModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });
            
            var json = getspecialityModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctor(string specialityid)
        {
            //if (String.IsNullOrEmpty(specialityid)) return null;
            var getdoctorModel = new List<GenericListModel>();
            getdoctorModel = !string.IsNullOrEmpty(specialityid) ? _clPatientStatisticsDB.GetDoctor(int.Parse(specialityid)) : getdoctorModel;
            getdoctorModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });
            
            var json = getdoctorModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPIN(string doctorid, string fromdate, string todate)
        {
            //if (String.IsNullOrEmpty(doctorid)) return null;
            var getpinModel = new List<GenericListModel>();
            getpinModel = !string.IsNullOrEmpty(doctorid) ? _clPatientStatisticsDB.GetPIN(int.Parse(doctorid), fromdate, todate) : getpinModel;
            getpinModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });

            var json = getpinModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStandard(bool isoldstandard)
        {
            var getstandardModel = _clPatientStatisticsDB.GetStandard(isoldstandard);
            var json = getstandardModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
