using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCData;
using CCData.Infrastructure;
using CCMvc.Infrastructure;
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
                .Where(r => r.OrganizationId == organizationId && !r.CompletedOn.HasValue)
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

        #region RaceDetail
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult RaceDetail(long raceId, long organizationId)
        {
            if (!db.Races.Any(r => r.RaceId == raceId && r.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return View("NotFound");

            var model = db.Races.Where(r => r.RaceId == raceId).Select(r => new RaceViewModel
            {
                RaceId = raceId,
                OrganizationId = organizationId,
                Description = r.Description,
                CompletedOnUTC = r.CompletedOn,
                OrganizationName = r.Organization.Name,
            }).SingleOrDefault();

            return View(model);
        } 
        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult Edit(long raceId, long organizationId)
        {
            if (!db.Races.Any(r => r.RaceId == raceId && r.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = db.Races.Where(r => r.RaceId == raceId).Select(r => new RaceEditViewModel
            {
                RaceId = raceId,
                OrganizationId = organizationId,
                Description = r.Description,
                GenderRestriction = r.GenderRestriction,
                StartTimeInUTC = r.StartedOn,
                CompletedTimeInUTC = r.CompletedOn,
                Remarks = r.Remarks,
                NumberOfRunners = r.RunnerRaceRecords.Count,
            }).SingleOrDefault();

            return PartialView("EditForm", model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RaceEditViewModel model)
        {
            if (!db.Races.Any(r => r.RaceId == model.RaceId && r.OrganizationId == model.OrganizationId)
                || !AccessIsAllowed(model.OrganizationId))
                ModelState.AddModelError("Error", "You are unauthorized to make changes to this race");

            if (ModelState.IsValid)
            {
                var race = db.Races.Single(r => r.RaceId == model.RaceId);
                if (race.CompletedOn.HasValue)
                {
                    race.Description = model.Description;
                    race.Remarks = model.Remarks;
                }
                else
                {
                    race.Description = model.Description;
                    race.StartedOn = model.StartTimeInUTC.Value.ToUniversalTime();
                    race.Remarks = model.Remarks;
                    if (!race.RunnerRaceRecords.Any())
                        race.GenderRestriction = model.GenderRestriction;
                }
                race.ModifiedBy = LoggedInUserId;
                TryDBChange(() => db.SaveChanges());
            }

            return PartialView("EditForm", model);
        } 
        #endregion


        #region MarkAsCompleted
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult MarkAsCompleted(long raceId, long organizationId)
        {
            if (!db.Races.Any(r => r.RaceId == raceId && r.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                ModelState.AddModelError("Error", "You are not authorized to modify this race");
            else
            {
                var race = db.Races.Single(r => r.RaceId == raceId);
                race.CompletedOn = DateTime.UtcNow;
                TryDBChange(() => db.SaveChanges());
            }

            if (ModelState.IsValid)
                return Json(new { });

            return Json(new { Errors = ModelState.Errors() });
        } 
        #endregion

        #region Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult Cancel(long raceId, long organizationId)
        {
            if (!db.Races.Any(r => r.RaceId == raceId && r.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                ModelState.AddModelError("Error", "You are not authorized to cancel this race");
            else if (db.Races.Where(r => r.RaceId == raceId).Select(r => r.CompletedOn).SingleOrDefault().HasValue)
                ModelState.AddModelError("Error", "You may not cancel a completed race");
            else
                TryDBChange(() => Race.DeleteWithId(raceId));

            if (ModelState.IsValid)
                return Json(new { });

            return Json(new { Errors = ModelState.Errors() });
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
