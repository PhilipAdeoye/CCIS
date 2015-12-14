using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCMvc.ViewModels;
using CCMvc.Infrastructure;
using System.Web.Security;
using CCData;
using Helpers;

namespace CCMvc.Controllers
{
    public class HomeController : Controller
    {
        #region Index
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated && Session["membershipUser"] != null)
                return RedirectUserToHomeByRole();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                CCMembershipProvider membershipProvider = new CCMembershipProvider();
                var ableToValidate = false;

                CCMembershipUser membershipUser = null;
                try
                {
                    membershipUser = membershipProvider.GetUser(model.UserName, model.Password);
                    ableToValidate = true;
                }
                catch (InvalidCredentialsException)
                {
                    ModelState.AddModelError("Error", "Sorry, but the user name or password provided you provided is incorrect.");
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError("Error", "Sorry, but we are having technical problems logging you in. " +
                        "Please try again. If this persists, please contact Payorlogic.");
                    //new ExceptionEmail()
                    //{
                    //    ex = exception,
                    //    Subject = "Unhandled Exception in Payorlogic Portal",
                    //    ExtraData = new List<KeyValuePair<string, string>>()
                    //    {
                    //        new KeyValuePair<string, string>("URL", Request.Url.ToString()),
                    //        new KeyValuePair<string, string>("Query String", Request.QueryString.ToString()),
                    //        new KeyValuePair<string, string>("HTTP Request Headers", string.Join("<br>", Request.Headers.AllKeys.Select(key => key + ": " + Request.Headers[key]).ToArray()))
                    //    }
                    //}.Send();
                }

                if (ableToValidate)
                {
                    Session.RemoveAll();

                    FormsAuthentication.SetAuthCookie(membershipUser.UserName, true);
                    Session.Add("membershipUser", membershipUser);
                    Session.Add("membershipProvider", membershipProvider);

                    return RedirectToStartingPoint(returnUrl);
                }

            }
            return View(model);
        }
        #endregion

        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach + "," + Role.Names.Athlete)]
        public ActionResult Landing()
        {
            return View();
        }

        #region KeepMeLoggedIn
        [HttpGet]
        [Authorize(Roles = Role.Names.Admin + "," + Role.Names.Coach + "," + Role.Names.Athlete)]
        public void KeepMeLoggedIn() { }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session.RemoveAll();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region RedirectToStartingPoint
        private ActionResult RedirectToStartingPoint(string returnUrl = "")
        {
            var user = ((CCMembershipUser)Session["membershipUser"]);

            
            if (MiscFunctions.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectUserToHomeByRole(user.UserName);
        }
        #endregion

        #region RedirectUserToHomeByRole
        private ActionResult RedirectUserToHomeByRole(string userName = "")
        {
            if (String.IsNullOrWhiteSpace(userName))
            {
                if (User.IsInRole(Role.Names.Admin))
                    return RedirectToAction("Landing", "Home");
                else if (User.IsInRole(Role.Names.Coach))
                    return RedirectToAction("Landing", "Home");
                else if (User.IsInRole(Role.Names.Athlete))
                    return RedirectToAction("Landing", "Home");
                else
                    return RedirectToAction("Logout", "Home");
            }
            else
            {
                if (Roles.Provider.IsUserInRole(userName, Role.Names.Admin))
                    return RedirectToAction("Landing", "Home");
                else if (Roles.Provider.IsUserInRole(userName, Role.Names.Coach))
                    return RedirectToAction("Landing", "Home");
                else if (Roles.Provider.IsUserInRole(userName, Role.Names.Athlete))
                    return RedirectToAction("Landing", "Home");
                else
                    return RedirectToAction("Logout", "Home");
            }
        }
        #endregion
    }
}
