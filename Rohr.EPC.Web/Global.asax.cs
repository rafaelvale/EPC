using System;
using System.Web.Optimization;
using System.Web.Routing;

namespace Rohr.EPC.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            BundleTable.EnableOptimizations = true;

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}