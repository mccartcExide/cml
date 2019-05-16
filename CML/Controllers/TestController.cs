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
using CML.Utilities;
using CML.Authorize;

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

        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult Index()
        {
            // RequestedItem ri = new RequestedItem();
            //  ri.Definitions = db.CML_TestDefinition.ToList();
            IList<CML_Status> stats = db.CML_Status.ToList();
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["testers"] = assig;
            ViewData["statuses"] = stats;
           
            return View();
        }
        [HttpPost]
        public ActionResult Update(TestModel test)
        {
            test.UpdatedBy = Session["DisplayName"].ToString();
            test.UpdatedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
               // var id = test.CML_Status.ID;

                var t = db.Tests.Find(test.TestID);
                t.StatusID = test.StatusID;
                t.AssignedTo = test.AssignedTo;
                t.Comments = test.Comments;
                t.UpdatedOn = test.UpdatedOn;
                t.UpdatedBy = test.UpdatedBy;
                t.SampleAnalysis = test.SampleAnalysis;
                t.SamplePreparations = test.SamplePreparations;
                t.DataAnalysis = test.DataAnalysis;
                t.Reports = test.Reports;
                t.Cleaning = test.Cleaning;
                t.Total = ( test.SampleAnalysis + test.SamplePreparations + test.DataAnalysis + test.Reports + test.Cleaning );
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
                test.Total = t.Total;
            }

            if (test.AssignedChanged)
            {
                var lab = db.CML_Settings.Where( a => a.Name.Equals( Structs.Players.LabManager ) ).First().IntVal.Value;

                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestAssignment, null, test.TestID, lab );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestAssignment, null, test.TestID, test.AssignedTo.Value );
            }

            if(test.SaveAndStay)
            {
                test.TestNotes = db.Notes.Where( a => a.ParentID == test.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
                test.Attachments = GetFiles( test.TestID );
                test.CML_Status = db.CML_Status.Find( test.StatusID );
                test.Request = db.Requests.Find( test.CMLRequest );
                ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "DisplayName");
                IList<CML_SampleType> s = db.CML_SampleType.ToList();
                ViewData["sampletypes"] = s;
                TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-success", Title = "Success!", Message = string.Format("Update of Test Number {0} has completed.", test.TestID) };
                test.Request = db.Requests.Find( test.CMLRequest );
                IList<CML_User> assig = db.CML_User.ToList();
                ViewData["testers"] = assig;
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
            test.CreatedBy = (string)Session["DisplayName"];
            test.CreatedOn = DateTime.Now;
            test.CML_Status = db.CML_Status.Where( a => a.ID == 1 ).First();
            test.StatusID = 1;
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

            ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "DisplayName");
            IList<CML_SampleType> s = db.CML_SampleType.ToList();
            ViewData["sampletypes"] = s;
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["testers"] = assig;
            tm.Request = db.Requests.Find( ri.RequestID );
            tm.Attachments = GetFiles( test.TestID);
            tm.TestNotes = db.Notes.Where( a => a.ParentID == test.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            return View("Edit",tm);
        }

        public ActionResult CreateStart([DataSourceRequest]DataSourceRequest request, int requestid)
        {
            RequestedItem ri = new RequestedItem();
            ri.RequestID = requestid;
            return View(ri);
        }
        public ActionResult StartTest(int id )
        {
            TestStartResultModel tsrm = new TestStartResultModel();
            var t = db.Tests.Find( id );
            t.StatusID = 4;
            t.TestStarted = DateTime.Now;
            t.UpdatedBy = ( string )Session["DisplayName"];
            t.UpdatedOn = DateTime.Now;
            t.Request.StatusID = Structs.Statuses.Testing;
            db.Tests.Attach( t );
            db.Entry( t ).State = EntityState.Modified;
            db.SaveChanges();
            //tsrm.time = DateTime.Now;
            //tsrm.status = 4;
            tsrm.message = "Test has been started";
            
            return Json( tsrm, JsonRequestBehavior.AllowGet );
        }

        public ActionResult CompleteTest(int id )
        {
            TestStartResultModel tsrm = new TestStartResultModel();
            var t = db.Tests.Find( id );
            t.StatusID = 5;
            t.TestFinished = DateTime.Now;
            t.UpdatedBy = ( string )Session["DisplayName"];
            t.UpdatedOn = DateTime.Now;
            db.Tests.Attach( t );
            db.Entry( t ).State = EntityState.Modified;
            db.SaveChanges();
            tsrm.time = DateTime.Now;
            tsrm.message = "Test Complete";

            //send email notices
            var req = db.Requests.Find( t.CMLRequest.Value );
            var watchlist = req.WatchList;
            if ( !string.IsNullOrEmpty( watchlist ) )
            {
                var list = watchlist.Split( ',' );
                foreach(var l in list )
                {
                    Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestComplete, null, id, Convert.ToInt32( l ) );
                        
                }

            }
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestComplete, null, id, req.RequestedBy.Value );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestComplete, null, id, db.GetLaboratoryManager().ID );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestComplete, null, id, db.GetUserFromUsername(req.CreatedBy).ID );

            //check for all tests complete
            var alltests = db.Tests.Where( a => a.CMLRequest == t.CMLRequest.Value && a.StatusID != Structs.Statuses.Complete ).Count();

            if(alltests == 0 )
            {
                //we have completed all tests so set request to complete
                req.StatusID = Structs.Statuses.TestsComplete;
                req.TestsFinished = DateTime.Now;
                req.UpdatedBy = Session["DisplayName"].ToString();
                req.UpdateOn = DateTime.Now;
                db.Requests.Attach( req );
                db.Entry( req ).State = EntityState.Modified;
                db.SaveChanges();
                if ( !string.IsNullOrEmpty( watchlist ) )
                {
                    var list = watchlist.Split( ',' );
                    foreach ( var l in list )
                    {
                        Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestComplete, req.RequestID, null, Convert.ToInt32( l ) );

                    }

                }
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestComplete, req.RequestID, null, req.RequestedBy.Value );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestComplete, req.RequestID, null, db.GetLaboratoryManager().ID );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestComplete, req.RequestID, null, db.GetUserFromUsername( req.CreatedBy ).ID );
            }

            return Json(tsrm,JsonRequestBehavior.AllowGet);
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

        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult GetTest([DataSourceRequest]DataSourceRequest request, int id, int req)
        {
            var t = db.Tests.Find(id);
            var name = User.Identity.Name;
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
                IsFromRequest = true,
                AssignedChanged = false,
                IsManager = System.Web.Security.Roles.IsUserInRole( Utilities.Utilities.Instance.StripDomain( name ), "Manager" )

        };
            tm.CML_Status = db.CML_Status.Find( tm.StatusID );
            tm.TestNotes = db.Notes.Where( a => a.ParentID == tm.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            tm.Attachments = GetFiles( id );
            ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "DisplayName");
            IList<CML_SampleType> s = db.CML_SampleType.ToList();
            ViewData["sampletypes"] = s;
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["testers"] = assig;
            return View("Edit",tm);
        }
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
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
                IsFromRequest = false,
                AssignedChanged = false

            };
            tm.TestNotes = db.Notes.Where( a => a.ParentID == tm.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            tm.Attachments = GetFiles( id );
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["testers"] = assig;
            IList<CML_SampleType> s = db.CML_SampleType.ToList();
            ViewData["sampletypes"] = s;
            ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "DisplayName");
            return View("Edit", tm);
        }
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult GetTestFromSampleView(  int id, string msg )
        {

            var t = db.Tests.Find( id );
            var name = User.Identity.Name;
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
                IsFromRequest = false,
                AssignedChanged = false,
                IsManager = System.Web.Security.Roles.IsUserInRole( Utilities.Utilities.Instance.StripDomain( name ), "Manager" )

            };
            tm.TestNotes = db.Notes.Where( a => a.ParentID == tm.TestID && a.NoteType.Equals( "T" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            tm.Attachments = GetFiles( id );
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["testers"] = assig;
            IList<CML_SampleType> s = db.CML_SampleType.ToList();
            ViewData["sampletypes"] = s;
            ViewData["assigneddl"] = new SelectList( db.CML_User, "ID", "DisplayName" );
            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-success", Title = "Success!", Message = msg };
            return View( "Edit", tm );
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
                TotalSamplesNbr = s.TotalSamples,
                SampleTypeID = s.SampleTypeID
                
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
