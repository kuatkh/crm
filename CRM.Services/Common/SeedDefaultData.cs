using CRM.DataModel.Data;
using CRM.DataModel.Models;
using CRM.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
                    CrmEmployee = new CrmEmployees()
                    {
                        SurnameRu = "СуперАдмин",
                        SurnameKz = "СуперАдмин",
                        SurnameEn = "SuperAdmin",
                        NameRu = "СуперАдмин",
                        NameKz = "СуперАдмин",
                        NameEn = "SuperAdmin",
                        BirthDate = DateTime.Now,
                        IsActive = true,
                        CreatedDateTime = DateTime.Now
                    },
                    CreatedDateTime = DateTime.Now
                };
                var supadmresult = _userManager.CreateAsync(supadm, "1q@W3e$R").GetAwaiter().GetResult();

                if (supadmresult.Succeeded)
                {
                    _logger.LogWarning($"super_admin SUCCESSFULLY ADDED!");

                    if (supadm.Id > 0 && supadm.CrmEmployeesId > 0)
                    {
                        var crmEmployee = _context.CrmEmployees.FirstOrDefault(e => e.Id == supadm.CrmEmployeesId && e.CrmUsersId == null);
                        if (crmEmployee != null)
                        {
                            crmEmployee.CrmUsersId = supadm.Id;
                            _context.CrmEmployees.Update(crmEmployee);
                            _context.SaveChanges();
                        }
                    }
                    if (supadm.Id > 0 && supadm.CrmPatientsId > 0)
                    {
                        var crmPatient = _context.CrmPatients.FirstOrDefault(e => e.Id == supadm.CrmPatientsId && e.CrmUsersId == null);
                        if (crmPatient != null)
                        {
                            crmPatient.CrmUsersId = supadm.Id;
                            _context.CrmPatients.Update(crmPatient);
                            _context.SaveChanges();
                        }
                    }

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
                    Email = "admin@crm.kz",
                    CrmEmployee = new CrmEmployees()
                    {
                        SurnameRu = "Администратор",
                        SurnameKz = "Администратор",
                        SurnameEn = "Administrator",
                        NameRu = "Администратор",
                        NameKz = "Администратор",
                        NameEn = "Administrator",
                        BirthDate = DateTime.Now,
                        IsActive = true,
                        CreatedDateTime = DateTime.Now
                    },
                    CreatedDateTime = DateTime.Now
                };
                var result = _userManager.CreateAsync(user, "1q@W3e$R").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _logger.LogWarning($"admin SUCCESSFULLY ADDED!");

                    if (user.Id > 0 && user.CrmEmployeesId > 0)
                    {
                        var crmEmployee = _context.CrmEmployees.FirstOrDefault(e => e.Id == user.CrmEmployeesId && e.CrmUsersId == null);
                        if (crmEmployee != null)
                        {
                            crmEmployee.CrmUsersId = user.Id;
                            _context.CrmEmployees.Update(crmEmployee);
                            _context.SaveChanges();
                        }
                    }
                    if (user.Id > 0 && user.CrmPatientsId > 0)
                    {
                        var crmPatient = _context.CrmPatients.FirstOrDefault(e => e.Id == user.CrmPatientsId && e.CrmUsersId == null);
                        if (crmPatient != null)
                        {
                            crmPatient.CrmUsersId = user.Id;
                            _context.CrmPatients.Update(crmPatient);
                            _context.SaveChanges();
                        }
                    }

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
                    _logger.LogError($"SMTH WENT WRONG WHILE ADDING admin!");
                }

                var user2 = new CrmUsers
                {
                    UserName = "user",
                    Email = "user@crm.kz",
                    CrmEmployee = new CrmEmployees()
                    {
                        SurnameRu = "Пользователь",
                        SurnameKz = "Пользователь",
                        SurnameEn = "User",
                        NameRu = "Пользователь",
                        NameKz = "Пользователь",
                        NameEn = "User",
                        BirthDate = DateTime.Now,
                        IsActive = true,
                        CreatedDateTime = DateTime.Now
                    },
                    CreatedDateTime = DateTime.Now
                };
                var result2 = _userManager.CreateAsync(user2, "1q@W3e$R").GetAwaiter().GetResult();

                if (result2.Succeeded)
                {
                    _logger.LogWarning($"user SUCCESSFULLY ADDED!");

                    if (user2.Id > 0 && user2.CrmEmployeesId > 0)
                    {
                        var crmEmployee = _context.CrmEmployees.FirstOrDefault(e => e.Id == user2.CrmEmployeesId && e.CrmUsersId == null);
                        if (crmEmployee != null)
                        {
                            crmEmployee.CrmUsersId = user2.Id;
                            _context.CrmEmployees.Update(crmEmployee);
                            _context.SaveChanges();
                        }
                    }
                    if (user2.Id > 0 && user2.CrmPatientsId > 0)
                    {
                        var crmPatient = _context.CrmPatients.FirstOrDefault(e => e.Id == user2.CrmPatientsId && e.CrmUsersId == null);
                        if (crmPatient != null)
                        {
                            crmPatient.CrmUsersId = user2.Id;
                            _context.CrmPatients.Update(crmPatient);
                            _context.SaveChanges();
                        }
                    }

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

                var user3 = new CrmUsers
                {
                    UserName = "user_patient",
                    Email = "user_patient@crm.kz",
                    CrmPatient = new CrmPatients()
                    {
                        SurnameRu = "Пациент2",
                        SurnameKz = "Пациент2",
                        SurnameEn = "Patient2",
                        NameRu = "Пациент2",
                        NameKz = "Пациент2",
                        NameEn = "Patient2",
                        BirthDate = DateTime.Now,
                        IsActive = true,
                        CreatedDateTime = DateTime.Now
                    },
                    CreatedDateTime = DateTime.Now
                };
                var result3 = _userManager.CreateAsync(user3, "1q@W3e$R").GetAwaiter().GetResult();

                if (result3.Succeeded)
                {
                    _logger.LogWarning($"user_patient SUCCESSFULLY ADDED!");

                    if (user3.Id > 0 && user3.CrmEmployeesId > 0)
                    {
                        var crmEmployee = _context.CrmEmployees.FirstOrDefault(e => e.Id == user3.CrmEmployeesId && e.CrmUsersId == null);
                        if (crmEmployee != null)
                        {
                            crmEmployee.CrmUsersId = user3.Id;
                            _context.CrmEmployees.Update(crmEmployee);
                            _context.SaveChanges();
                        }
                    }
                    if (user3.Id > 0 && user3.CrmPatientsId > 0)
                    {
                        var crmPatient = _context.CrmPatients.FirstOrDefault(e => e.Id == user3.CrmPatientsId && e.CrmUsersId == null);
                        if (crmPatient != null)
                        {
                            crmPatient.CrmUsersId = user3.Id;
                            _context.CrmPatients.Update(crmPatient);
                            _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    _logger.LogError($"SMTH WENT WRONG WHILE ADDING user_patient!");
                }
            }

            if (!_context.DictCountries.Any())
            {
                _context.DictCountries.Add(new DictCountries()
                {
                    NameEn = "Kazakhstan",
                    NameRu = "Казахстан",
                    NameKz = "Казахстан",
                    CreatedDateTime = DateTime.Now
                });
                _context.SaveChanges();
            }

            if (!_context.DictGenders.Any())
            {
                _context.DictGenders.Add(new DictGenders()
                {
                    NameEn = "Male",
                    NameRu = "Муж",
                    NameKz = "Муж",
                    CreatedDateTime = DateTime.Now
                });
                _context.DictGenders.Add(new DictGenders()
                {
                    NameEn = "Female",
                    NameRu = "Жен",
                    NameKz = "Жен",
                    CreatedDateTime = DateTime.Now
                });
                _context.DictGenders.Add(new DictGenders()
                {
                    NameEn = "Anonim",
                    NameRu = "Анонимно",
                    NameKz = "Анонимно",
                    CreatedDateTime = DateTime.Now
                });
                _context.SaveChanges();
            }

            if (!_context.DictStatuses.Any())
            {
                _context.DictStatuses.Add(new DictStatuses()
                {
                    NameEn = "Created",
                    NameRu = "Заполнено",
                    NameKz = "Заполнено",
                    CreatedDateTime = DateTime.Now
                });
                _context.DictStatuses.Add(new DictStatuses()
                {
                    NameEn = "Approved",
                    NameRu = "Подтверждено",
                    NameKz = "Подтверждено",
                    CreatedDateTime = DateTime.Now
                });
                _context.DictStatuses.Add(new DictStatuses()
                {
                    NameEn = "Rejected",
                    NameRu = "Отклонено",
                    NameKz = "Отклонено",
                    CreatedDateTime = DateTime.Now
                });
                _context.SaveChanges();
            }

            if (!_context.DictEnterprises.Any())
            {
                var enterprise = new DictEnterprises()
                {
                    NameEn = "Test company",
                    NameRu = "Тест компания",
                    NameKz = "Тест компания",
                    Address = "Astana",
                    PhoneNumber = "87777",
                    CreatedDateTime = DateTime.Now
                };
                _context.DictEnterprises.Add(enterprise);
                _context.SaveChanges();

                _context.DictPositions.Add(new DictPositions()
                {
                    NameEn = "Test position",
                    NameRu = "Тест позиция",
                    NameKz = "Тест позиция",
                    DictEnterprisesId = enterprise.Id,
                    Code = "001",
                    Category = "1",
                    DescriptionEn = "Descr",
                    DescriptionRu = "Опис",
                    DescriptionKz = "Опис",
                    CreatedDateTime = DateTime.Now
                });
                _context.SaveChanges();
            }

            if (!_context.DictDepartments.Any())
            {
                var enterprise = _context.DictEnterprises.FirstOrDefault();

                _context.DictDepartments.Add(new DictDepartments()
                {
                    NameEn = "Test department",
                    NameRu = "Тест департамент",
                    NameKz = "Тест департамент",
                    DictEnterprisesId = enterprise?.Id,
                    CreatedDateTime = DateTime.Now
                });
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {

        }
    }
}
