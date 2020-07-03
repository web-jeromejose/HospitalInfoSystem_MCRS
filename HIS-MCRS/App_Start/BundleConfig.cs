 using System.Web;
using System.Web.Optimization;

namespace HIS_MCRS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /**Standard CSS Plugins by the group**/
            bundles.Add(new StyleBundle("~/Styles/plugins/global").Include(
                "~/Content/plugins/bootstrap/css/bootstrap.css",
                "~/Content/plugins/datatables/css/dataTables.bootstrap.css"
                ));

            ///Standard JS Plugins by the group
            bundles.Add(new ScriptBundle("~/Scripts/plugins/global").Include(
                "~/Content/plugins/moment/moment.js",
                "~/Content/plugins/bootstrap/js/bootstrap.js",
                "~/Content/plugins/datatables/js/jquery.dataTables.js",
                "~/Content/plugins/datatables/js/jquery.bootstrap.dataTables.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/knockoutjs").Include(
                       "~/Scripts/knockout-3.3.0.js",
                       "~/Scripts/knockout-bindinghandler.js",
                       "~/Scripts/knockout-select2.js"
                       ));

            ///Standard CSS by the group
            bundles.Add(new StyleBundle("~/Styles/global").Include(
                "~/Content/styles/customererror.css",
                "~/Content/styles/globalstyle.css",
                "~/Content/styles/loading.css",
                "~/Content/styles/login.css",
                "~/Content/styles/mainstyles.css",
                "~/Content/styles/menu.css"
                ));

            ///Standard scripts by the group
            bundles.Add(new ScriptBundle("~/Scripts/global").Include(
                "~/Scripts/ajaxWrapper.js",
                "~/Scripts/jqSecurity.js"
                ));

            /**Customize CSS-Style-Plugin per programmer-global**/
            bundles.Add(new StyleBundle("~/Styles/custom").Include(
                //"~/Content/styles/Site.css",
                "~/Content/plugins/select2/select2.css",

                "~/Content/plugins/select2/select2-bootstrap.css",
                //"~/Content/plugins/sweetalert/sweet-alert.css",
                "~/Content/plugins/toastr/content/toastr.min.css",
                 "~/Content/plugins/datepicker/css/bootstrap-datepicker3.css"
                 /* icheck css */
                 //"~/Content/plugins/icheck/skins/flat/blue.css"
                 //"~/Content/plugins/datatables/fixedcolumns/css/fixedColumns.bootstrap.css"
                   // , "~/Content/plugins/datetimepicker/css/datetimepicker.css"

                 , "~/Content/plugins/datetimepicker/css/bootstrap-datetimepicker.css"

                 //, "~/Content/plugins/datetimepicker/css/bootstrap-datetimepicker.min.css"
                 
                ));

            /**Customize JS-Scripts-Plugin per programmer-global**/
            bundles.Add(new StyleBundle("~/Scripts/custom").Include(
                "~/Content/plugins/blockui/jquery.blockUI.js"
                ,"~/Content/plugins/bootbox/bootbox.js"
                //"~/Content/plugins/popup/popModal.js",
                //moment
                //, "~/Content/plugins/lodash/lodash.js"
               

                , "~/Content/plugins/datatables/editables/jquery.jeditable.js"
                , "~/Content/plugins/datatables/editables/jquery.dataTables.editable.js"
           
                , "~/Content/plugins/select2/select2.js"
                ,"~/Content/plugins/select2/underscore.js"
               
                
                ,"~/Content/plugins/toastr/scripts/toastr.min.js"
                //"~/Content/plugins/sweetalert/sweet-alert.min.js",                
                //"~/Scripts/IPBill/jqGlobal.js"

                
                
                /* icheck */
                 //"~/Content/plugins/icheck/icheck.js"
                /*data tables column reorder*/
                //"~/Content/plugins/datatables/js/columnresize.js"
                , "~/Scripts/MCRS/global/dialogwrapper.js"
                , "~/Scripts/MCRS/global/ajaxwrapper.js"

                ,"~/Scripts/MCRS/global/commonwrapper.js"

                , "~/Content/plugins/inputmask/jquery.inputmask.bundle.js"

                , "~/Content/plugins/datepicker/js/bootstrap-datepicker.js"
                , "~/Content/plugins/datetimepicker/js/bootstrap-datetimepicker.min.js"
                
            ));


            /**Customize JS-Function-Plugin per programmer**/

            bundles.Add(new ScriptBundle("~/Scripts/commonwrapper").Include(
            "~/Scripts/MCRS/global/dialogwrapper.js",
            "~/Scripts/MCRS/global/ajaxwrapper.js",
            "~/Scripts/MCRS/global/commonwrapper.js",
            "~/Scripts/MCRS/global/commonfunctions.js",
            "~/Scripts/MCRS/global/globalhandler.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/changestation").Include(
            "~/Scripts/MCRS/global/changestation.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/globalhandler").Include(
            "~/Scripts/MCRS/global/globalhandler.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsARIPBillCorrection").Include(
            "~/Scripts/MCRS/ipbillcorrection/js_ipbillcorrection.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsARCompanyCombinedStatement").Include(
            "~/Scripts/MCRS/arcombinedstatement/js_arcompanycombinedstatement.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsARCompanyPINWiseInvoice").Include(
            "~/Scripts/MCRS/arcompanypinwiseinvoice/js_arcompanypinwiseinvoice.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsOPInvoiceUBF").Include(
            "~/Scripts/MCRS/opinvoiceubf/js_opinvoiceubf.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsOPInvoiceUBFSummary").Include(
            "~/Scripts/MCRS/opinvoiceubf/js_opinvoiceubf_summary.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsArabicPatientName").Include(
            "~/Scripts/MCRS/arabicpatientname/js_arabicpatientname.js"
            ));
            bundles.Add(new ScriptBundle("~/Scripts/jsOPTariff").Include(
            "~/Scripts/MCRS/optariff/js_optariff.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsExpiredPolicies").Include(
            "~/Scripts/MCRS/expiredpolicies/js_expiredpolicies.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsPINGrossCreated").Include(
            "~/Scripts/MCRS/pingrosscreated/js_pingrosscreated.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsOPDTransactionCharges").Include(
            "~/Scripts/MCRS/opdtransactioncharges/js_opdtransactioncharges.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsPatientCharityLetterReport").Include(
            "~/Scripts/MCRS/patientcharityletterreport/js_patientcharityletterreport.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsBillingOfficerCompMapping").Include(
            "~/Scripts/MCRS/billingofficercompanymapping/js_billingofficercompanymapping.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsCoveringLetterGeneration").Include(
            "~/Scripts/MCRS/coveringletter/js_coveringlettergeneration.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsCoveringLetterPrinting").Include(
            "~/Scripts/MCRS/coveringletter/js_coveringletterprinting.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsARReasonforCancellation").Include(
            "~/Scripts/MCRS/reasonforcancellation/js_reasonforcancellation.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsARBillDeletion").Include(
            "~/Scripts/MCRS/arbilldeletion/js_arbilldeletion.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsGeneralInvoicePrinting").Include(
            "~/Scripts/MCRS/generalinvoiceprinting/js_generalinvoiceprinting.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsDashboardMonitoring").Include(
            "~/Scripts/MCRS/patientstatistics/js_dashboardmonitoring.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jsActAdjBillReports").Include(
            "~/Scripts/MCRS/arreports/js_actadjreports.js"
            ));
            

            //bundles.Add(new ScriptBundle("~/Scripts/DTPICKER").Include(
            //    "~/Content/plugins/datatimepicker/js/bootstrap-datetimepicker.js"
            //));

            /**Customize CSS-Function-Plugin per programmer**/
            //bundles.Add(new ScriptBundle("~/Styles/jqcanceldisch").Include(
            //    "~/Scripts/IPBill/jqcanceldisch.js",
            //     "~/Content/plugins/datepicker/js/moment.js",
            //    "~/Content/plugins/datepicker/js/bootstrap-datetimepicker.min.js"
            //    ));

            bundles.Add(new StyleBundle("~/Styles/arcustom").Include(
                "~/Content/styles/arcustomstyles.css"
                ));


        }
    }
}