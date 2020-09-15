using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
    //[Authorize(Roles = "superadmin,admin")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private CrmDbContext _crmContext;
        private static ICacheManager _cacheManager;
        private static ILogger<DictionariesController> _logger;

        public DictionariesController(CrmConfiguration configuration,
            ILogger<DictionariesController> logger,
            ICacheManager cacheManager,
            CrmDbContext crmContext)
        {
            _configuration = configuration;
            _logger = logger;
            _cacheManager = cacheManager;
            _crmContext = crmContext;
        }

        [HttpPost("GetDictionaryData")]
        public async Task<IActionResult> GetDictionaryData([FromBody] FilterDto filterDto)
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
                    var result = new ResultDto<List<DictionaryDto>>()
                    {
                        IsSuccess = true,
                    };
                    switch (filterDto.DictionaryName)
                    {
                        case "DictCities":
                            result.Data = await _crmContext.DictCities
                                .Where(d => d.DeletedDateTime == null)
                                .Include("DictCountry")
                                .AsNoTracking()
                                .Select(d => new DictionaryDto() {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    ParentId = d.DictCountriesId,
                                    ParentNameEn = d.DictCountry != null ? d.DictCountry.NameEn : null,
                                    ParentNameKz = d.DictCountry != null ? d.DictCountry.NameKz : null,
                                    ParentNameRu = d.DictCountry != null ? d.DictCountry.NameRu : null,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictCountries":
                            result.Data = await _crmContext.DictCountries
                                .Where(d => d.DeletedDateTime == null)
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictDepartments":
                            result.Data = await _crmContext.DictDepartments
                                .Where(d => d.DeletedDateTime == null)
                                .Include("DictEnterprise")
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    ParentId = d.DictEnterprisesId,
                                    ParentNameEn = d.DictEnterprise != null ? d.DictEnterprise.NameEn : null,
                                    ParentNameKz = d.DictEnterprise != null ? d.DictEnterprise.NameKz : null,
                                    ParentNameRu = d.DictEnterprise != null ? d.DictEnterprise.NameRu : null,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictEnterprises":
                            result.Data = await _crmContext.DictEnterprises
                                .Where(d => d.DeletedDateTime == null && d.ParentId == null)
                                .Include("EnterpriseBranches")
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    PhoneNumber = d.PhoneNumber,
                                    Address = d.Address,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    Children = d.EnterpriseBranches.Any(e => e.DeletedDateTime == null)
                                        ? d.EnterpriseBranches
                                            .Where(e => e.DeletedDateTime == null)
                                            .Select(e => new DictionaryDto()
                                            {
                                                Id = e.Id,
                                                NameEn = e.NameEn,
                                                NameKz = e.NameKz,
                                                NameRu = e.NameRu,
                                                PhoneNumber = e.PhoneNumber,
                                                Address = e.Address,
                                                CreatedDateTime = e.CreatedDateTime,
                                                EditedDateTime = e.EditedDateTime,
                                                DeletedDateTime = e.DeletedDateTime,
                                                CreatedDateTimeStr = e.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                                EditedDateTimeStr = e.EditedDateTime != null ? e.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                                DeletedDateTimeStr = e.DeletedDateTime != null ? e.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                            })
                                            .ToList()
                                        : null
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictGenders":
                            result.Data = await _crmContext.DictGenders
                                .Where(d => d.DeletedDateTime == null)
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictIntolerances":
                            result.Data = await _crmContext.DictIntolerances
                                .Where(d => d.DeletedDateTime == null)
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    DescriptionEn = d.DescriptionEn,
                                    DescriptionRu = d.DescriptionRu,
                                    DescriptionKz = d.DescriptionKz,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictLoyaltyPrograms":
                            result.Data = await _crmContext.DictLoyaltyPrograms
                                .Where(d => d.DeletedDateTime == null)
                                .Include("DictEnterprise")
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    DescriptionEn = d.DescriptionEn,
                                    DescriptionRu = d.DescriptionRu,
                                    DescriptionKz = d.DescriptionKz,
                                    ParentId = d.DictEnterprisesId,
                                    ParentNameEn = d.DictEnterprise != null ? d.DictEnterprise.NameEn : null,
                                    ParentNameKz = d.DictEnterprise != null ? d.DictEnterprise.NameKz : null,
                                    ParentNameRu = d.DictEnterprise != null ? d.DictEnterprise.NameRu : null,
                                    Amount = d.DiscountAmount,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictPositions":
                            result.Data = await _crmContext.DictPositions
                                .Where(d => d.DeletedDateTime == null)
                                .Include("DictEnterprise")
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    DescriptionEn = d.DescriptionEn,
                                    DescriptionRu = d.DescriptionRu,
                                    DescriptionKz = d.DescriptionKz,
                                    PositionCategory = d.Category,
                                    ParentId = d.DictEnterprisesId,
                                    ParentNameEn = d.DictEnterprise != null ? d.DictEnterprise.NameEn : null,
                                    ParentNameKz = d.DictEnterprise != null ? d.DictEnterprise.NameKz : null,
                                    ParentNameRu = d.DictEnterprise != null ? d.DictEnterprise.NameRu : null,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictServices":
                            result.Data = await _crmContext.DictServices
                                .Where(d => d.DeletedDateTime == null)
                                .Include("DictDepartment")
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    DescriptionEn = d.DescriptionEn,
                                    DescriptionRu = d.DescriptionRu,
                                    DescriptionKz = d.DescriptionKz,
                                    ParentId = d.DictDepartmentsId,
                                    ParentNameEn = d.DictDepartment != null ? d.DictDepartment.NameEn : null,
                                    ParentNameKz = d.DictDepartment != null ? d.DictDepartment.NameKz : null,
                                    ParentNameRu = d.DictDepartment != null ? d.DictDepartment.NameRu : null,
                                    Amount = d.Price,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        case "DictStatuses":
                            result.Data = await _crmContext.DictStatuses
                                .Where(d => d.DeletedDateTime == null)
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    NameEn = d.NameEn,
                                    NameKz = d.NameKz,
                                    NameRu = d.NameRu,
                                    CreatedDateTime = d.CreatedDateTime,
                                    EditedDateTime = d.EditedDateTime,
                                    DeletedDateTime = d.DeletedDateTime,
                                    CreatedDateTimeStr = d.CreatedDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                                    EditedDateTimeStr = d.EditedDateTime != null ? d.EditedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                    DeletedDateTimeStr = d.DeletedDateTime != null ? d.DeletedDateTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : null,
                                })
                                .OrderBy(filterDto.OrderBy + " " + filterDto.Order)
                                .Skip(filterDto.Page * filterDto.RowsPerPage)
                                .Take(filterDto.RowsPerPage)
                                .ToListAsync();
                            break;
                        default:
                            result.Data = new List<DictionaryDto>();
                            break;
                    }
                    return Ok(result);
                }
                else
                {
                    return Ok(new ResultDto<string>()
                    {
                        IsSuccess = false,
                        Data = "Empty current user"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetDictionaryData. MSG: {JsonConvert.SerializeObject(ex)}");

                return Ok(new ResultDto<string>()
                {
                    IsSuccess = false,
                    Data = JsonConvert.SerializeObject(ex)
                });
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictCountries")]
        public async Task<IActionResult> SaveDictCountries([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictCountries item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictCountries.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictCountries()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            CreatedDateTime = DateTime.Now,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictCountries.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.EditedDateTime = DateTime.Now;
                        item.Code = dictionaryData.Code;

                        _crmContext.DictCountries.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictCountries. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictCountries. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictCities")]
        public async Task<IActionResult> SaveDictCities([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictCities item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictCities.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }

                    if (item == null)
                    {
                        item = new DictCities()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            CreatedDateTime = DateTime.Now,
                            DictCountriesId = dictionaryData.ParentId ?? 1,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictCities.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.EditedDateTime = DateTime.Now;
                        item.Code = dictionaryData.Code;
                        item.DictCountriesId = dictionaryData.ParentId ?? 1;

                        _crmContext.DictCities.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictCities. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictCities. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictDepartments")]
        public async Task<IActionResult> SaveDictDepartments([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictDepartments item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictDepartments.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }

                    if (item == null)
                    {
                        item = new DictDepartments()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            CreatedDateTime = DateTime.Now,
                            DictEnterprisesId = dictionaryData.ParentId
                        };

                        await _crmContext.DictDepartments.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.EditedDateTime = DateTime.Now;
                        item.DictEnterprisesId = dictionaryData.ParentId;

                        _crmContext.DictDepartments.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictDepartments. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictDepartments. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }
        
        [HttpPost("SaveDictPositions")]
        public async Task<IActionResult> SaveDictPositions([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictPositions item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictPositions.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }

                    if (item == null)
                    {
                        item = new DictPositions()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            DescriptionEn = dictionaryData.DescriptionEn,
                            DescriptionRu = dictionaryData.DescriptionRu,
                            DescriptionKz = dictionaryData.DescriptionKz,
                            Category = dictionaryData.PositionCategory,
                            CreatedDateTime = DateTime.Now,
                            DictEnterprisesId = dictionaryData.ParentId
                        };

                        await _crmContext.DictPositions.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.DescriptionEn = dictionaryData.DescriptionEn;
                        item.DescriptionRu = dictionaryData.DescriptionRu;
                        item.DescriptionKz = dictionaryData.DescriptionKz;
                        item.Category = dictionaryData.PositionCategory;
                        item.EditedDateTime = DateTime.Now;
                        item.DictEnterprisesId = dictionaryData.ParentId;

                        _crmContext.DictPositions.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictPositions. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictPositions. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }
        
        [HttpPost("SaveDictServices")]
        public async Task<IActionResult> SaveDictServices([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictServices item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictServices.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictServices()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            DescriptionEn = dictionaryData.DescriptionEn,
                            DescriptionRu = dictionaryData.DescriptionRu,
                            DescriptionKz = dictionaryData.DescriptionKz,
                            Price = dictionaryData.Amount ?? 0,
                            CreatedDateTime = DateTime.Now,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictServices.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.DescriptionEn = dictionaryData.DescriptionEn;
                        item.DescriptionRu = dictionaryData.DescriptionRu;
                        item.DescriptionKz = dictionaryData.DescriptionKz;
                        item.Price = dictionaryData.Amount ?? 0;
                        item.EditedDateTime = DateTime.Now;
                        item.Code = dictionaryData.Code;

                        _crmContext.DictServices.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictServices. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictServices. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictIntolerances")]
        public async Task<IActionResult> SaveDictIntolerances([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictIntolerances item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictIntolerances.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictIntolerances()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            DescriptionEn = dictionaryData.DescriptionEn,
                            DescriptionRu = dictionaryData.DescriptionRu,
                            DescriptionKz = dictionaryData.DescriptionKz,
                            CreatedDateTime = DateTime.Now,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictIntolerances.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.DescriptionEn = dictionaryData.DescriptionEn;
                        item.DescriptionRu = dictionaryData.DescriptionRu;
                        item.DescriptionKz = dictionaryData.DescriptionKz;
                        item.EditedDateTime = DateTime.Now;
                        item.Code = dictionaryData.Code;

                        _crmContext.DictIntolerances.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictIntolerances. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictIntolerances. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictGenders")]
        public async Task<IActionResult> SaveDictGenders([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictGenders item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictGenders.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictGenders()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictGenders.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.EditedDateTime = DateTime.Now;

                        _crmContext.DictGenders.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictGenders. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictGenders. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictLoyaltyPrograms")]
        public async Task<IActionResult> SaveDictLoyaltyPrograms([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictLoyaltyPrograms item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictLoyaltyPrograms.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictLoyaltyPrograms()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            DescriptionEn = dictionaryData.DescriptionEn,
                            DescriptionRu = dictionaryData.DescriptionRu,
                            DescriptionKz = dictionaryData.DescriptionKz,
                            DiscountAmount = dictionaryData.Amount ?? 0,
                            DictEnterprisesId = dictionaryData.ParentId,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictLoyaltyPrograms.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.DescriptionEn = dictionaryData.DescriptionEn;
                        item.DescriptionRu = dictionaryData.DescriptionRu;
                        item.DescriptionKz = dictionaryData.DescriptionKz;
                        item.DiscountAmount = dictionaryData.Amount ?? 0;
                        item.DictEnterprisesId = dictionaryData.ParentId;
                        item.EditedDateTime = DateTime.Now;

                        _crmContext.DictLoyaltyPrograms.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictLoyaltyPrograms. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictLoyaltyPrograms. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictStatuses")]
        public async Task<IActionResult> SaveDictStatuses([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictStatuses item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictStatuses.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictStatuses()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictStatuses.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.EditedDateTime = DateTime.Now;

                        _crmContext.DictStatuses.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictStatuses. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictStatuses. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveDictEnterprises")]
        public async Task<IActionResult> SaveDictEnterprises([FromBody] DictionaryDto dictionaryData)
        {
            var result = new ResultDto<string>()
            {
                IsSuccess = false,
            };

            try
            {
                UserHelper.TryGetCurrentName(this.User, out string username);
                UserDto currentUser = (UserDto)_cacheManager.Get($"CrmUser_{username}");

                if (currentUser == null)
                {
                    currentUser = await UserHelper.GetCurrentUser(_crmContext, username);

                    _cacheManager.Set($"CrmUser_{username}", currentUser, new TimeSpan(1, 0, 0, 0));
                }

                if (currentUser != null && dictionaryData != null)
                {
                    DictEnterprises item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictEnterprises.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictEnterprises()
                        {
                            NameEn = dictionaryData.NameEn,
                            NameKz = dictionaryData.NameKz,
                            NameRu = dictionaryData.NameRu,
                            Address = dictionaryData.Address,
                            PhoneNumber = dictionaryData.PhoneNumber,
                            ParentId = dictionaryData.ParentId,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictEnterprises.AddAsync(item);
                    }
                    else
                    {
                        item.NameEn = dictionaryData.NameEn;
                        item.NameKz = dictionaryData.NameKz;
                        item.NameRu = dictionaryData.NameRu;
                        item.Address = dictionaryData.Address;
                        item.PhoneNumber = dictionaryData.PhoneNumber;
                        item.ParentId = dictionaryData.ParentId;
                        item.EditedDateTime = DateTime.Now;

                        _crmContext.DictEnterprises.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictEnterprises. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictEnterprises. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }
    }
}