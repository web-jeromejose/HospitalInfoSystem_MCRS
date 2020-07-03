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
    public class QpsReportsController : Controller
    {
        //
        // GET: /ManagementReports/QpsReports/
        QpsReportDB qpsreportDb = new QpsReportDB();
        PatientStatisticsDB patientstatsDB = new PatientStatisticsDB();
        DepartmentDB departmentDB = new DepartmentDB();
        EmployeeDB employeeDB = new EmployeeDB();
        CategoryDB categoryDB = new CategoryDB();
        CompanyDB companyDB = new CompanyDB();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PatientOrDone()
        {
            var viewModel = new QpsPatientOrDone()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
           
            return View(viewModel);
        }

        public ActionResult PatientAdmittedICU()
        {
            var viewModel = new QpsPatientAdmiitedICU()
            {
                StartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
            };
            return View(viewModel);
        }

        public ActionResult PatientAdmittedErOr()
        {
            var viewModel = new QpsPatientAdmittedInErOr() {
            StartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
            EndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day)
            };
            return View(viewModel);
        }

        public ActionResult PatientVisitedInER()
        {
            var viewModel = new QpsPatientVisitedER()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            return View(viewModel);
        }

        public ActionResult PatientListByAge()
        {
            
            var viewModel = new QpsPatientListByAge()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                AgeRange = patientstatsDB.GetAgeRange(),
            };
            return View(viewModel);
        }
        
        public ActionResult ErConsultationStats()
        {
            var viewModel = new ErConsultationStats() 
            { 
                StartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day)
            };
            return View(viewModel);
        }

        public ActionResult MrdAdmissionDischargeStats()
        {
            var viewModel = new MrdAdmissionDischargeStats()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                RepType = "0"

            };

            return View(viewModel);
        }

        public ActionResult AvgLengthStayCriticalAreas()
        {
            var viewModel = new AvgLengthStayCriticalAreas() {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDB.getAllDepartment(),
                //BillTypes = new List<KeyValuePair<int, string>>{
                //                         new KeyValuePair<int, string>(0,"ALL"),
                //                         new KeyValuePair<int, string>(1,"DISCHARGED"),
                //                         new KeyValuePair<int, string>(2,"UNDISCHARGED")
                // }

       
            };
            return View(viewModel);

        }
        public ActionResult XrayReferral()
        {
            var viewModel = new QpsXrayReferral() { 
            StartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
            EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };
         
            return View(viewModel);
        }

        public ActionResult BedOccupancy()
        {
            var viewModel = new QpsBedOccupancy()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                picu = "21", 
                nicu = "21"
            };

            return View(viewModel);
        }

        public ActionResult PatientCriticalDiagnosis()
        {
            var viewModel = new QpsPatientCriticalDiagnosis()
            {
                //StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
               
            };

            return View(viewModel);
        }


        public ActionResult BedOccupancyFloorWise()
        {
            var viewModel = new QpsBedOccupancyFloorWise()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
               
            };

            return View(viewModel);
        }

        public ActionResult MedicalTowerCases()
        {
            var viewModel = new QpsMedicalTowerCases()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Doctors = employeeDB.getAllDoctors().OrderBy(i => i.FullName).ToList(),
            };

            return View(viewModel);
        }

        public ActionResult DiagnosisReportICD()
        {
            var viewModel = new QpsDiagnosisReportICD() {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Department = departmentDB.getAllDepartment(),
                //Company = companyDB.getAllCompany(),
                Category = categoryDB.getCategories(),
            
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DiagnosisReportICD(QpsDiagnosisReportICD qpsDiagnosis)
        {
            var vm = qpsDiagnosis;
            if(Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                string patientType = "";
                DataTable reportData = new DataTable();

                    reportData = qpsreportDb.getPatientDiagnosisInOrOut(vm.InPatient.ToString(), vm.DepartmentId, vm.CategoryId, vm.CompanyId, vm.StartDate, vm.EndDate.AddDays(1));
                   

                    if (reportData.Rows.Count == 0)
                        return Content(Errors.ReportContent("NO RECORDS FOUND"));

                    reportViewer.ProcessingMode = ProcessingMode.Local;
                  
                    if (vm.InPatient.ToString() == "0") //In-Patient
                    {
                        ReportDataSource datasourceItem = new ReportDataSource("InPatient", reportData);
                        reportViewer.LocalReport.DataSources.Add(datasourceItem);
                        patientType = "IN";
                        reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientDiagnosisICD_In.rdl";
                    }
                    else //Out-Patient 
                    {
                        ReportDataSource datasourceItem = new ReportDataSource("OutPatient", reportData);
                        reportViewer.LocalReport.DataSources.Add(datasourceItem);
                        patientType = "OUT";
                        reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientDiagnosisICD_Out.rdl";
                    }

                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                    reportViewer.LocalReport.SetParameters(new ReportParameter("patientType", patientType));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                    reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
                 
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




        [HttpPost]
        public ActionResult MedicalTowerCases(QpsMedicalTowerCases qpsMedTower)
        {
            var vm = qpsMedTower;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = qpsreportDb.getMedicalTowerCases(vm.StartDate, vm.EndDate.AddDays(1),vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsMedicalTowerCases.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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


        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctors(searchString), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchCompanies(string searchString)
        {
            return Json(companyDB.findCompanies(searchString), JsonRequestBehavior.AllowGet);
        }
        


        [HttpPost]
        public ActionResult BedOccupancyFloorWise(QpsBedOccupancyFloorWise qpsbedfloorwise)
        {
            var vm = qpsbedfloorwise;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = qpsreportDb.getBedOccupancyFloorWise(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsBedOccupancyFloorWise.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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

        [HttpPost]
        public ActionResult PatientCriticalDiagnosis(QpsPatientCriticalDiagnosis qpspatientcritical)
        {
            var vm = qpspatientcritical;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                string headermsg = "";
                DataTable reportData = new DataTable();

                if (vm.stroption == "1")
                {
                    headermsg = vm.strName + " " + vm.EndDate.ToString("dd-MMM-yyyy");
                    
                }
                else if (vm.stroption == "2")
                {
                    headermsg = vm.strName + " " + vm.EndDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    headermsg = "Current In-Patient";
                    vm.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                }

                reportData = qpsreportDb.getPatientCriticalDiagnosis(vm.EndDate, vm.stroption.ToString());
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientCriticalDiagnosis.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                //reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.E.ToString("dd-MMM-yyyy")));
               
                reportViewer.LocalReport.SetParameters(new ReportParameter("strName", headermsg));
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


        

       

             [HttpPost]
        public ActionResult BedOccupancy(QpsBedOccupancy bedoccupancy)
        {
            var vm = bedoccupancy;
            if (Request.IsAjaxRequest())
            {
                 //Start ProcessBedTransferNICU() skip

                //End ProcessBedTransferNICU()


                //ReportViewerVm reportVM = new ReportViewerVm();
                //ReportViewer reportViewer = new ReportViewer();
                //string reportDocPath = "";
                //DataTable reportData = new DataTable();


                //reportData = qpsreportDb.getXrayFromReferral(vm.StartDate, vm.EndDate.AddDays(1));
                //reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsXrayFromEr.rdl";

                //if (reportData.Rows.Count == 0)
                //    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                //reportViewer.ProcessingMode = ProcessingMode.Local;
                //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                //ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                //reportViewer.LocalReport.DataSources.Add(datasourceItem);
                //reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                //reportViewer.SizeToReportContent = true;
                //reportViewer.Height = Unit.Percentage(100);
                //reportViewer.Width = Unit.Percentage(100);
                //reportViewer.ShowPrintButton = true;
                //reportVM.ReportViewer = reportViewer;
                //System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                //System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);
                //return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();
        }


        [HttpPost]
        public ActionResult XrayReferral(QpsXrayReferral xrayreferral)
        {
            var vm = xrayreferral;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = qpsreportDb.getXrayFromReferral(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsXrayFromEr.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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


        [HttpPost]
        public ActionResult AvgLengthStayCriticalAreas(AvgLengthStayCriticalAreas averagelenthareas)
        {

            var vm = averagelenthareas;

            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = qpsreportDb.getAvgLengthCriticalArea(vm.StartDate, vm.EndDate.AddDays(1),vm.DepartmentId,vm.Area);
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsAvgLengthCriticalArea.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", vm.StartDate.ToString("dd-MMM-yyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", vm.EndDate.ToString("dd-MMM-yyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Dept", vm.DeptName.ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Area", vm.Area.ToString()));
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

        [HttpPost]
        public ActionResult MrdAdmissionDischargeStats(MrdAdmissionDischargeStats MrdAdminDischarStats)
        {

            var vm = MrdAdminDischarStats;

            if(Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = qpsreportDb.getMrdAdmissionDischargeStats(vm.StartDate, vm.EndDate.AddDays(1),vm.RepType);
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsMrdAdmissionDischargeStats.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("startdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enddate", vm.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("reptype", vm.RepType.ToString()));
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



        [HttpPost]
        public ActionResult ErConsultationStats(ErConsultationStats erConsultStats)
        {
            var vm = erConsultStats;
            if(Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                
                reportData = qpsreportDb.getErConsultationStatistics(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsErConsultationStats.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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

        [HttpPost]
        public ActionResult PatientListByAge(QpsPatientListByAge QpsPatientList)
        {
            var vm = QpsPatientList;

            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = qpsreportDb.getPatientListByAge(vm.AgeRangeID,vm.checkDate, vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientListByAge.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                //reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("rangename", vm.AgeRangeName.ToString()));
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

//            [MCRS].[QpsReport_GetPatientListByAge]
//(@stAge int, @enAge int, @xOption int, @regSTDate datetime, @regENDate datetime)

        }



        [HttpPost]
        public ActionResult PatientVisitedInER(QpsPatientVisitedER qpsadmit)
        {
            var vm = qpsadmit;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = qpsreportDb.getPatientVisitER(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientVisitER.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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
           

        [HttpPost]
        public ActionResult PatientAdmittedErOr(QpsPatientAdmittedInErOr qpsadmit)
        {
            var vm = qpsadmit;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = qpsreportDb.getPatientAdmittedErOR(vm.StartDate, vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientAdmittedinErOR.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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


        [HttpPost]
        public ActionResult PatientAdmittedICU(QpsPatientAdmiitedICU qpsadmitted)
        {
            var vm = qpsadmitted;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = qpsreportDb.getPatientAdmittedICU(vm.StartDate,vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientAdmittedinICU.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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

        [HttpPost]
        public ActionResult PatientOrDone(QpsPatientOrDone QpsPatientOrDone)
        {
            var vm = QpsPatientOrDone;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();
                reportData = qpsreportDb.getPatientOrDone(vm.StartDate,vm.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\QpsReports\Report_QpsPatientOrDone.rdl";
                //sama
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
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


    }
}
