using CML.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CML.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [CMLRoleAuthorize( Authorize.Roles.Admin )]
        public ActionResult Index()
        {
            return View("Index","_AdminLayout");
        }
    }
}