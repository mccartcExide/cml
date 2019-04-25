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
    public class RequestTypeController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }

        public static IEnumerable<RequestTypeModel> GetRequestTypes()
        {
            var dbs = new CMLEntities();
            var req = dbs.CML_RequestType.Select(r => new RequestTypeModel
            {
                ID = r.ID,
                RequestType = r.RequestType
            });

            return req;
        }

        public ActionResult CML_RequestType_Read([DataSourceRequest]DataSourceRequest request)
        {
           return Json(GetRequestTypes().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_RequestType_Create([DataSourceRequest]DataSourceRequest request, RequestTypeModel cML_RequestType)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_RequestType
                {
                    RequestType = cML_RequestType.RequestType,
                };

                db.CML_RequestType.Add(entity);
                db.SaveChanges();
                cML_RequestType.ID = entity.ID;
            }

            return Json(new[] { cML_RequestType }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_RequestType_Update([DataSourceRequest]DataSourceRequest request, RequestTypeModel cML_RequestType)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_RequestType
                {
                    ID = cML_RequestType.ID,
                    RequestType = cML_RequestType.RequestType,
                };

                db.CML_RequestType.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_RequestType }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_RequestType_Destroy([DataSourceRequest]DataSourceRequest request, RequestTypeModel cML_RequestType)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_RequestType
                {
                    ID = cML_RequestType.ID,
                    RequestType = cML_RequestType.RequestType,
                };

                db.CML_RequestType.Attach(entity);
                db.CML_RequestType.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { cML_RequestType }.ToDataSourceResult(request, ModelState));
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
