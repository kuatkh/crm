using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CRM.DataModel.Data;
using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private static ILogger<AuthController> _logger;

        public AuthController(CrmConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInDto logInData)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new Dictionary<string, string>();
                    content.Add("username", logInData.UserName);
                    content.Add("password", logInData.UserSecret);
                    content.Add("ismobile", logInData.IsMobile.ToString());

                    content.Add("client_id", _configuration.Client.Id);
                    content.Add("client_secret", _configuration.Client.Secret);
                    content.Add("grant_type", _configuration.GrantType);
                    content.Add("scope", _configuration.Client.Scope);
                    content.Add("ContentType", "application/x-www-form-urlencoded");

                    var postContent = new FormUrlEncodedContent(content);

                    string authServerUrl = _configuration.AuthServerUrl;
                    HttpResponseMessage logInResult = await client.PostAsync($"{authServerUrl}connect/token", postContent);

                    if (logInResult.IsSuccessStatusCode)
                    {
                        string logInResultContent = await logInResult.Content.ReadAsStringAsync();
                        LogInResultDto tokenModel = JsonConvert.DeserializeObject<LogInResultDto>(logInResultContent);

                        var success = new ResultDto<string>()
                        {
                            IsSuccess = true,
                            Data = tokenModel.access_token
                        };
                        return Ok(success);
                    }
                    else
                    {
                        string logInErrorContent = await logInResult.Content.ReadAsStringAsync();
                        LogInResultDto errorModel = JsonConvert.DeserializeObject<LogInResultDto>(logInErrorContent);

                        var logInErr = new ResultDto<string>()
                        {
                            IsSuccess = false,
                            Msg = errorModel.error,
                            Data = errorModel.error_description
                        };
                        return Ok(logInErr);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR LogIn. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
        }
    }
}