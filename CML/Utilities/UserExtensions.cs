using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public static class UserExtensions
    {
        public static CML_User GetUserFromUsername(this CMLEntities db, string user )
        {
            var name = Utilities.Instance.StripDomain( user );

            return db.CML_User.Where( a => a.UserID.Equals( name ) ).First();

        }

        public static CML_User GetUserFromID(this CMLEntities db, int id)
        {
            return db.CML_User.Find( id );
        }
    }
}