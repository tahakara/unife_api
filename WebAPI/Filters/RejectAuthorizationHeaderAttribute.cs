using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RejectAuthorizationHeaderAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthHeader = context.HttpContext.Request.Headers.ContainsKey("Authorization");

            if (hasAuthHeader)
            {
                context.Result = new UnauthorizedObjectResult("This endpoint does not allow the Authorization header.");
            }
        }
    }
}