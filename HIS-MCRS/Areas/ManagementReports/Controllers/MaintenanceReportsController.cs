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
    public class MaintenanceReportsController : Controller
    {
        //
        // GET: /ManagementReports/MaintenanceReports/
        DepartmentDB departmentDb = new DepartmentDB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EquipmentBreakdownReport()
        {
            var viewModel = new MaintenanceReportsEquipmentBreakdown()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDb.getAllDepartment(),
                checkboxInDate = "0"

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EquipmentBreakdownReport(MaintenanceReportsEquipmentBreakdown maintenanceReportsBreakdown)
        {
            var vm = maintenanceReportsBreakdown;
            if(Request.IsAjaxRequest()){
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                // AS per Sir Jericho Table doesnt exist. mcrs made a mdf file that we dont have. so skip this module for now.
             //               Select a.FA_ITEM_NO,b.FA_ITEM_DESCRIPTION,d.DEPT_NAME,
             //case when a.status = 0 then 'Pending/On-Going' when a.status = 1 then 'Completed' else 'Closed' end ItemStatus,
             //a.JR_NO,a.DIAGNOSIS,a.REQ_DATE,a.IMPORTANCE,a.STATUS,a.WORK_TYPE_ID,a.EMP_NO,a.USER_ID
             //--,case when nvl(b.critical,0) = 1 then 'Critical' else 'Non-Critical' end ItemType
             //from MNT_WORK_REQUEST a 
             //left join syn_equipment_master_sgh4 b on a.FA_ITEM_NO = b.FA_ITEM_NO 
             //left join employee c on a.emp_no = c.emp_no
             //left join depart_ment d on c.department = d.department 
             //where a.JR_NO <> 0 


            
            }
            return View();
        }
    }
}
