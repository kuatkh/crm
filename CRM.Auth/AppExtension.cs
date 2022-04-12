using CRM.Auth.Auth;
using CRM.Auth.Configuration;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Auth
{
    public static class AppExtension
    {
        public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder, CrmAuthConfiguration configuration)
        {
            var clients = configuration.Clients.Select(
                source => new Client
                {
                    AllowAccessTokensViaBrowser = true,
                    ClientId = source.Id,
                    ClientSecrets = { new Secret(source.Secret.Sha256()) },
                    AllowedGrantTypes = new List<string>() { GrantType.ResourceOwnerPassword, CustomGrantType.Silent, CustomGrantType.Db },
                    AllowedScopes = source.Scopes,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = configuration.TokenLifeTimeInSec,

                    AllowOfflineAccess = true
                }
            );

            return builder.AddInMemoryClients(clients);
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResource("crm.v1.full", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Audience, JwtClaimTypes.Role, "role" })
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("crm.v0.full", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Audience, JwtClaimTypes.Role, "role" }),
                new ApiScope("crm.v0", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Audience, JwtClaimTypes.Role, "role" }),
                new ApiScope("crm.v1.full", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Audience, JwtClaimTypes.Role, "role" }),
                new ApiScope("crm.v1", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Audience, JwtClaimTypes.Role, "role" })
            };
        }

        public static IIdentityServerBuilder AddApiResources(this IIdentityServerBuilder builder)
        {
            var resources = new[]
            {
                new ApiResource("api1"),
                new ApiResource
                {
                    Name = "crm.full",
                    Scopes =
                    {
                        "crm.v1",
                        "crm.v1.full"
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Audience,
                        JwtClaimTypes.Issuer,
                        JwtClaimTypes.JwtId
                    }
                },
                new ApiResource
                {
                    Name = "crm.adm.full",
                    Scopes =
                    {
                        "crm.v0",
                        "crm.v0.full"
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Audience,
                        JwtClaimTypes.Issuer,
                        JwtClaimTypes.JwtId
                    }
                }
            };

            return builder.AddInMemoryApiResources(resources);
        }

    }
}
