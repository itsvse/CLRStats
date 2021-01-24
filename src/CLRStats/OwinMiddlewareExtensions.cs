using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace CLRStats
{
    public static class OwinMiddlewareExtensions
    {
        public static void UseCLRStatsDashboard(
            this IAppBuilder app,
            string pathMatch = CLRStatsService.DEFAULT_PATH, DashboardOptions options = null)
        {
            options = options ?? new DashboardOptions();
            options.Authorization = options.Authorization ?? new IDashboardAuthorizationFilter[0];
            app.Map(pathMatch, subApp => subApp.Use<OwinDashboardMiddleware>(options));
        }
    }

    internal class OwinDashboardMiddleware : OwinMiddleware
    {
        private readonly DashboardOptions _options;
        public OwinDashboardMiddleware(OwinMiddleware next, DashboardOptions options) : base(next)
        {
            _options = options;
        }
        public override Task Invoke(IOwinContext context)
        {
            foreach (var filter in _options.Authorization)
            {
                if (!filter.Authorize(context.Request))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return context.Response.WriteAsync("401 Unauthorized");
                }
            }
            context.Response.ContentType = Constants.ContentType;
            return context.Response.WriteAsync(CLRStatsService.GetCurrentCLRStatsToJson());
        }
    }
}
