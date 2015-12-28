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
    [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
    public class OrganizationController : BaseController
    {
        private CCEntities db = new CCEntities();

        #region Index
        [Authorize(Roles = Role.Names.Admin)]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Organizations
        [Authorize(Roles = Role.Names.Admin)]
        [HttpGet]
        public ActionResult Organizations()
        {
            var model = db.Organizations.Select(o => new OrganizationVM
            {
                OrganizationId = o.OrganizationId,
                Name = o.Name,
                MascotName = o.MascotName,
                MascotImageFileLocation = o.MascotImageFileLocation,
                Tagline = o.Tagline
            }).OrderBy(o => o.OrganizationId);

            return PartialView(model);
        }
        #endregion

        #region Create
        [Authorize(Roles = Role.Names.Admin)]
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("CreateForm");
        }

        [Authorize(Roles = Role.Names.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationVM model)
        {
            if (ModelState.IsValid)
            {
                var org = new Organization
                {
                    Name = model.Name,
                    MascotName = model.MascotName,
                    Tagline = model.Tagline,
                    CreatedBy = LoggedInUserId,
                    CreatedOn = DateTime.UtcNow,
                };
                db.Organizations.AddObject(org);
                TryDBChange(() => db.SaveChanges());
            }

            return PartialView("CreateForm", model);
        }
        #endregion

        #region OrganizationDetail
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpGet]
        public ActionResult OrganizationDetail(long organizationId = 0)
        {
            organizationId = WorkingOrgId(organizationId);

            var model = db.Organizations.Where(o => o.OrganizationId == organizationId)
                .Select(o => new OrganizationVM
                {
                    Name = o.Name,
                    OrganizationId = o.OrganizationId
                })
                .SingleOrDefault();

            return View(model);
        }
        #endregion

        #region Edit
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpGet]
        public ActionResult Edit(long organizationId)
        {
            if (!db.Organizations.Any(o => o.OrganizationId == organizationId)
                || !AccessIsAllowed(organizationId))
                return HttpNotFound();

            var model = db.Organizations.Where(o => o.OrganizationId == organizationId)
                .Select(o => new OrganizationVM
                {
                    OrganizationId = o.OrganizationId,
                    Name = o.Name,
                    MascotName = o.MascotName,
                    Tagline = o.Tagline,
                })
                .SingleOrDefault();

            return PartialView("EditForm", model);
        }

        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganizationVM model)
        {
            if (!AccessIsAllowed(model.OrganizationId))
                ModelState.AddModelError("Error", "Sorry, but you're not authorized to modify this school");

            if (ModelState.IsValid)
            {
                var org = db.Organizations.SingleOrDefault(o => o.OrganizationId == model.OrganizationId);
                if (org == null)
                    return HttpNotFound();

                org.Name = model.Name;
                org.MascotName = model.MascotName;
                org.Tagline = model.Tagline;
                org.ModifiedBy = LoggedInUserId;

                TryDBChange(() => db.SaveChanges());
            }
            return PartialView("EditForm", model);
        } 
        #endregion

        #region Helper Methods

        #region AccessIsAllowed
        private bool AccessIsAllowed(long organizationId)
        {
            return organizationId > 0 && (organizationId == LoggedInUsersOrganizationId || User.IsInRole(Role.Names.Admin) );
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
