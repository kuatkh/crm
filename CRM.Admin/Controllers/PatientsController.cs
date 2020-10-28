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
    public class PatientsController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private CrmDbContext _crmContext;
        private static ICacheManager _cacheManager;
        private static ILogger<PatientsController> _logger;

        public PatientsController(CrmConfiguration configuration,
            ILogger<PatientsController> logger,
            ICacheManager cacheManager,
            CrmDbContext crmContext)
        {
            _configuration = configuration;
            _logger = logger;
            _cacheManager = cacheManager;
            _crmContext = crmContext;
        }

        [HttpGet("GetPatientByDocumentNumber")]
        public async Task<IActionResult> GetPatientByDocumentNumber(string docNum)
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
                    var patient = await _crmContext.CrmPatients
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.DeletedDateTime == null &&
                            (p.Iin == docNum.Trim() || p.DocumentNumber == docNum.Trim()));

                    var success = new ResultDto<CrmPatients>()
                    {
                        IsSuccess = true,
                        Data = patient
                    };

                    return Ok(success);
                }
                else
                {
                    _logger.LogError($"ERROR GetPatientByDocumentNumber. EMPTY CURRENT USER. currentUser = {JsonConvert.SerializeObject(currentUser)}; docNum = {docNum}");

                    var emptyResult = new ResultDto<string>()
                    {
                        IsSuccess = false
                    };

                    return Ok(emptyResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetPatientByDocumentNumber. docNum = {docNum}. MSG: {JsonConvert.SerializeObject(ex)}");
                return BadRequest();
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

    }
}