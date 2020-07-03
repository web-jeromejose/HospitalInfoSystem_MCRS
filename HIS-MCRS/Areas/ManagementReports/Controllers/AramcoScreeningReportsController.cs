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

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class AramcoScreeningReportsController : Controller
    {
        OPRevenueDB opRevenueDB = new OPRevenueDB();
        IPDischargeDB ipDischargeDB = new IPDischargeDB();
        CompanyDB companyDB = new CompanyDB();
        EmployeeDB employeeDB = new EmployeeDB();
        DepartmentDB departmentDB = new DepartmentDB();
        IPRevenueDB ipRevenueDB = new IPRevenueDB();
        CategoryDB categoryDB = new CategoryDB();
        AdjustmentsDB adjustmentDB = new AdjustmentsDB();
        DoctorAnalysisDB DoctorAnalysisDB = new DoctorAnalysisDB();
        AramcoScreeningDB AramcoDb = new AramcoScreeningDB();

        //
        // GET: /ManagementReports/AramcoScreeningReports/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult AramcoScreeningTest()
        {
            var viewModel = new AramcoReportsViewModel()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryAgeList = AramcoDb.getMCRS_AramcoScreenFile(),
            };
            return View(viewModel);
        }


    }
}
