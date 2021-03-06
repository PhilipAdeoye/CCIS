﻿using System;
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

            var model = db.Users.Where(o => o.OrganizationId == organizationId)
                .Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    OrganizationId = u.OrganizationId,
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Email = u.Email,
                    RoleName = u.Role.RoleName,
                    EligibleForRaces = u.EligibleForRaces
                });

            return PartialView(model);
        } 
        #endregion

        #region Create
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
                RoleList = new SelectList(Role.GetRolesForOrganizationCreatableByRole(organizationId, GetLoggedInUser().RoleId),
                    "RoleId", "RoleName"),
                RunnerClassifications = new SelectList(db.RunnerClassifications.ToList(), 
                    "RunnerClassificationId", "RunnerClassificationName"),
                VarsityLevels = new SelectList(db.VarsityLevels.ToList(), "VarsityLevelId", "VarsityLevelName"),
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
                ModelState.AddModelError("Error", "You are not authorized to add users");

            if (GetLoggedInUser().RoleId > model.RoleId)
                ModelState.AddModelError("RoleId", "You are not authorized to add a user in this role");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    OrganizationId = model.OrganizationId,
                    Username = model.Username,
                    Password = PasswordHash.CreateHash(model.Password),
                    RoleId = model.RoleId,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Middlename = model.Middlename,
                    Gender = model.Gender,
                    GraduationYear = model.GraduationYear,
                    EligibleForRaces = model.EligibleForRaces,
                    DefaultVarsityLevelId = model.DefaultVarsityLevelId.HasValue ? (int?)model.DefaultVarsityLevelId.Value : null,
                    DefaultRunnerClassificationId = model.DefaultRunnerClassificationId.HasValue ? 
                        (int?)model.DefaultRunnerClassificationId : null,
                    CreatedBy = LoggedInUserId,
                };
                db.Users.AddObject(user);
                TryDBChange(() => db.SaveChanges());
            }

            model.RoleList = new SelectList(Role.GetRolesForOrganizationCreatableByRole(model.OrganizationId, GetLoggedInUser().RoleId),
                    "RoleId", "RoleName");
            model.RunnerClassifications = new SelectList(db.RunnerClassifications.ToList(),
                    "RunnerClassificationId", "RunnerClassificationName");
            model.VarsityLevels = new SelectList(db.VarsityLevels.ToList(), "VarsityLevelId", "VarsityLevelName");

            return PartialView("CreateForm", model);
        } 
        #endregion

        #region Edit
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpGet]
        public ActionResult Edit(long userId, long organizationId)
        {
            if (!db.Users.Any(u => u.UserId == userId && u.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var user = db.Users.Single(u => u.UserId == userId);
            var model = new UserEditViewModel
            {
                UserId = user.UserId,
                OrganizationId = user.OrganizationId,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Middlename = user.Middlename,
                Gender = user.Gender,
                Email = user.Email,
                GraduationYear = user.GraduationYear,
                EligibleForRaces = user.EligibleForRaces,
                DefaultVarsityLevelId = user.DefaultVarsityLevelId,
                DefaultRunnerClassificationId = user.DefaultRunnerClassificationId,
                RoleId = user.RoleId,
                RoleList = new SelectList(Role.GetRolesForOrganizationCreatableByRole(organizationId, GetLoggedInUser().RoleId)
                    , "RoleId", "RoleName"),
                RunnerClassifications = new SelectList(db.RunnerClassifications.ToList(),
                    "RunnerClassificationId", "RunnerClassificationName"),
                VarsityLevels = new SelectList(db.VarsityLevels.ToList(), "VarsityLevelId", "VarsityLevelName"),
            };

            return PartialView("EditForm", model);
        }

        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel model)
        {
            if (!db.Users.Any(u => u.UserId == model.UserId && u.OrganizationId == model.OrganizationId)
                || !AccessIsAllowed(model.OrganizationId))
                ModelState.AddModelError("Error", "You are not authorized to modify this user");

            if (GetLoggedInUser().RoleId > model.RoleId)
                ModelState.AddModelError("RoleId", "You are not authorized to modify a user in this role");

            if (ModelState.IsValid)
            {
                var user = db.Users.Single(u => u.UserId == model.UserId);
                user.Firstname = model.Firstname;
                user.Lastname = model.Lastname;
                user.Middlename = model.Middlename;
                user.Gender = model.Gender;
                user.Email = model.Email;
                user.GraduationYear = model.GraduationYear;
                user.EligibleForRaces = model.EligibleForRaces;
                user.RoleId = model.RoleId;
                user.DefaultVarsityLevelId = model.DefaultVarsityLevelId.HasValue ? (int?)model.DefaultVarsityLevelId.Value : null;
                user.DefaultRunnerClassificationId = model.DefaultRunnerClassificationId.HasValue ?
                        (int?)model.DefaultRunnerClassificationId : null;
                user.ModifiedBy = LoggedInUserId;

                TryDBChange(() => db.SaveChanges());
            }

            model.RoleList = new SelectList(Role.GetRolesForOrganizationCreatableByRole(model.OrganizationId, GetLoggedInUser().RoleId),
                    "RoleId", "RoleName");
            model.RunnerClassifications = new SelectList(db.RunnerClassifications.ToList(),
                    "RunnerClassificationId", "RunnerClassificationName");
            model.VarsityLevels = new SelectList(db.VarsityLevels.ToList(), "VarsityLevelId", "VarsityLevelName");

            return PartialView("EditForm", model);
        }  
        #endregion

        #region Helper Methods

        #region AccessIsAllowed
        private bool AccessIsAllowed(long organizationId)
        {
            return organizationId > 0 && (organizationId == LoggedInUsersOrganizationId || User.IsInRole(Role.Names.Admin));
        }
        #endregion

        #region GetLoggedInUser
        protected override User GetLoggedInUser()
        {
            return db.Users.Single(u => u.UserId == LoggedInUserId);
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
                    ModelState.AddModelError(error.PropertyName, error.Message);
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
