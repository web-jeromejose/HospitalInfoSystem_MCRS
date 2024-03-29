using System.Web.Optimization;

//[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(HIS_MCRS.App_Start.DatePickerHelperBundleConfig), "RegisterBundles")]

namespace HIS_MCRS.App_Start
{
	public class DatePickerHelperBundleConfig
	{
		public static void RegisterBundles()
		{
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
            "~/Scripts/bootstrap-datepicker.js",
            "~/Scripts/locales/bootstrap-datepicker.*"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/datepicker").Include(
            "~/Content/bootstrap-datepicker.css"));
		}
	}
}