using CML.Authorize;
using CML.Models;
using CML.Utilities;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CML.Controllers
{
    public class SamplesController : Controller
    {
        private CMLEntities db = new CMLEntities();
        // GET: Samples
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager )]
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
        public ActionResult Create(SampleParam param)
        {
            SampleModel sm = new SampleModel();
            sm.TestID = param.testid;
            sm.TestAbbrev = param.abbrev;
            sm.IsNewSample = true;
            sm.Nbr = 0;
            sm.PosOtherNbr = 0;
            sm.PosStandardNbr = 0;
            sm.PosTMBNbr = 0;
            sm.NegOtherNbr = 0;
            sm.NegStandardNbr = 0;
            sm.NegTMBNbr = 0;

            sm.ID = 0;
            ViewData["sampletypes"] = new SelectList( db.CML_SampleType, "ID", "SampleType" );
           
            return View("Create",sm);
        }
        [HttpPost]
        public ActionResult Update_Sample( [DataSourceRequest]DataSourceRequest request, SampleModel sample )
        {  var message = string.Empty;
            if ( ModelState.IsValid )
            {
              
                Sample s = new Sample
                {
                    ID = sample.ID,
                    NegComment = sample.NegComment,
                    NegOtherID = sample.NegOtherID,
                    NegOtherNumber = sample.NegOtherNbr,
                    NegStandardIDs = sample.NegStandardIDs,
                    NegStandardNbr = sample.NegStandardNbr,
                    NegTMBIds = sample.NegTMBIds,
                    NegTMBNbr = sample.NegTMBNbr,
                    Number = sample.Nbr,
                    OxideType = sample.OxideType,
                    PosComment = sample.PosComment,
                    PosOtherID = sample.PosOtherID,
                    PosOtherNumber = sample.PosOtherNbr,
                    PosStandardIDs = sample.PosStandardIDs,
                    PosStandardNbr = sample.PosStandardNbr,
                    PosTMBIds = sample.PosTMBIds,
                    PosTMBNbr = sample.PosTMBNbr,
                    SampleDesc = sample.SampleDesc,
                    SampleIDs = sample.SampleIDs,
                    SampleName = sample.SampleName,
                    SampleTypeID = sample.SampleTypeID,
                    SeperatorType = sample.SeperatorType,
                    TestID = sample.TestID,
                    TotalSamples = sample.TotalSamplesNbr,
                    UpdatedBy = Session["DisplayName"].ToString(),
                    UpdatedOn = DateTime.Now

                };









                db.Samples.Attach( s );
                db.Entry( s ).State = System.Data.Entity.EntityState.Modified;
                message = string.Format( "Sample id: #{0} has been updated.", sample.ID );




                db.SaveChanges();

                sample.ID = s.ID;

                CalculateSamples cs = new CalculateSamples( sample.ID );
               // sample.IsNewSample = false;
                //ViewData["sampletypes"] = new SelectList( db.CML_SampleType, "ID", "SampleType" );
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest, "Error occured in posting sample." );
            }

          
                
                return RedirectToAction( "GetTestFromSampleView", "Test", new { id = sample.TestID, msg = message } ); //View("Create",sample);
            
        }
        [HttpPost]
        public ActionResult Create_Sample(SampleModel sample )
        {
            var message = string.Empty;
            if ( ModelState.IsValid )
            {
                Sample s = new Sample
                {
                    NegComment = sample.NegComment,
                    NegOtherID = sample.NegOtherID,
                    NegOtherNumber = sample.NegOtherNbr,
                    NegStandardIDs = sample.NegStandardIDs,
                    NegStandardNbr = sample.NegStandardNbr,
                    NegTMBIds = sample.NegTMBIds,
                    NegTMBNbr = sample.NegTMBNbr,
                    Number = sample.Nbr,
                    OxideType = sample.OxideType,
                    PosComment = sample.PosComment,
                    PosOtherID = sample.PosOtherID,
                    PosOtherNumber = sample.PosOtherNbr,
                    PosStandardIDs = sample.PosStandardIDs,
                    PosStandardNbr = sample.PosStandardNbr,
                    PosTMBIds = sample.PosTMBIds,
                    PosTMBNbr = sample.PosTMBNbr,
                    SampleDesc = sample.SampleDesc,
                    SampleIDs = sample.SampleIDs,
                    SampleName = sample.SampleName,
                    SampleTypeID = sample.SampleTypeID,
                    SeperatorType = sample.SeperatorType,
                    TestID = sample.TestID,
                    TotalSamples = sample.TotalSamplesNbr,
                    
                    

                };
                var stype = db.CML_SampleType.Find( sample.SampleTypeID ).SampleType;
                  s.CreatedBy = Session["DisplayName"].ToString();
                    s.CreatedOn = DateTime.Now;
                    db.Samples.Add( s );
                   
               
               
                db.SaveChanges();
                sample.ID = s.ID;
                CalculateSamples cs = new CalculateSamples( sample.ID );
                message = string.Format( "Sample Type {0} has been created under ID: {1}", stype, sample.ID );
                sample.IsNewSample = false;
                ViewData["sampletypes"] = new SelectList( db.CML_SampleType, "ID", "SampleType" );
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest, "Error occured in posting sample." );
            }
           
            return RedirectToAction( "GetTestFromSampleView", "Test", new { id = sample.TestID, msg = message } ); //View("Create",sample);
            
        }

        public ActionResult GetSample( [DataSourceRequest]DataSourceRequest request, int id)
        {
            var sample = db.Samples.Find( id );
            var smpl = new SampleModel
            {
                ID = sample.ID,
                NegComment = sample.NegComment,
                NegOtherID = sample.NegOtherID,
                NegOtherNbr = sample.NegOtherNumber,
                NegStandardIDs = sample.NegStandardIDs,
                NegStandardNbr = sample.NegStandardNbr,
                NegTMBIds = sample.NegTMBIds,
                NegTMBNbr = sample.NegTMBNbr,
                Nbr = sample.Number,
                OxideType = sample.OxideType,
                PosComment = sample.PosComment,
                PosOtherID = sample.PosOtherID,
                PosOtherNbr = sample.PosOtherNumber,
                PosStandardIDs = sample.PosStandardIDs,
                PosStandardNbr = sample.PosStandardNbr,
                PosTMBIds = sample.PosTMBIds,
                PosTMBNbr = sample.PosTMBNbr,
                SampleDesc = sample.SampleDesc,
                SampleIDs = sample.SampleIDs,
                SampleName = sample.SampleName,
                SampleTypeID = sample.SampleTypeID,
                SeperatorType = sample.SeperatorType,
                TestID = sample.TestID,
                TotalSamplesNbr = sample.TotalSamples
                



            };
            smpl.IsNewSample = false;
            ViewData["sampletypes"] = new SelectList( db.CML_SampleType, "ID", "SampleType" );
            return View( "Update", smpl );
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
