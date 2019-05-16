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
using CML.Utilities;
using CML.Authorize;

namespace CML.Controllers
{
    public class ApprovalController : Controller
    {
        private CMLEntities db = new CMLEntities();

        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult Index()
        {
            IList<CML_User> approv = db.CML_User.ToList();
            ViewData["approvers"] = approv;
            return View();
        }

        [CMLRoleAuthorize( Authorize.Roles.Admin, Authorize.Roles.Manager, Authorize.Roles.User )]
        public ActionResult GetApprovalRec(int id, int req)
        {
            var approval = db.CML_Approvals.Find( id );

            var user = db.GetUserFromUsername( User.Identity.Name );

            if ( user.ID != approval.UserID )
            {
                return View( "AccessDenied" );

            }




            var request = db.Requests.Find( req );
            CMLRequest r = new CMLRequest().Convert( request );
            ApprovalModel am = new ApprovalModel();
            am.ReqiestID = req;
            am.ApprovalID = id;
            am.Comments = approval.Comments;
            am.Approval = approval;
            am.Request = r;
            am.ActionTaken = approval.Outcome;
            am.RequestedBy = db.CML_User.Where( c => c.ID == r.RequestedBy ).Select( b => b.DisplayName ).First().ToString();
            if ( r.AssignedTo.HasValue )
                am.AssignedTo = db.CML_User.Where( c => c.ID == r.AssignedTo.Value ).Select( b => b.DisplayName ).First().ToString();

           


            return View("ActionApprovalView",am);

        }
        [HttpPost]
        public ActionResult ApproveRequest(ApprovalModel am)
        {
            var ap = db.CML_Approvals.Find( am.ApprovalID );
            
            ap.Outcome = am.ActionTaken;
            ap.DateActioned = DateTime.Now;
            ap.Comments = am.Comments;

            Request req = db.Requests.Find( am.ReqiestID );

            if ( am.ActionTaken.Equals( "Approved" ))
            {
                //Approved
                if ( req.DirectorApprovalRequired.Value )
                {
                    CML_Approvals lab = new CML_Approvals();
                    lab.RequestID = am.ReqiestID;
                    lab.CML_User = db.GetLaboratoryManager();
                    lab.UserID = lab.CML_User.ID;
                    lab.DateAssigned = DateTime.Now;

                    db.CML_Approvals.Add( lab );
                    req.DirectorApprovalRequired = false;
                    Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.ApprovalRequired, am.ReqiestID, null, lab.UserID );


                }
                else
                {
                    req.StatusID = Structs.Statuses.Approved;
                    req.AssignedTo = db.CML_Settings.Where( a => a.Name.Equals(Structs.Players.LabManager ) ).Select( a => a.IntVal ).First();
                    //send the approved notice

                    var creator = db.GetUserFromUsername( req.CreatedBy );
                    Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestApproved, req.RequestID, null, creator.ID );
                    Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestApproved, req.RequestID, null, req.RequestedBy.Value );
                    Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestApproved, req.RequestID, null, db.GetLaboratoryManager().ID );



                }
            }
            else
            {
                //rejected
                req.StatusID = Structs.Statuses.Rejected;
                var creator = db.GetUserFromUsername( req.CreatedBy );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestRejection, req.RequestID, null, creator.ID );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestRejection, req.RequestID, null, req.RequestedBy.Value );
                Utilities.Utilities.Instance.CreateEmailNotice( Structs.EmailNotices.RequestRejection, req.RequestID, null, db.GetLaboratoryManager().ID );
            }

            db.CML_Approvals.Attach( ap );
            db.Entry( ap ).State = EntityState.Modified;

            


            db.SaveChanges();

           return RedirectToAction( "Index", "Approval" );
        }

        public ActionResult CML_Approvals_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<CML_Approvals> cml_approvals = db.CML_Approvals;
            DataSourceResult result = cml_approvals.ToDataSourceResult(request, cML_Approvals => new {
                ID = cML_Approvals.ID,
                Outcome = cML_Approvals.Outcome,
                DateAssigned = cML_Approvals.DateAssigned,
                DateActioned = cML_Approvals.DateActioned,
                Comments = cML_Approvals.Comments,
                RequestID = cML_Approvals.RequestID,
                UserID = cML_Approvals.UserID,
                CMLNumber = cML_Approvals.Request.CMLNumber,
                Name = cML_Approvals.Request.Name

               // Request = cML_Approvals.Request
            });

            return Json(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
