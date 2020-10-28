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

namespace CRM.Controllers
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
                        //.Include("CrmPatient")
                        //.Include("ToCrmEmployee.DictPosition")
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
                            //Iin = a.CrmPatient != null && currentUser.RoleId == 1 ? a.CrmPatient.Iin : null,
                            //DocumentNumber = a.CrmPatient != null && currentUser.RoleId == 1 ? a.CrmPatient.DocumentNumber : null,
                            SurnameEn = a.CrmPatient != null ? a.CrmPatient.SurnameEn : null,
                            SurnameRu = a.CrmPatient != null ? a.CrmPatient.SurnameRu : null,
                            SurnameKz = a.CrmPatient != null ? a.CrmPatient.SurnameKz : null,
                            NameEn = a.CrmPatient != null ? a.CrmPatient.NameEn : null,
                            NameRu = a.CrmPatient != null ? a.CrmPatient.NameRu : null,
                            NameKz = a.CrmPatient != null ? a.CrmPatient.NameKz : null,
                            MiddlenameEn = a.CrmPatient != null ? a.CrmPatient.MiddlenameEn : null,
                            MiddlenameRu = a.CrmPatient != null ? a.CrmPatient.MiddlenameRu : null,
                            MiddlenameKz = a.CrmPatient != null ? a.CrmPatient.MiddlenameKz : null,
                            //PhoneNumber = a.CrmPatient != null && currentUser.RoleId == 1 ? a.CrmPatient.PhoneNumber : null,
                            //ToEmployee = a.ToCrmEmployee != null 
                            //    ? new SelectWithPositionDto()
                            //    {
                            //        Id = a.ToCrmEmployeesId,
                            //        NameEn = UserHelper.GetUserShortName(a.ToCrmEmployee.SurnameEn, a.ToCrmEmployee.NameEn, a.ToCrmEmployee.MiddlenameEn),
                            //        NameRu = UserHelper.GetUserShortName(a.ToCrmEmployee.SurnameRu, a.ToCrmEmployee.NameRu, a.ToCrmEmployee.MiddlenameRu),
                            //        NameKz = UserHelper.GetUserShortName(a.ToCrmEmployee.SurnameKz, a.ToCrmEmployee.NameKz, a.ToCrmEmployee.MiddlenameKz),
                            //        PositionId = a.ToCrmEmployee.DictPositionsId,
                            //        PositionNameEn = a.ToCrmEmployee.DictPosition.NameEn,
                            //        PositionNameRu = a.ToCrmEmployee.DictPosition.NameRu,
                            //        PositionNameKz = a.ToCrmEmployee.DictPosition.NameKz
                            //    }
                            //    : null
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

        [HttpPost("SaveDoctorsAppointment")]
        public async Task<IActionResult> SaveDoctorsAppointment([FromBody] AppointmentDto appointmentData)
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
                    var appointment = await _crmContext.CrmPatientsAppointments.FirstOrDefaultAsync(a => appointmentData.Id > 0 && a.Id == appointmentData.Id && a.DeletedDateTime == null);
                    if (appointment != null)
                    {
                        appointment.DoctorsAppointment = appointmentData.DoctorsAppointment;
                        appointment.EditedDateTime = DateTime.Now;
                        appointment.EditorId = currentUser.Id;

                        _crmContext.CrmPatientsAppointments.Update(appointment);
                        await _crmContext.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogError($"ERROR SaveDoctorsAppointment. EMPTY APPOINTMENT. appointmentData = {JsonConvert.SerializeObject(appointmentData)};");

                        var emptyPatient = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = "empty_patient"
                        };

                        return Ok(emptyPatient);
                    }

                    var success = new ResultDto<string>()
                    {
                        IsSuccess = true
                    };

                    return Ok(success);
                }
                else
                {
                    _logger.LogError($"ERROR SaveDoctorsAppointment. EMPTY CURRENT USER OR APPOINTMENT DATA. currentUser = {JsonConvert.SerializeObject(currentUser)}; appointmentData = {JsonConvert.SerializeObject(appointmentData)}");

                    var emptyResult = new ResultDto<string>()
                    {
                        IsSuccess = false
                    };

                    return Ok(emptyResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR SaveDoctorsAppointment. MSG: {JsonConvert.SerializeObject(ex)}");

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