using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication12.Models;
using WebApplication12.Extensions;

namespace WebApplication12.MiddleWares
{
    public class AuthCheckMiddeWare
    {
        private readonly RequestDelegate _next;

        public AuthCheckMiddeWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            //var ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
            var url = httpContext.Request.Path;
            //Debug.WriteLine("userAgent: " + userAgent);
            //Debug.WriteLine("ipAddress: " + ipAddress);
            Debug.WriteLine("url AuthCheck: " + url);

            AuthUser userTemp = httpContext.Session.GetObjectFromJson<AuthUser>("myuserdata");

            var mysessionval = httpContext.Session.GetString("Test1");
            Debug.Write(mysessionval);
            if (userTemp != null)
            {
                //AuthUser userTemp1 =(AuthUser)userTemp;
                UserPermission mypermission =(UserPermission) userTemp.Permissions.Where(a => a.Path == url).FirstOrDefault();
                if (mypermission==null && url!="/" && mypermission.IsPublic==false)
                {
                    Debug.Write("Un Authorized Access by User " + userTemp.UserName + " For Resource " + url);
                    //return _next( UnauthorizedAccessException("Access to this Resource is Restricted");
                    httpContext.Response.StatusCode = 401; //UnAuthorized
                    httpContext.Response.WriteAsync("UnAuthorized Access to the Resource");
                    return _next(httpContext);
                }

                Debug.WriteLine("Found User as" + userTemp.UserName);
            }

            return _next(httpContext);
        }
    }

    public static class AuthCheckMiddeWareExtensions
    {
        public static IApplicationBuilder UseAuthCheckMiddeWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthCheckMiddeWare>();
        }
    }

}

