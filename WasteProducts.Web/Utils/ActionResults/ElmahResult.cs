﻿using System;
using System.Web.Mvc;
using Elmah;

namespace WasteProducts.Web.Utils.ActionResults
{
    /// <summary>
    /// Elmah errors page ActionResult
    /// </summary>
    public class ElmahResult : ActionResult
    {
        private const string DetailAction = "Detail";

        /// <summary>
        /// ExecuteResult for errors list
        /// </summary>
        /// <param name="context">controller context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) return;

            // try and get the resource from the {resource} part of the route
            var routeDataValues = context.RequestContext.RouteData.Values;
            var resource = routeDataValues["resource"];
            if (resource == null)
            {
                // alternatively, try the {action} 
                var action = routeDataValues["action"];
                // but only if it is elmah/Detail/{resource}
                if (action != null && DetailAction.Equals(action.ToString(), StringComparison.OrdinalIgnoreCase))
                    resource = action;
            }

            var httpContext = context.HttpContext;

            if (httpContext == null) return;

            var request = httpContext.Request;
            var currentPath = request.Path;
            var queryString = request.QueryString;
            if (resource != null)
            {
                // make sure that ELMAH knows what the resource is
                var pathInfo = "." + resource;
                // also remove the resource from the path - else it will start chaining
                // e.g. /elmah/detail/detail/stylesheet
                var newPath = currentPath.Remove(currentPath.Length - pathInfo.Length);
                httpContext.RewritePath(newPath, pathInfo, queryString.ToString());
            }
            else
            {
                // we can't have paths such as elmah/ as the ELMAH handler will generate URIs such as elmah//stylesheet
                if (currentPath != null && currentPath.EndsWith("/"))
                {
                    var newPath = currentPath.Remove(currentPath.Length - 1);
                    httpContext.RewritePath(newPath, null, queryString.ToString());
                }
            }

            if (httpContext.ApplicationInstance != null)
            {
                var unwrappedHttpContext = httpContext.ApplicationInstance.Context;
                var handler = new ErrorLogPageFactory().GetHandler(unwrappedHttpContext, null, null, null);
                handler?.ProcessRequest(unwrappedHttpContext);
            }
        }
    }
}