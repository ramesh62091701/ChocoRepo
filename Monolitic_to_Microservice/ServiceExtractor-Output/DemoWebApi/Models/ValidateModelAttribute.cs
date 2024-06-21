﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Diagnostics;

namespace DemoWebApi.Models
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState.Values.SelectMany(v => v.Errors);
                var errorMessages = errors.Select(d => d.Exception==null ? d.ErrorMessage : d.Exception.ToString());
                Trace.TraceError($"Validation error, Controller: {actionContext.ControllerContext.ControllerDescriptor.ControllerName}  Action: {actionContext.ActionDescriptor.ActionName}  {String.Join(Environment.NewLine, errorMessages)}");
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
