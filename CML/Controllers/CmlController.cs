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
    public class CmlController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult CmlIndex()
        {
            return View();
        }

        public ActionResult Requests_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Request> requests = db.Requests;
            DataSourceResult result = requests.ToDataSourceResult(request, req => new {
                RequestID = req.RequestID,
                Name = req.Name,
                CMLNumber = req.CMLNumber,
                RequestedBy = req.RequestedBy,
                ProjectNumber = req.ProjectNumber,
                DeviationNumber = req.DeviationNumber,
                EWRNumber = req.EWRNumber,
                DirectorApprovalRequired = req.DirectorApprovalRequired,
                RetentionDate = req.RetentionDate,
                Phone = req.Phone,
                TestObjectives = req.TestObjectives,
                CreatedOn = req.CreatedOn,
                DateRequired = req.DateRequired,
                CreatedBy = req.CreatedBy,
                UpdateOn = req.UpdateOn,
                UpdatedBy = req.UpdatedBy,
                TestsStarted = req.TestsStarted,
                TestsFinished = req.TestsFinished,
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Requests_Create([DataSourceRequest]DataSourceRequest req, Request request)
        {
            if (ModelState.IsValid)
            {
                var entity = new Request
                {
                    Name = request.Name,
                    CMLNumber = request.CMLNumber,
                    RequestedBy = request.RequestedBy,
                    ProjectNumber = request.ProjectNumber,
                    DeviationNumber = request.DeviationNumber,
                    EWRNumber = request.EWRNumber,
                    DirectorApprovalRequired = request.DirectorApprovalRequired,
                    RetentionDate = request.RetentionDate,
                    Phone = request.Phone,
                    TestObjectives = request.TestObjectives,
                    CreatedOn = request.CreatedOn,
                    DateRequired = request.DateRequired,
                    CreatedBy = request.CreatedBy,
                    UpdateOn = request.UpdateOn,
                    UpdatedBy = request.UpdatedBy,
                    TestsStarted = request.TestsStarted,
                    TestsFinished = request.TestsFinished,
                };

                db.Requests.Add(entity);
                db.SaveChanges();
                request.RequestID = entity.RequestID;
            }

            return Json(new[] { request }.ToDataSourceResult(req, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Requests_Update([DataSourceRequest]DataSourceRequest req, Request request)
        {
            if (ModelState.IsValid)
            {
                var entity = new Request
                {
                    RequestID = request.RequestID,
                    Name = request.Name,
                    CMLNumber = request.CMLNumber,
                    RequestedBy = request.RequestedBy,
                    ProjectNumber = request.ProjectNumber,
                    DeviationNumber = request.DeviationNumber,
                    EWRNumber = request.EWRNumber,
                    DirectorApprovalRequired = request.DirectorApprovalRequired,
                    RetentionDate = request.RetentionDate,
                    Phone = request.Phone,
                    TestObjectives = request.TestObjectives,
                    CreatedOn = request.CreatedOn,
                    DateRequired = request.DateRequired,
                    CreatedBy = request.CreatedBy,
                    UpdateOn = request.UpdateOn,
                    UpdatedBy = request.UpdatedBy,
                    TestsStarted = request.TestsStarted,
                    TestsFinished = request.TestsFinished,
                };

                db.Requests.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { request }.ToDataSourceResult(req, ModelState));
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
