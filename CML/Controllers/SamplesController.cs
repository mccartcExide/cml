using CML.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CML.Controllers
{
    public class SamplesController : Controller
    {
        private CMLEntities db = new CMLEntities();
        // GET: Samples
        public ActionResult Index()
        {
            return View();
        }

        // GET: Samples/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Samples/Create
        public ActionResult Create(int id)
        {
            SampleModel sm = new SampleModel();
            sm.TestID = id;
            sm.IsNewSample = true;

            ViewData["sampletypes"] = new SelectList( db.CML_SampleType, "ID", "SampleType" );
            return View(sm);
        }

        public ActionResult GetSample( [DataSourceRequest]DataSourceRequest request, int id, int test)
        {
            var s = db.Samples.Find( id );
            var smpl = new SampleModel
            {
                ID = s.ID,

            };
            return View( "Edit", smpl );
        }

        // POST: Samples/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Samples/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Samples/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Samples/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Samples/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
