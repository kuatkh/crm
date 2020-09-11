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
        private CrmDbContext _abContext;
        private static ICacheManager _cacheManager;
        private static ILogger<UsersController> _logger;

        public UsersController(CrmConfiguration configuration,
            ILogger<UsersController> logger,
            ICacheManager cacheManager,
            CrmDbContext abContext)
        {
            _configuration = configuration;
            _logger = logger;
            _cacheManager = cacheManager;
            _abContext = abContext;
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
                    currentUser = await _abContext.Users
                        .Where(u => u.UserName == username)
                        .AsNoTracking()
                        .Select(u => new UserDto()
                        {
                            Id = u.Id,
                            Iin = u.Iin,
                            UserName = u.UserName,
                            BirthDate = u.BirthDate,
                            MiddlenameKz = u.MiddlenameKz,
                            MiddlenameRu = u.MiddlenameRu,
                            NameKz = u.NameKz,
                            NameRu = u.NameRu,
                            SurnameKz = u.SurnameKz,
                            SurnameRu = u.SurnameRu,
                            PhotoB64 = u.PhotoB64,
                            PhotoPath = u.PhotoPath
                        })
                        .FirstOrDefaultAsync();

                    currentUser.ShortNameRu = UserHelper.GetUserShortName(currentUser.SurnameRu, currentUser.NameRu, currentUser.MiddlenameRu);
                    currentUser.ShortNameKz = UserHelper.GetUserShortName(currentUser.SurnameKz, currentUser.NameKz, currentUser.MiddlenameKz);

                    if (!string.IsNullOrEmpty(currentUser.PhotoPath) && System.IO.File.Exists(currentUser.PhotoPath))
                    {
                        var content = await System.IO.File.ReadAllBytesAsync(currentUser.PhotoPath);
                        currentUser.PhotoB64 = Convert.ToBase64String(content);
                    }

                    _cacheManager.Set($"AbUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
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
                await _abContext.DisposeAsync();
            }
        }

    }
}