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

        public static List<Role> GetRolesForOrganizationCreatableByRole(long organizationId, int creatorRoleId)
        {
            using (var db = new CCEntities())
            {
                if (creatorRoleId == Ids.Admin && organizationId == Organization.Ids.CCIS)
                    return db.Roles.ToList();
                else
                    return db.Roles.Where(r => r.RoleId != Ids.Admin).ToList();
            }
        }
    }
}
