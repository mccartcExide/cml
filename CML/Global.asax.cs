using CML.Models;
using CML.Utilities;
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

            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler( DoWork );
            worker.WorkerReportsProgress = false;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( WorkerCompleted );



            CacheManager.Instance.Add( "BackgroundWorker", worker );
            worker.RunWorkerAsync(); //we can also pass parameters to the async method...


        }
        private void WorkerCompleted( object sender, System.ComponentModel.RunWorkerCompletedEventArgs e )
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            if ( worker != null )
            {

                System.Threading.Thread.Sleep( TimeSpan.FromMinutes( 1 ) );
                worker.RunWorkerAsync();
            }
        }

        private void DoWork( object sender, System.ComponentModel.DoWorkEventArgs e )
        {

            BackgroundEmailer backgroundEmail = new BackgroundEmailer();
            backgroundEmail.ProcessMail();

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
