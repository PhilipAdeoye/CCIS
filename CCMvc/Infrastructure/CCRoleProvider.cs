using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCData;
using System.Web.Security;

namespace CCMvc.Infrastructure
{
    public class CCRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            using (var db = new CCEntities())
            {
                string[] array;
                User user = db.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
                var roles = (from ur in db.Roles
                             join u in db.Users on ur.RoleId equals u.RoleId
                             where u.Username == username
                             select ur.RoleName);

                if (roles != null)
                {
                    array = roles.ToArray();
                    return roles.ToArray();
                }
                else
                    return new string[] { };
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (var db = new CCEntities())
            {
                return (from r in db.Roles
                        join u in db.Users on r.RoleId equals u.RoleId
                        where r.RoleName.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
                        select u.Username).ToArray();

            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = new CCEntities())
            {
                // Find user by username
                User user = db.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                // Select user role by username, compare
                var role = (from ur in db.Roles
                            join u in db.Users on ur.RoleId equals u.RoleId
                            where u.Username == username
                            select ur.RoleName);

                if (user != null)
                    return role.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                else
                    return false;
            }
        }

        public override bool RoleExists(string roleName)
        {
            using (var db = new CCEntities())
            {
                return db.Roles.Any(r => r.RoleName.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        #region Not Implemented Overrides Section
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
        #endregion
    }
}