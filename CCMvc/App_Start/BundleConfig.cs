using System.Web;
using System.Web.Optimization;

namespace CCMvc
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Shared Javascript Bundles
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/DataTables/jquery.dataTables.*",
                        "~/Scripts/DataTables/dataTables.*"));

            bundles.Add(new ScriptBundle("~/bundles/utils").Include(
                        "~/Scripts/moment.*",
                        "~/Scripts/bootstrap-datetimepicker.*",
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/typeahead.bundle.*",
                        "~/Scripts/toastr.*"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/ScriptsCC/Site.js"));

            bundles.Add(new ScriptBundle("~/bundles/sessionTimeout").Include(
                        "~/ScriptsCC/SessionTimeout.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*")); 
            #endregion

            #region CSS bundles

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                        "~/Content/bootstrap-paper.css"));

            bundles.Add(new StyleBundle("~/Content/utilsCSS").Include(
                        "~/Content/bootstrap-datetimepicker.css",
                        "~/Content/bootstrap-multiselect.css",
                        "~/Content/typeahead.css",
                        "~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Content/sitecss").Include(
                        "~/ContentCC/Site.css"));

            #endregion
        }
    }
}