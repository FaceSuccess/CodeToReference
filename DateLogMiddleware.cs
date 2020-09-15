using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.MiddleWares
{
    public class DateLogMiddleware
    {
        private readonly RequestDelegate _next;

        public DateLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine("Date: " + DateTime.Now.ToLongDateString());
            Debug.WriteLine("User: " + httpContext.User.Identity.Name);
            Debug.WriteLine("Host" + httpContext.Request.Host.ToString());

            return _next(httpContext);
        }
    }

    public static class DateLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseDateLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DateLogMiddleware>();
        }
    }



}
