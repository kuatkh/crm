using CRM.DataModel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Auth.AuthProvider
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<CrmUsers, CrmRoles>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<CrmUsers> userManager,
            RoleManager<CrmRoles> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(CrmUsers user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("sub", user.UserName ?? ""));
            return identity;
        }
    }
}
