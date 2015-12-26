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

        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult Create(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = new RaceViewModel
            {
                OrganizationId = organizationId
            };

            return PartialView("CreateForm", model);
        }

        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult UpcomingRaces(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            //var model = db.Races
            //    .Where(r => r.OrganizationId == organizationId && r.StartedOn > DateTime.UtcNow)
            //    .Select(r => new RaceViewModel
            //    {
            //        RaceId = r.RaceId,
            //        OrganizationId = r.OrganizationId,
            //        Description = r.Description,
            //        Remarks = r.Remarks,
            //        Runners = r.RunnerRaceRecords.Count,
            //        StartedOnUTC = r.StartedOn,
            //        CompletedOnUTC = r.CompletedOn
            //    });

            var model = new List<RaceViewModel>() {
                new RaceViewModel {
                    RaceId = 1,
                    OrganizationId = 1,
                    Description = "First Race",
                    Remarks = "Run Run Away!",
                    Runners = 3,
                    StartedOnUTC = DateTime.UtcNow,
                }
            };

            return PartialView(model);
        }

        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult CompletedRaces(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            //var model = db.Races
            //    .Where(r => r.OrganizationId == organizationId && r.CompletedOn.HasValue)
            //    .Select(r => new RaceViewModel
            //    {
            //        RaceId = r.RaceId,
            //        OrganizationId = r.OrganizationId,
            //        Description = r.Description,
            //        Remarks = r.Remarks,
            //        Runners = r.RunnerRaceRecords.Count,
            //        StartedOnUTC = r.StartedOn,
            //        CompletedOnUTC = r.CompletedOn
            //    })
            //    .OrderByDescending(r => r.StartedOnUTC);

            var model = new List<RaceViewModel>() {
                new RaceViewModel {
                    RaceId = 1,
                    OrganizationId = 1,
                    Description = "Completed Race",
                    Remarks = "Jimmy in First Place",
                    Runners = 7,
                    StartedOnUTC = DateTime.Parse("01/24/2012 4:42:34 PM"),
                    CompletedOnUTC = DateTime.Parse("01/24/2012 5:30:56 PM")
                }
            };

            return PartialView(model);
        }

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
