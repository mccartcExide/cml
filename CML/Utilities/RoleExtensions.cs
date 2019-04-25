using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public static class RoleExtensions
    {
        public static bool IsUserInRole(this CMLEntities db, string username)
        {
            //var roles = db.CML_Roles.Where(r => r.UserID.ToUpper().Equals(username.ToUpper())).SingleOrDefault();
            //if (roles != null)
            //    return true;
            //else
            //    return false;

            return true;
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