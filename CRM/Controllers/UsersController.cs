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

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "user,superadmin,admin")]
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

                    if (currentUser != null)
                    {
                        string photoPath = null;
                        if (currentUser.CrmPatientsId != null)
                        {
                            photoPath = (await _crmContext.CrmPatients.FirstOrDefaultAsync(e => e.Id == currentUser.CrmPatientsId))?.PhotoPath;
                        }
                        else
                        {
                            photoPath = (await _crmContext.CrmEmployees.FirstOrDefaultAsync(e => e.Id == currentUser.CrmEmployeesId))?.PhotoPath;
                        }
                        result.IsSuccess = true;
                        result.Data = await UserHelper.GetUserPhotoByPath(photoPath);
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

    }
}