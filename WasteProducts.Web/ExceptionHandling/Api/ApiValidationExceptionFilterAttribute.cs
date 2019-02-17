﻿using FluentValidation;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace WasteProducts.Web.ExceptionHandling.Api
{
    /// <summary>
    /// WebApi validation exception filter attribute
    /// </summary>
    public class ApiValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is AggregateException aggregateException)
            {
                if (aggregateException.InnerExceptions[0] is ValidationException exception)
                {
                    HandleValidationException(actionExecutedContext, exception);
                }
            }
            else if (actionExecutedContext.Exception is ValidationException exception)
            {
                HandleValidationException(actionExecutedContext, exception);
            }
        }

        private void HandleValidationException(HttpActionExecutedContext actionExecutedContext, ValidationException exception)
        {
            if (exception.Errors.Any())
            {
                var modelState = actionExecutedContext.ActionContext.ModelState;

                foreach (var validationFailure in exception.Errors)
                {
                    modelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }

                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                actionExecutedContext.Response.Content = new StringContent(exception.Message);
            }
        }
    }
}