using CRM.DataModel.Data;
using CRM.DataModel.Models;
using CRM.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Services.Common
{
    public class SeedDefaultData: ISeedDefaultData
    {
        private readonly CrmDbContext _context;
        private readonly UserManager<CrmUsers> _userManager;
        private readonly RoleManager<CrmRoles> _roleManager;
        private readonly ILogger<SeedDefaultData> _logger;

        public SeedDefaultData(CrmDbContext context,
            UserManager<CrmUsers> userManager,
            RoleManager<CrmRoles> roleManager,
            ILogger<SeedDefaultData> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void Seed()
        {
            if (!_context.Users.Any())
            {
                if (!_roleManager.RoleExistsAsync("superadmin").GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new CrmRoles()
                    {
                        Name = "superadmin",
                    }).GetAwaiter().GetResult();
                }

                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new CrmRoles()
                    {
                        Name = "admin",
                        Description = "Администратор"
                    }).GetAwaiter().GetResult();
                }

                if (!_roleManager.RoleExistsAsync("user").GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new CrmRoles()
                    {
                        Name = "user",
                        Description = "Пользователь"
                    }).GetAwaiter().GetResult();
                }

                var supadm = new CrmUsers
                {
                    UserName = "super_admin",
                    Email = "super_admin@CRM.kz",
                    SurnameRu = "СуперАдмин",
                    NameRu = "СуперАдмин",
                    BirthDate = DateTime.Now,
                    CreatedDateTime = DateTime.Now
                };
                var supadmresult = _userManager.CreateAsync(supadm, "1q@W3e$R").GetAwaiter().GetResult();

                if (supadmresult.Succeeded)
                {
                    _logger.LogWarning($"super_admin SUCCESSFULLY ADDED!");

                    var userrole = _userManager.AddToRoleAsync(supadm, "superadmin").GetAwaiter().GetResult();
                    if (userrole.Succeeded)
                    {
                        _logger.LogWarning($"ROLE superadmin TO super_admin SUCCESSFULLY ADDED!");
                    }
                    else
                    {
                        _logger.LogError($"SMTH WENT WRONG WHILE ADDING ROLE superadmin TO super_admin!");
                    }
                }
                else
                {
                    _logger.LogError($"SMTH WENT WRONG WHILE ADDING super_admin! {JsonConvert.SerializeObject(supadmresult.Errors)}");
                }

                var user = new CrmUsers
                {
                    UserName = "admin",
                    Email = "admin@CRM.kz",
                    SurnameRu = "Администратор",
                    NameRu = "Администратор",
                    BirthDate = DateTime.Now,
                    CreatedDateTime = DateTime.Now
                };
                var result = _userManager.CreateAsync(user, "1q@W3e$R").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _logger.LogWarning($"admin SUCCESSFULLY ADDED!");

                    var userrole = _userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();
                    if (userrole.Succeeded)
                    {
                        _logger.LogWarning($"ROLE admin TO admin SUCCESSFULLY ADDED!");
                    }
                    else
                    {
                        _logger.LogError($"SMTH WENT WRONG WHILE ADDING ROLE admin TO admin!");
                    }
                }
                else
                {
                    _logger.LogError($"SMTH WENT WRONG WHILE ADDING ab_admin!");
                }

                var user2 = new CrmUsers
                {
                    UserName = "user",
                    Email = "user@CRM.kz",
                    SurnameRu = "АдминистраторРуко",
                    NameRu = "АдминистраторРуко",
                    BirthDate = DateTime.Now,
                    CreatedDateTime = DateTime.Now
                };
                var result2 = _userManager.CreateAsync(user2, "1q@W3e$R").GetAwaiter().GetResult();

                if (result2.Succeeded)
                {
                    _logger.LogWarning($"ab_admin_head SUCCESSFULLY ADDED!");

                    var userrole2 = _userManager.AddToRoleAsync(user2, "user").GetAwaiter().GetResult();
                    if (userrole2.Succeeded)
                    {
                        _logger.LogWarning($"ROLE user TO user SUCCESSFULLY ADDED!");
                    }
                    else
                    {
                        _logger.LogError($"SMTH WENT WRONG WHILE ADDING ROLE user TO user!");
                    }
                }
                else
                {
                    _logger.LogError($"SMTH WENT WRONG WHILE ADDING user!");
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
