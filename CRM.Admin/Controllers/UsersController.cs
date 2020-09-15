using System;
using System.Collections.Generic;
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
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                var result = new ResultDto<string>()
                {
                    IsSuccess = false
                };

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));

                    if (!string.IsNullOrEmpty(currentUser.PhotoPath) && System.IO.File.Exists(currentUser.PhotoPath))
                    {
                        var content = await System.IO.File.ReadAllBytesAsync(currentUser.PhotoPath);
                        result.Data = Convert.ToBase64String(content);
                        result.IsSuccess = true;
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetCurrentUserPhoto. MSG: {JsonConvert.SerializeObject(ex)}");
                return BadRequest();
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetUsersBySearch")]
        public async Task<IActionResult> GetUsersBySearch(string searchData)
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
                    var users = await _crmContext.Users
                    .Include("CrmEmployee")
                    .Where(u => (!string.IsNullOrEmpty(searchData) && u.CrmEmployee != null &&
                        (u.CrmEmployee.SurnameRu != null && u.CrmEmployee.SurnameRu.ToLower().Contains(searchData.ToLower()) ||
                        u.CrmEmployee.NameRu != null && u.CrmEmployee.NameRu.ToLower().Contains(searchData.ToLower()) ||
                        u.CrmEmployee.MiddlenameRu != null && u.CrmEmployee.MiddlenameRu.ToLower().Contains(searchData.ToLower())) ||
                        (u.CrmEmployee.SurnameKz != null && u.CrmEmployee.SurnameKz.ToLower().Contains(searchData.ToLower()) ||
                        u.CrmEmployee.NameKz != null && u.CrmEmployee.NameKz.ToLower().Contains(searchData.ToLower()) ||
                        u.CrmEmployee.MiddlenameKz != null && u.CrmEmployee.MiddlenameKz.ToLower().Contains(searchData.ToLower())) ||
                        (u.CrmEmployee.SurnameEn != null && u.CrmEmployee.SurnameEn.ToLower().Contains(searchData.ToLower()) ||
                        u.CrmEmployee.NameEn != null && u.CrmEmployee.NameEn.ToLower().Contains(searchData.ToLower()) ||
                        u.CrmEmployee.MiddlenameEn != null && u.CrmEmployee.MiddlenameEn.ToLower().Contains(searchData.ToLower())) ||
                        string.IsNullOrEmpty(searchData) && u.Id < 30) && u.DeletedDateTime == null)
                    //.Select(u => new AbUsers()
                    //{
                    //    Id = u.Id,
                    //    MiddlenameKz = u.MiddlenameKz,
                    //    MiddlenameRu = u.MiddlenameRu,
                    //    NameKz = u.NameKz,
                    //    NameRu = u.NameRu,
                    //    SurnameKz = u.SurnameKz,
                    //    SurnameRu = u.SurnameRu,
                    //})
                    .Select(u => new SelectDto()
                    {
                        Id = u.Id,
                        NameRu = u.CrmEmployee != null ? UserHelper.GetUserFullName(u.CrmEmployee.SurnameRu, u.CrmEmployee.NameRu, u.CrmEmployee.MiddlenameRu) : null,
                        NameKz = u.CrmEmployee != null ? UserHelper.GetUserFullName(u.CrmEmployee.SurnameKz, u.CrmEmployee.NameKz, u.CrmEmployee.MiddlenameKz) : null,
                        NameEn = u.CrmEmployee != null ? UserHelper.GetUserFullName(u.CrmEmployee.SurnameEn, u.CrmEmployee.NameEn, u.CrmEmployee.MiddlenameEn) : null,
                    })
                    .ToListAsync();

                    //var selected = users
                    //    .Select(u => new SelectDto()
                    //    {
                    //        Id = u.Id,
                    //        NameRu = UserHelper.GetUserFullName(u.SurnameRu, u.NameRu, u.MiddlenameRu)
                    //    }).ToList();

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
    }
}