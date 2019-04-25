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
    public class BusinessUnitController : Controller
    {
        private CMLEntities db = new CMLEntities();

        private static IEnumerable<BusinessUnitModel> GetBusinessUnits()
        {
            var dbs = new CMLEntities();
            var bus = dbs.CML_BusinessUnit.Select(b => new BusinessUnitModel
            {
                ID = b.ID,
                BusinessUnit = b.BusinessUnit

            });
            return bus;
        }

        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }

        public ActionResult CML_BusinessUnit_Read([DataSourceRequest]DataSourceRequest request)
        {
           return Json(GetBusinessUnits().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_BusinessUnit_Create([DataSourceRequest]DataSourceRequest request, BusinessUnitModel cML_BusinessUnit)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_BusinessUnit
                {
                    BusinessUnit = cML_BusinessUnit.BusinessUnit,
                };

                db.CML_BusinessUnit.Add(entity);
                db.SaveChanges();
                cML_BusinessUnit.ID = entity.ID;
            }

            return Json(new[] { cML_BusinessUnit }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_BusinessUnit_Update([DataSourceRequest]DataSourceRequest request, BusinessUnitModel cML_BusinessUnit)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_BusinessUnit
                {
                    ID = cML_BusinessUnit.ID,
                    BusinessUnit = cML_BusinessUnit.BusinessUnit,
                };

                db.CML_BusinessUnit.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_BusinessUnit }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_BusinessUnit_Destroy([DataSourceRequest]DataSourceRequest request, BusinessUnitModel cML_BusinessUnit)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_BusinessUnit
                {
                    ID = cML_BusinessUnit.ID,
                    BusinessUnit = cML_BusinessUnit.BusinessUnit,
                };

                db.CML_BusinessUnit.Attach(entity);
                db.CML_BusinessUnit.Remove(entity);
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
