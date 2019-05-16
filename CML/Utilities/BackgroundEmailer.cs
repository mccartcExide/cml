using CML.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CML.Utilities
{
    public class BackgroundEmailer
    {

        public void ProcessMail()
        {
            System.Diagnostics.Debug.WriteLine( "We is here" );

            using (CMLEntities db = new CMLEntities() )
            {
                IList<Notification> unsent = db.Notifications.Where( a => a.Sent == false ).ToList<Notification>();
                foreach(var us in unsent){

                    if(us.MessageType == Structs.EmailNotices.ApprovalNotice )
                    {
                        SendApprovalNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.ApprovalRequired)
                    {
                        SendApprovalRequired( db, us );
                    }
                    if(us.MessageType == Structs.EmailNotices.ApprovalCancelled )
                    {
                        CreateApprovalCancelled( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.CreatedRequest )
                    {
                        CreatedRequestNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.RequestApproved )
                    {
                        RequestApprovedNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.RequestAssignment )
                    {
                        RequestAssignedNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.RequestComplete )
                    {
                        RequestCompleteNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.RequestRejection )
                    {
                        RequestRejectedNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.TestAssignment )
                    {
                        TestAssignedNotice( db, us );
                    }
                    if ( us.MessageType == Structs.EmailNotices.TestComplete)
                    {
                        TestCompleteNotice( db, us );
                    }
                    if(us.MessageType == Structs.EmailNotices.TestCancelled )
                    {
                        TestCancelledNotice( db, us );
                    }
                    if(us.MessageType == Structs.EmailNotices.RequestCancelled )
                    {
                        RequestCancelledNotice( db, us );
                    }

                    us.Sent = true;
                    us.SentOn = DateTime.Now;
                    db.Notifications.Attach( us );
                    db.Entry( us ).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

       

        private static string GetHost()
        {
            return ConfigurationManager.AppSettings.Get( "MailHost" );
        }
        private static string GetPath()
        {
            string path = "";

            path = ConfigurationManager.AppSettings.Get( "Path" );

            return path;
        }
        private void TestCompleteNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ));
            message.Subject = string.Format( "CML Test Complete for test {0}" ,us.TestID );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Test ID " + us.TestID + " on Request "+ us.Test.Request.CMLNumber+ " has been completed <br/>";
            body += "<a href='" + GetPath() + "Test/GetTestView?id=" + us.TestID + "'> Click here to go to the test</a><br/>";
            body += "<hr style='color:orange;'/>";
            
            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach(var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString()+" "+n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }

            body += "<hr style='color:orange;'/>";

            message.Body = body ;
            SmtpClient client = new SmtpClient(GetHost());
            
            client.Send( message );
        }

        private void TestCancelledNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Test {0} Cancelled", us.TestID );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Test ID " + us.TestID + " on Request " + us.Test.Request.CMLNumber + " has been cancelled <br/>";
            body += "<a href='" + GetPath() + "Test/GetTestView?id=" + us.TestID + "'> Click here to go to the test</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }

            body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }



        private void CreateApprovalCancelled( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML approval CANCELLATION");

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Approval for CML " + us.Request.CMLNumber + " is no longer required. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.RequestID && a.NoteType.Equals( "R" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }
        private void TestAssignedNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Test {0} has been assigned", us.TestID );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Test ID " + us.TestID + " on Request " + us.Test.Request.CMLNumber + " has been assigned to you <br/>";
            body += "<a href='" + GetPath() + "Test/GetTestView?id=" + us.TestID + "'> Click here to go to the test</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }

            body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }

        private void RequestRejectedNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been rejected.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been rejected. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            body += "Approvals <br/>";
            var approvals = us.Request.CML_Approvals;
            foreach(var ap in approvals )
            {
                string date = ap.DateActioned.HasValue ? ap.DateActioned.Value.ToShortDateString() : string.Empty;
                body += "<strong>" + ap.CML_User.DisplayName + " " + date + "</strong><br/>";
                body += "Outcome: " + ap.Outcome + "<br/>";
                body += ap.Comments + "<br/>";

            }
            body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );

        }
        private void RequestCancelledNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been cancelled.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been cancelled. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            body += "Approvals <br/>";
            //var approvals = us.Request.CML_Approvals;
            //foreach ( var ap in approvals )
            //{
            //    body += "<strong>" + ap.CML_User.DisplayName + " " + ap.DateActioned.Value.ToShortDateString() + "</strong><br/>";
            //    body += "Outcome: " + ap.Outcome + "<br/>";
            //    body += ap.Comments + "<br/>";

            //}
            //body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }

        private void RequestCompleteNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been completed.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been completed. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            body += "Approvals <br/>";
            var approvals = us.Request.CML_Approvals;
            foreach ( var ap in approvals )
            {
                string date = ap.DateActioned.HasValue ? ap.DateActioned.Value.ToShortDateString() : string.Empty;
                body += "<strong>" + ap.CML_User.DisplayName + " " + date + "</strong><br/>";
                body += "Outcome: " + ap.Outcome + "<br/>";
                body += ap.Comments + "<br/>";

            }
            body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );

        }

        private void RequestAssignedNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been assigned to you.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been assigned. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            //body += "Approvals <br/>";
            //var approvals = us.Request.CML_Approvals;
            //foreach ( var ap in approvals )
            //{
            //    body += "<strong>" + ap.CML_User.DisplayName + " " + ap.DateActioned.Value.ToShortDateString() + "</strong><br/>";
            //    body += "Outcome: " + ap.Outcome + "<br/>";
            //    body += ap.Comments + "<br/>";

            //}
           // body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );

        }

        private void RequestApprovedNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been approved.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been approved. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            body += "Approvals <br/>";
            var approvals = us.Request.CML_Approvals;
            foreach ( var ap in approvals )
            {
               // String date = string.Empty;
                string date = ap.DateActioned.HasValue ? ap.DateActioned.Value.ToShortDateString() : string.Empty;
                body += "<strong>" + ap.CML_User.DisplayName + " " + date + "</strong><br/>";
                body += "Outcome: " + ap.Outcome + "<br/>";
                body += ap.Comments + "<br/>";

            }
            body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }

        private void CreatedRequestNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been created.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been created. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += us.Request.TestObjectives + "<br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            //body += "Approvals <br/>";
            //var approvals = us.Request.CML_Approvals;
            //foreach ( var ap in approvals )
            //{
            //    body += "<strong>" + ap.CML_User.DisplayName + " " + ap.DateActioned.Value.ToShortDateString() + "</strong><br/>";
            //    body += "Outcome: " + ap.Outcome + "<br/>";
            //    body += ap.Comments + "<br/>";

            //}
            //body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }

        private void SendApprovalRequired( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} approval required.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been sent to you for approval. <br/>";
            body += "<strong> Approval via email is not permitted in this application.</strong>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            //body += "Approvals <br/>";
            //var approvals = us.Request.CML_Approvals;
            //foreach ( var ap in approvals )
            //{
            //    if ( ap.DateActioned.HasValue )
            //    {
            //        body += "<strong>" + ap.CML_User.DisplayName + " " + ap.DateActioned.Value.ToShortDateString() + "</strong><br/>";
            //        body += "Outcome: " + ap.Outcome + "<br/>";
            //        body += ap.Comments + "<br/>";
            //    }

            //}
            //body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }

        private void SendApprovalNotice( CMLEntities db, Notification us )
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.From = new MailAddress( "exide.admin@exide.com", "CML Admin" );
            message.To.Add( new MailAddress( db.CML_User.Find( us.Recipient ).Email ) );
            message.Subject = string.Format( "CML Request {0} has been sent for approval.", us.Request.CMLNumber );

            string body = string.Empty;
            body += "<hr style='color:orange;'/>";
            body += "Request " + us.Request.CMLNumber + " has been sent for approval. <br/>";
            body += "<a href='" + GetPath() + "Request/Edit/" + us.RequestID + "'> Click here to go to the request</a><br/>";
            body += "<hr style='color:orange;'/>";

            var notes = db.Notes.Where( a => a.ParentID == us.TestID && a.NoteType.Equals( "T" ) );
            foreach ( var n in notes )
            {
                body += "<strong>" + n.CreatedOn.Value.ToShortDateString() + " " + n.CreatedBy + "</strong><br>";
                body += n.NoteText + "<br/>";
                body += "<br/>";
            }
            body += "<hr/>";
            body += "Approvals <br/>";
            var approvals = us.Request.CML_Approvals;
            foreach ( var ap in approvals )
            {
                if ( ap.DateActioned.HasValue )
                {
                    string date = ap.DateActioned.HasValue ? ap.DateActioned.Value.ToShortDateString() : string.Empty;
                    body += "<strong>" + ap.CML_User.DisplayName + " " + date + "</strong><br/>";
                    body += "Outcome: " + ap.Outcome + "<br/>";
                    body += ap.Comments + "<br/>";
                }

            }
            body += "<hr style='color:orange;'/>";

            message.Body = body;
            SmtpClient client = new SmtpClient( GetHost() );

            client.Send( message );
        }
    }
}