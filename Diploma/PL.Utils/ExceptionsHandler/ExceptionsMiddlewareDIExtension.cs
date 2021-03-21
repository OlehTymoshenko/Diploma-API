using Microsoft.AspNetCore.Builder;

namespace PL.Utils.ExceptionsHandler
{
    public static class ExceptionsMiddlewareDIExtension
    {
        public static IApplicationBuilder UseCustomExceptionsHandlerMiddleware(this IApplicationBuilder appBuidler)
        {
            appBuidler.UseMiddleware<ExceptionsHandlerMiddleware>();
            return appBuidler;
        }
    }
}
