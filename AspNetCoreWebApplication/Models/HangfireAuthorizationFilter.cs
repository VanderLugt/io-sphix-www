using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreWebApplication.Models
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private string policyName;

        public HangfireAuthorizationFilter(string policyName)
        {
            this.policyName = policyName;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var _result = authService.AuthorizeAsync(httpContext.User, this.policyName).ConfigureAwait(false).GetAwaiter().GetResult();
            return _result.Succeeded;
        }
    }
}
