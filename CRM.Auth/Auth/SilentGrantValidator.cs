using BigProject.Auth.AuthProvider;
using BigProject.Auth.IdentityProvider;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BigProject.Auth.Auth
{
    public class SilentGrantValidator : IExtensionGrantValidator
    {
        private readonly ILogger<SilentGrantValidator> _logger;
        private readonly IIdentityProvider _windowsIdentityProvider;
        private readonly IAuthProvider _authProvider;

        public SilentGrantValidator(IIdentityProvider windowsIdentityProvider, ILogger<SilentGrantValidator> logger, IAuthProvider authProvider)
        {
            _windowsIdentityProvider = windowsIdentityProvider;
            _logger = logger;
            _authProvider = authProvider;
        }

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                if (!_windowsIdentityProvider.TryGetCurrentName(out var name) && string.IsNullOrEmpty(context.Request.Raw.Get("username")))
                {
                    context.Result = InvalidGrant;
                    return Task.CompletedTask;
                }
                var username = !string.IsNullOrEmpty(name)
                    ? name.Split('\\')[1]
                    : context.Request.Raw.Get("username");

                var isRegistration = context.Request.Raw.Get("isregistration");

                if (!string.IsNullOrEmpty(isRegistration) && isRegistration.ToLower() == "true")
                {
                    var role = context.Request.Raw.Get("role");
                    var result = _authProvider.Register(username, "", role, username).Result;
                    if (result.IsRegistered)
                    {
                        Dictionary<string, object> response = new Dictionary<string, object>
                        {
                            { "isregistered", result.IsRegistered },
                            { "id", result.Message }
                        };
                        context.Result = new GrantValidationResult(response);
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Result =
                            new GrantValidationResult(TokenRequestErrors.InvalidRequest, result.Message);
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    var result = _authProvider.Login(username, null).Result;
                    _logger.LogInformation(1, JsonConvert.SerializeObject(result));
                    if (result.IsLoggedIn)
                    {
                        if (!string.IsNullOrEmpty(result.Roles))
                        {
                            var rolesArray = result.Roles.Split(";").ToList().Where(c => !string.IsNullOrEmpty(c));
                            List<Claim> claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Role, result.Roles)
                            };
                            foreach (var role in rolesArray)
                            {
                                claims.Add(new Claim(role, role));
                            }
                            context.Result = new GrantValidationResult(username, GrantType, claims);
                        }
                        else
                        {
                            List<Claim> claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Role, "user"),
                                new Claim("user", "user")
                            };
                            context.Result = new GrantValidationResult(username, GrantType, claims);
                        }
                    }
                    else
                    {
                        context.Result = InvalidGrant;
                        return Task.CompletedTask;
                    }
                }

                context.Result = new GrantValidationResult(username, GrantType);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(1, ex.Message);
                context.Result = InvalidGrant;
            }
            return Task.CompletedTask;
        }

        public string GrantType => CustomGrantType.Silent;
        private static GrantValidationResult InvalidGrant => new GrantValidationResult(TokenRequestErrors.InvalidGrant);
    }
}
