using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Authorization
{
    // https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authorization/iauthorizationpolicyprovider.md
    internal class PermittedRoleAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "PermittedRole";

        public PermittedRoleAuthorizeAttribute(string PermittedRoles) => this.PermittedRoles = PermittedRoles;

        // Get or set the Age property by manipulating the underlying Policy property
        public string PermittedRoles
        {
            get
            {
                if (Policy.StartsWith(POLICY_PREFIX))
                {
                    return Policy.Substring(POLICY_PREFIX.Length);
                }
                return default;
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
