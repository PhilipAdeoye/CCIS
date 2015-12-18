﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCData;
using CCData.Infrastructure;
using CCMvc.ViewModels;

namespace CCMvc.Controllers
{
    [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach + "," + Role.Names.Athlete)]
    public class UserController : BaseController
    {
        private CCEntities db = new CCEntities();

        #region Users
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult Users(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var loggedInUserRoleId = GetLoggedInUser().RoleId;

            var model = db.Humen.Where(o => o.OrganizationId == organizationId)
                .Select(u => new UserViewModel
                {
                    HumanId = u.HumanId,
                    OrganizationId = u.OrganizationId,
                    Username = u.Username,
                    FirstName = u.Firstname,
                    LastName = u.Lastname,
                    Email = u.Email,
                    RoleName = u.Role.RoleName,

                    // u can be deleted if u isn't the logged in user && its roleId
                    // is higher than the logged in user's (i.e u's role is subject to that
                    // of the logged in user)
                    CanBeDeleted = u.HumanId != LoggedInUserId && u.RoleId > loggedInUserRoleId
                });

            return PartialView(model);
        } 
        #endregion

        #region Helper Methods

        #region AccessIsAllowed
        private bool AccessIsAllowed(long organizationId)
        {
            return organizationId > 0 && (organizationId == LoggedInUsersOrganizationId || User.IsInRole(Role.Names.Admin));
        }
        #endregion

        #region WorkingOrgId
        // Admins are allowed to access every organization's data
        // Non admins can only access their org's data
        private long WorkingOrgId(long organizationId)
        {
            if (organizationId == 0)
                organizationId = LoggedInUsersOrganizationId;

            if (!User.IsInRole(Role.Names.Admin))
                organizationId = LoggedInUsersOrganizationId;

            return organizationId;
        }
        #endregion

        #region GetLoggedInUser
        protected override Human GetLoggedInUser()
        {
            return db.Humen.Single(u => u.HumanId == LoggedInUserId);
        }
        #endregion

        #region TryDBChange
        protected override void TryDBChange(Action method)
        {
            try
            {
                method();
            }
            catch (DbValidationException ex)
            {
                ex.DbValidationErrors.ToList().ForEach(delegate(DbValidationError error)
                {
                    ModelState.AddModelError(error.Message, error.PropertyName);
                });
            }
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        #endregion
    }
}
