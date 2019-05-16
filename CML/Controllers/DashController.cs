﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CML.Models;


namespace CML.Controllers
{
    public class DashController : Controller
    {
        private CMLEntities db = new CMLEntities();

        public ActionResult Index()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Requests_Read()
        {
            //return Json(db.Requests.Select(request =>  new {
            //    Name = request.Name,
            //    CMLNumber = request.CMLNumber,
            //    RequestedBy = request.RequestedBy,
            //    ProjectNumber = request.ProjectNumber,
            //    DeviationNumber = request.DeviationNumber,
            //    EWRNumber = request.EWRNumber,
            //    DirectorApprovalRequired = request.DirectorApprovalRequired,
            //    RetentionDate = request.RetentionDate,
            //    Phone = request.Phone,
            //    TestObjectives = request.TestObjectives,
            //    CreatedOn = request.CreatedOn,
            //    DateRequired = request.DateRequired,
            //    CreatedBy = request.CreatedBy,
            //    UpdateOn = request.UpdateOn,
            //    UpdatedBy = request.UpdatedBy,
            //    TestsStarted = request.TestsStarted,    
            //    TestsFinished = request.TestsFinished,
            //    TotalSamples = request.TotalSamples,
            //    WatchList = request.WatchList,
            //    StatusID = request.StatusID,
            //    CML_Status =  request.CML_Status
            //}));

            var test = db.Requests.Where(a=> a.StatusID < 5  ).GroupBy( a => a.StatusID ).
                Select( g => new PieChartStatus
                {
                    ID = g.Key,
                    count = g.Count(),
                    Status = string.Empty
                } ).ToList();
            foreach(var t in test )
            {
                t.Status = db.CML_Status.Find( t.ID ).Status;
            }
            return Json( test );
            //return Json( db.Requests.GroupBy(a=> a.StatusID).Select( request => new {


            //    Status = request.StatusID,
            //    ID = request.CML_Status.ID

            //} ) );


        }
        [HttpPost]
        public ActionResult Pdf_Export_Save( string contentType, string base64, string fileName )
        {
            var fileContents = Convert.FromBase64String( base64 );

            return File( fileContents, contentType, fileName );
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
