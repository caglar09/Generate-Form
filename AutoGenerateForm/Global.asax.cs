using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace AutoGenerateForm
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        var id = (FormsIdentity)HttpContext.Current.User.Identity;
                        var ticket = (id.Ticket);
                        if (!string.IsNullOrEmpty(ticket.UserData))
                        {
                            string userData = ticket.UserData;
                            string[] roles = userData.Split(',');
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
                        }
                    }
                }
            }
        }
    }
}
