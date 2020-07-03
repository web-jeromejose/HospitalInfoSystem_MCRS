using System;
using System.Collections.Generic;
using System.Data;
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
    public class ERConsultationStatisticCountController : Controller
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ERConsultationStatisticCountModel param)
        {
            var returnModel = new ERConsultationStatisticCountModel();
            var erconsultationstatisticcountModel = _clPatientStatisticsDB.ERConsultationStatisticCount(param.DateFrom, param.DateTo);

            returnModel.DateFrom = param.DateFrom;
            returnModel.DateTo = param.DateTo;
            returnModel.ERDoctors = (int)erconsultationstatisticcountModel.Rows[0][0];
            returnModel.NonERDoctors = (int)erconsultationstatisticcountModel.Rows[1][1];
            returnModel.TotalChargeinER = returnModel.ERDoctors + returnModel.NonERDoctors;

            return View(returnModel);
        }

    }
}
