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
    //[Authorize(Roles = "superadmin,admin")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private CrmDbContext _crmContext;
        private static ICacheManager _cacheManager;
        private static ILogger<AppointmentsController> _logger;

        public AppointmentsController(CrmConfiguration configuration,
            ILogger<AppointmentsController> logger,
            ICacheManager cacheManager,
            CrmDbContext crmContext)
        {
            _configuration = configuration;
            _logger = logger;
            _cacheManager = cacheManager;
            _crmContext = crmContext;
        }

        [HttpPost("GetAppointments")]
        public async Task<IActionResult> GetAppointments([FromBody] AppointmentDto appointmentData)
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

                if (currentUser != null && appointmentData != null && appointmentData.ToEmployee != null)
                {
                    var appointments = await _crmContext.CrmPatientsAppointments
                        .Include("CrmPatient")
                        .Include("ToCrmEmployee.DictPosition")
                        .Include("CrmPatientsAppointmentsServices.DictService")
                        .AsNoTracking()
                        .Where(a => a.DeletedDateTime == null && a.AppointmentStartDateTime.Date >= appointmentData.Start.Date && a.AppointmentEndDateTime.Date <= appointmentData.End.Date.AddDays(1) && a.ToCrmEmployeesId == appointmentData.ToEmployee.Id)
                        .Select(a => new AppointmentDto()
                        {
                            Id = a.Id,
                            Code = a.Code,
                            Complain = a.Complain,
                            Start = a.AppointmentStartDateTime,
                            End = a.AppointmentEndDateTime,
                            CrmPatientsId = a.CrmPatientsId,
                            Iin = a.CrmPatient != null && (currentUser.RoleId == 1 || currentUser.RoleId == 2) ? a.CrmPatient.Iin : null,
                            DocumentNumber = a.CrmPatient != null && currentUser.RoleId == 1 ? a.CrmPatient.DocumentNumber : null,
                            SurnameEn = a.CrmPatient != null ? a.CrmPatient.SurnameEn : null,
                            SurnameRu = a.CrmPatient != null ? a.CrmPatient.SurnameRu : null,
                            SurnameKz = a.CrmPatient != null ? a.CrmPatient.SurnameKz : null,
                            NameEn = a.CrmPatient != null ? a.CrmPatient.NameEn : null,
                            NameRu = a.CrmPatient != null ? a.CrmPatient.NameRu : null,
                            NameKz = a.CrmPatient != null ? a.CrmPatient.NameKz : null,
                            MiddlenameEn = a.CrmPatient != null ? a.CrmPatient.MiddlenameEn : null,
                            MiddlenameRu = a.CrmPatient != null ? a.CrmPatient.MiddlenameRu : null,
                            MiddlenameKz = a.CrmPatient != null ? a.CrmPatient.MiddlenameKz : null,
                            PhoneNumber = a.CrmPatient != null && currentUser.RoleId == 1 ? a.CrmPatient.PhoneNumber : null,
                            ToEmployee = a.ToCrmEmployee != null 
                                ? new SelectWithPositionDto()
                                {
                                    Id = a.ToCrmEmployeesId,
                                    NameEn = UserHelper.GetUserShortName(a.ToCrmEmployee.SurnameEn, a.ToCrmEmployee.NameEn, a.ToCrmEmployee.MiddlenameEn),
                                    NameRu = UserHelper.GetUserShortName(a.ToCrmEmployee.SurnameRu, a.ToCrmEmployee.NameRu, a.ToCrmEmployee.MiddlenameRu),
                                    NameKz = UserHelper.GetUserShortName(a.ToCrmEmployee.SurnameKz, a.ToCrmEmployee.NameKz, a.ToCrmEmployee.MiddlenameKz),
                                    PositionId = a.ToCrmEmployee.DictPositionsId,
                                    PositionNameEn = a.ToCrmEmployee.DictPosition.NameEn,
                                    PositionNameRu = a.ToCrmEmployee.DictPosition.NameRu,
                                    PositionNameKz = a.ToCrmEmployee.DictPosition.NameKz
                                }
                                : null,
                            SelectedProcedures = a.CrmPatientsAppointmentsServices != null && a.CrmPatientsAppointmentsServices.Any(p => p.DeletedDateTime == null)
                                ? a.CrmPatientsAppointmentsServices
                                    .Where(p => p.DeletedDateTime == null)
                                    .Select(p => new DictionaryDto()
                                    {
                                        Id = p.DictService != null ? p.DictService.Id : 0,
                                        NameRu = p.DictService != null ? p.DictService.NameRu : null,
                                        NameEn = p.DictService != null ? p.DictService.NameEn : null,
                                        NameKz = p.DictService != null ? p.DictService.NameKz : null,
                                        DescriptionRu = p.DictService != null ? p.DictService.DescriptionRu : null,
                                        DescriptionEn = p.DictService != null ? p.DictService.DescriptionEn : null,
                                        DescriptionKz = p.DictService != null ? p.DictService.DescriptionKz : null,
                                        ParentId = p.Id
                                    })
                                    .ToList()
                                : null
                        })
                        .ToListAsync();

                    var success = new ResultDto<List<AppointmentDto>>()
                    {
                        IsSuccess = true,
                        Data = appointments
                    };

                    return Ok(success);
                }
                else
                {
                    _logger.LogError($"ERROR GetAppointments. EMPTY CURRENT USER. MSG: {JsonConvert.SerializeObject(currentUser)}");

                    var emptyCurrentUser = new ResultDto<List<AppointmentDto>>()
                    {
                        IsSuccess = false,
                        Msg = "empty_current_user"
                    };

                    return Ok(emptyCurrentUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetAppointments. MSG: {JsonConvert.SerializeObject(ex)}");

                var error = new ResultDto<List<AppointmentDto>>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(error);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }

        [HttpPost("SaveAppointment")]
        public async Task<IActionResult> SaveAppointment([FromBody] AppointmentDto appointmentData)
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

                if (currentUser != null && appointmentData != null && appointmentData.ToEmployee != null)
                {
                    if (!string.IsNullOrEmpty(appointmentData.Iin.Trim()) || !string.IsNullOrEmpty(appointmentData.DocumentNumber.Trim()))
                    {
                        var crmPatient = await _crmContext.CrmPatients.FirstOrDefaultAsync(c => (c.Iin == appointmentData.Iin.Trim() || c.DocumentNumber == appointmentData.DocumentNumber.Trim()) && c.DeletedDateTime == null);

                        if (crmPatient != null)
                        {
                            crmPatient.Iin = appointmentData.Iin;
                            crmPatient.DocumentNumber = appointmentData.DocumentNumber;
                            crmPatient.EditedDateTime = DateTime.Now;
                            crmPatient.EditorId = currentUser.Id;
                            crmPatient.IsActive = true;
                            crmPatient.SurnameEn = appointmentData.SurnameEn;
                            crmPatient.SurnameRu = appointmentData.SurnameRu;
                            crmPatient.SurnameKz = appointmentData.SurnameKz;
                            crmPatient.NameEn = appointmentData.NameEn;
                            crmPatient.NameRu = appointmentData.NameRu;
                            crmPatient.NameKz = appointmentData.NameKz;
                            crmPatient.MiddlenameEn = appointmentData.MiddlenameEn;
                            crmPatient.MiddlenameRu = appointmentData.MiddlenameRu;
                            crmPatient.MiddlenameKz = appointmentData.MiddlenameKz;
                            crmPatient.PhoneNumber = appointmentData.PhoneNumber;

                            _crmContext.CrmPatients.Update(crmPatient);
                            await _crmContext.SaveChangesAsync();
                        }
                        else
                        {
                            crmPatient = new CrmPatients()
                            {
                                Id = 0,
                                CreatedDateTime = DateTime.Now,
                                AuthorId = currentUser.Id,
                                Iin = appointmentData.Iin,
                                DocumentNumber = appointmentData.DocumentNumber,
                                IsActive = true,
                                SurnameEn = appointmentData.SurnameEn,
                                SurnameRu = appointmentData.SurnameRu,
                                SurnameKz = appointmentData.SurnameKz,
                                NameEn = appointmentData.NameEn,
                                NameRu = appointmentData.NameRu,
                                NameKz = appointmentData.NameKz,
                                MiddlenameEn = appointmentData.MiddlenameEn,
                                MiddlenameRu = appointmentData.MiddlenameRu,
                                MiddlenameKz = appointmentData.MiddlenameKz,
                                PhoneNumber = appointmentData.PhoneNumber,
                            };

                            await _crmContext.CrmPatients.AddAsync(crmPatient);
                            await _crmContext.SaveChangesAsync();
                        }

                        if (crmPatient.Id > 0)
                        {
                            var appointment = await _crmContext.CrmPatientsAppointments.FirstOrDefaultAsync(a => appointmentData.Id > 0 && a.Id == appointmentData.Id && a.DeletedDateTime == null);
                            if (appointment != null)
                            {
                                appointment.Code = appointmentData.Code;
                                appointment.Complain = appointmentData.Complain;
                                appointment.AppointmentStartDateTime = appointmentData.Start;
                                appointment.AppointmentEndDateTime = appointmentData.End;
                                appointment.ToCrmEmployeesId = appointmentData.ToEmployee.Id;
                                appointment.CrmPatientsId = crmPatient.Id;
                                appointment.EditedDateTime = DateTime.Now;
                                appointment.EditorId = currentUser.Id;

                                _crmContext.CrmPatientsAppointments.Update(appointment);
                                await _crmContext.SaveChangesAsync();
                            }
                            else
                            {
                                appointment = new CrmPatientsAppointments()
                                {
                                    Id = 0,
                                    CreatedDateTime = DateTime.Now,
                                    AuthorId = currentUser.Id,
                                    Code = appointmentData.Code,
                                    Complain = appointmentData.Complain,
                                    AppointmentStartDateTime = appointmentData.Start,
                                    AppointmentEndDateTime = appointmentData.End,
                                    DictStatusesId = 1,
                                    ToCrmEmployeesId = appointmentData.ToEmployee.Id,
                                    CrmPatientsId = crmPatient.Id,
                                };

                                await _crmContext.CrmPatientsAppointments.AddAsync(appointment);
                                await _crmContext.SaveChangesAsync();
                            }

                            if (appointmentData.SelectedProcedures != null && appointmentData.SelectedProcedures.Any() && appointment.Id > 0)
                            {
                                // удаляем старые сохраненные, которых нету в новом листе
                                var selectedProceduresIds = appointmentData.SelectedProcedures.Select(s => s.Id.ToString()).ToList();
                                var oldProcedures = await _crmContext.CrmPatientsAppointmentsServices
                                    .Where(p => p.CrmPatientsAppointmentsId == appointment.Id && p.DictServicesId != null && !selectedProceduresIds.Any(s => s == p.DictServicesId.Value.ToString()) && p.DeletedDateTime == null)
                                    .ToListAsync();

                                if (oldProcedures != null && oldProcedures.Any())
                                {
                                    foreach (var oldProc in oldProcedures)
                                    {
                                        oldProc.DeletedDateTime = DateTime.Now;
                                    }
                                    _crmContext.CrmPatientsAppointmentsServices.UpdateRange(oldProcedures);
                                    await _crmContext.SaveChangesAsync();
                                }

                                // игнорим старые сохраненные, которые присутствуют в листе
                                var savedProcedures = await _crmContext.CrmPatientsAppointmentsServices
                                    .Where(p => p.CrmPatientsAppointmentsId == appointment.Id && p.DictServicesId != null && selectedProceduresIds.Any(s => s == p.DictServicesId.Value.ToString()) && p.DeletedDateTime == null)
                                    .AsNoTracking()
                                    .Select(p => new CrmPatientsAppointmentsServices()
                                    {
                                        Id = p.Id,
                                        DictServicesId = p.DictServicesId
                                    })
                                    .ToListAsync();

                                foreach (var proc in appointmentData.SelectedProcedures.Where(s => savedProcedures != null && !savedProcedures.Any(p => p.DictServicesId == s.Id) || savedProcedures == null || !savedProcedures.Any()))
                                {
                                    await _crmContext.CrmPatientsAppointmentsServices.AddAsync(new CrmPatientsAppointmentsServices()
                                    {
                                        CrmPatientsAppointmentsId = appointment.Id,
                                        CreatedDateTime = DateTime.Now,
                                        DictServicesId = proc.Id
                                    });
                                    await _crmContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                // удаляем старые сохраненные, так как пустой лист
                                var oldProcedures = await _crmContext.CrmPatientsAppointmentsServices
                                    .Where(p => p.CrmPatientsAppointmentsId == appointment.Id && p.DeletedDateTime == null)
                                    .ToListAsync();

                                if (oldProcedures != null && oldProcedures.Any())
                                {
                                    foreach (var oldProc in oldProcedures)
                                    {
                                        oldProc.DeletedDateTime = DateTime.Now;
                                    }
                                    _crmContext.CrmPatientsAppointmentsServices.UpdateRange(oldProcedures);
                                    await _crmContext.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            _logger.LogError($"ERROR SaveAppointment. EMPTY PATIENT. appointmentData = {JsonConvert.SerializeObject(appointmentData)}; crmPatient = {JsonConvert.SerializeObject(crmPatient)}");

                            var emptyPatient = new ResultDto<string>()
                            {
                                IsSuccess = false,
                                Msg = "empty_patient"
                            };

                            return Ok(emptyPatient);
                        }
                    }
                    else
                    {
                        _logger.LogError($"ERROR SaveAppointment. EMPTY PATIENT'S DOCUMENT NUMBER. appointmentData = {JsonConvert.SerializeObject(appointmentData)}");

                        var emptyPatientDocuments = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = "empty_patient_documents"
                        };

                        return Ok(emptyPatientDocuments);
                    }

                    var success = new ResultDto<string>()
                    {
                        IsSuccess = true
                    };

                    return Ok(success);
                }
                else
                {
                    _logger.LogError($"ERROR SaveAppointment. EMPTY CURRENT USER OR APPOINTMENT DATA. currentUser = {JsonConvert.SerializeObject(currentUser)}; appointmentData = {JsonConvert.SerializeObject(appointmentData)}");

                    var emptyResult = new ResultDto<string>()
                    {
                        IsSuccess = false
                    };

                    return Ok(emptyResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveAppointment. MSG: {JsonConvert.SerializeObject(ex)}");

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

        [HttpPost("SetAppointmentStartEnd")]
        public async Task<IActionResult> SetAppointmentStartEnd([FromBody] AppointmentDto appointmentData)
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

                if (currentUser != null && appointmentData != null)
                {
                    var appointment = await _crmContext.CrmPatientsAppointments.FirstOrDefaultAsync(a => appointmentData.Id > 0 && a.Id == appointmentData.Id && a.DeletedDateTime == null);
                    if (appointment != null)
                    {
                        appointment.AppointmentStartDateTime = appointmentData.Start;
                        appointment.AppointmentEndDateTime = appointmentData.End;
                        appointment.EditedDateTime = DateTime.Now;
                        appointment.EditorId = currentUser.Id;

                        _crmContext.CrmPatientsAppointments.Update(appointment);
                        await _crmContext.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogError($"ERROR SetAppointmentStartEnd. APPOINTMENT NOT FOUND. appointmentData = {JsonConvert.SerializeObject(appointmentData)}");

                        var emptyPatientDocuments = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = "appointment_not_found"
                        };

                        return Ok(emptyPatientDocuments);
                    }

                    var success = new ResultDto<string>()
                    {
                        IsSuccess = true
                    };

                    return Ok(success);
                }
                else
                {
                    _logger.LogError($"ERROR SetAppointmentStartEnd. EMPTY CURRENT USER OR APPOINTMENT DATA. currentUser = {JsonConvert.SerializeObject(currentUser)}; appointmentData = {JsonConvert.SerializeObject(appointmentData)}");

                    var emptyResult = new ResultDto<string>()
                    {
                        IsSuccess = false
                    };

                    return Ok(emptyResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SetAppointmentStartEnd. MSG: {JsonConvert.SerializeObject(ex)}");

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

        [HttpGet("DeleteAppointment")]
        public async Task<IActionResult> DeleteAppointment(long id)
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

                if (currentUser != null && id > 0)
                {
                    var appointment = await _crmContext.CrmPatientsAppointments.FirstOrDefaultAsync(a => a.Id == id);
                    appointment.DeletedDateTime = DateTime.Now;
                    appointment.EditorId = currentUser.Id;

                    _crmContext.CrmPatientsAppointments.Update(appointment);
                    await _crmContext.SaveChangesAsync();

                    var success = new ResultDto<List<AppointmentDto>>()
                    {
                        IsSuccess = true
                    };

                    return Ok(success);
                }
                else
                {
                    _logger.LogError($"ERROR DeleteAppointment. EMPTY CURRENT USER OR ID. MSG: {JsonConvert.SerializeObject(currentUser)}");

                    var emptyCurrentUser = new ResultDto<List<AppointmentDto>>()
                    {
                        IsSuccess = false,
                        Msg = "empty_current_user_or_id"
                    };

                    return Ok(emptyCurrentUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR DeleteAppointment. MSG: {JsonConvert.SerializeObject(ex)}");

                var error = new ResultDto<List<AppointmentDto>>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };

                return Ok(error);
            }
            finally
            {
                await _crmContext.DisposeAsync();
            }
        }
    }
}