using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCData;
using CCData.Infrastructure;
using CCMvc.ViewModels;
using System.Configuration;
using System.Net.Mail;
using Helpers;

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
                using (MailMessage msg = new MailMessage(
                ConfigurationManager.AppSettings[ConfigKeys.SendExceptionEmailsFrom],
                ConfigurationManager.AppSettings[ConfigKeys.SendExceptionEmailsTo]))
                {
                    msg.Subject = "[CCCP] User Feedback on Unhandled Exception";
                    msg.Body = "The user with the email address " + model.Email +
                        " sent the following: " + model.Message;

                    using (SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings[ConfigKeys.MailServer]))
                    {
                        mailClient.Send(msg);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        #endregion

        #region Helper Methods

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
