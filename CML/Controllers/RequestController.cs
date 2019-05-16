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
using System.Web.Security;
using System.Security.Principal;
using CML.Authorize;

namespace CML.Controllers
{
    public class RequestController : Controller
    {
        private CMLEntities db = new CMLEntities();
        [CMLRoleAuthorize( Authorize.Roles.User,Authorize.Roles.Manager, Authorize.Roles.Admin)]
        public ActionResult Index()
        {
            IList<CML_Location> locs = db.CML_Location.ToList();
            ViewData["locations"] = locs;
            IList<CML_Status> stats = db.CML_Status.ToList();
            ViewData["statuses"] = stats;
            IList < CML_RequestType > rts = db.CML_RequestType.ToList();
            ViewData["request_types"] = rts;
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["assignee"] = assig;
            return View();
        }
        
      
        public ActionResult Requests_Create()
        {
            //IList<CML_Location> locs = db.CML_Location.ToList();
            BuildSelectLists();
            
            return View();
        }

       
        public IEnumerable<TestModel> GetTests(int reqid)
        {
            var tests = db.Tests.Where(a => a.CMLRequest == reqid).Select(test => new TestModel
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

            return tests.ToList<TestModel>();
        }
        public ActionResult Tests_Read([DataSourceRequest]DataSourceRequest request,int reqid)
        {

            return Json(GetTests(reqid).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Approvals_Read([DataSourceRequest]DataSourceRequest request, int reqid )
        {
            IQueryable<CML_Approvals> cml_app = db.CML_Approvals.Where( a => a.RequestID == reqid );
            DataSourceResult result = cml_app.ToDataSourceResult( request, app => new
            {
                ID = app.ID,
                Outcome = app.Outcome,
                RequestID = app.RequestID,
                UserID = app.UserID,
                Comments = app.Comments,
                DateAssigned = app.DateAssigned,
                DateActioned = app.DateActioned
            } );
            return Json( result, JsonRequestBehavior.AllowGet );

        }
        private void BuildSelectLists()
        {
            ViewData["locationsddl"] = new SelectList(db.CML_Location, "LocationID", "Location");
            ViewData["request_typesddl"] = new SelectList(db.CML_RequestType, "ID", "RequestType");
            ViewData["businessunitddl"] = new SelectList(db.CML_BusinessUnit, "ID", "BusinessUnit");
            ViewData["dispositionddl"] = new SelectList(db.CML_Disposition, "ID", "Disposition");
            ViewData["priorityddl"] = new SelectList(db.CML_Priority, "ID", "Priority");
            ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "DisplayName");
            ViewData["requesterddl"] = new SelectList(db.CML_User, "ID", "DisplayName");

            IList<CML_Status> stats = db.CML_Status.ToList();
           //// IList<CML_User> assig = db.CML_User.ToList();
            //IList<CML_User> approv = db.CML_User.ToList();
            IList<CML_User> recipients = db.CML_User.ToList();
          //  ViewData["approvers"] = approv;
            ViewData["statuses"] = stats;
            //ViewData["testers"] = assig;
            ViewData["recipients"] = recipients;

            //var t = db.CML_User.Where(l => l.CML_RoleType.RoleType.Equals("Approver")).ToList();




            //ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "Email");
        }

        [HttpPost]
        public ActionResult Requests_Create([DataSourceRequest]DataSourceRequest request, CMLRequest cmlRequest)
        {
            if (ModelState.IsValid)
            {
                var cml = BuildCMLNumber( cmlRequest.BusinessUnitID, cmlRequest.RequestTypeID, cmlRequest.LocationID );
                cmlRequest.CMLNumber = cml;
                cmlRequest.StatusID = 1;
                var req = new Request
                {
                    Name = cmlRequest.Name,
                    CMLNumber = cml,
                    RequestedBy = cmlRequest.RequestedBy,
                    ProjectNumber = cmlRequest.ProjectNumber,
                    DeviationNumber = cmlRequest.DeviationNumber,
                    EWRNumber = cmlRequest.EWRNumber,
                    DirectorApprovalRequired = false,
                    RetentionDate = cmlRequest.RetentionDate,
                    Phone = cmlRequest.Phone,
                    TestObjectives = cmlRequest.TestObjectives,
                    CreatedOn = DateTime.Now,
                    CreatedBy = WindowsIdentity.GetCurrent().Name,
                    DateRequired = cmlRequest.DateRequired,
                   
                    LocationID = cmlRequest.LocationID,
                    BusinessUnitID = cmlRequest.BusinessUnitID,
                    DispositionID = cmlRequest.DispositionID,
                    PriorityID = cmlRequest.PriorityID,
                    RequestTypeID = cmlRequest.RequestTypeID,
                    StatusID = 1,
                   
                    
                    //UpdateOn = request.UpdateOn,
                    //UpdatedBy = request.UpdatedBy,
                    //TestsStarted = request.TestsStarted,
                    //TestsFinished = request.TestsFinished,

                };
                db.Requests.Add(req);
                db.SaveChanges();
                cmlRequest.RequestID = req.RequestID;
                BuildSelectLists();
            }

            //send notification
            var creator = db.GetUserFromUsername( User.Identity.Name );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.CreatedRequest, cmlRequest.RequestID, null, creator.ID );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.CreatedRequest, cmlRequest.RequestID, null, cmlRequest.RequestedBy.Value );


            cmlRequest.RequestNotes = db.Notes.Where( a => a.ParentID == cmlRequest.RequestID && a.NoteType.Equals( "R" ) ).OrderByDescending( a => a.CreatedOn ).ToList();
            cmlRequest.Attachments = GetFiles( cmlRequest.RequestID );
            
            return View("Edit",cmlRequest);
            //return Json(new[] { cmlRequest }.ToDataSourceResult(request, ModelState));
        }

        private string BuildCMLNumber( int businessUnitID, int requestTypeID, int locationID )
        {
            string result = string.Empty;
            var bu = db.CML_BusinessUnit.Where( a => a.ID == businessUnitID ).Select( a => a.BusinessUnit ).First();
            result = bu.Substring( bu.IndexOf( '(' )+1, 2 );
            result = result + db.NextNumber().Trim();
            result = result + db.CML_RequestType.Find( requestTypeID ).Category.Trim();
            result = result + DateTime.Now.Year.ToString().Substring( 2 ).Trim();
            result = result + db.CML_Location.Find( locationID ).Code;
            return result;
        }

        private AttachmentsModel GetFiles(int reqid )
        {
            string dirPath = @"~\Upload\Requests\" + reqid;
            AttachmentsModel result = new AttachmentsModel();
            result.ParentID = reqid;
            result.ParentType = "R";

            List<Files> list = new List<Files>();
            result.Attachments = list;
            if ( Directory.Exists( Server.MapPath(dirPath) ) )
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
       
        
        private static IEnumerable<CMLRequest> GetRequests()
        {
            var dbs = new CMLEntities();

            var reqs = dbs.Requests.Select(r => new CMLRequest
            {
                RequestID = r.RequestID,
                Name = r.Name,
                LocationID = r.LocationID,
                BusinessUnitID = r.BusinessUnitID,
                DispositionID = r.DispositionID,
                PriorityID = r.PriorityID,
                RequestTypeID = r.RequestTypeID,
                CMLNumber = r.CMLNumber,
                RequestedBy = r.RequestedBy,
                ProjectNumber = r.ProjectNumber,
                DeviationNumber = r.DeviationNumber,
                EWRNumber = r.EWRNumber,
                DirectorApprovalRequired = r.DirectorApprovalRequired,
                RetentionDate = r.RetentionDate,
                Phone = r.Phone,
                TestObjectives = r.TestObjectives,
                CreatedOn = r.CreatedOn,
                DateRequired = r.DateRequired,
                CreatedBy = r.CreatedBy,
                UpdateOn = r.UpdateOn,
                UpdatedBy = r.UpdatedBy,
                TestsStarted = r.TestsStarted,
                TestsFinished = r.TestsFinished,
                StatusID = r.StatusID,
                AssignedTo = r.AssignedTo,
                AssigneeeChanged = false
                
            });

            return reqs;
        }
        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager,Authorize.Roles.User )]
        public  ActionResult Edit(int? id )
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"No ID was passed to the controller");
            }
            var name = User.Identity.Name.ToString();
          
            BuildSelectLists();
            CMLEntities db = new CMLEntities();
            var req = db.Requests.Find(id);//.Where(r1 => r1.RequestID == id).FirstOrDefault();

            CMLRequest r = new CMLRequest().Convert(req);


            //TO DO check on manager
            r.IsManager = System.Web.Security.Roles.IsUserInRole( Utilities.Utilities.Instance.StripDomain(name ), "Manager" );

            r.RequestNotes = db.Notes.Where( a => a.ParentID == id && a.NoteType.Equals( "R" ) ).OrderByDescending(a=>a.CreatedOn).ToList();
            r.Attachments = GetFiles( id.Value );
            //if ( r.DirectorApprovalRequired.Value )
            //{
            //    TempData["ApprovalMessage"] = new MessageVM() { CssClassName = "alert alert-info", Title = "Notice!", Message = string.Format( "Director approval is required for this request!." ) };
            //}
            return View(r);
            
        }

        public ActionResult CheckApproval( int id, int priorityid )
        {
            CalculateSamples cs = new CalculateSamples();
            Result result = cs.CheckForApproval( id, priorityid );

            //  var result  { isApproval: "Yes"};
            return Json( result, JsonRequestBehavior.AllowGet );
        }


        [HttpPost]
        //public ActionResult SaveChanges([DataSourceRequest]DataSourceRequest request, CMLRequest req)
        public ActionResult SaveChanges( CMLRequest req)
        {
            
            if (ModelState.IsValid)
            {
                CMLEntities db = new CMLEntities();
                Request entity = new Request();

                entity.AssignedTo = req.AssignedTo;
                entity.BusinessUnitID = req.BusinessUnitID;
                entity.CMLNumber = req.CMLNumber;
                entity.CreatedBy = req.CreatedBy;
                entity.CreatedOn = req.CreatedOn;
                entity.DateRequired = req.DateRequired;
                entity.DeviationNumber = req.DeviationNumber;
                entity.DirectorApprovalRequired = req.DirectorApprovalRequired;
                entity.DispositionID = req.DispositionID;
                entity.EWRNumber = req.EWRNumber;
                entity.LocationID = req.LocationID;
                entity.Name = req.Name;
                entity.Phone = req.Phone;
                entity.PriorityID = req.PriorityID;
                entity.ProjectNumber = req.ProjectNumber;
                entity.RequestedBy = req.RequestedBy;
                entity.RequestID = req.RequestID;
                entity.RequestTypeID = req.RequestTypeID;
                entity.RetentionDate = req.RetentionDate;
                entity.StatusID = req.StatusID;
                entity.TestObjectives = req.TestObjectives;
                entity.TestsFinished = req.TestsFinished;
                entity.TestsStarted = req.TestsStarted;
                entity.UpdatedBy = Session["DisplayName"].ToString();
                entity.UpdateOn = DateTime.Now;

                if ( req.WatchList != null )
                {
                    string watchList = string.Empty;
                    foreach ( var wl in req.WatchList )
                    {
                        watchList = watchList + wl + ",";
                    }
                    if ( !string.IsNullOrEmpty( watchList ) )
                    {
                        watchList = watchList.Substring( 0, watchList.Length - 1 );
                    }
                    entity.WatchList = watchList;
                }
                    db.Requests.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                

                if (!String.IsNullOrEmpty( req.Note ) )
                {
                    Note n = new Note();
                    n.NoteType = "R";
                    n.ParentID = req.RequestID;
                    n.NoteText = req.Note;
                    n.CreatedOn = DateTime.Now;
                    n.CreatedBy = Session["DisplayName"].ToString();
                    db.Notes.Add( n );
                    
                }
              
                db.SaveChanges();
                req.Note = string.Empty;
            }
            //Notifications
            if ( req.AssigneeeChanged )
            {
               // var user = db.CML_User.Find(  );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestAssignment, req.RequestID,null, req.AssignedTo.Value );

            }
            req.RequestNotes = db.Notes.Where( a => a.ParentID == req.RequestID && a.NoteType.Equals( "R" ) ).OrderByDescending(a=> a.CreatedOn).ToList();
            req.Attachments = GetFiles( req.RequestID );
            BuildSelectLists();
            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-success", Title = "Success!", Message = string.Format( "Update of {0} has completed.",req.CMLNumber) };
            return View("Edit",req);
           // return View(req);
            //return Json(new[] { req }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult Requests_Read([DataSourceRequest]DataSourceRequest request)
        {
           

            return Json(GetRequests().ToDataSourceResult(request));
        }
        public JsonResult WatchList_Read(string text )
        {
            var peeps = db.CML_User.Select( p => new UserModel
            {
                ID = p.ID,
                DisplayName = p.DisplayName
            } );

            if ( !string.IsNullOrEmpty( text ) )
            {
                peeps = peeps.Where( a => a.DisplayName.Contains( text ) );
            }
            return Json( peeps, JsonRequestBehavior.AllowGet );
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        //[ValidateAntiForgeryToken]
        public ActionResult Destroy(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request req = db.Requests.Find(id);
            if(req == null)
            {
                return HttpNotFound();
            }
            db.Requests.Remove(req);
            db.SaveChanges();
            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-success", Title = "Success!", Message = string.Format("Deletion of {0} has completed.", req.CMLNumber) };
            return RedirectToAction("Index");
        }
        public ActionResult CompleteRequest(int id, string comment )
        {
            var req = db.Requests.Find( id );
            req.StatusID = Structs.Statuses.Complete;
            req.ClosureNotes = comment;


            req.UpdatedBy = Session["DisplayName"].ToString();
            req.UpdateOn = DateTime.Now;

            db.Requests.Attach( req );
            db.Entry( req ).State = EntityState.Modified;


           db.SaveChanges();
            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-info", Title = "Information", Message = string.Format( "The request {0} has been completed.", req.CMLNumber ) };


            return JavaScript( "location.reload(true)" );

            //return Content("done");
        }
        public ActionResult CancelRequest(int id )
        {

            var req = db.Requests.Find( id );
            req.StatusID = Structs.Statuses.Cancelled;

            //Cancel Workflow
            var app = req.CML_Approvals.Where( a => a.Outcome == null || a.Outcome.Equals( string.Empty ) ).FirstOrDefault();
            if ( app != null )
            {
                app.Outcome = Structs.Outcomes.Cancelled;
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalCancelled, req.RequestID, null, app.UserID );
            }
            var tests = req.Tests;
            foreach ( var t in tests )
            {
                if ( t.StatusID != Structs.Statuses.Complete )
                {
                    t.StatusID = Structs.Statuses.Cancelled;
                    if ( t.AssignedTo.HasValue )
                        Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.TestCancelled, null, t.TestID, t.AssignedTo.Value );


                }
            }

            //send cancellation messages
            var labManager = db.GetLaboratoryManager().ID;
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestCancelled, req.RequestID, null, req.RequestedBy.Value );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestCancelled, req.RequestID, null, labManager );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestCancelled, req.RequestID, null, db.GetUserFromUsername( req.CreatedBy ).ID );

            var watchlist = req.WatchList;
            if ( !string.IsNullOrEmpty( watchlist ) )
            {
                var list = watchlist.Split( ',' );
                foreach ( var l in list )
                {
                    Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestCancelled, id, null, Convert.ToInt32( l ) );

                }

            }





            var note = new Note();
            note.CreatedBy = Session["DisplayName"].ToString();
            note.CreatedOn = DateTime.Now;
            note.NoteText = "The request and any approvals have been cancelled";
            note.NoteType = "R";
            note.ParentID = id;

            db.Notes.Add( note );

            IList<CML_Location> locs = db.CML_Location.ToList();
            ViewData["locations"] = locs;
            IList<CML_Status> stats = db.CML_Status.ToList();
            ViewData["statuses"] = stats;
            IList<CML_RequestType> rts = db.CML_RequestType.ToList();
            ViewData["request_types"] = rts;
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["assignee"] = assig;
            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-danger", Title = "Warning!", Message = string.Format( "The request {0} and its approvals and Tests have been cancelled.", req.CMLNumber ) };
            //return View( "Index" );
            return JavaScript( "location.reload(true)" );
        }
        public ActionResult CancelApproval(int id )
        {
            var labManager = db.GetLaboratoryManager().ID;
            var req = db.Requests.Find( id );
            req.StatusID = Structs.Statuses.Open;

            var note = new Note();
            note.CreatedBy = Session["DisplayName"].ToString();
            note.CreatedOn = DateTime.Now;
            note.NoteText = "The workflow has been cancelled";
            note.NoteType = "R";
            note.ParentID = id;

            db.Notes.Add( note );


            //Cancel Workflow
            var app = req.CML_Approvals.Where( a => a.Outcome == null || a.Outcome.Equals( string.Empty ) ).First();
            app.Outcome = Structs.Outcomes.Cancelled;
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalCancelled, req.RequestID, null, app.UserID );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalCancelled, req.RequestID, null, req.RequestedBy.Value );
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalCancelled, req.RequestID, null, labManager );
            //cancel tests






            db.Requests.Attach( req );
            db.Entry( req ).State = EntityState.Modified;


            db.SaveChanges();


            IList<CML_Location> locs = db.CML_Location.ToList();
            ViewData["locations"] = locs;
            IList<CML_Status> stats = db.CML_Status.ToList();
            ViewData["statuses"] = stats;
            IList<CML_RequestType> rts = db.CML_RequestType.ToList();
            ViewData["request_types"] = rts;
            IList<CML_User> assig = db.CML_User.ToList();
            ViewData["assignee"] = assig;
            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-warning", Title = "Warning!", Message = string.Format( "The request {0} has its request for approval cancelled.", req.CMLNumber ) };
            //var msg = new MessageVM() { CssClassName = "alert alert-warning", Title = "Warning!", Message = string.Format( "The request {0} has its request for approval cancelled.", req.CMLNumber ) };
            // return View( "Index" );
            //return RedirectToAction( "Index", "Request" );
            //return Json( msg, JsonRequestBehavior.AllowGet );
           return  JavaScript( "location.reload(true)" );

        }
        public ActionResult RequestApproval(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request req = db.Requests.Find(id);
            if (req == null)
            {
                return HttpNotFound();
            }

            var labManager = db.GetLaboratoryManager().ID;

            var approv = 0;
            if ( req.DirectorApprovalRequired.Value )
            {
                //get director approval
                approv = req.CML_BusinessUnit.ApproverID;
                
            }
            else
            { // Get lab manager approval
              // approv = db.CML_User.Where( a => a.CML_RoleType.RoleType.Equals( "Laboratory Manager" ) ).Select(b=> b.ID).First();
              //need to find better soluton for this
                approv = labManager;
            }
            CML_Approvals rec = new CML_Approvals
            {
                UserID = approv,
                DateAssigned = DateTime.Now,
                RequestID = req.RequestID

            };
            db.CML_Approvals.Add( rec );
            req.StatusID = 2;

            db.Requests.Attach( req );
            db.Entry( req ).State = EntityState.Modified;

            db.SaveChanges();
            Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalRequired, req.RequestID, null, approv );

            var sendNotice = db.Notifications.Where( a => a.RequestID == req.RequestID && a.MessageType.Equals( Structs.EmailNotices.ApprovalNotice ) ).Count();
            if(sendNotice == 0 )
            {
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalNotice, req.RequestID, null, req.RequestedBy.Value );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalNotice, req.RequestID, null, labManager );

            }

            TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-info", Title = "Information!", Message = string.Format("The request {0} has been submitted for approval.", req.CMLNumber) };

            return RedirectToAction("Index");
        }
        public ActionResult CreateTest(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request req = db.Requests.Find(id);
            if (req == null)
            {
                return HttpNotFound();
            }
            //TempData["UserMessage"] = new MessageVM() { CssClassName = "alert alert-info", Title = "Information!", Message = string.Format("Something is going to happen to {0}.", req.CMLNumber) };

            //return RedirectToAction("Index");
            return RedirectToAction("CreateStart", "Test", new { requestid = req.RequestID });
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
