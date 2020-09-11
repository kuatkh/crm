using BigProject.DataModel.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BigProject.Auth.Auth
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<BpUsers> _userManager;

        public ProfileService(UserManager<BpUsers> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Subject, context.Subject.FindFirst("sub")?.Value),
                    new Claim(JwtClaimTypes.Name, context.Subject.FindFirst("sub")?.Value),
                    new Claim(ClaimTypes.Name, context.Subject.FindFirst("sub")?.Value)
            };
            List<string> userRoles = new List<string>();
            if (context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject) != null)
            {
                var user = _userManager.FindByNameAsync(context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value).Result;
                if (user != null)
                {
                    userRoles = _userManager.GetRolesAsync(user).Result.ToList();
                    if (userRoles.Count > 0)
                    {
                        foreach (var role in userRoles)
                        {
                            claims.Add(new Claim(role, "true"));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim("user", "true"));
                    }
                }
                else
                {
                    claims.Add(new Claim("user", "true"));
                }
            }

            if (string.IsNullOrEmpty(context.Subject.FindFirst("role")?.Value))
            {
                if (userRoles.Count > 0)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, string.Join(";", userRoles)));
                }
                else
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, "user"));
                }
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, context.Subject.FindFirst("role")?.Value));
            }

            claims.Add(new Claim(JwtClaimTypes.Audience, context.Subject.FindFirst("aud")?.Value ?? "BigProject.full"));

            context.IssuedClaims = claims;

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //var user = _userManager.GetUserAsync(context.Subject).Result;
            //context.IsActive = context.Subject.Identity.IsAuthenticated;
            context.IsActive = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject) != null;
            //context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
