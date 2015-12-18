using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCMvc.Infrastructure
{
    public static class MvcHtmlExtensions
    {
        #region MenuLink
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string itemText, string actionName, string controllerName, MvcHtmlString[] childElements = null)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string finalHtml;
            var linkBuilder = new TagBuilder("a");
            var liBuilder = new TagBuilder("li");

            // If this is a dropdown menu
            if (childElements != null && childElements.Length > 0)
            {
                // Build up the link tag
                linkBuilder.MergeAttribute("href", "#");
                linkBuilder.AddCssClass("dropdown-toggle");
                linkBuilder.InnerHtml = itemText + " <span class=\"caret\"></span>";
                linkBuilder.MergeAttribute("data-toggle", "dropdown");
                linkBuilder.MergeAttribute("role", "button");
                linkBuilder.MergeAttribute("aria-expanded", "false");

                // Build up the ul tag
                var ulBuilder = new TagBuilder("ul");
                ulBuilder.AddCssClass("dropdown-menu");
                ulBuilder.MergeAttribute("role", "menu");

                foreach (var item in childElements)
                {
                    ulBuilder.InnerHtml += item.ToString() + "\n";
                }

                // Build the li tag that the link and list go into
                liBuilder.InnerHtml = linkBuilder.ToString() + "\n" + ulBuilder.ToString();
                liBuilder.AddCssClass("dropdown");

                if (controllerName == "Admin"
                    && (currentController == "Organization"))
                {
                    liBuilder.AddCssClass("active");
                }
                //else if (controllerName == "Applicants"
                //    && (currentController == "ApplicantVisits"
                //    || currentController == "InsuranceFinder"
                //    || currentController == "BulkSubmission"))
                //{
                //    liBuilder.AddCssClass("active");
                //}

                finalHtml = liBuilder.ToString() + ulBuilder.ToString();
            }
            else
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection);
                linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName));
                linkBuilder.InnerHtml = itemText;
                liBuilder.InnerHtml = linkBuilder.ToString();
                if (controllerName == currentController)
                {
                    liBuilder.AddCssClass("active");
                }

                finalHtml = liBuilder.ToString();
            }

            return new MvcHtmlString(finalHtml);
        }
        #endregion
    }
}