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
    public class TestDefinitionController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }
        private static IEnumerable<TestDefinitionModel> GetTestDefinitions()
        {
            var dbs = new CMLEntities();
            var defs = dbs.CML_TestDefinition.Select(d => new TestDefinitionModel {

                TestDefinitionID = d.TestDefinitionID,
                Name = d.Name,
                Abbrev = d.Abbrev,
                Determines = d.Determines,
                SampleType = d.SampleType,
                RequiredSampleSize = d.RequiredSampleSize
            });

            return defs;
        }
        public ActionResult CML_TestDefinition_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetTestDefinitions().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_TestDefinition_Create([DataSourceRequest]DataSourceRequest request, TestDefinitionModel cML_TestDefinition)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_TestDefinition
                {
                    Name = cML_TestDefinition.Name,
                    Abbrev = cML_TestDefinition.Abbrev,
                    Determines = cML_TestDefinition.Determines,
                    SampleType = cML_TestDefinition.SampleType,
                    RequiredSampleSize = cML_TestDefinition.RequiredSampleSize
                };

                db.CML_TestDefinition.Add(entity);
                db.SaveChanges();
                cML_TestDefinition.TestDefinitionID = entity.TestDefinitionID;
            }

            return Json(new[] { cML_TestDefinition }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_TestDefinition_Update([DataSourceRequest]DataSourceRequest request, TestDefinitionModel cML_TestDefinition)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_TestDefinition
                {
                    TestDefinitionID = cML_TestDefinition.TestDefinitionID,
                    Name = cML_TestDefinition.Name,
                    Abbrev = cML_TestDefinition.Abbrev,
                    Determines = cML_TestDefinition.Determines,
                    SampleType = cML_TestDefinition.SampleType,
                    RequiredSampleSize = cML_TestDefinition.RequiredSampleSize
                };

                db.CML_TestDefinition.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { cML_TestDefinition }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CML_TestDefinition_Destroy([DataSourceRequest]DataSourceRequest request, TestDefinitionModel cML_TestDefinition)
        {
            if (ModelState.IsValid)
            {
                var entity = new CML_TestDefinition
                {
                    TestDefinitionID = cML_TestDefinition.TestDefinitionID,
                    Name = cML_TestDefinition.Name,
                    Abbrev = cML_TestDefinition.Abbrev,
                    Determines = cML_TestDefinition.Determines,
                    SampleType = cML_TestDefinition.SampleType,
                    RequiredSampleSize = cML_TestDefinition.RequiredSampleSize
                };

                db.CML_TestDefinition.Attach(entity);
                db.CML_TestDefinition.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { cML_TestDefinition }.ToDataSourceResult(request, ModelState));
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
