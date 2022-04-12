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
    [Authorize(Roles = "superadmin,admin")]
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
                                    Name = d.Name,
                                    ParentId = d.DictCountriesId,
                                    ParentName = d.DictCountry != null ? d.DictCountry.Name : null,
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
                                    Name = d.Name,
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
                                    Name = d.Name,
                                    ParentId = d.DictEnterprisesId,
                                    ParentName = d.DictEnterprise != null ? d.DictEnterprise.Name : null,
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
                                    Name = d.Name,
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
                                                Name = e.Name,
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
                                    Name = d.Name,
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
                                    Name = d.Name,
                                    Description = d.Description,
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
                        case "DictLanguages":
                            result.Data = await _crmContext.DictLanguages
                                .Where(d => d.DeletedDateTime == null)
                                .AsNoTracking()
                                .Select(d => new DictionaryDto()
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    Code = d.Code,
                                    Description = d.Description,
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
                                    Name = d.Name,
                                    Description = d.Description,
                                    ParentId = d.DictEnterprisesId,
                                    ParentName = d.DictEnterprise != null ? d.DictEnterprise.Name : null,
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
                                    Name = d.Name,
                                    Description = d.Description,
                                    PositionCategory = d.Category,
                                    ParentId = d.DictEnterprisesId,
                                    ParentName = d.DictEnterprise != null ? d.DictEnterprise.Name : null,
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
                                    Name = d.Name,
                                    Description = d.Description,
                                    ParentId = d.DictDepartmentsId,
                                    ParentName = d.DictDepartment != null ? d.DictDepartment.Name : null,
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
                                    Name = d.Name,
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
                            Name = dictionaryData.Name,
                            CreatedDateTime = DateTime.Now,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictCountries.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
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
                            Name = dictionaryData.Name,
                            CreatedDateTime = DateTime.Now,
                            DictCountriesId = dictionaryData.ParentId ?? 1,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictCities.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
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
                            Name = dictionaryData.Name,
                            CreatedDateTime = DateTime.Now,
                            DictEnterprisesId = dictionaryData.ParentId
                        };

                        await _crmContext.DictDepartments.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
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
                            Name = dictionaryData.Name,
                            Description = dictionaryData.Description,
                            Category = dictionaryData.PositionCategory,
                            CreatedDateTime = DateTime.Now,
                            DictEnterprisesId = dictionaryData.ParentId
                        };

                        await _crmContext.DictPositions.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
                        item.Description = dictionaryData.Description;
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
                            Name = dictionaryData.Name,
                            Description = dictionaryData.Description,
                            Price = dictionaryData.Amount ?? 0,
                            CreatedDateTime = DateTime.Now,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictServices.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
                        item.Description = dictionaryData.Description;
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
                            Name = dictionaryData.Name,
                            Description = dictionaryData.Description,
                            CreatedDateTime = DateTime.Now,
                            Code = dictionaryData.Code
                        };

                        await _crmContext.DictIntolerances.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
                        item.Description = dictionaryData.Description;
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
                            Name = dictionaryData.Name,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictGenders.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
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
                            Name = dictionaryData.Name,
                            Description = dictionaryData.Description,
                            DiscountAmount = dictionaryData.Amount ?? 0,
                            DictEnterprisesId = dictionaryData.ParentId,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictLoyaltyPrograms.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
                        item.Description = dictionaryData.Description;
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
                            Name = dictionaryData.Name,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictStatuses.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
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
                            Name = dictionaryData.Name,
                            Address = dictionaryData.Address,
                            PhoneNumber = dictionaryData.PhoneNumber,
                            ParentId = dictionaryData.ParentId,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictEnterprises.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
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

        [HttpPost("SaveDictLanguages")]
        public async Task<IActionResult> SaveDictLanguages([FromBody] DictionaryDto dictionaryData)
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
                    DictLanguages item = null;
                    if (dictionaryData.Id > 0)
                    {
                        item = await _crmContext.DictLanguages.FirstOrDefaultAsync(d => d.Id == dictionaryData.Id);
                    }
                    
                    if (item == null)
                    {
                        item = new DictLanguages()
                        {
                            Name = dictionaryData.Name,
                            Code = dictionaryData.Code,
                            Description = dictionaryData.Description,
                            CreatedDateTime = DateTime.Now
                        };

                        await _crmContext.DictLanguages.AddAsync(item);
                    }
                    else
                    {
                        item.Name = dictionaryData.Name;
                        item.Code = dictionaryData.Code;
                        item.Description = dictionaryData.Description;
                        item.EditedDateTime = DateTime.Now;

                        _crmContext.DictLanguages.Update(item);
                    }

                    await _crmContext.SaveChangesAsync();

                    result.IsSuccess = true;
                    return Ok(result);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDictLanguages. MSG: CURRENT USER OR DICTIONARY DATA IS EMPTY");
                    result.IsSuccess = false;
                    result.Msg = "CURRENT USER OR DICTIONARY DATA IS EMPTY";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDictLanguages. MSG: {JsonConvert.SerializeObject(ex)}");
                result.IsSuccess = false;
                result.Msg = JsonConvert.SerializeObject(ex);

                return Ok(result);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpGet("GetDictServicesData")]
        public async Task<IActionResult> GetDictServicesData(string searchData)
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

                    result.Data = await _crmContext.DictServices
                        .Where(d => d.DeletedDateTime == null && (!string.IsNullOrEmpty(searchData) && 
                                (d.Name != null && d.Name.ToLower().Contains(searchData.ToLower()) ||
                                d.Description != null && d.Description.ToLower().Contains(searchData.ToLower()))
                            || string.IsNullOrEmpty(searchData) && d.Id < 30))
                        .AsNoTracking()
                        .Select(d => new DictionaryDto()
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Description = d.Description,
                            Amount = d.Price,
                        })
                        .ToListAsync();
                    return Ok(result);
                }
                else
                {
                    return Ok(new ResultDto<string>()
                    {
                        IsSuccess = false,
                        Data = "empty_current_user"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetDictServicesData. MSG: {JsonConvert.SerializeObject(ex)}");

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
    }
}