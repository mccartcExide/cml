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
    public class DispositionController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }
        private static IEnumerable<DispositionModel> GetDispositions()
        {
            var dbs = new CMLEntities();
            var dips = dbs.CML_Disposition.Select(d => new DispositionModel
            {
                ID = d.ID,
                Disposition = d.Disposition
            });

            return dips;
        }
        public ActionResult CML_Disposition_Read([DataSourceRequest]DataSourceRequest request)
        {
           return Json(GetDispositions().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Disposition_Create([DataSourceRequest]DataSourceRequest request, DispositionModel cML_Disposition)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Disposition
                {
                    Disposition = cML_Disposition.Disposition,
                };

                db.CML_Disposition.Add(entity);
                db.SaveChanges();
                cML_Disposition.ID = entity.ID;
            }

            return Json(new[] { cML_Disposition }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Disposition_Update([DataSourceRequest]DataSourceRequest request, DispositionModel cML_Disposition)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Disposition
                {
                    ID = cML_Disposition.ID,
                    Disposition = cML_Disposition.Disposition,
                };

                db.CML_Disposition.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_Disposition }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Disposition_Destroy([DataSourceRequest]DataSourceRequest request, DispositionModel cML_Disposition)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Disposition
                {
                    ID = cML_Disposition.ID,
                    Disposition = cML_Disposition.Disposition,
                };

                db.CML_Disposition.Attach(entity);
                db.CML_Disposition.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { cML_Disposition }.ToDataSourceResult(request, ModelState));
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
