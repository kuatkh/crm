using CRM.Auth.AuthProvider;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Auth.Auth
{
    public class DbGrantValidator : IExtensionGrantValidator
    {
        private readonly IAuthProvider _authProvider;

        public DbGrantValidator(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                var isLogout = context.Request.Raw.Get("isLogout");
                if (!string.IsNullOrEmpty(isLogout) && isLogout.ToLower() == "true")
                {
                    var result = _authProvider.Logout();
                    context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "loggedout");
                    return Task.CompletedTask;
                }
                var username = context.Request.Raw.Get("username");
                var secret = context.Request.Raw.Get("password");
                var email = context.Request.Raw.Get("email");
                var clientId = context.Request.Client.ClientId;
                var isRegistration = context.Request.Raw.Get("isregistration");
                var departmentName = context.Request.Raw.Get("departmentname");
                var positionName = context.Request.Raw.Get("positionname");
                var isChangeSecret = context.Request.Raw.Get("ischangepassword");
                var isWipePassword = context.Request.Raw.Get("iswipepassword");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(secret))
                {
                    if (!string.IsNullOrEmpty(isRegistration) && bool.TryParse(isRegistration, out bool isNewUser) && isNewUser == true)
                    {
                        var role = context.Request.Raw.Get("role");
                        var result = _authProvider.Register(username, secret, role, email, departmentName, positionName).Result;
                        Dictionary<string, object> responce = new Dictionary<string, object>();
                        responce.Add("isregistered", result.IsRegistered);
                        responce.Add("id", result.Message);
                        context.Result = new GrantValidationResult(responce);
                        return Task.CompletedTask;
                    }
                    else if (!string.IsNullOrEmpty(isChangeSecret) && bool.TryParse(isChangeSecret, out bool isChangePassword) && isChangePassword == true)
                    {
                        var oldSecret = context.Request.Raw.Get("oldpassword");
                        var newSecret = context.Request.Raw.Get("newpassword");
                        if (!string.IsNullOrEmpty(oldSecret) && !string.IsNullOrEmpty(newSecret) &&
                            !string.IsNullOrEmpty(username))
                        {
                            var result = _authProvider.ChangePassword(username, oldSecret, newSecret).Result;
                            if (result.Succeeded)
                            {
                                Dictionary<string, object> responce = new Dictionary<string, object>();
                                responce.Add("ispasswordchanged", true);
                                context.Result = new GrantValidationResult(responce);
                            }
                            else
                            {
                                var errormessage = "";
                                foreach (var error in result.Errors)
                                {
                                    errormessage += error.Description + " ";
                                }
                                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, errormessage);
                                return Task.CompletedTask;
                            }
                        }
                        else
                        {
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "username or password is empty");
                            return Task.CompletedTask;
                        }
                    }
                    else if (!string.IsNullOrEmpty(isWipePassword) && bool.TryParse(isWipePassword, out bool isWipe) && isWipe == true)
                    {
                        var result = _authProvider.WipePassword(username).Result;
                        if (result.Succeeded)
                        {
                            Dictionary<string, object> responce = new Dictionary<string, object>();
                            responce.Add("ispasswordwiped", true);
                            context.Result = new GrantValidationResult(responce);
                        }
                        else
                        {
                            var errormessage = "";
                            foreach (var error in result.Errors)
                            {
                                errormessage += error.Description + " ";
                            }
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, errormessage);
                            return Task.CompletedTask;
                        }
                    }
                    else
                    {
                        var result = _authProvider.Login(username, secret).Result;
                        if (result.IsLoggedIn)
                        {
                            List<Claim> claims = new List<Claim>();
                            if (!string.IsNullOrEmpty(result.Roles))
                            {
                                var rolesArray = result.Roles.Split(";").ToList().Where(c => !string.IsNullOrEmpty(c));
                                claims.Add(new Claim(JwtClaimTypes.Role, result.Roles));
                                foreach (var role in rolesArray)
                                {
                                    claims.Add(new Claim(role, role));
                                }
                            }
                            claims.Add(new Claim(JwtClaimTypes.Audience, "CRM.full"));

                            context.Result = new GrantValidationResult(result.Message, "db", claims);
                        }
                        else
                        {
                            context.Result =
                                new GrantValidationResult(TokenRequestErrors.InvalidRequest, result.Message);
                            return Task.CompletedTask;
                        }
                    }
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "username or password is empty");
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            }
            return Task.CompletedTask;
        }

        public string GrantType => CustomGrantType.Db;
    }
}
