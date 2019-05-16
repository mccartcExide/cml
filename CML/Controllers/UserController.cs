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
    public class UserController : Controller
    {
        private CMLEntities db = new CMLEntities();

        //private void BuildForeignList()
        //{
        //    CMLEntities db = new CMLEntities();
        //    ViewData["roletypes"] = new SelectList(db.CML_RoleType.ToList(), "ID", "RoleType");
        //}
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult Index()
        {
            IList<CML_RoleType> roles = db.CML_RoleType.ToList();
            ViewData["roletypes"] = roles;
            return View();
        }

        public ActionResult CML_User_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<CML_User> cml_user = db.CML_User;
            DataSourceResult result = cml_user.ToDataSourceResult(request, cML_User => new UserModel {
                ID = cML_User.ID,
                UserID = cML_User.UserID,
                Email = cML_User.Email,
                DisplayName = cML_User.DisplayName,
                
            });
            //BuildForeignList();
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_User_Create([DataSourceRequest]DataSourceRequest request, UserModel cML_User)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_User
                {
                    UserID = cML_User.UserID,
                    Email = cML_User.Email,
                    DisplayName = cML_User.DisplayName,
                   
                };

                db.CML_User.Add(entity);
                db.SaveChanges();
                cML_User.ID = entity.ID;
            }
            //BuildForeignList();
            return Json(new[] { cML_User }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_User_Update([DataSourceRequest]DataSourceRequest request, UserModel cML_User)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_User
                {
                    ID = cML_User.ID,
                    UserID = cML_User.UserID,
                    Email = cML_User.Email,
                    DisplayName = cML_User.DisplayName,
                   
                };

                db.CML_User.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
            //BuildForeignList();
            return Json(new[] { cML_User }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_User_Destroy([DataSourceRequest]DataSourceRequest request, UserModel cML_User)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_User
                {
                    ID = cML_User.ID,
                    UserID = cML_User.UserID,
                    Email = cML_User.Email,
                    DisplayName = cML_User.DisplayName,
                  
                };

                db.CML_User.Attach(entity);
                db.CML_User.Remove(entity);
                db.SaveChanges();
            }
            //BuildForeignList();
            return Json(new[] { cML_User }.ToDataSourceResult(request, ModelState));
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
