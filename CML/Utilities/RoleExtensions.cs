using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public static class RoleExtensions
    {
        public static bool IsUserInRole(this CMLEntities db, string username,string role)
        {
            var roles = db.CML_Roles.Where(r => r.UserID.ToUpper().Equals(username.ToUpper() ) && r.Role.Equals( role ) ).SingleOrDefault();
            if (roles != null)
                return true;
            else
                return false;

            
        }

        public static String[] GetRoles(this CMLEntities db, string username )
        {
            List<string> roles = db.CML_Roles.Where( a => a.UserID.ToUpper().Equals( username.ToUpper() ) ).Select( a => a.Role ).ToList();
            return roles.ToArray();
        }

        public static string ExtractUsername(string user)
        {
            // TODO: Implement this method
            string temp;
            if (user.Contains("\\"))
            {
                temp = user.Substring(user.LastIndexOf("\\") + 1);
            }
            else
            {
                temp = user;
            }

            return temp;
        }
    }
}