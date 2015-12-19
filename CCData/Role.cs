using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCData
{
    public partial class Role
    {
        public class Ids
        {
            public const int Admin = 1;
            public const int Coach = 2;
            public const int Athlete = 3;
        }

        public class Names
        {
            public const string Admin = "Admin";
            public const string Coach = "Coach";
            public const string Athlete = "Athlete";
        }

        public static IEnumerable<Role> GetRolesCreatableByRole(int roleId)
        {
            using (var db = new CCEntities())
            {
                if (roleId == Ids.Admin)
                    return db.Roles;
                else
                    return db.Roles.Where(r => r.RoleId != Ids.Admin);
            }
        }
    }
}
