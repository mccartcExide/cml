using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public static class DashboardExtensions
    {
        //public static void GetCounts(this CMLEntities db, string type, int userid, string name )
        //{
        //    if()

        //}

        public static int GetOpenRequests( this CMLEntities db, int userid, string name)
        {
            return db.Requests.Where( a => a.RequestedBy == userid || a.CreatedBy.Equals( name ) && (a.StatusID >= 4 || a.StatusID == 8)).Count();

        }
        public static int GetOpenRequestsForApproval( this CMLEntities db, int userid, string name )
        {

            return db.Requests.Where( a => (a.RequestedBy == userid || a.CreatedBy.Equals( name )) && ( a.StatusID == 1  ) ).Count();

        }


        public static int GetRequestsAssignedToMe( this CMLEntities db, int userid )
        {
            return db.Requests.Where( a => a.AssignedTo == userid ).Count();
        }

        public static int GetTestNotStartedAssigned(this CMLEntities db, int userid)
        {
            return db.Tests.Where( a => a.AssignedTo == userid && a.StatusID == Structs.Statuses.Open ).Count();

        }
        public static int GetTestStartedAssigned( this CMLEntities db, int userid )
        {
            return db.Tests.Where( a => a.AssignedTo == userid && a.StatusID == Structs.Statuses.Testing ).Count();

        }
        public static int GetApprovalsCount(this CMLEntities db, int userid )
        {
            return db.CML_Approvals.Where( a => a.UserID == userid && ( a.Outcome == null || a.Outcome.Equals( string.Empty ) ) ).Count();
        }

    }
}