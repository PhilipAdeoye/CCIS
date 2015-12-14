using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCMvc.Infrastructure;
using CCData;
using System.IO;
using System.Web.Routing;

namespace CCMvc.Controllers
{
    public class BaseController : Controller
    {
        #region Accessors
        public int? LoggedInUserId
        {
            get
            {
                return Convert.ToInt32(((CCMembershipUser)Session["membershipUser"]).ProviderUserKey);
            }
        }

        public CCMembershipProvider CurrentMembershipProvider
        {
            get { return (CCMembershipProvider)Session["membershipProvider"]; }
        }        
        #endregion

        #region RenderPartialViewToString
        public static string RenderPartialViewToString(Controller thisController, string viewName, object model)
        {
            // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
            thisController.ViewData.Model = model;

            // initialize a string builder
            using (StringWriter sw = new StringWriter())
            {
                // find and load the view or partial view, pass it through the controller factory
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

                // Render it
                viewResult.View.Render(viewContext, sw);

                // Return partial view as a string
                return sw.ToString();
            }
        }
        #endregion

        #region OnActionExecuting
        // ---------------------------------------------------------------------
        // Name: OnActionExecuting
        // Abstract: If user session is expired or user is not authenticated - redirect to logout view
        // ---------------------------------------------------------------------
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;

            base.OnActionExecuting(filterContext);

            var membershipUser = (CCMembershipUser)Session["membershipUser"];

            // If the user is no longer authenticated or is just null
            if (!User.Identity.IsAuthenticated || membershipUser == null)
            {
                // If the request was made via ajax, set the AjaxSessionExpired. See Global.asax
                // Application_EndRequest and Site.js for more details
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.HttpContext.Items["AjaxSessionExpired"] = true;

                // If a regular request, then kick the user out
                else
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Logout" } });
            }

        }
        #endregion

        #region OnException
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.HttpContext.Items["AjaxError"] = true;
        }
        #endregion

        protected virtual void TryDBChange(Action method)
        {
            throw new NotImplementedException("Method Not Implemented: TryDBChange()");
        }

        protected virtual Human GetLoggedInUser()
        {
            throw new NotImplementedException("Method Not Implemented: GetLoggedInUser()");
        }
    }
}
