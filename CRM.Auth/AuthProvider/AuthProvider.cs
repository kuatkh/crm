using BigProject.Auth.Configuration;
using BigProject.Auth.Models;
using BigProject.Auth.Responses;
using BigProject.DataModel.Data;
using BigProject.DataModel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.AuthProvider
{
    public class AuthProvider : IAuthProvider
    {
        private readonly ILogger<AuthProvider> _logger;
        private readonly SignInManager<BpUsers> _signInManager;
        private readonly UserManager<BpUsers> _userManager;
        private readonly RoleManager<BpRoles> _roleManager;
        private readonly BpAuthConfiguration _configuration;
        private BpDbContext _abContext;

        public AuthProvider(ILogger<AuthProvider> logger,
            SignInManager<BpUsers> signInManager, UserManager<BpUsers> userManager,
            RoleManager<BpRoles> roleManager, BpAuthConfiguration configuration, BpDbContext abContext)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _abContext = abContext;
        }

        public async Task<LogInResult> Login(string username, string password)
        {
            try
            {
                //var loginresult = await _signInManager.PasswordSignInAsync(username, password, true,
                //    lockoutOnFailure: false);

                if (!string.IsNullOrWhiteSpace(password))
                {
                    var loginresult = await _signInManager.PasswordSignInAsync(username, password, true,
                        lockoutOnFailure: false);
                    if (loginresult.Succeeded)
                    {
                        _logger.LogInformation(1, $"User logged in with specified username: '{username}'.");

                        var loggedinuser = await _userManager.FindByNameAsync(username);
                        var rolesForUser = await _userManager.GetRolesAsync(loggedinuser);
                        return new LogInResult(true, loggedinuser.UserName, string.Join(';', rolesForUser));
                    }
                    return new LogInResult("Invalid_username_or_password");
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(username);
                    if (user != null)
                    {
                        await _signInManager.SignInAsync(user, false);
                        var rolesForUser = await _userManager.GetRolesAsync(user);
                        return new LogInResult(true, user.UserName, string.Join(';', rolesForUser));
                    }
                    else
                    {
                        return new LogInResult("Invalid_username_or_password");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(1, ex.Message);
                return new LogInResult(ex.Message);
            }
        }

        public async Task<RegistrationResult> Register(string username, string password, string role, string email, string departmentName = null, string positionName = null)
        {
            try
            {
                var user = new BpUsers { UserName = username, Email = !string.IsNullOrEmpty(email) ? email : username, BirthDate = DateTime.Now, CreatedDateTime = DateTime.Now };

                var checking = await _userManager.FindByNameAsync(username);
                if (checking == null)
                {
                    var result = new IdentityResult();
                    if (string.IsNullOrEmpty(password))
                    {
                        result = await _userManager.CreateAsync(user);
                    }
                    else
                    {
                        result = await _userManager.CreateAsync(user, password);
                    }

                    if (result.Succeeded)
                    {
                        // добавляем роли для пользователя
                        if (!string.IsNullOrEmpty(role))
                        {
                            var searchrole = _roleManager.FindByNameAsync(role).Result;
                            if (searchrole == null)
                            {
                                return new RegistrationResult("Role doesn't exist");
                            }
                            else
                            {
                                var rolesForUser = await _userManager.GetRolesAsync(user);
                                if (!rolesForUser.Contains(role))
                                {
                                    var userrole = await _userManager.AddToRoleAsync(user, role);
                                    if (userrole.Succeeded)
                                    {
                                        _logger.LogInformation(1, $"Created a new account with specified username: '{username}' and specified role: '{role}'.");
                                    }
                                    else
                                    {
                                        _logger.LogInformation(1, $"Role: '{role}' doesn't added to created user with specified username: '{username}.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            _logger.LogInformation(1, $"Created a new account with specified username: '{username}' and password.");
                        }
                        var registereduser = await _userManager.FindByNameAsync(username);
                        return new RegistrationResult(true, registereduser.UserName, "");
                    }
                    else
                    {
                        return new RegistrationResult(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    return new RegistrationResult("alerady_exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(1, ex.Message);
                return new RegistrationResult(ex.Message);
            }
            finally
            {
                await _abContext.DisposeAsync();
            }
        }

        public async Task<IdentityResult> ChangePassword(string userguid, string oldpassword, string newpassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userguid);

                if (user != null && await _userManager.CheckPasswordAsync(user, oldpassword))
                {
                    var result = await _userManager.RemovePasswordAsync(user);

                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, newpassword);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation(1, $"Password successfully changed. ID {userguid}");
                            return IdentityResult.Success;
                        }
                        else
                        {
                            _logger.LogInformation(1, $"Password not changed. ID {userguid}");
                            return IdentityResult.Failed(new IdentityError() { Description = "password_not_changed" });
                        }
                    }
                    else
                    {
                        _logger.LogInformation(1, $"Old password not removed. Password not changed. ID {userguid}");
                        return IdentityResult.Failed(new IdentityError() { Description = "password_not_changed" });
                    }
                }
                else if (user == null)
                {
                    _logger.LogInformation(1, $"User with id {userguid} not found");
                    return IdentityResult.Failed(new IdentityError() { Description = "user_not_found" });
                }
                else
                {
                    _logger.LogInformation(1, $"Not valid password for user with id '{userguid}' pass '{oldpassword}'");
                    return IdentityResult.Failed(new IdentityError() { Description = "not_valid_password" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(1, ex.Message);
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> WipePassword(string username)
        {
            try
            {
                BpUsers user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, "123456Aa");
                        if (result.Succeeded)
                        {
                            _logger.LogInformation(1, $"Password successfully wiped. UserName {username}");
                            return IdentityResult.Success;
                        }
                        else
                        {
                            _logger.LogInformation(1, $"Password not wiped. UserName {username}");
                            return IdentityResult.Failed(new IdentityError() { Description = "password_not_wiped" });
                        }
                    }
                    else
                    {
                        _logger.LogInformation(1, $"Old password not removed. Password not wiped. UserName {username}");
                        return IdentityResult.Failed(new IdentityError() { Description = "password_not_removed" });
                    }
                }
                else
                {
                    _logger.LogInformation(1, $"User with UserName {username} not found");
                    return IdentityResult.Failed(new IdentityError() { Description = "user_not_found" });
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $@"error: {JsonConvert.SerializeObject(ex)}" });
            }
        }

        public async Task<LogInResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new LogInResult(false, "logged out", "");
        }
    }
}
