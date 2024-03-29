﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CRM.DataModel.Data;
using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using CRM.Services.Helpers;
using CRM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CRM.Admin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "superadmin,admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private CrmDbContext _crmContext;
        private static ICacheManager _cacheManager;
        private readonly UserManager<CrmUsers> _userManager;
        private static ILogger<AdminController> _logger;

        public AdminController(CrmConfiguration configuration,
            ILogger<AdminController> logger,
            ICacheManager cacheManager,
            CrmDbContext crmContext,
            UserManager<CrmUsers> userManager)
        {
            _configuration = configuration;
            _logger = logger;
            _cacheManager = cacheManager;
            _crmContext = crmContext;
            _userManager = userManager;
        }

        [HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsers([FromBody] FilterDto filterDto)
        {
            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null)
                {
                    var result = new ResultDto<List<UserEditDto>>()
                    {
                        IsSuccess = true
                    };

                    var isSuperAdmin = false;
                    if (await _userManager.IsInRoleAsync(await _userManager.FindByNameAsync(currentUser.UserName), "superadmin"))
                    {
                        isSuperAdmin = true;
                    }

                    var endFilter = filterDto.EndDate.AddDays(1);

                    result.Data = await _crmContext.Users
                        .Include("UserRoles.Role")
                        .Include("CrmEmployee.DictDepartment")
                        .AsNoTracking()
                        .Where(u => (filterDto.DepartmentId == null || filterDto.DepartmentId == 0 || filterDto.DepartmentId > 0 && u.CrmEmployee != null && u.CrmEmployee.DictDepartmentsId == filterDto.DepartmentId) &&
                            u.DeletedDateTime == null &&
                            (isSuperAdmin || !isSuperAdmin && u.UserRoles.All(r => r.Role != null && r.Role.Description != null && r.Role.Description != "")) && 
                            (filterDto.filterByCreatedDate && u.CreatedDateTime >= filterDto.BeginDate && u.CreatedDateTime <= endFilter || !filterDto.filterByCreatedDate))
                        .AsNoTracking()
                        .Select(u =>
                            new UserEditDto
                            {
                                Id = u.Id,
                                CreatedDateTimeStr = u.CreatedDateTime.ToString("dd.MM.yyyy HH:mm"),
                                Iin = u.CrmEmployee != null ? u.CrmEmployee.Iin : null,
                                BirthDate = u.CrmEmployee != null ? u.CrmEmployee.BirthDate : null,
                                MiddlenameKz = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameKz : null,
                                MiddlenameRu = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameRu : null,
                                MiddlenameEn = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameEn : null,
                                NameKz = u.CrmEmployee != null ? u.CrmEmployee.NameKz : null,
                                NameRu = u.CrmEmployee != null ? u.CrmEmployee.NameRu : null,
                                NameEn = u.CrmEmployee != null ? u.CrmEmployee.NameEn : null,
                                SurnameKz = u.CrmEmployee != null ? u.CrmEmployee.SurnameKz : null,
                                SurnameRu = u.CrmEmployee != null ? u.CrmEmployee.SurnameRu : null,
                                SurnameEn = u.CrmEmployee != null ? u.CrmEmployee.SurnameEn : null,
                                DictPositionsId = u.CrmEmployee != null ? u.CrmEmployee.DictPositionsId : null,
                                DictDepartmentsId = u.CrmEmployee != null ? u.CrmEmployee.DictDepartmentsId : null,
                                DepartmentNameRu = u.CrmEmployee != null && u.CrmEmployee.DictDepartment != null ? u.CrmEmployee.DictDepartment.NameRu : null,
                                DepartmentNameKz = u.CrmEmployee != null && u.CrmEmployee.DictDepartment != null ? u.CrmEmployee.DictDepartment.NameKz : null,
                                DepartmentNameEn = u.CrmEmployee != null && u.CrmEmployee.DictDepartment != null ? u.CrmEmployee.DictDepartment.NameEn : null,
                                PositionNameRu = u.CrmEmployee != null && u.CrmEmployee.DictPosition != null ? u.CrmEmployee.DictPosition.NameRu : null,
                                PositionNameKz = u.CrmEmployee != null && u.CrmEmployee.DictPosition != null ? u.CrmEmployee.DictPosition.NameKz : null,
                                PositionNameEn = u.CrmEmployee != null && u.CrmEmployee.DictPosition != null ? u.CrmEmployee.DictPosition.NameEn : null,
                                Email = u.Email,
                                UserName = u.UserName
                            })
                        .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                        .Skip(filterDto.Page * filterDto.RowsPerPage)
                        .Take(filterDto.RowsPerPage)
                        .ToListAsync();

                    foreach(var user in result.Data)
                    {
                        var roles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(user.UserName));
                        if (roles.Count > 0)
                        {
                            var roleName = roles.FirstOrDefault();
                            var role = await _crmContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                            user.RoleName = role?.Description ?? role?.Name ?? null;
                        }
                    }

                    result.RowsCount = await _crmContext.Users
                        .Include("UserRoles.Role")
                        .Include("CrmEmployee.DictDepartment")
                        .AsNoTracking()
                        .Where(u => (filterDto.DepartmentId == null || filterDto.DepartmentId == 0 || filterDto.DepartmentId > 0 && u.CrmEmployee != null && u.CrmEmployee.DictDepartmentsId == filterDto.DepartmentId) &&
                            u.DeletedDateTime == null &&
                            (isSuperAdmin || !isSuperAdmin && u.UserRoles.All(r => r.Role != null && r.Role.Description != null && r.Role.Description != "")) &&
                            (filterDto.filterByCreatedDate && u.CreatedDateTime >= filterDto.BeginDate && u.CreatedDateTime <= endFilter || !filterDto.filterByCreatedDate))
                        .AsNoTracking()
                        .CountAsync();

                    return Ok(result);
                }
                else
                {
                    var emptyUser = new ResultDto<string>()
                    {
                        IsSuccess = false,
                        Msg = "empty_user"
                    };
                    return Ok(emptyUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetDepartments. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetEnterprises")]
        public async Task<IActionResult> GetEnterprises()
        {
            try
            {
                var enterprises = await _crmContext.DictEnterprises
                    .Where(d => d.DeletedDateTime == null)
                    .Include("ParentEnterprise")
                    .AsNoTracking()
                    .Select(d => new SelectWithParentDto()
                    {
                        Id = d.Id,
                        NameRu = d.NameRu,
                        NameEn = d.NameEn,
                        NameKz = d.NameKz,
                        ParentId = d.ParentId,
                        ParentNameEn = d.ParentEnterprise != null ? d.ParentEnterprise.NameEn : null,
                        ParentNameKz = d.ParentEnterprise != null ? d.ParentEnterprise.NameKz : null,
                        ParentNameRu = d.ParentEnterprise != null ? d.ParentEnterprise.NameRu : null
                    })
                    .OrderBy(d => d.ParentId)
                    .ToListAsync();
                return Ok(enterprises);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetEnterprises. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _crmContext.DictDepartments
                    .Where(d => d.DeletedDateTime == null)
                    .AsNoTracking()
                    .Select(d => new SelectDto()
                    {
                        Id = d.Id,
                        NameRu = d.NameRu,
                        NameEn = d.NameEn,
                        NameKz = d.NameKz
                    })
                    .ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetDepartments. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetPositions")]
        public async Task<IActionResult> GetPositions()
        {
            try
            {
                var positions = await _crmContext.DictPositions
                    .Where(p => p.DeletedDateTime == null)
                    .AsNoTracking()
                    .Select(p => new SelectDto()
                    {
                        Id = p.Id,
                        NameRu = p.NameRu,
                        NameEn = p.NameEn,
                        NameKz = p.NameKz
                    })
                    .ToListAsync();
                return Ok(positions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetPositions. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var isSuperAdmin = false;
                if (UserHelper.TryGetCurrentName(this.User, out string username) && await _userManager.IsInRoleAsync(await _userManager.FindByNameAsync(username), "superadmin"))
                {
                    isSuperAdmin = true;
                }

                var roles = await _crmContext.Roles
                    .Where(r => isSuperAdmin || !isSuperAdmin && r.Description != null && r.Description != "")
                    .AsNoTracking()
                    .Select(r => new SelectDto()
                    {
                        Id = r.Id,
                        Code = r.Name,
                        NameRu = r.Description != null
                            ? r.Description
                            : isSuperAdmin
                                ? r.Name
                                : ""
                    })
                    .ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetRoles. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser([FromBody] UserEditDto userData)
        {
            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null)
                {
                    if (userData != null)
                    {
                        if (userData.RoleId <= 0)
                        {
                            var emptyRole = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_role"
                            };
                            return Ok(emptyRole);
                        }

                        if (userData.RoleId > 1 && (userData.Enterprise == null || userData.Enterprise.Id <= 0))
                        {
                            var emptyEnterprise = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_enterprise"
                            };
                            return Ok(emptyEnterprise);
                        }

                        if (userData.RoleId > 2 && (userData.Department == null || userData.Department.Id <= 0))
                        {
                            var emptyDepartment = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_department"
                            };
                            return Ok(emptyDepartment);
                        }

                        if (userData.RoleId > 2 && (userData.Position == null || userData.Position.Id <= 0))
                        {
                            var emptyPosition = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_position"
                            };
                            return Ok(emptyPosition);
                        }

                        if (userData.Id == 0 && string.IsNullOrEmpty(userData.UserSecret))
                        {
                            var emptySecret = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_secret"
                            };
                            return Ok(emptySecret);
                        }

                        CrmUsers user = new CrmUsers()
                        {
                            Id = userData.Id,
                            UserName = userData.UserName,
                            CreatedDateTime = DateTime.Now,
                            Email = userData.Email
                        };

                        if (userData.CrmEmployeesId > 0 && await _crmContext.CrmEmployees.AnyAsync(c => c.Id == userData.CrmEmployeesId))
                        {
                            user.CrmEmployeesId = userData.CrmEmployeesId;
                            user.CrmEmployee = await _crmContext.CrmEmployees.FirstOrDefaultAsync(c => c.Id == userData.CrmEmployeesId);
                            user.CrmEmployee.EditorId = currentUser.Id;
                            user.CrmEmployee.EditedDateTime = DateTime.Now;
                        }
                        else if (userData.CrmPatientsId > 0 && await _crmContext.CrmPatients.AnyAsync(c => c.Id == userData.CrmPatientsId))
                        {
                            user.CrmPatientsId = userData.CrmPatientsId;
                            user.CrmPatient = await _crmContext.CrmPatients.FirstOrDefaultAsync(c => c.Id == userData.CrmPatientsId);
                            user.CrmPatient.EditorId = currentUser.Id;
                            user.CrmPatient.EditedDateTime = DateTime.Now;
                        }
                        else
                        {
                            user.CrmEmployeesId = 0;
                            user.CrmEmployee = new CrmEmployees()
                            {
                                Id = 0,
                                SurnameRu = userData.SurnameRu,
                                SurnameKz = userData.SurnameKz,
                                SurnameEn = userData.SurnameEn,
                                NameRu = userData.NameRu,
                                NameKz = userData.NameKz,
                                NameEn = userData.NameEn,
                                MiddlenameRu = userData.MiddlenameRu,
                                MiddlenameKz = userData.MiddlenameKz,
                                MiddlenameEn = userData.MiddlenameEn,
                                DictEnterprisesId = userData.Enterprise.Id,
                                DictDepartmentsId = userData.Department.Id,
                                DictPositionsId = userData.Position.Id,
                                Iin = userData.Iin,
                                IsActive = userData.IsActive,
                                BirthDate = userData.BirthDate,
                                CreatedDateTime = DateTime.Now,
                                AuthorId = currentUser.Id
                            };
                        }

                        if (user.Id > 0)
                        {
                            user.EditedDateTime = DateTime.Now;

                            var updateUserResult = await _userManager.UpdateAsync(user);

                            if (!updateUserResult.Succeeded)
                            {
                                _logger.LogError($"ERROR SaveUser UPDATE. MSG: {JsonConvert.SerializeObject(updateUserResult.Errors)}");
                                var updateErr = new ResultDto<string>()
                                {
                                    IsSuccess = false,
                                    Msg = JsonConvert.SerializeObject(updateUserResult.Errors)
                                };
                                return Ok(updateErr);
                            }
                            else
                            {
                                if (user.Id > 0 && user.CrmEmployeesId > 0 && await _crmContext.CrmEmployees.AnyAsync(e => e.Id == user.CrmEmployeesId && e.CrmUsersId == null))
                                {
                                    var crmEmployee = await _crmContext.CrmEmployees.FirstOrDefaultAsync(e => e.Id == user.CrmEmployeesId && e.CrmUsersId == null);
                                    if (crmEmployee != null)
                                    {
                                        crmEmployee.CrmUsersId = user.Id;
                                        _crmContext.CrmEmployees.Update(crmEmployee);
                                        await _crmContext.SaveChangesAsync();
                                    }
                                }
                                if (user.Id > 0 && user.CrmPatientsId > 0 && await _crmContext.CrmPatients.AnyAsync(e => e.Id == user.CrmPatientsId && e.CrmUsersId == null))
                                {
                                    var crmPatient = await _crmContext.CrmPatients.FirstOrDefaultAsync(e => e.Id == user.CrmPatientsId && e.CrmUsersId == null);
                                    if (crmPatient != null)
                                    {
                                        crmPatient.CrmUsersId = user.Id;
                                        _crmContext.CrmPatients.Update(crmPatient);
                                        await _crmContext.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                        else
                        {
                            user.CreatedDateTime = DateTime.Now;

                            var addUserResult = await _userManager.CreateAsync(user, userData.UserSecret);

                            if (!addUserResult.Succeeded)
                            {
                                _logger.LogError($"ERROR SaveUser ADD. MSG: {JsonConvert.SerializeObject(addUserResult.Errors)}");
                                var addErr = new ResultDto<string>()
                                {
                                    IsSuccess = false,
                                    Msg = JsonConvert.SerializeObject(addUserResult.Errors)
                                };
                                return Ok(addErr);
                            }
                            else
                            {
                                if (user.Id > 0 && user.CrmEmployeesId > 0)
                                {
                                    var crmEmployee = await _crmContext.CrmEmployees.FirstOrDefaultAsync(e => e.Id == user.CrmEmployeesId && e.CrmUsersId == null);
                                    if (crmEmployee != null)
                                    {
                                        crmEmployee.CrmUsersId = user.Id;
                                        _crmContext.CrmEmployees.Update(crmEmployee);
                                        await _crmContext.SaveChangesAsync();
                                    }
                                }
                                if (user.Id > 0 && user.CrmPatientsId > 0)
                                {
                                    var crmPatient = await _crmContext.CrmPatients.FirstOrDefaultAsync(e => e.Id == user.CrmPatientsId && e.CrmUsersId == null);
                                    if (crmPatient != null)
                                    {
                                        crmPatient.CrmUsersId = user.Id;
                                        _crmContext.CrmPatients.Update(crmPatient);
                                        await _crmContext.SaveChangesAsync();
                                    }
                                }
                            }
                        }

                        _cacheManager.Remove($"CrmUser_{userData.UserName}");

                        if (userData.RoleId > 0)
                        {
                            var role = await _crmContext.Roles.FirstOrDefaultAsync(r => r.Id == userData.RoleId);
                            if (role != null)
                            {
                                if (await _userManager.IsInRoleAsync(user, role.Name))
                                {
                                    var updateUserRoleExistsSuccess = new ResultDto<string>()
                                    {
                                        IsSuccess = true,
                                        Msg = role.Description
                                    };

                                    return Ok(updateUserRoleExistsSuccess);
                                }
                                else
                                {
                                    var oldRoles = await _userManager.GetRolesAsync(user);
                                    if (oldRoles.Count > 0)
                                    {
                                        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, oldRoles);
                                        if (removeRolesResult.Succeeded)
                                        {
                                            _logger.LogWarning($"WARNING SaveUser REMOVE ROLES. USER ID = {user.Id}; REMOVED ROLES = ({JsonConvert.SerializeObject(oldRoles)});");
                                        }
                                        else
                                        {
                                            _logger.LogError($"ERROR SaveUser REMOVE ROLES. USER ID = {user.Id}; TO REMOVE ROLES = ({JsonConvert.SerializeObject(oldRoles)}).  MSG: {JsonConvert.SerializeObject(removeRolesResult.Errors)}");
                                        }
                                    }
                                    var userRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                                    if (userRoleResult.Succeeded)
                                    {
                                        var updateUserWithRoleSuccess = new ResultDto<string>()
                                        {
                                            IsSuccess = true,
                                            Msg = role.Description
                                        };

                                        return Ok(updateUserWithRoleSuccess);
                                    }
                                    else
                                    {
                                        _logger.LogError($"ERROR SaveUser ADD ROLE. MSG: {JsonConvert.SerializeObject(userRoleResult.Errors)}");
                                        var addRoleErr = new ResultDto<string>()
                                        {
                                            IsSuccess = true,
                                            Msg = "role_err"
                                        };
                                        return Ok(addRoleErr);
                                    }
                                }
                            }
                            else
                            {
                                var roleNotFound = new ResultDto<string>()
                                {
                                    IsSuccess = true,
                                    Msg = "role_not_found"
                                };
                                return Ok(roleNotFound);
                            }
                        }
                        else
                        {
                            var updateUserSuccess = new ResultDto<string>()
                            {
                                IsSuccess = true,
                                Msg = "without_role"
                            };

                            return Ok(updateUserSuccess);
                        }
                    }
                    else
                    {
                        var emptyUserData = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = "empty_data"
                        };
                        return Ok(emptyUserData);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveUser. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(err);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        //[HttpGet("GetTreeViewData")]
        //public async Task<IActionResult> GetTreeViewData()
        //{
        //    try
        //    {
        //        var isSuperAdmin = false;
        //        if (UserHelper.TryGetCurrentName(this.User, out string username) && await _userManager.IsInRoleAsync(await _userManager.FindByNameAsync(username), "superadmin"))
        //        {
        //            isSuperAdmin = true;
        //        }

        //        var treeViewData = await _crmContext.CardsMatchingTree
        //            .Include("MatchingUser.Department")
        //            .Include("MatchingUser.Position")
        //            .Include("CardsType")
        //            .Where(c => (isSuperAdmin || !isSuperAdmin && c.DeletedDateTime == null) && c.ParentId == null)
        //            .AsNoTracking()
        //            .Select(c => new TreeViewDto() {
        //                TreeId = c.Id,
        //                ParentId = c.ParentId,
        //                MatchingUserId = c.MatchingUserId,
        //                CardsTypeId = c.CardsTypeId,
        //                CardsTypeNameRu = c.CardsType != null ? c.CardsType.NameRu : null,
        //                MatchingUserFullNameRu = c.MatchingUser != null
        //                    ? UserHelper.GetUserFullName(c.MatchingUser.SurnameRu, c.MatchingUser.NameRu, c.MatchingUser.MiddlenameRu)
        //                    : null,
        //                MatchingUserDepartmentsNameRu = c.MatchingUser != null && c.MatchingUser.Department != null
        //                    ? c.MatchingUser.Department.NameRu
        //                    : null,
        //                MatchingUserDepartmentsId = c.MatchingUser != null
        //                    ? c.MatchingUser.DepartmentsId
        //                    : 0,
        //                MatchingUserPositionsNameRu = c.MatchingUser != null && c.MatchingUser.Position != null
        //                    ? c.MatchingUser.Position.NameRu
        //                    : null,
        //                MatchingUserPositionsId = c.MatchingUser != null
        //                    ? c.MatchingUser.PositionsId
        //                    : 0,
        //                IsSubstitute = c.IsSubstitute,
        //                CreatedDateTimeStr = c.CreatedDateTime.ToString("dd.MM.yyyy HH:mm"),
        //                EditedDateTimeStr = c.EditedDateTime != null ? c.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm") : null,
        //                DeletedDateTimeStr = c.DeletedDateTime != null ? c.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm") : null,
        //                Expanded = true,
        //                NoDragging = true
        //            })
        //            .ToListAsync();

        //        var filledChildsTreeView = await FillChilds(_crmContext, treeViewData, isSuperAdmin);

        //        return Ok(filledChildsTreeView);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"ERROR GetTreeViewData. MSG: {JsonConvert.SerializeObject(ex)}");

        //        var err = new ResultDto<string>()
        //        {
        //            IsSuccess = false,
        //            Msg = JsonConvert.SerializeObject(ex)
        //        };
        //        return Ok(err);
        //    }
        //    finally
        //    {
        //        await _crmContext.DisposeAsync();
        //    }
        //}

        //private async Task<List<TreeViewDto>> FillChilds(AbDbContext dbContext, List<TreeViewDto> listData, bool isSuperAdmin)
        //{
        //    foreach (var data in listData)
        //    {
        //        data.Children = await dbContext.CardsMatchingTree
        //            .Where(c => (isSuperAdmin || !isSuperAdmin && c.DeletedDateTime == null) && c.ParentId == data.TreeId)
        //            .AsNoTracking()
        //            .Select(c => new TreeViewDto()
        //            {
        //                TreeId = c.Id,
        //                ParentId = c.ParentId,
        //                MatchingUserId = c.MatchingUserId,
        //                CardsTypeId = c.CardsTypeId,
        //                CardsTypeNameRu = c.CardsType != null ? c.CardsType.NameRu : null,
        //                MatchingUserFullNameRu = c.MatchingUser != null
        //                    ? UserHelper.GetUserFullName(c.MatchingUser.SurnameRu, c.MatchingUser.NameRu, c.MatchingUser.MiddlenameRu)
        //                    : null,
        //                MatchingUserDepartmentsNameRu = c.MatchingUser != null && c.MatchingUser.Department != null
        //                    ? c.MatchingUser.Department.NameRu
        //                    : null,
        //                MatchingUserDepartmentsId = c.MatchingUser != null
        //                    ? c.MatchingUser.DepartmentsId
        //                    : 0,
        //                MatchingUserPositionsNameRu = c.MatchingUser != null && c.MatchingUser.Position != null
        //                    ? c.MatchingUser.Position.NameRu
        //                    : null,
        //                MatchingUserPositionsId = c.MatchingUser != null
        //                    ? c.MatchingUser.PositionsId
        //                    : 0,
        //                IsSubstitute = c.IsSubstitute,
        //                CreatedDateTimeStr = c.CreatedDateTime.ToString("dd.MM.yyyy HH:mm"),
        //                EditedDateTimeStr = c.EditedDateTime != null ? c.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm") : null,
        //                DeletedDateTimeStr = c.DeletedDateTime != null ? c.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm") : null,
        //                Expanded = true,
        //                NoDragging = true
        //            })
        //            .ToListAsync();
        //        data.Children = await FillChilds(dbContext, data.Children, isSuperAdmin);
        //    }
        //    return listData;
        //}
    }
}