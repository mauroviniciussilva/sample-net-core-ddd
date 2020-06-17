using Sample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Sample.Api.Filter
{
    public class DomainExceptionFilter : IExceptionFilter
    {
        public DomainExceptionFilter()
        {

        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new { Erro = context.Exception.Message });
            }

            if (context.Exception is ExpiredUserException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(new { Erro = context.Exception.Message });
            }
        }
    }
}