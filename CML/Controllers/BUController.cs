﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CML.Models;
using CML.Authorize;

namespace CML.Controllers
{
    public class BUController : Controller
    {
        private CMLEntities db = new CMLEntities();

        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager )]
        public ActionResult Index()
        {
            IList<CML_User> approv = db.CML_User.ToList();
            ViewData["approvers"] = approv;
            return View();
        }

        public ActionResult CML_BusinessUnit_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<CML_BusinessUnit> cml_businessunit = db.CML_BusinessUnit;
            DataSourceResult result = cml_businessunit.ToDataSourceResult(request, cML_BusinessUnit => new {
                ID = cML_BusinessUnit.ID,
                BusinessUnit = cML_BusinessUnit.BusinessUnit,
                ApproverID = cML_BusinessUnit.ApproverID
                
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_BusinessUnit_Create([DataSourceRequest]DataSourceRequest request, CML_BusinessUnit cML_BusinessUnit)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_BusinessUnit
                {
                    BusinessUnit = cML_BusinessUnit.BusinessUnit,
                    ApproverID = cML_BusinessUnit.ApproverID
                };

                db.CML_BusinessUnit.Add(entity);
                db.SaveChanges();
                cML_BusinessUnit.ID = entity.ID;
            }

            return Json(new[] { cML_BusinessUnit }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_BusinessUnit_Update([DataSourceRequest]DataSourceRequest request, CML_BusinessUnit cML_BusinessUnit)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_BusinessUnit
                {
                    ID = cML_BusinessUnit.ID,
                    BusinessUnit = cML_BusinessUnit.BusinessUnit,
                    ApproverID = cML_BusinessUnit.ApproverID
                };

                db.CML_BusinessUnit.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_BusinessUnit }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
