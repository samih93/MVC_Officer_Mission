using System.Web;
using System.Web.Optimization;

namespace MVC_Officer_Mission
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/function.js"));

            //jquery selectpicker Scripts
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-selectpicker").Include(
                      "~/Content/Lib/BootstrapSelect/js/bootstrap-select.min.js"));
            //jquery selectpicker Styles
            bundles.Add(new StyleBundle("~/Content/bootstrap-selectpicker").Include(
                      "~/Content/Lib/BootstrapSelect/css/bootstrap-select.min.css"));

            //daypilot calendar Styles
            bundles.Add(new StyleBundle("~/Content/daypilotcalendar").Include(
                     //"~/Content/Lib/DailyPilotCalendar/css/media/elements.css?v=3121",
                     "~/Content/Lib/DailyPilotCalendar/css/themes/month_traditional.css",
                     "~/Content/Lib/DailyPilotCalendar/css/themes/navigator_green.css"));

            //daypilot calendar Scripts
            bundles.Add(new ScriptBundle("~/bundles/daypilotcalendar").Include(
                      "~/Content/Lib/DailyPilotCalendar/js/daypilot-all.min.js"));

            //fancybox Scripts
            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                      "~/Content/Lib/fancybox-master/dist/jquery.fancybox.min.js"));

            //Fancybox Styles
            bundles.Add(new StyleBundle("~/Content/fancybox").Include(
                      "~/Content/Lib/fancybox-master/dist/jquery.fancybox.min.css"));

            //datatables Scripts
            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                      "~/Content/Lib/DataTables/DataTables-1.10.18/js/jquery.dataTables.min.js",
                      "~/Content/Lib/DataTables/DataTables-1.10.18/js/dataTables.bootstrap.min.js"
                      ));

            //datatables Styles
            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                      "~/Content/Lib/DataTables/DataTables-1.10.18/css/dataTables.bootstrap.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css"));
        }
    }
}
