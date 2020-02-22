using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AspDotNetCoreDemo.WebInfrastructure.Filters
{
    public class HangfireDashboardAuthorizationFilter
        : IDashboardAuthorizationFilter
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public HangfireDashboardAuthorizationFilter(
            IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public bool Authorize(DashboardContext context)
        {
            
            if (hostEnvironment.IsDevelopment())
            {
                return true;
            } 
            else
            {
                var httpContext = context.GetHttpContext();

                // Allow all authenticated users to see the Dashboard (potentially dangerous).
                return httpContext.User.Identity.IsAuthenticated;

            }
        }
    }
}
