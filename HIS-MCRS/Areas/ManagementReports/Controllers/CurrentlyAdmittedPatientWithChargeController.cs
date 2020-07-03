using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Data;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;
using Microsoft.Reporting.WebForms;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class CurrentlyAdmittedPatientWithChargeController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            //var radioModels = new ParamRadioModel();
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamRadioModel param)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            switch (param.id)
            {
                case 0:

                    var patientadmitted10daysModel = _clPatientStatisticsDB.PatientAdmitted10Days();
                    var param1 = _clPatientStatisticsDB.Param1();
                    var param2 = _clPatientStatisticsDB.Param2();
                    var paramtotalcharges = _clPatientStatisticsDB.ParamTotalCharges();

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientAdmitted10Days.rdl";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsPatientAdmitted10Days", patientadmitted10daysModel));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsParam1", param1));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsParam2", param2));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsParamTotalCharges", paramtotalcharges));

                    break;
                case 1:
                    var patientadmitted10dayschargeModel = _clPatientStatisticsDB.PatientAdmitted10DaysCharge();

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientAdmitted10DaysCharge.rdl";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsPatientAdmitted10DaysCharge", patientadmitted10dayschargeModel));

                    break;
                case 2:
                    var patientadmitted10daysdoctorModel = _clPatientStatisticsDB.PatientAdmitted10DaysDoctor();

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientAdmitted10DaysDoctor.rdl";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsPatientAdmitted10DaysDoctor", patientadmitted10daysdoctorModel));
                    break;
            }

            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View();
            //return View("CathProcedureDoneList", ViewBag.ReportViewer);

        }
    }
}
