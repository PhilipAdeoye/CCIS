using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Forloop.HtmlHelpers.Clone;
using CCMvc.Infrastructure;
using CCMvc.Controllers;
using Helpers;

namespace CCMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const int InternalServerError = 500;
        private const int Unauthorized = 401;

        #region Application_Start
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.DefaultBinder = new StringTrimmingModelBinder();

            ScriptContext.ScriptPathResolver = Scripts.Render;
        } 
        #endregion

        #region Application_Error
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                var routeData = new RouteData();

                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("exception", ex);
                routeData.Values.Add("statusCode", InternalServerError);
                routeData.Values.Add("action", "Error");

                var loggedInUser = (CCMembershipUser)Session["membershipUser"];

                // Only send the email if the exception wasn't caused by a wrong route
                // eg. a non-existent controller, or a non-public (or non-existent) method  on a controller

                /* if (!(exception.GetType() == typeof(HttpException) && exception.Message.Contains("was not found")))
                 {*/
                // Send error email at this point
                Logger.LogException(ex, "Unhandled Exception in CCMvc");
                new ExceptionEmail()
                {
                    ex = ex,
                    Subject = "Unhandled Exception in CCMvc",
                    ExtraData = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("URL", Request.Url.ToString()),
                            new KeyValuePair<string, string>("Query String", Request.QueryString.ToString()),
                            loggedInUser != null ? 
                                new KeyValuePair<string, string>("UserId", loggedInUser.ProviderUserKey.ToString()) :
                                new KeyValuePair<string, string>("UserId", 0.ToString()),
                            loggedInUser != null ? 
                                new KeyValuePair<string, string>("Email", loggedInUser.Email) :
                                new KeyValuePair<string, string>("Email", ""),
                            new KeyValuePair<string, string>("HTTP Request Headers", string.Join("<br>", Request.Headers.AllKeys.Select(key => key + ": " + Request.Headers[key]).ToArray()))
                        }
                }.Send();
                // }

                Server.ClearError();

                if (!new HttpRequestWrapper(HttpContext.Current.Request).IsAjaxRequest())
                {
                    IController controller = new ErrorController();
                    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                    Response.End();
                }
            }
        }
        #endregion

        #region Application_PreSendRequestHeaders
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Set("Server", "CCCP Resources");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
        #endregion

        #region Application_EndRequest
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Context.Items["AjaxSessionExpired"] is bool && (bool)Context.Items["AjaxSessionExpired"])
            {
                Context.Response.StatusCode = Unauthorized;
                Context.Response.End();
            }
            if (Context.Items["AjaxError"] is bool && (bool)Context.Items["AjaxError"])
            {
                Context.Response.StatusCode = InternalServerError;
                Context.Response.End();
            }
        }
        #endregion
    }
}