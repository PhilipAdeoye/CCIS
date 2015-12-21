using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCData;
using CCData.Infrastructure;
using CCMvc.ViewModels;
using Helpers;

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
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
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

        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpGet]
        public ActionResult Create(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = new UserCreateViewModel
            {
                OrganizationId = organizationId,
                RoleList = new SelectList(Role.GetRolesCreatableByRole(GetLoggedInUser().RoleId), "RoleId", "RoleName"),
            };

            return PartialView("CreateForm", model);
        }

        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateViewModel model)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == model.OrganizationId)
                || !AccessIsAllowed(model.OrganizationId))
                ModelState.AddModelError("Error", "You're not authorized to add users");

            if (GetLoggedInUser().RoleId > model.RoleId)
                ModelState.AddModelError("RoleId", "You're not authorized to add a user in this role");

            if (ModelState.IsValid)
            {
                var user = new Human
                {
                    OrganizationId = model.OrganizationId,
                    Username = model.Username,
                    Password = PasswordHash.CreateHash(model.Password),
                    RoleId = model.RoleId,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Middlename = model.Middlename,
                    GraduationYear = model.GraduationYear,
                    CreatedBy = LoggedInUserId,
                };
                db.Humen.AddObject(user);
                TryDBChange(() => db.SaveChanges());
            }

            model.RoleList = new SelectList(Role.GetRolesCreatableByRole(GetLoggedInUser().RoleId), "RoleId", "RoleName");

            return PartialView("CreateForm", model);
        }

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
