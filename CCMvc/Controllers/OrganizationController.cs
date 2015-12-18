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
    [Authorize(Roles = Role.Names.Admin)]
    public class OrganizationController : BaseController
    {
        private CCEntities db = new CCEntities();

        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region Organizations
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
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("CreateForm");
        }

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

        #region Helper Methods

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
