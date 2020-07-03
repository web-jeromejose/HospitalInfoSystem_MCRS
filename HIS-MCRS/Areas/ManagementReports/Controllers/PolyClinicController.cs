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
    public class PolyClinicController : BaseController
    {
        PolyClinicDB _clPolyClinicDB = new PolyClinicDB();
        EmployeeDB employeeDB = new EmployeeDB();
        DepartmentDB departmentDB = new DepartmentDB();
        ItemDB itemDB = new ItemDB();
        //
        // GET: /MCRS/PolyClinic/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OPPatientCount()
        {
            var viewModel = new PolyClinicOPPatientCount() {

             StartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day),
             EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
             OPDSelectionList = new List<KeyValuePair<OPDSelectionCount,string>>(){
                 new KeyValuePair<OPDSelectionCount,string>(OPDSelectionCount.PATIENTVISITCOUNT,"Patient Visits Count"),
                 new KeyValuePair<OPDSelectionCount,string>(OPDSelectionCount.PATIENTREVISITCOUNT,"Patient Revisits Count"),
                 new KeyValuePair<OPDSelectionCount,string>(OPDSelectionCount.NEWPATIENTCOUNT,"New Patient Count")
             },
             OPDSelection = OPDSelectionCount.PATIENTVISITCOUNT
            
            };


            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OPPatientCount(PolyClinicOPPatientCount viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                var opdpatientcountModel = _clPolyClinicDB.OPPatientCount(viewModel.StartDate.ToString("dd-MMM-yyyy"), viewModel.EndDate.AddDays(1).ToString("dd-MMM-yyyy"), (int)viewModel.OPDSelection);

                if (opdpatientcountModel.Rows.Count > 0)
                {

                    ReportViewerVm reportVM = new ReportViewerVm();
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    var description = "";
                    if (viewModel.OPDSelection == OPDSelectionCount.PATIENTVISITCOUNT)
                    {
                        description = "Number of Patient Visited the Hospital Report";
                    }
                    else if (viewModel.OPDSelection == OPDSelectionCount.PATIENTREVISITCOUNT)
                    {
                           description = "Patient Revisit Against Total Number/Department Report" ;
                    }
                    else
                    {
                           description = "Number of New Patients Visited the Hospital Report" ;
                    }

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OpPatientVisitCount.rdl";
                    reportViewer.LocalReport.SetParameters(new ReportParameter("FromDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("ToDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("Label", description));

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsOPDPatientCount", opdpatientcountModel));
                    reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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
                    return Content(Errors.ReportContent("NO RECORD FOUND"));
                }

            }


            return Content("");
        }

        public ActionResult OPProcedureStatistics()
        {
            var viewModel = new PolyClinicProcedureStatistics()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ReportTypeList = new List<KeyValuePair<ReportType, string>>()
                {
                    new KeyValuePair<ReportType, string>(ReportType.DEFAULT, "Default"),
                    new KeyValuePair<ReportType, string>(ReportType.BARGRAPH, "Bar Graph"),
                    new KeyValuePair<ReportType, string>(ReportType.LINEGRAPH, "Line Graph")
                },
                ReportType = ReportType.DEFAULT
            };


            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OPProcedureStatistics(PolyClinicProcedureStatistics viewModel)
        {
                if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest()) {

                    var data = _clPolyClinicDB.getOPProcedureStatistics(viewModel.StartDate, viewModel.EndDate.AddDays(1));
                    if (data.Rows.Count > 0)
                    {
                        ReportViewerVm reportVM = new ReportViewerVm();
                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        if (viewModel.ReportType == ReportType.BARGRAPH)
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OPProcedureStatistics_BarGraph.rdlc";
                        
                        }
                        else if (viewModel.ReportType == ReportType.LINEGRAPH)
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OPProcedureStatistics_LineGraph.rdlc";
                        
                        }
                        else
                        {
                            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OPProcedureStatistics_Table.rdlc";
                        
                        }

                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("OPProcedureStatistics", data));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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

        public ActionResult SummaryOfCancelledAppointments()
        {
            var viewModel = new PolyClinicSummaryOfCancelledAppointments()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                PatientTypeList = new List<KeyValuePair<DoctorSchedulePatientType, string>>(){
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.ALL,DoctorSchedulePatientType.ALL.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.IP,DoctorSchedulePatientType.IP.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.OP,DoctorSchedulePatientType.OP.ToString())
                 },
                 DepartmentList = departmentDB.getAllDepartment(),
                 ReportOption  = 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SummaryOfCancelledAppointments(PolyClinicSummaryOfCancelledAppointments viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    var cancelledReservations = new DataTable();
                    var reportFilePath = "";
                    if (viewModel.ReportOption == 0)
                    {
                        cancelledReservations = _clPolyClinicDB.getCancelledPatientReservationSummary(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.EmployeeId.HasValue ? viewModel.EmployeeId.Value : 0, (int)viewModel.PatientType);
                        reportFilePath = @"\Areas\ManagementReports\Reports\PolyClinic\CancelledPatientReservationSummary.rdlc";
                    }
                    else if (viewModel.ReportOption == 1)
                    {
                        cancelledReservations = _clPolyClinicDB.getCancelledPatientReservationSummaryByDoctor(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.DoctorId.HasValue ? viewModel.DoctorId.Value : 0, (int)viewModel.PatientType);
                        reportFilePath = @"\Areas\ManagementReports\Reports\PolyClinic\CancelledPatientReservationSummaryByDoctor.rdlc";
                    }
                    else
                    {
                        cancelledReservations = _clPolyClinicDB.getCancelledPatientReservationSummaryByDepartment(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.DepartmentId, (int)viewModel.PatientType);
                        reportFilePath = @"\Areas\ManagementReports\Reports\PolyClinic\CancelledPatientReservationSummaryByDepartment.rdlc";
                    }
                       

                    if (cancelledReservations.Rows.Count > 0)
                    {
                        
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportFilePath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsSummaryPatientReservation", cancelledReservations));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");

                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));

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

        public ActionResult PatientReservationSummary()
        {
            var viewModel = new PolyClinicPatientReservationSummary()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                PatientTypeList = new List<KeyValuePair<DoctorSchedulePatientType, string>>(){
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.ALL,DoctorSchedulePatientType.ALL.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.IP,DoctorSchedulePatientType.IP.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.OP,DoctorSchedulePatientType.OP.ToString())
                }
            };

            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult PatientReservationSummary(PolyClinicPatientReservationSummary viewModel)
        {
            if (ModelState.IsValid)
            {
                var patientnationalitystatisticsModel = _clPolyClinicDB.SummaryPatientReservation(viewModel.EmployeeId.HasValue? viewModel.EmployeeId.Value:0, viewModel.StartDate, viewModel.EndDate.AddDays(1));

                ReportViewer reportViewer = new ReportViewer();
                ReportViewerVm reportVM = new ReportViewerVm();
                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\SummaryPatientReservation.rdl";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsSummaryPatientReservation", patientnationalitystatisticsModel));
                reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToString()));

                reportViewer.SizeToReportContent = true;
                reportViewer.Width = Unit.Percentage(100);
                reportViewer.Height = Unit.Percentage(100);
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }
            

            return Content("");
        }

        public ActionResult CancelledPatientAppointment()
        {
            var viewModel = new PolyClinicListOfCancelledPatientAppointment()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day),
                EndDate   = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day),
                PatientTypeList = new List<KeyValuePair<DoctorSchedulePatientType, string>>(){
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.ALL,DoctorSchedulePatientType.ALL.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.IP,DoctorSchedulePatientType.IP.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.OP,DoctorSchedulePatientType.OP.ToString())
                },
                DoctorId  = 0,
                DepartmentList = departmentDB.getAllDepartment()

            };



            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CancelledPatientAppointment(PolyClinicListOfCancelledPatientAppointment viewModel)
        {
            if (ModelState.IsValid)
            {
                if(Request.IsAjaxRequest()){
                    var operatorId = viewModel.EmployeeId.HasValue? viewModel.EmployeeId.Value: 0;
                    var doctorId = viewModel.DoctorId.HasValue? viewModel.DoctorId.Value: 0;
                    var appointments = _clPolyClinicDB.getCancelledPatientReservation(viewModel.StartDate, viewModel.EndDate.AddDays(1), operatorId, doctorId,(int) viewModel.PatientType, viewModel.DepartmentId);

                    if (appointments.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\CancelledPatientAppointmentList.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CancelledPatientAppointments", appointments));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                        if (viewModel.DoctorId.HasValue)
                        {
                            reportViewer.LocalReport.SetParameters(new ReportParameter("filterDoctor", Boolean.TrueString));
                        }
                        if (viewModel.EmployeeId.HasValue)
                        {
                            reportViewer.LocalReport.SetParameters(new ReportParameter("filterCancelledByEmployee", Boolean.TrueString));
                        }
                        if (viewModel.DepartmentId > 0)
                        {
                            reportViewer.LocalReport.SetParameters(new ReportParameter("filterDepartment", Boolean.TrueString));
                        }
                        if (viewModel.PatientType> 0)
                        {
                            reportViewer.LocalReport.SetParameters(new ReportParameter("filterPatientType", Boolean.TrueString));
                        }

                     
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


        public ActionResult OPItemBillCount()
        {
            var viewModel = new PolyClinicOPItemBillCount() {
             StartDate      = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
             EndDate        = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
             Services = _clPolyClinicDB.getOPService()//getOPService
            };



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult OPItemBillCount(PolyClinicOPItemBillCount viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    var serviceItems = _clPolyClinicDB.getOPBillServiceItemCount(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.StrItemCodes, viewModel.ServiceId);

                    if (serviceItems.Rows.Count > 0)
                    {
                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OPItemBillCount.rdl";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("OPItemBillCount", serviceItems));
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


            return Content("") ;
        }

        public ActionResult SearchEmployee(string searchString)
        {
            return Json(employeeDB.findEmployee(searchString), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctors(searchString), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchServiceItem(string searchString, string serviceId)
        {
            return Json(itemDB.searchServiceItem(searchString, serviceId), JsonRequestBehavior.AllowGet);
        }


        public ActionResult BookedAppointment()
        {
            var viewModel = new PolyClinicSummaryOfCancelledAppointments()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                PatientTypeList = new List<KeyValuePair<DoctorSchedulePatientType, string>>(){
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.ALL,DoctorSchedulePatientType.ALL.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.IP,DoctorSchedulePatientType.IP.ToString()),
                 new KeyValuePair<DoctorSchedulePatientType,string>(DoctorSchedulePatientType.OP,DoctorSchedulePatientType.OP.ToString())
                 },
                DepartmentList = departmentDB.getAllDepartment(),
                ReportOption = 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BookedAppointment(PolyClinicSummaryOfCancelledAppointments viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    var cancelledReservations = new DataTable();
                    var reportFilePath = "";

                    if (viewModel.ReportOption == 1)//by doctor
                    {

                        cancelledReservations = _clPolyClinicDB.getBookedAppointmentByDoctor(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.DoctorId.HasValue ? viewModel.DoctorId.Value : 0, (int)viewModel.PatientType);
                        reportFilePath = @"\Areas\ManagementReports\Reports\PolyClinic\BookedAppointmentByDoctor.rdl";
                    
                    }
                    else
                    {

                        cancelledReservations = _clPolyClinicDB.getBookedAppointmentByDepartment(viewModel.StartDate, viewModel.EndDate.AddDays(1), viewModel.DepartmentId.ToString(), (int)viewModel.PatientType);
                        reportFilePath = @"\Areas\ManagementReports\Reports\PolyClinic\BookedAppointmentByDepartment.rdl";
                    

                    }



                    if (cancelledReservations.Rows.Count > 0)
                    {

                        ReportViewer reportViewer = new ReportViewer();
                        ReportViewerVm reportVM = new ReportViewerVm();
                        reportViewer.ProcessingMode = ProcessingMode.Local;

                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportFilePath;
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", cancelledReservations));
                        reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
                        reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                        reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));

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

        public ActionResult BookvsWalkin()
        {
            var viewModel = new PolyClinicOPPatientCount()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                OPDSelectionList = new List<KeyValuePair<OPDSelectionCount, string>>(){
                 new KeyValuePair<OPDSelectionCount,string>(OPDSelectionCount.PATIENTVISITCOUNT,"Patient Visits Count"),
                 new KeyValuePair<OPDSelectionCount,string>(OPDSelectionCount.PATIENTREVISITCOUNT,"Patient Revisits Count"),
                 new KeyValuePair<OPDSelectionCount,string>(OPDSelectionCount.NEWPATIENTCOUNT,"New Patient Count")
             },
                OPDSelection = OPDSelectionCount.PATIENTVISITCOUNT

            };



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BookvsWalkin(PolyClinicOPPatientCount viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                var opdpatientcountModel = _clPolyClinicDB.BookvsWalkin(viewModel.StartDate.ToString("dd-MMM-yyyy"), viewModel.EndDate.AddDays(1).ToString("dd-MMM-yyyy"), (int)viewModel.OPDSelection);

                if (opdpatientcountModel.Rows.Count > 0)
                {

                    ReportViewerVm reportVM = new ReportViewerVm();
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    var description = "";
                    
                    description = "Appointment and Walk-in Patients ";
                 

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\BookvsWalkin.rdl";
                    reportViewer.LocalReport.SetParameters(new ReportParameter("FromDate", viewModel.StartDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("ToDate", viewModel.EndDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("Label", description));

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", opdpatientcountModel));
                    reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
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
                    return Content(Errors.ReportContent("NO RECORD FOUND"));
                }

            }


            return Content("");
        }



    }

}
