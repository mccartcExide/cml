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
    public class StatusController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }
        private static IEnumerable<StatusModel> GetStatuses()
        {
            var dbs = new CMLEntities();
            var st = dbs.CML_Status.Select(s => new StatusModel {
                ID = s.ID,
                Status = s.Status
            });

            return st;
        }

        public ActionResult CML_Status_Read([DataSourceRequest]DataSourceRequest request)
        {
           return Json(GetStatuses().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Status_Create([DataSourceRequest]DataSourceRequest request, StatusModel cML_Status)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Status
                {
                    Status = cML_Status.Status,
                };

                db.CML_Status.Add(entity);
                db.SaveChanges();
                cML_Status.ID = entity.ID;
            }

            return Json(new[] { cML_Status }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Status_Update([DataSourceRequest]DataSourceRequest request, StatusModel cML_Status)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Status
                {
                    ID = cML_Status.ID,
                    Status = cML_Status.Status,
                };

                db.CML_Status.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_Status }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Status_Destroy([DataSourceRequest]DataSourceRequest request, StatusModel cML_Status)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Status
                {
                    ID = cML_Status.ID,
                    Status = cML_Status.Status,
                };

                db.CML_Status.Attach(entity);
                db.CML_Status.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { cML_Status }.ToDataSourceResult(request, ModelState));
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
