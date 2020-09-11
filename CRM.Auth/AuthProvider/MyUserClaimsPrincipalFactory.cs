using BigProject.DataModel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BigProject.Auth.AuthProvider
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<BpUsers, BpRoles>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<BpUsers> userManager,
            RoleManager<BpRoles> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(BpUsers user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("sub", user.UserName ?? ""));
            return identity;
        }
    }
}
