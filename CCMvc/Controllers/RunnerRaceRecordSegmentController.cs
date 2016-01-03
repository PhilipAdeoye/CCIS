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
    public class RunnerRaceRecordSegmentController : BaseController
    {
        private CCEntities db = new CCEntities();

        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        public ActionResult ManageSplits(long runnerRaceRecordId)
        {
            if (!AccessIsAllowed(runnerRaceRecordId))
                return HttpNotFound();

            var model = new ManageSplitsViewModel
            {
                RunnerRaceRecordId = runnerRaceRecordId,
                Splits = RunnerRaceRecordSegment.GetSplitsForRunnerRaceRecord(runnerRaceRecordId)
                    .Select(s => new SplitViewModel { 
                        RunnerRaceRecordSegmentId = s.RunnerRaceRecordSegmentId,
                        ElapsedTimeInSeconds = s.ElapsedTimeInSeconds,
                        IntervalFromPriorSplit = s.IntervalFromPriorSplit,
                    }).ToList()
            };

            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [ValidateAntiForgeryToken]
        public ActionResult ManageSplits(ManageSplitsViewModel model)
        {
            if (!AccessIsAllowed(model.RunnerRaceRecordId))
                ModelState.AddModelError("Error", "You are not authorized to do this. Go away!");

            if (ModelState.IsValid)
            {
                RunnerRaceRecordSegment.DeleteSegmentsByRunnerRaceRecordId(model.RunnerRaceRecordId);
     
                foreach (var split in model.Splits)
                {
                    db.RunnerRaceRecordSegments.AddObject(new RunnerRaceRecordSegment
                    {
                        CreatedBy = LoggedInUserId,
                        ElapsedTimeInSeconds = split.ElapsedTimeInSeconds,
                        RunnerRaceRecordId = model.RunnerRaceRecordId
                    });
                }
                TryDBChange(() => db.SaveChanges());
                if(ModelState.IsValid)
                    return Json(new { });
            }
            return Json(new { Errors = ModelState.Errors() });            
        }

        #region Helper Methods

        #region AccessIsAllowed
        private bool AccessIsAllowed(long runnerRaceRecordId)
        {
            var organizationId = db.RunnerRaceRecords.Where(rr => rr.RunnerRaceRecordId == runnerRaceRecordId)
                .Select(rr => rr.Race.OrganizationId).SingleOrDefault();

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
