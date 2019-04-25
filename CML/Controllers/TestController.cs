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
using System.IO;

namespace CML.Controllers
{
    public class Params 
    {
        public int testid { get; set; }
        public int requestid { get; set; }
    }
    public class TestController : Controller
    {
        private CMLEntities db = new CMLEntities();
       

        public ActionResult Index()
        {
            // RequestedItem ri = new RequestedItem();
            //  ri.Definitions = db.CML_TestDefinition.ToList();
            IList<CML_Status> stats = db.CML_Status.ToList();
            IList<CML_User> assig = db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Tester")).ToList();
            ViewData["statuses"] = stats;
            ViewData["testers"] = assig;
            return View();
        }
        [HttpPost]
        public ActionResult Update(TestModel test)
        {
            test.UpdatedBy = Session["DisplayName"].ToString();
            test.UpdatedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                var t = db.Tests.Find(test.TestID);

                t.AssignedTo = test.AssignedTo;
                t.Comments = test.Comments;
                t.UpdatedOn = test.UpdatedOn;
                t.UpdatedBy = test.UpdatedBy;
                db.Tests.Attach(t);
                db.Entry(t).State = EntityState.Modified;

                if ( !String.IsNullOrEmpty( test.Note ) )
                {
                    Note note = new Note();
                    note.CreatedBy = Session["DisplayName"].ToString();
                    note.CreatedOn = DateTime.Now;
                    note.ParentID = test.TestID;
                    note.NoteType = "T";
                    note.NoteText = test.Note;

                    db.Notes.Add( note );

                    test.Note = string.Empty;

                }



                db.SaveChanges();
            }

            if(test.SaveAndStay)
            {
                test.TestNotes = db.Notes.Where( a => a.ParentID == test.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
                test.Attachments = GetFiles( test.TestID );

                ViewData["assigneddl"] = new SelectList(db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Tester")), "ID", "DisplayName");
                TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-success", Title = "Success!", Message = string.Format("Update of Test Number {0} has completed.", test.TestID) };
                return View("Edit", test);
            }
            else
            {
                TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-success", Title = "Success!", Message = string.Format("Update of Test Number {0} has completed.", test.TestID) };
                return RedirectToAction("Edit", "Request", new { id = test.CMLRequest });

            }

            
            
        }
        [HttpPost]
        public ActionResult Create(RequestedItem ri)
        {
            Test test = new Test();
            var t = db.CML_TestDefinition.Find(ri.SelectedTestID);
            test.CMLRequest = ri.RequestID;
            test.TestDef = ri.SelectedTestID;

            test.Name = t.Name;
            test.Abbrev = t.Abbrev;
            test.Determines = t.Determines;
            test.SampleType = t.SampleType;
            test.SampleSize = t.RequiredSampleSize;
            test.CreatedBy = (string)Session["DisplayName"];// db.CML_User.Where(a => a.UserID.Equals(Utilities.RoleExtensions.ExtractUsername(User.Identity.Name))).Select(b => b.DisplayName).ToString();
            test.CreatedOn = DateTime.Now;

            db.Tests.Add(test);
            db.SaveChanges();
            var tm = new TestModel
            {
                Abbrev = t.Abbrev,
                // AssignedTo = test.AssignedTo,
                // Cleaning = test.Cleaning,
                CMLRequest = ri.RequestID,
                //CML_User = test.CML_User,
                //CML_TestDefinition = test.CML_TestDefinition,
                CML_Status = db.CML_Status.Where(a => a.ID == 1).First(),
                //Comments = test.Comments,
                CreatedBy = (string)Session["DisplayName"],
                CreatedOn = DateTime.Now,
                //DataAnalysis = test.DataAnalysis,
                Determines = t.Determines,
                Name = t.Name,
                //Reports = test.Reports,
                //Request = test.Request,
                //SampleAnalysis = test.SampleAnalysis,
                //SamplePreparations = test.SamplePreparations,
                //Samples = test.Samples,
                SampleSize = t.RequiredSampleSize,
                SampleType = t.SampleType,
                StatusID = 1,
                TestDef = ri.SelectedTestID,
                //TestFinished = test.TestFinished,
                TestID = test.TestID,
                //TestStarted = test.TestStarted,
                //Total = test.Total,
                //TotalSamples = test.TotalSamples,
                //UpdatedBy = test.UpdatedBy,
                //UpdatedOn = test.UpdatedOn
               

            };

            ViewData["assigneddl"] = new SelectList(db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Tester")), "ID", "DisplayName");
            return View("Edit",tm);
        }

        public ActionResult CreateStart([DataSourceRequest]DataSourceRequest request, int requestid)
        {
            RequestedItem ri = new RequestedItem();
            ri.RequestID = requestid;
            return View(ri);
        }

        public static IEnumerable<TestModel> GetTests()
        {
            var dbs = new CMLEntities();
            var ts = dbs.Tests.Select(test => new TestModel
            {
                TestID = test.TestID,
                CMLRequest = test.CMLRequest,
                AssignedTo = test.AssignedTo,
                TestStarted = test.TestStarted,
                StatusID = test.StatusID,
                Status = test.CML_Status.Status,
                TestFinished = test.TestFinished,
                Abbrev = test.Abbrev,
                Name = test.Name,
                SampleType = test.SampleType,
                SampleSize = test.SampleSize
            });

            return ts.ToList<TestModel>();
        }
        public ActionResult Tests_Read([DataSourceRequest]DataSourceRequest request)
        {
           
            return Json(GetTests().ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }
        private AttachmentsModel GetFiles( int reqid )
        {
            string dirPath = @"~\Upload\Tests\" + reqid;
            AttachmentsModel result = new AttachmentsModel();
            result.ParentID = reqid;
            result.ParentType = "T";

            List<Files> list = new List<Files>();
            result.Attachments = list;
            if ( Directory.Exists( Server.MapPath( dirPath ) ) )
            {

                DirectoryInfo dir = new DirectoryInfo( Server.MapPath( dirPath ) );
                var files = dir.GetFiles().ToList();

                foreach ( var f in files )
                {
                    FileInfo fi = new FileInfo( f.FullName );
                    Files fum = new Files
                    {
                        Name = fi.Name,
                        Extension = fi.Extension,
                        Size = Convert.ToInt32( fi.Length )


                    };
                    list.Add( fum );
                }

                result.Attachments = list;
            }
            return result;
        }

        public ActionResult GetTest([DataSourceRequest]DataSourceRequest request, int id, int req)
        {
            var t = db.Tests.Find(id);
            var tm = new TestModel
            {
                Abbrev = t.Abbrev,
                AssignedTo = t.AssignedTo,
                Cleaning = t.Cleaning,
                CMLRequest = t.CMLRequest,
                CML_User = t.CML_User,
                CML_TestDefinition = t.CML_TestDefinition,
                CML_Status = t.CML_Status,
                Comments = t.Comments,
                CreatedBy = t.CreatedBy,
                CreatedOn = t.CreatedOn,
                DataAnalysis = t.DataAnalysis,
                Determines = t.Determines,
                Name = t.Name,
                Reports = t.Reports,
                Request = t.Request,
                SampleAnalysis = t.SampleAnalysis,
                SamplePreparations = t.SamplePreparations,
                Samples = t.Samples,
                SampleSize = t.SampleSize,
                SampleType = t.SampleType,
                StatusID = t.StatusID,
                TestDef = t.TestDef,
                TestFinished = t.TestFinished,
                TestID = t.TestID,
                TestStarted = t.TestStarted,
                Total = t.Total,
                TotalSamples = t.TotalSamples,
                UpdatedBy = t.UpdatedBy,
                UpdatedOn = t.UpdatedOn,
                IsFromRequest = true
             
            };
            tm.TestNotes = db.Notes.Where( a => a.ParentID == tm.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            tm.Attachments = GetFiles( id );
            ViewData["assigneddl"] = new SelectList(db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Tester")), "ID", "DisplayName");
            return View("Edit",tm);
        }
        public ActionResult GetTestView([DataSourceRequest]DataSourceRequest request, int id)
        {
            var t = db.Tests.Find(id);
            var tm = new TestModel
            {
                Abbrev = t.Abbrev,
                AssignedTo = t.AssignedTo,
                Cleaning = t.Cleaning,
                CMLRequest = t.CMLRequest,
                CML_User = t.CML_User,
                CML_TestDefinition = t.CML_TestDefinition,
                CML_Status = t.CML_Status,
                Comments = t.Comments,
                CreatedBy = t.CreatedBy,
                CreatedOn = t.CreatedOn,
                DataAnalysis = t.DataAnalysis,
                Determines = t.Determines,
                Name = t.Name,
                Reports = t.Reports,
                Request = t.Request,
                SampleAnalysis = t.SampleAnalysis,
                SamplePreparations = t.SamplePreparations,
                Samples = t.Samples,
                SampleSize = t.SampleSize,
                SampleType = t.SampleType,
                StatusID = t.StatusID,
                TestDef = t.TestDef,
                TestFinished = t.TestFinished,
                TestID = t.TestID,
                TestStarted = t.TestStarted,
                Total = t.Total,
                TotalSamples = t.TotalSamples,
                UpdatedBy = t.UpdatedBy,
                UpdatedOn = t.UpdatedOn,
                IsFromRequest = false

            };
            tm.TestNotes = db.Notes.Where( a => a.ParentID == tm.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            tm.Attachments = GetFiles( id );
            ViewData["assigneddl"] = new SelectList(db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Tester")), "ID", "DisplayName");
            return View("Edit", tm);
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult Samples_Read([DataSourceRequest]DataSourceRequest request, int testid )
        {
            return Json( GetSamples( testid ).ToDataSourceResult( request ), JsonRequestBehavior.AllowGet );
        }

        private IEnumerable<SampleModel> GetSamples( int testid )
        {
            var samples = db.Samples.Where( a => a.TestID == testid ).Select( s => new SampleModel
            {
                ID = s.ID,
                TestID = s.ID,
                SampleName = s.SampleName,
                TotalSamples = s.TotalSamples
            } );
            return samples.ToList<SampleModel>();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
