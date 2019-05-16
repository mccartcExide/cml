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
    public class RoleTypeController : Controller
    {
        private CMLEntities db = new CMLEntities();
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager )]
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult CML_RoleType_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<CML_RoleType> cml_roletype = db.CML_RoleType;
            DataSourceResult result = cml_roletype.ToDataSourceResult(request, cML_RoleType => new RoleTypeModel {
                ID = cML_RoleType.ID,
                RoleType = cML_RoleType.RoleType,
               
            });
            
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_RoleType_Create([DataSourceRequest]DataSourceRequest request, RoleTypeModel cML_RoleType)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_RoleType
                {
                    RoleType = cML_RoleType.RoleType,
                   
                };

                db.CML_RoleType.Add(entity);
                db.SaveChanges();
                cML_RoleType.ID = entity.ID;
            }
            return Json(new[] { cML_RoleType }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_RoleType_Update([DataSourceRequest]DataSourceRequest request, RoleTypeModel cML_RoleType)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_RoleType
                {
                    ID = cML_RoleType.ID,
                    RoleType = cML_RoleType.RoleType,
                    
                };

                db.CML_RoleType.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new[] { cML_RoleType }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_RoleType_Destroy([DataSourceRequest]DataSourceRequest request, RoleTypeModel cML_RoleType)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_RoleType
                {
                    ID = cML_RoleType.ID,
                    RoleType = cML_RoleType.RoleType,
                    
                };

                db.CML_RoleType.Attach(entity);
                db.CML_RoleType.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { cML_RoleType }.ToDataSourceResult(request, ModelState));
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
