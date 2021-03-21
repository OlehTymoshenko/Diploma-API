using Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using PL.Utils.ExceptionsHandler.Models;
using Microsoft.IdentityModel.Tokens;

namespace PL.Utils.ExceptionsHandler
{
    class ExceptionsHandlerMiddleware
    {
        readonly RequestDelegate _next;

        public ExceptionsHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (DiplomaApiExpection ex)
            {
                var exceptionModel = ExceptionModel.FromException(ex);
                await HandleExceptionAsync(httpContext, exceptionModel);
            }
            catch (Exception ex)
            {
                var exceptionModel = ExceptionModel.FromException(ex);
                await HandleExceptionAsync(httpContext, exceptionModel);
            }
        }

        async static Task HandleExceptionAsync(HttpContext httpContext, ExceptionModel exceptionModel)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)exceptionModel.StatusCode;

            await httpContext.Response.WriteAsync(exceptionModel.ToString());
        }
    }
}
