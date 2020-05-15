using System;
using System.Configuration;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ToolManage.Helper;

namespace ToolManage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var timer = new Timer(obj =>
                new Detect().SendMail()
                , null, 0, int.Parse(ConfigurationManager.AppSettings["SendEmailInterval"])* 86400000L);
        }
    }
}
