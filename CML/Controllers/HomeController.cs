using CML.Authorize;
using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CML.Utilities;

namespace CML.Controllers
{
    public class HomeController : Controller
    {
        private CMLEntities db = new CMLEntities();
        // GET: Home
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult Index()
        {
            HomeModel hm = new HomeModel();
            CML_User us = db.GetUserFromUsername( User.Identity.Name );
            hm.MyOpenCount = db.GetOpenRequests( us.ID, User.Identity.Name );
            hm.RequestAssigned = db.GetRequestsAssignedToMe( us.ID );
            hm.TestsNotStarted = db.GetTestNotStartedAssigned( us.ID );
            hm.TestsStarted = db.GetTestStartedAssigned( us.ID );
            hm.AwaitingApproval = db.GetApprovalsCount( us.ID );
            hm.RequestsForApproval = db.GetOpenRequestsForApproval( us.ID, User.Identity.Name );
            //  var c = db.get

            return View("Index", hm);
        }

        public ActionResult Admin()
        {
            return View();
        }



        protected override void Dispose( bool disposing )
        {
            db.Dispose();
            base.Dispose( disposing );
        }
    }
}