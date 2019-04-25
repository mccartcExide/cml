using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CML
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

        protected void Session_Start(object sender, EventArgs e)
        {
            var temp = User.Identity.Name;

            temp = Utilities.RoleExtensions.ExtractUsername(User.Identity.Name);
            using (CMLEntities db = new CMLEntities())
            {
                var dspname = db.CML_User.Where(a => a.UserID.Equals(temp)).Select(a => a.DisplayName).FirstOrDefault();

                Session["DisplayName"] = dspname.ToString();
            }


        }
    }
}
