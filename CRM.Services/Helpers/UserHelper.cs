using IdentityModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CRM.Services.Helpers
{
    public class UserHelper
    {
        public static List<string> GetUserRoles(List<Claim> claims)
        {
            List<string> list = new List<string>();
            var roles = claims.FindAll(u => u.Type == ClaimsIdentity.DefaultRoleClaimType || u.Type == "role")
                .FirstOrDefault(c => c.Value != string.Empty)?.Value;
            if (!string.IsNullOrEmpty(roles))
            {
                if (roles.Contains(";"))
                {
                    list = roles.Split(";").Where(q => q != string.Empty).ToList();
                }
                else
                {
                    list = new List<string>
                    {
                        roles
                    };
                }
            }
            else
            {
                list = new List<string>
                {
                    "user"
                };
            }

            return list;
        }

        public static List<string> GetUserRolesFromToken(string access_token)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(access_token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(access_token) as JwtSecurityToken;
                var roles = token?.Claims
                    .Where(c => c.Value != string.Empty)
                    .FirstOrDefault(u => u.Type == ClaimsIdentity.DefaultRoleClaimType || u.Type == "role")?.Value;
                if (!string.IsNullOrWhiteSpace(roles))
                {
                    if (roles.Contains(";"))
                    {
                        list = roles.Split(";").Where(q => q != string.Empty).ToList();
                    }
                    else
                    {
                        list = new List<string>
                        {
                            roles
                        };
                    }
                }
                else
                {
                    list = new List<string>
                    {
                        "user"
                    };
                }
            }
#if DEBUG
            else
            {
                list = new List<string>
                {
                    "user"
                };
            }
#endif
            return list;
        }

        public static bool TryGetCurrentName(ClaimsPrincipal identityUser, out string name)
        {
            name = null;
            if (!string.IsNullOrEmpty(identityUser.Identity.Name))
            {
                var nameSplit = identityUser.Identity.Name.Split('\\');
                if (nameSplit.Length > 1)
                {
                    name = nameSplit[1];
                }
                else
                {
                    name = nameSplit[0];
                }

                return true;
            }
            else if (identityUser.Claims.Any(c => c.Type == JwtClaimTypes.Name || c.Type == ClaimTypes.Name))
            {
                var nameSplit = identityUser.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name || c.Type == ClaimTypes.Name)?.Value.Split('\\') ?? null;
                if (nameSplit == null)
                {
                    name = null;
                    return false;
                }
                else
                {
                    if (nameSplit.Length > 1)
                    {
                        name = nameSplit[1];
                    }
                    else
                    {
                        name = nameSplit[0];
                    }

                    return true;
                }
            }
#if DEBUG
            else if (!string.IsNullOrWhiteSpace(WindowsIdentity.GetCurrent().Name))
            {
                name = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                return true;
            }
#endif
            return false;
        }

        public static string GetUserShortName(string surname, string name, string middlename = null)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(surname) && !string.IsNullOrEmpty(name))
            {
                result = $"{surname} {name.First()}.";
                if (!string.IsNullOrEmpty(middlename))
                {
                    result = $"{result} {middlename.First()}.";
                }
            }

            return result;
        }

        public static string GetUserFullName(string surname, string name, string middlename)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(surname) && !string.IsNullOrEmpty(name))
            {
                result = $"{surname} {name}";
                if (!string.IsNullOrEmpty(middlename))
                {
                    result = $"{result} {middlename}";
                }
            }

            return result;
        }

    }
}
