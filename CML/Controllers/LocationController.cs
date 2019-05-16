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
    public class LocationController : Controller
    {
        private CMLEntities db = new CMLEntities();

        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager )]
        public ActionResult Index()
        {
            return View("Index");
        }
        private static IEnumerable<LocationModel> GetLocations()
        {
            var dbs = new CMLEntities();
            var locs = dbs.CML_Location.Select( l => new LocationModel
            {
                Code = l.Code,
                LocationID = l.LocationID,
                Location = l.Location
            });


            return locs;
        }
        public ActionResult CML_Location_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetLocations().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Location_Create([DataSourceRequest]DataSourceRequest request, LocationModel cML_Location)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Location
                {
                    Code = cML_Location.Code,
                    Location = cML_Location.Location,
                };

                db.CML_Location.Add(entity);
                db.SaveChanges();
                cML_Location.LocationID = entity.LocationID;
            }

            return Json(new[] { cML_Location }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Location_Update([DataSourceRequest]DataSourceRequest request, LocationModel cML_Location)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Location
                {
                    Code = cML_Location.Code,
                    LocationID = cML_Location.LocationID,
                    Location = cML_Location.Location,
                };

                db.CML_Location.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_Location }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_Location_Destroy([DataSourceRequest]DataSourceRequest request, LocationModel cML_Location)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_Location
                {
                    Code = cML_Location.Code,
                    LocationID = cML_Location.LocationID,
                    Location = cML_Location.Location,
                };

                db.CML_Location.Attach(entity);
                db.CML_Location.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { cML_Location }.ToDataSourceResult(request, ModelState));
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
