using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomePage.Authorization
{
    // This class contains logic for determining whether Permitted Roles in authorization
    // policies are satisfied or not
    internal class PermittedRoleAuthorizationHandler : AuthorizationHandler<PermittedRoleRequirement>
    {
        private readonly ILogger<PermittedRoleAuthorizationHandler> _logger;

        public PermittedRoleAuthorizationHandler(ILogger<PermittedRoleAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermittedRoleRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            _logger.LogWarning("Evaluating authorization requirement for roles {" + string.Join(',',requirement.PermittedRoles) + "}");

            // Check the user's age
            var roleClaim = context.User.FindFirst(c => c.Type == "roles");
            if (roleClaim != null)
            {
                var rolesClaimed = roleClaim.Value
                        .Replace("[","")
                        .Replace("]", "")
                        .Split(",")
                        .Select(item => item.Replace("\"", "").Trim());

                if(rolesClaimed.Any(role => requirement.PermittedRoles.Contains(role)))
                {
                    _logger.LogInformation("Permitted Roles authorization requirement satisfied");
                    context.Succeed(requirement);
                }
                else
                {
                    _logger.LogInformation("Permitted Roles authorization requirement failed");
                }               
            }
            else
            {
                _logger.LogInformation("No role claim present");
            }

            return Task.CompletedTask;
        }
    }
}
