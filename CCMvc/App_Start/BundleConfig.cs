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
                            "~/Scripts/jquery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.11.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/mvcfoolproof.unobtrusive.min.js",
                        "~/Scripts/MvcFoolproofJQueryValidation.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/DataTables/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/utils").Include(
                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/typeahead.bundle.min.js",
                        "~/Scripts/toastr.min.js"));

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

            #region Page specific Javascript bundles
            
            bundles.Add(new ScriptBundle("~/bundles/Organization").Include(
                "~/ScriptsCC/Organization.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/OrganizationDetail").Include(
                "~/ScriptsCC/OrganizationDetail.js",
                "~/ScriptsCC/OrganizationDetailUser.js",
                "~/ScriptsCC/OrganizationDetailRace.js"));

            bundles.Add(new ScriptBundle("~/bundles/RaceDetail").Include(
                "~/ScriptsCC/RaceDetail.js",
                "~/ScriptsCC/RaceDetailRunner.js"));

            #endregion
        }
    }
}