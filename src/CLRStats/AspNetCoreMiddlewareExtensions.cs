using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CLRStats
{
    public static class AspNetCoreMiddlewareExtensions
    {
        public static IApplicationBuilder UseCLRStatsDashboard(
            this IApplicationBuilder app,
            string pathMatch = CLRStatsService.DEFAULT_PATH, DashboardOptions options = null)
        {
            var services = app.ApplicationServices;
            options = options ?? services.GetService<DashboardOptions>() ?? new DashboardOptions();
            options.Authorization = options.Authorization ?? Array.Empty<IDashboardAuthorizationFilter>();
            app.Map(new PathString(pathMatch), x => x.UseMiddleware<AspNetCoreDashboardMiddleware>(options));
            return app;
        }
    }

    internal class AspNetCoreDashboardMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DashboardOptions _options;

        public AspNetCoreDashboardMiddleware(RequestDelegate next, DashboardOptions options)
        {
            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext httpContext)
        {
            foreach (var filter in _options.Authorization)
            {
                if (!filter.Authorize(httpContext.Request))
                {
                    var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated;

                    httpContext.Response.StatusCode = isAuthenticated == true
                        ? (int)HttpStatusCode.Forbidden
                        : (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                }
            }
            httpContext.Response.ContentType = Constants.ContentType;
            return httpContext.Response.WriteAsync(CLRStatsService.GetCurrentCLRStatsToJson());
        }
    }
}
