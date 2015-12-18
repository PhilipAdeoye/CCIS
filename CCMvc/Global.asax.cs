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

namespace CCMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const int InternalServerError = 500;
        private const int Unauthorized = 401;

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