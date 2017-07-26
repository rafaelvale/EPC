using System.Web.Optimization;

namespace Rohr.EPC.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                "~/Content/Style/rohr-responsive.css",
                "~/Content/Style/rohr.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Content/Script/vendor/jquery-1.10.2.min.js",
                "~/Content/Script/vendor/bootstrap.min.js",
                "~/Content/Script/rohr-1.0.0.js",
                "~/Content/Script/vendor/jquery-scrolltofixed-min.js"
                ));
        }
    }
}