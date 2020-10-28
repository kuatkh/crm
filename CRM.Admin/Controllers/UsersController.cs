using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CRM.DataModel.Data;
using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using CRM.Services.Helpers;
using CRM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CRM.Admin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "superadmin,admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private CrmDbContext _crmContext;
        private static ICacheManager _cacheManager;
        private static ILogger<UsersController> _logger;

        public UsersController(CrmConfiguration configuration,
            ILogger<UsersController> logger,
            ICacheManager cacheManager,
            CrmDbContext crmContext)
        {
            _configuration = configuration;
            _logger = logger;
            _cacheManager = cacheManager;
            _crmContext = crmContext;
        }

        [HttpGet("GetCurrentUserData")]
        public async Task<IActionResult> GetCurrentUserData()
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

                return Ok(currentUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetCurrentUserData. MSG: {JsonConvert.SerializeObject(ex)}");
                return BadRequest();
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetCurrentUserPhoto")]
        public async Task<IActionResult> GetCurrentUserPhoto()
        {
            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);

                string photoPath = (string)_cacheManager.Get($"CrmUser_PhotoPath_{username}");
                if (string.IsNullOrEmpty(photoPath))
                {
                    photoPath = await UserHelper.GetUserPhotoPathByUserName(_crmContext, username);
                    if (!string.IsNullOrEmpty(photoPath))
                    {
                        _cacheManager.Set($"CrmUser_PhotoPath_{username}", photoPath, new TimeSpan(7, 0, 0, 0));
                    }
                    else
                    {
                        _cacheManager.Set($"CrmUser_PhotoPath_{username}", "empty", new TimeSpan(7, 0, 0, 0));
                    }
                }

                string photoB64 = null;

                if (!string.IsNullOrEmpty(photoPath) && photoPath != "empty")
                {
                    photoB64 = await UserHelper.GetUserPhotoByPath(photoPath);
                }

                var result = new ResultDto<string>()
                {
                    IsSuccess = true,
                    Data = photoB64
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetCurrentUserPhoto. MSG: {JsonConvert.SerializeObject(ex)}");

                var errResult = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(errResult);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetUsersBySearch")]
        public async Task<IActionResult> GetUsersBySearch(string searchData, bool toAppointment = false)
        {
            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                _logger.LogError($"searchData = {searchData}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null)
                {
                    var users = await _crmContext.CrmEmployees
                    .Include("DictPosition")
                    .Where(u => (!string.IsNullOrEmpty(searchData) && u.DictPosition != null &&
                        (toAppointment && (_configuration.AppointmentPositions == null || !_configuration.AppointmentPositions.Any() || 
                            u.DictPosition != null && _configuration.AppointmentPositions != null && _configuration.AppointmentPositions.Any(c => c == u.DictPosition.Code)) ||
                            !toAppointment) &&
                        ((u.SurnameRu != null && u.SurnameRu.ToLower().Contains(searchData.ToLower()) ||
                        u.NameRu != null && u.NameRu.ToLower().Contains(searchData.ToLower()) ||
                        u.MiddlenameRu != null && u.MiddlenameRu.ToLower().Contains(searchData.ToLower())) ||
                        (u.SurnameKz != null && u.SurnameKz.ToLower().Contains(searchData.ToLower()) ||
                        u.NameKz != null && u.NameKz.ToLower().Contains(searchData.ToLower()) ||
                        u.MiddlenameKz != null && u.MiddlenameKz.ToLower().Contains(searchData.ToLower())) ||
                        (u.SurnameEn != null && u.SurnameEn.ToLower().Contains(searchData.ToLower()) ||
                        u.NameEn != null && u.NameEn.ToLower().Contains(searchData.ToLower()) ||
                        u.MiddlenameEn != null && u.MiddlenameEn.ToLower().Contains(searchData.ToLower()))) ||
                        string.IsNullOrEmpty(searchData) && u.Id < 30) && u.DeletedDateTime == null)
                    .Select(u => new SelectWithPositionDto()
                    {
                        Id = u.Id,
                        NameRu = UserHelper.GetUserFullName(u.SurnameRu, u.NameRu, u.MiddlenameRu),
                        NameKz = UserHelper.GetUserFullName(u.SurnameKz, u.NameKz, u.MiddlenameKz),
                        NameEn = UserHelper.GetUserFullName(u.SurnameEn, u.NameEn, u.MiddlenameEn),
                        PositionId = toAppointment ? u.DictPositionsId : null,
                        PositionNameEn = toAppointment && u.DictPosition != null ? u.DictPosition.NameEn : null,
                        PositionNameRu = toAppointment && u.DictPosition != null ? u.DictPosition.NameRu : null,
                        PositionNameKz = toAppointment && u.DictPosition != null ? u.DictPosition.NameKz : null,
                    })
                    .OrderBy(u => u.PositionId)
                    .ThenBy(u => u.Id)
                    .ToListAsync();

                    return Ok(users);
                }
                else
                {
                    _logger.LogError($"ERROR GetUsersBySearch. MSG: CURRENT USER IS EMPTY");
                    return Ok(new List<CrmUsers>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetUsersBySearch. MSG: {JsonConvert.SerializeObject(ex)}");
                return Ok(new List<CrmUsers>());
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile(long? userId = null)
        {
            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);

                var userData = await _crmContext.Users
                    .Include("UserRoles")
                    .Include("CrmEmployee.DictEnterprise")
                    .Include("CrmEmployee.DictEnterpriseBranches")
                    .Include("CrmEmployee.DictDepartment")
                    .Include("CrmEmployee.DictPosition")
                    .Include("CrmEmployee.DictCity")
                    .Include("CrmPatient.DictCity")
                    .Where(u => (userId != null && u.Id == userId || userId == null && u.UserName == username) && u.DeletedDateTime == null &&
                        (u.CrmEmployee != null && u.CrmEmployee.DeletedDateTime == null && u.CrmEmployee.IsActive == true || u.CrmPatient != null && u.CrmPatient.DeletedDateTime == null && u.CrmPatient.IsActive == true))
                    .AsNoTracking()
                    .Select(u => new UserEditDto()
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        CrmEmployeesId = u.CrmEmployeesId,
                        CrmPatientsId = u.CrmPatientsId,
                        Iin = u.CrmEmployee != null ? u.CrmEmployee.Iin : u.CrmPatient != null ? u.CrmPatient.Iin : null,
                        BirthDate = u.CrmEmployee != null ? u.CrmEmployee.BirthDate : u.CrmPatient != null ? u.CrmPatient.BirthDate : null,
                        MiddlenameKz = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameKz : u.CrmPatient != null ? u.CrmPatient.MiddlenameKz : null,
                        MiddlenameRu = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameRu : u.CrmPatient != null ? u.CrmPatient.MiddlenameRu : null,
                        MiddlenameEn = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameEn : u.CrmPatient != null ? u.CrmPatient.MiddlenameEn : null,
                        NameKz = u.CrmEmployee != null ? u.CrmEmployee.NameKz : u.CrmPatient != null ? u.CrmPatient.NameKz : null,
                        NameRu = u.CrmEmployee != null ? u.CrmEmployee.NameRu : u.CrmPatient != null ? u.CrmPatient.NameRu : null,
                        NameEn = u.CrmEmployee != null ? u.CrmEmployee.NameEn : u.CrmPatient != null ? u.CrmPatient.NameEn : null,
                        SurnameKz = u.CrmEmployee != null ? u.CrmEmployee.SurnameKz : u.CrmPatient != null ? u.CrmPatient.SurnameKz : null,
                        SurnameRu = u.CrmEmployee != null ? u.CrmEmployee.SurnameRu : u.CrmPatient != null ? u.CrmPatient.SurnameRu : null,
                        SurnameEn = u.CrmEmployee != null ? u.CrmEmployee.SurnameEn : u.CrmPatient != null ? u.CrmPatient.SurnameEn : null,
                        DictGendersId = u.CrmEmployee != null
                            ? u.CrmEmployee.DictGendersId
                            : u.CrmPatient != null && u.CrmPatient.DictGendersId != null
                                ? u.CrmPatient.DictGendersId
                                : null,
                        DictEnterprisesId = u.CrmEmployee != null ? u.CrmEmployee.DictEnterprisesId : null,
                        DictPositionsId = u.CrmEmployee != null ? u.CrmEmployee.DictPositionsId : null,
                        DictDepartmentsId = u.CrmEmployee != null ? u.CrmEmployee.DictDepartmentsId : null,
                        IsActive = u.CrmEmployee != null ? u.CrmEmployee.IsActive : u.CrmPatient != null ? u.CrmPatient.IsActive : false,
                        //BirthDateStr = u.CrmEmployee != null && u.CrmEmployee.BirthDate != null
                        //? u.CrmEmployee.BirthDate.Value.ToString("dd.MM.yyyy")
                        //: u.CrmPatient != null && u.CrmPatient.BirthDate != null
                        //    ? u.CrmPatient.BirthDate.Value.ToString("dd.MM.yyyy")
                        //    : null,
                        RoleId = u.UserRoles.Any() && u.UserRoles.FirstOrDefault() != null ? u.UserRoles.FirstOrDefault().RoleId : 0,
                        Address = u.CrmEmployee != null ? u.CrmEmployee.Address : u.CrmPatient != null ? u.CrmPatient.Address : null,
                        PhoneNumber = u.PhoneNumber,
                        AboutMe = u.CrmEmployee != null ? u.CrmEmployee.AboutMe : u.CrmPatient != null ? u.CrmPatient.AboutMe : null,
                        Position = u.CrmEmployee != null && u.CrmEmployee.DictPosition != null
                            ? new SelectDto()
                            {
                                Id = u.CrmEmployee.DictPosition.Id,
                                NameKz = u.CrmEmployee.DictPosition.NameKz,
                                NameRu = u.CrmEmployee.DictPosition.NameRu,
                                NameEn = u.CrmEmployee.DictPosition.NameEn
                            }
                            : null,
                        Department = u.CrmEmployee != null && u.CrmEmployee.DictDepartment != null
                            ? new SelectDto()
                            {
                                Id = u.CrmEmployee.DictDepartment.Id,
                                NameKz = u.CrmEmployee.DictDepartment.NameKz,
                                NameRu = u.CrmEmployee.DictDepartment.NameRu,
                                NameEn = u.CrmEmployee.DictDepartment.NameEn
                            }
                            : null,
                        Enterprise = u.CrmEmployee != null && u.CrmEmployee.DictEnterprise != null
                            ? new SelectDto()
                            {
                                Id = u.CrmEmployee.DictEnterprise.Id,
                                NameKz = u.CrmEmployee.DictEnterprise.NameKz,
                                NameRu = u.CrmEmployee.DictEnterprise.NameRu,
                                NameEn = u.CrmEmployee.DictEnterprise.NameEn
                            }
                            : null,
                        City = u.CrmEmployee != null && u.CrmEmployee.DictCity != null
                            ? new SelectDto()
                            {
                                Id = u.CrmEmployee.DictCity.Id,
                                NameKz = u.CrmEmployee.DictCity.NameKz,
                                NameRu = u.CrmEmployee.DictCity.NameRu,
                                NameEn = u.CrmEmployee.DictCity.NameEn
                            }
                            : u.CrmPatient != null && u.CrmPatient.DictCity != null
                                ? new SelectDto()
                                {
                                    Id = u.CrmPatient.DictCity.Id,
                                    NameKz = u.CrmPatient.DictCity.NameKz,
                                    NameRu = u.CrmPatient.DictCity.NameRu,
                                    NameEn = u.CrmPatient.DictCity.NameEn
                                }
                                : null,
                        Gender = u.CrmEmployee != null && u.CrmEmployee.DictGender != null
                            ? new SelectDto()
                            {
                                Id = u.CrmEmployee.DictGender.Id,
                                NameKz = u.CrmEmployee.DictGender.NameKz,
                                NameRu = u.CrmEmployee.DictGender.NameRu,
                                NameEn = u.CrmEmployee.DictGender.NameEn
                            }
                            : u.CrmPatient != null && u.CrmPatient.DictGender != null
                                ? new SelectDto()
                                {
                                    Id = u.CrmPatient.DictGender.Id,
                                    NameKz = u.CrmPatient.DictGender.NameKz,
                                    NameRu = u.CrmPatient.DictGender.NameRu,
                                    NameEn = u.CrmPatient.DictGender.NameEn
                                }
                                : null
                    })
                    .FirstOrDefaultAsync();

                var result = new ResultDto<UserEditDto>()
                {
                    IsSuccess = true,
                    Data = userData
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetProfile. MSG: {JsonConvert.SerializeObject(ex)}");

                var errResult = new ResultDto<UserEditDto>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(errResult);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveProfile")]
        public async Task<IActionResult> SaveProfile([FromBody] UserEditDto profileData)
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

                if (currentUser != null && profileData != null)
                {
                    if (profileData.CrmEmployeesId != null)
                    {
                        var crmEmployee = await _crmContext.CrmEmployees.FirstOrDefaultAsync(c => c.Id == profileData.CrmEmployeesId && c.DeletedDateTime == null);
                        if (crmEmployee != null)
                        {
                            crmEmployee.NameRu = profileData.NameRu;
                            crmEmployee.SurnameRu = profileData.SurnameRu;
                            crmEmployee.MiddlenameRu = profileData.MiddlenameRu;
                            crmEmployee.AboutMe = profileData.AboutMe;
                            crmEmployee.PhoneNumber = profileData.PhoneNumber;

                            _crmContext.CrmEmployees.Update(crmEmployee);
                            await _crmContext.SaveChangesAsync();
                        }
                        else
                        {
                            _logger.LogError($"ERROR SaveProfile. empty_employee. data: {JsonConvert.SerializeObject(profileData)}");

                            var emptyEmployee = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_employee"
                            };

                            return Ok(emptyEmployee);
                        }
                    }
                    else if (profileData.CrmPatientsId != null)
                    {
                        var crmPatient = await _crmContext.CrmPatients.FirstOrDefaultAsync(c => c.Id == profileData.CrmPatientsId && c.DeletedDateTime == null);
                        if (crmPatient != null)
                        {
                            crmPatient.NameRu = profileData.NameRu;
                            crmPatient.SurnameRu = profileData.SurnameRu;
                            crmPatient.MiddlenameRu = profileData.MiddlenameRu;
                            crmPatient.AboutMe = profileData.AboutMe;
                            crmPatient.PhoneNumber = profileData.PhoneNumber;

                            _crmContext.CrmPatients.Update(crmPatient);
                            await _crmContext.SaveChangesAsync();
                        }
                        else
                        {
                            _logger.LogError($"ERROR SaveProfile. empty_patient. data: {JsonConvert.SerializeObject(profileData)}");

                            var emptyPatient = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_patient"
                            };

                            return Ok(emptyPatient);
                        }
                    }
                    else if (profileData.Id > 0)
                    {
                        var crmUser = await _crmContext.Users
                            .Include("CrmEmployee")
                            .Include("CrmPatient")
                            .FirstOrDefaultAsync(c => c.Id == profileData.Id && c.DeletedDateTime == null && (c.CrmEmployee != null && c.CrmEmployee.DeletedDateTime == null || c.CrmPatient != null && c.CrmPatient.DeletedDateTime == null));
                        if (crmUser != null)
                        {
                            if (crmUser.CrmEmployee != null)
                            {
                                crmUser.CrmEmployee.NameRu = profileData.NameRu;
                                crmUser.CrmEmployee.SurnameRu = profileData.SurnameRu;
                                crmUser.CrmEmployee.MiddlenameRu = profileData.MiddlenameRu;
                                crmUser.CrmEmployee.AboutMe = profileData.AboutMe;
                                crmUser.CrmEmployee.PhoneNumber = profileData.PhoneNumber;

                                _crmContext.CrmEmployees.Update(crmUser.CrmEmployee);
                                await _crmContext.SaveChangesAsync();
                            }
                            else if (crmUser.CrmPatient != null)
                            {
                                crmUser.CrmPatient.NameRu = profileData.NameRu;
                                crmUser.CrmPatient.SurnameRu = profileData.SurnameRu;
                                crmUser.CrmPatient.MiddlenameRu = profileData.MiddlenameRu;
                                crmUser.CrmPatient.AboutMe = profileData.AboutMe;
                                crmUser.CrmPatient.PhoneNumber = profileData.PhoneNumber;

                                _crmContext.CrmPatients.Update(crmUser.CrmPatient);
                                await _crmContext.SaveChangesAsync();
                            }
                            else
                            {
                                _logger.LogError($"ERROR SaveProfile. empty_employee_and_patient. data: {JsonConvert.SerializeObject(profileData)}");

                                var emptyBoth = new ResultDto<string>()
                                {
                                    IsSuccess = false,
                                    Msg = "empty_employee_and_patient"
                                };

                                return Ok(emptyBoth);
                            }
                        }
                        else
                        {
                            _logger.LogError($"ERROR SaveProfile. empty_user. data: {JsonConvert.SerializeObject(profileData)}");

                            var emptyUser = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_user"
                            };

                            return Ok(emptyUser);
                        }
                    }
                    else
                    {
                        _logger.LogError($"ERROR SaveProfile. empty_id. data: {JsonConvert.SerializeObject(profileData)}");

                        var emptyIdResult = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = "empty_id"
                        };

                        return Ok(emptyIdResult);
                    }

                    var successResult = new ResultDto<string>()
                    {
                        IsSuccess = true
                    };

                    return Ok(successResult);
                }
                else
                {
                    _logger.LogError($"ERROR SaveProfile. empty_current_user_or_profile_data. data: {JsonConvert.SerializeObject(profileData)}");

                    var emptyResult = new ResultDto<string>()
                    {
                        IsSuccess = false,
                        Msg = "empty_current_user_or_profile_data"
                    };

                    return Ok(emptyResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveProfile. MSG: {JsonConvert.SerializeObject(ex)}");

                var errResult = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(errResult);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveProfilePhoto")]
        public async Task<IActionResult> SaveProfilePhoto([FromBody] UserEditDto profileData)
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

                if (currentUser != null && profileData != null)
                {
                    if (string.IsNullOrEmpty(profileData.PhotoB64))
                    {
                        var fileName = string.Empty;
                        CrmEmployees crmEmployee = null;
                        CrmPatients crmPatient = null;
                        if (profileData.CrmEmployeesId != null)
                        {
                            fileName = $"ProfilePhoto_Employee_{profileData.CrmEmployeesId}_{Guid.NewGuid().ToString()}.jpg";

                            crmEmployee = await _crmContext.CrmEmployees.FirstOrDefaultAsync(c => c.Id == profileData.CrmEmployeesId && c.DeletedDateTime == null);
                        }
                        else if (profileData.CrmPatientsId != null)
                        {
                            fileName = $"ProfilePhoto_Patient_{profileData.CrmPatientsId}_{Guid.NewGuid().ToString()}.jpg";

                            crmPatient = await _crmContext.CrmPatients.FirstOrDefaultAsync(c => c.Id == profileData.CrmPatientsId && c.DeletedDateTime == null);
                        }
                        else if (profileData.Id > 0)
                        {
                            fileName = $"ProfilePhoto_User_{profileData.Id}_{Guid.NewGuid().ToString()}.jpg";

                            var crmUser = await _crmContext.Users
                                .Include("CrmEmployee")
                                .Include("CrmPatient")
                                .FirstOrDefaultAsync(c => c.Id == profileData.Id && c.DeletedDateTime == null && (c.CrmEmployee != null && c.CrmEmployee.DeletedDateTime == null || c.CrmPatient != null && c.CrmPatient.DeletedDateTime == null));
                            if (crmUser != null && crmUser.CrmEmployee != null)
                            {
                                crmEmployee = crmUser.CrmEmployee;
                            }
                            else if (crmUser != null && crmUser.CrmPatient != null)
                            {
                                crmPatient = crmUser.CrmPatient;
                            }
                            else
                            {
                                _logger.LogError($"ERROR SaveProfilePhoto. empty_user. data: {JsonConvert.SerializeObject(profileData)}");

                                var emptyUser = new ResultDto<string>()
                                {
                                    IsSuccess = false,
                                    Msg = "empty_user"
                                };

                                return Ok(emptyUser);
                            }
                        }
                        else
                        {
                            _logger.LogError($"ERROR SaveProfilePhoto. empty_id. data: {JsonConvert.SerializeObject(profileData)}");

                            var emptyUserId = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_id"
                            };

                            return Ok(emptyUserId);
                        }

                        var filePath = Path.Combine(_configuration.ProfilePhotoPath, fileName);
                        using (MemoryStream mem = new MemoryStream())
                        {
                            var base64String = profileData.PhotoB64.IndexOf("base64,") > -1
                                ? profileData.PhotoB64.Split("base64,")[1]
                                : profileData.PhotoB64;
                            mem.Write(Convert.FromBase64String(base64String), 0, (int)Convert.FromBase64String(base64String).Length);
                            using (FileStream fileStream = new FileStream(filePath, System.IO.FileMode.CreateNew))
                            {
                                mem.WriteTo(fileStream);
                                mem.Close();
                                fileStream.Close();
                            }
                        }

                        if (crmEmployee != null)
                        {
                            crmEmployee.PhotoPath = filePath;
                            _crmContext.CrmEmployees.Update(crmEmployee);
                            await _crmContext.SaveChangesAsync();

                            _cacheManager.Remove($"CrmUser_PhotoPath_{username}");
                        }
                        else if (crmPatient != null)
                        {
                            crmPatient.PhotoPath = filePath;
                            _crmContext.CrmPatients.Update(crmPatient);
                            await _crmContext.SaveChangesAsync();

                            _cacheManager.Remove($"CrmUser_PhotoPath_{username}");
                        }
                        else
                        {
                            _logger.LogError($"ERROR SaveProfilePhoto. empty_employee_and_patient. data: {JsonConvert.SerializeObject(profileData)}");

                            var emptyBoth = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_employee_and_patient"
                            };

                            return Ok(emptyBoth);
                        }

                        var successResult = new ResultDto<string>()
                        {
                            IsSuccess = true
                        };

                        return Ok(successResult);
                    }
                    else
                    {
                        _logger.LogError($"ERROR SaveProfilePhoto. empty_photo. data: {JsonConvert.SerializeObject(profileData)}");

                        var emptyPhoto = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = "empty_photo"
                        };

                        return Ok(emptyPhoto);
                    }
                }
                else
                {
                    _logger.LogError($"ERROR SaveProfilePhoto. empty_current_user_or_profile_data. data: {JsonConvert.SerializeObject(profileData)}");

                    var result = new ResultDto<string>()
                    {
                        IsSuccess = false,
                        Msg = "empty_current_user_or_profile_data"
                    };

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveProfile. MSG: {JsonConvert.SerializeObject(ex)}");

                var errResult = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(errResult);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }
    }
}