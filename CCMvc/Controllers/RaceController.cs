using System;
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
    public class RaceController : BaseController
    {
        private CCEntities db = new CCEntities();

        #region Create
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult Create(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = new RaceCreateViewModel
            {
                OrganizationId = organizationId
            };

            return PartialView("CreateForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult Create(RaceCreateViewModel model)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == model.OrganizationId)
                || !AccessIsAllowed(model.OrganizationId))
                ModelState.AddModelError("Error", "You are unauthorized to create races for this school");

            if (ModelState.IsValid)
            {
                var race = new Race
                {
                    OrganizationId = model.OrganizationId,
                    Description = model.Description,
                    StartedOn = model.StartsAtUTC.Value.ToUniversalTime(),
                    Remarks = model.Remarks,
                    GenderRestriction = model.GenderRestriction,
                    CreatedBy = LoggedInUserId,
                };
                db.Races.AddObject(race);
                TryDBChange(() => db.SaveChanges());
            }

            return PartialView("CreateForm", model);
        } 
        #endregion

        #region UpcomingRaces
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult UpcomingRaces(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = db.Races
                .Where(r => r.OrganizationId == organizationId 
                    && (r.StartedOn > DateTime.UtcNow || !r.CompletedOn.HasValue))
                .Select(r => new RaceViewModel
                {
                    RaceId = r.RaceId,
                    OrganizationId = r.OrganizationId,
                    Description = r.Description,
                    Remarks = r.Remarks,
                    Runners = r.RunnerRaceRecords.Count,
                    StartedOnUTC = r.StartedOn,
                }).OrderByDescending(r => r.RaceId);

            return PartialView(model);
        } 
        #endregion

        #region CompletedRaces
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult CompletedRaces(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = db.Races
                .Where(r => r.OrganizationId == organizationId && r.CompletedOn.HasValue)
                .Select(r => new RaceViewModel
                {
                    RaceId = r.RaceId,
                    OrganizationId = r.OrganizationId,
                    Description = r.Description,
                    Remarks = r.Remarks,
                    Runners = r.RunnerRaceRecords.Count,
                    StartedOnUTC = r.StartedOn,
                    CompletedOnUTC = r.CompletedOn
                })
                .OrderByDescending(r => r.RaceId);

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
