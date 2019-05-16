using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CML.Models;
using CML.Utilities;

namespace CML.Authorize
{
    public class CMLRoleProvider : System.Web.Security.RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using(CMLEntities db = new CMLEntities() )
            {
                return db.GetRoles( username );
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            string user = ExtractUsername(username);
            using(CMLEntities db = new CMLEntities())
            {
                return db.IsUserInRole(user,roleName);
            }

        }
      
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        private string ExtractUsername(string user)
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