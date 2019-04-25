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

namespace CML.Controllers
{
    public class PriorityController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }
        private static IEnumerable<PriorityModel> GetPriorities()
        {
            var dbs = new CMLEntities();
            var pri = dbs.CML_Priority.Select(p => new PriorityModel
            {
                ID = p.ID,
                Priority = p.Priority
            });

            return pri;
        }
        public ActionResult CML_Priority_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetPriorities().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Priority_Create([DataSourceRequest]DataSourceRequest request, PriorityModel cML_Priority)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Priority
                {
                    Priority = cML_Priority.Priority,
                };

                db.CML_Priority.Add(entity);
                db.SaveChanges();
                cML_Priority.ID = entity.ID;
            }

            return Json(new[] { cML_Priority }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Priority_Update([DataSourceRequest]DataSourceRequest request, PriorityModel cML_Priority)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Priority
                {
                    ID = cML_Priority.ID,
                    Priority = cML_Priority.Priority,
                };

                db.CML_Priority.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_Priority }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Priority_Destroy([DataSourceRequest]DataSourceRequest request, PriorityModel cML_Priority)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Priority
                {
                    ID = cML_Priority.ID,
                    Priority = cML_Priority.Priority,
                };

                db.CML_Priority.Attach(entity);
                db.CML_Priority.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { cML_Priority }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
