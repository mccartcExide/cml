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
    public class RequestController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {
            IList<CML_Location> locs = db.CML_Location.ToList();
            ViewData["locations"] = locs;
            IList<CML_Status> stats = db.CML_Status.ToList();
            ViewData["statuses"] = stats;
            IList < CML_RequestType > rts = db.CML_RequestType.ToList();
            ViewData["request_types"] = rts;
            IList<CML_User> assig = db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Assignee")).ToList();
            ViewData["assignee"] = assig;
            return View();
        }
        
        //public JsonResult GetLocations()
        //{
        //    return Json(db.CML_Location, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetPriorities()
        //{
        //    return Json(db.CML_Priority, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetBusinesUnits()
        //{
        //    return Json(db.CML_BusinessUnit, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetRequestTypes()
        //{
        //    return Json(db.CML_RequestType, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDispositions()
        //{
        //    return Json(db.CML_Disposition, JsonRequestBehavior.AllowGet);
        //}
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
        private void BuildSelectLists()
        {
            ViewData["locationsddl"] = new SelectList(db.CML_Location, "LocationID", "Location");
            ViewData["request_typesddl"] = new SelectList(db.CML_RequestType, "ID", "RequestType");
            ViewData["businessunitddl"] = new SelectList(db.CML_BusinessUnit, "ID", "BusinessUnit");
            ViewData["dispositionddl"] = new SelectList(db.CML_Disposition, "ID", "Disposition");
            ViewData["priorityddl"] = new SelectList(db.CML_Priority, "ID", "Priority");
            ViewData["assigneddl"] = new SelectList(db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Assignee")), "ID", "DisplayName");
            ViewData["requesterddl"] = new SelectList(db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Requester")), "ID", "DisplayName");

            IList<CML_Status> stats = db.CML_Status.ToList();
            IList<CML_User> assig = db.CML_User.Where(a => a.CML_RoleType.RoleType.Equals("Tester")).ToList();
            ViewData["statuses"] = stats;
            ViewData["testers"] = assig;

            //var t = db.CML_User.Where(l => l.CML_RoleType.RoleType.Equals("Approver")).ToList();




            //ViewData["assigneddl"] = new SelectList(db.CML_User, "ID", "Email");
        }

        [HttpPost]
        public ActionResult Requests_Create([DataSourceRequest]DataSourceRequest request, CMLRequest cmlRequest)
        {
            if (ModelState.IsValid)
            {
                var req = new Request
                {
                    Name = cmlRequest.Name,
                    CMLNumber = cmlRequest.CMLNumber,
                    RequestedBy = cmlRequest.RequestedBy,
                    ProjectNumber = cmlRequest.ProjectNumber,
                    DeviationNumber = cmlRequest.DeviationNumber,
                    EWRNumber = cmlRequest.EWRNumber,
                    DirectorApprovalRequired = cmlRequest.DirectorApprovalRequired,
                    RetentionDate = cmlRequest.RetentionDate,
                    Phone = cmlRequest.Phone,
                    TestObjectives = cmlRequest.TestObjectives,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "mccartc",
                    DateRequired = cmlRequest.DateRequired,
                   
                    LocationID = cmlRequest.LocationID,
                    BusinessUnitID = cmlRequest.BusinessUnitID,
                    DispositionID = cmlRequest.DispositionID,
                    PriorityID = cmlRequest.PriorityID,
                    RequestTypeID = cmlRequest.RequestTypeID,
                    StatusID = 1
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



            return View(cmlRequest);
            //return Json(new[] { cmlRequest }.ToDataSourceResult(request, ModelState));
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
                AssignedTo = r.AssignedTo
                
            });

            return reqs;
        }
        public  ActionResult Edit(int? id )
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"No ID was passed to the controller");
            }

            BuildSelectLists();
            CMLEntities db = new CMLEntities();
            var req = db.Requests.Find(id);//.Where(r1 => r1.RequestID == id).FirstOrDefault();

            CMLRequest r = new CMLRequest().Convert(req);
            r.RequestNotes = db.Notes.Where( a => a.ParentID == id && a.NoteType.Equals( "R" ) ).OrderByDescending(a=>a.CreatedOn).ToList();
            r.Attachments = GetFiles( id.Value );
            return View(r);
            
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
                //CML_BusinessUnit = req.CML_BusinessUnit,
                //CML_Disposition = req.CML_Disposition,
                //CML_Location = req.CML_Location,
                //CML_Priority = req.CML_Priority,
                //CML_RequestType = req.CML_RequestType,
                //CML_Status = req.CML_Status,
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
                entity.UpdatedBy = "mccartc'";
                entity.UpdateOn = DateTime.Now;
                
                
            
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
