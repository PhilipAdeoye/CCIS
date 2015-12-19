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
    public class ErrorController : BaseController
    {
        private CCEntities db = new CCEntities();

        #region Error
        public ActionResult Error()
        {
            var model = new ErrorViewModel()
            {
                Email = GetLoggedInUser().Email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Error(ErrorViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: Something useful with the user input
                return RedirectToAction("Index", "Home");
            }
            return View(model);
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
