using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Authorization
{
    internal class PermittedRoleRequirement : IAuthorizationRequirement
    {
        public List<string> PermittedRoles { get; private set; }

        public PermittedRoleRequirement(string roles) { PermittedRoles = roles.Split(",").Select(role => role.Trim()).ToList(); }
    }
}
