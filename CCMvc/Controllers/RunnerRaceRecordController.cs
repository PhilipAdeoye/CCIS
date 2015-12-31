using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCData.Infrastructure;
using CCMvc.Infrastructure;
using CCData;
using CCMvc.ViewModels;

namespace CCMvc.Controllers
{
    [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
    public class RunnerRaceRecordController : BaseController
    {
        private CCEntities db = new CCEntities();

        #region ManageRunners
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult ManageRunners(long raceId, long organizationId)
        {
            if (!db.Races.Any(r => r.RaceId == raceId && r.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = GetManageRunnersViewModel(raceId, organizationId);

            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRunners(ManageRunnersViewModel model)
        {
            if (!db.Races.Any(r => r.RaceId == model.RaceId && r.OrganizationId == model.OrganizationId)
                || !AccessIsAllowed(model.OrganizationId))
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                // Edit existing runners
                var existingRunnerIds = model.Runners.Where(r => r.RunnerRaceRecordId.HasValue && r.UserIsEnrolled)
                    .Select(r => r.RunnerRaceRecordId);

                var existingRunners = (from runner in db.RunnerRaceRecords
                                       join runnerId in existingRunnerIds on runner.RunnerRaceRecordId equals runnerId
                                       select runner);

                foreach (var runner in existingRunners)
                {
                    runner.VarsityLevelId = model.Runners.Where(r => r.RunnerRaceRecordId == runner.RunnerRaceRecordId).Select(r => r.VarsityLevelId.Value).FirstOrDefault();
                    runner.RunnerClassificationId = model.Runners.Where(r => r.RunnerRaceRecordId == runner.RunnerRaceRecordId).Select(r => r.RunnerClassificationId.Value).FirstOrDefault();
                }

                // Add newly enrolled runners
                foreach (var modelRunner in model.Runners.Where(r => !r.RunnerRaceRecordId.HasValue && r.UserIsEnrolled))
                {
                    db.RunnerRaceRecords.AddObject(new RunnerRaceRecord
                    {
                        RaceId = model.RaceId,
                        CreatedBy = LoggedInUserId,
                        RunnerClassificationId = modelRunner.RunnerClassificationId.Value,
                        UserId = modelRunner.UserId,
                        VarsityLevelId = modelRunner.VarsityLevelId.Value,
                    });
                }

                // Unenroll runners
                RunnerRaceRecord.UnEnrollRunners(model.Runners.Where(r => r.RunnerRaceRecordId.HasValue && !r.UserIsEnrolled)
                    .Select(r => r.RunnerRaceRecordId.Value));

                TryDBChange(() => db.SaveChanges());
                if (ModelState.IsValid)
                {
                    ModelState.Clear();
                    model = GetManageRunnersViewModel(model.RaceId, model.OrganizationId);
                }
            }
            else
            {
                var varsityLevels = new SelectList(db.VarsityLevels.ToList(), "VarsityLevelId", "VarsityLevelName");
                var runnerClassifications = new SelectList(db.RunnerClassifications.ToList(), "RunnerClassificationId", "RunnerClassificationName");

                foreach (var modelRunner in model.Runners)
                {
                    modelRunner.VarsityLevels = varsityLevels;
                    modelRunner.RunnerClassifications = runnerClassifications;
                }
            }

            return PartialView(model);
        } 
        #endregion

        #region Helper Methods

        #region GetManageRunnersViewModel
        private ManageRunnersViewModel GetManageRunnersViewModel(long raceId, long organizationId)
        {
            var varsityLevels = new SelectList(db.VarsityLevels.ToList(), "VarsityLevelId", "VarsityLevelName");
            var runnerClassifications = new SelectList(db.RunnerClassifications.ToList(), "RunnerClassificationId", "RunnerClassificationName");

            return new ManageRunnersViewModel
            {
                RaceId = raceId,
                OrganizationId = organizationId,
                Runners = RunnerRaceRecord.GetRunnerEnrolmentData(raceId, organizationId)
                    .Select(red => new RunnerViewModel
                    {
                        UserId = red.UserId,
                        RunnerRaceRecordId = red.RunnerRaceRecordId,
                        UserIsEnrolled = red.UserIsEnrolled,
                        RaceIsComplete = red.RaceIsComplete,
                        Name = red.Name,
                        VarsityLevelId = red.VarsityLevelId,
                        RunnerClassificationId = red.RunnerClassificationId,
                        VarsityLevels = varsityLevels,
                        RunnerClassifications = runnerClassifications,
                    }).ToList()
            };
        } 
        #endregion

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
