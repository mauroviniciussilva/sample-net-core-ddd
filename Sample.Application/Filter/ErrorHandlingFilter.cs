using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sample.Application.Response;
using Sample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sample.Application.Filter
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        public ErrorHandlingFilter()
        {

        }

        public void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = new Error(context.Exception.TargetSite.DeclaringType.Name, context.Exception.TargetSite.Name, new List<string>() { context.Exception.Message });

            if (context.Exception is DomainException exception)
            {
                statusCode = HttpStatusCode.BadRequest;
                result = new Error(context.Exception.TargetSite.DeclaringType.Name, context.Exception.TargetSite.Name, exception.Messages);
            }
            else if (context.Exception is ExpiredUserException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (context.Exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new JsonResult(result);
        }
    }
}