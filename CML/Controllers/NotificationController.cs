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
    public class NotificationController : Controller
    {
        private CMLEntities db = new CMLEntities();
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager )]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Notifications_Read([DataSourceRequest]DataSourceRequest request, int reqid)
        {
            IQueryable<Models.Notification> notifications = db.Notifications.Where(a=> a.RequestID == reqid);
            DataSourceResult result = notifications.ToDataSourceResult(request, notification => new {
                ID = notification.ID,
                MessageType = notification.MessageType,
                LoggedOn = notification.LoggedOn,
                SentOn = notification.SentOn,
                Sent = notification.Sent,
                Recipient = notification.Recipient,
                TestID = notification.TestID

            } );

            return Json(result);
        }
        public ActionResult Notifications_Read_Test( [DataSourceRequest]DataSourceRequest request,int testid )
        {
            IQueryable<Models.Notification> notifications = db.Notifications.Where(a=> a.TestID == testid);
            DataSourceResult result = notifications.ToDataSourceResult( request, notification => new {
                ID = notification.ID,
                MessageType = notification.MessageType,
                LoggedOn = notification.LoggedOn,
                SentOn = notification.SentOn,
                Sent = notification.Sent,
                Recipient = notification.Recipient,
                TestID = notification.TestID
            } );

            return Json( result );
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Notifications_Update([DataSourceRequest]DataSourceRequest request, Models.Notification notification )
        {
            if (ModelState.IsValid)
            {
                var entity = new Models.Notification
                {
                    ID = notification.ID,
                    MessageType = notification.MessageType,
                    LoggedOn = notification.LoggedOn,
                    SentOn = notification.SentOn,
                    Sent = notification.Sent,
                };

                db.Notifications.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { notification }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
