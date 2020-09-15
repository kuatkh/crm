using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CRM.DataModel.Data;
using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using CRM.Services.Helpers;
using CRM.Services.Interfaces;
using CertificateManager;
using CertificateManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CRM.Admin.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static CrmConfiguration _configuration;
        private static ILogger<AuthController> _logger;

        private static CreateCertificates _cc;

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

                    content.Add("client_id", _configuration.Client.Id);
                    content.Add("client_secret", _configuration.Client.Secret);
                    content.Add("grant_type", _configuration.GrantType);
                    content.Add("scope", _configuration.Client.Scope);
                    content.Add("ContentType", "application/x-www-form-urlencoded");

                    var postContent = new FormUrlEncodedContent(content);

                    string authServerUrl = _configuration.AuthServerUrl;
#if DEBUG
                    authServerUrl = "https://localhost:44338/";
#endif
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

#if DEBUG
        [AllowAnonymous]
#else
        [Authorize(Roles = "superadmin")]
#endif
        [HttpPost("GenerateNewCertificate")]
        public IActionResult GenerateNewCertificate([FromBody] CertDto certData)
        {
            try
            {
                var sp = new ServiceCollection()
                    .AddCertificateManager()
                    .BuildServiceProvider();

                _cc = sp.GetService<CreateCertificates>();

                string dnsName = "localhost";
                if (!string.IsNullOrEmpty(certData.DnsName))
                {
                    dnsName = certData.DnsName;
                }

                int validityPeriodInDays = 15;
                if (certData.ValidityPeriodInDays > 0)
                {
                    validityPeriodInDays = certData.ValidityPeriodInDays;
                }

                string password = "1q@W3e$R";
                if (!string.IsNullOrEmpty(certData.Secret))
                {
                    password = certData.Secret;
                }

                string path = "C:/config/";
                if (!string.IsNullOrEmpty(certData.CertPath))
                {
                    path = certData.CertPath;
                    if (path.Last() != '/')
                    {
                        path = $"{path}/";
                    }
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var iec = sp.GetService<ImportExportCertificate>();

                if (certData.CertType == 2)
                {
                    var ecdsaCert = CreateECDsaCertificate(dnsName, validityPeriodInDays);

                    var ecdsaCertPfxBytes =
                        iec.ExportSelfSignedCertificatePfx(password, ecdsaCert);
                    System.IO.File.WriteAllBytes($"{path}{(!string.IsNullOrEmpty(certData.FileName) ? certData.FileName : "abEcdsaCert")}.pfx", ecdsaCertPfxBytes);
                }
                else
                {
                    var rsaCert = CreateRsaCertificate(dnsName, validityPeriodInDays);

                    var rsaCertPfxBytes =
                        iec.ExportSelfSignedCertificatePfx(password, rsaCert);
                    System.IO.File.WriteAllBytes($"{path}{(!string.IsNullOrEmpty(certData.FileName) ? certData.FileName : "abRsaCert")}.pfx", rsaCertPfxBytes);
                }

                var success = new ResultDto<string>()
                {
                    IsSuccess = true
                };
                return Ok(success);
            }
            catch(Exception ex)
            {
                _logger.LogError($"ERROR GenerateNewCertificate. MSG: {JsonConvert.SerializeObject(ex)}");

                var err = new ResultDto<string>()
                {
                    IsSuccess = false,
                    Msg = JsonConvert.SerializeObject(ex)
                };
                return Ok(err);
            }
        }

        private static X509Certificate2 CreateRsaCertificate(string dnsName, int validityPeriodInDays)
        {
            var basicConstraints = new BasicConstraints
            {
                CertificateAuthority = false,
                HasPathLengthConstraint = false,
                PathLengthConstraint = 0,
                Critical = false
            };

            var subjectAlternativeName = new SubjectAlternativeName
            {
                DnsName = new List<string> { dnsName }
            };

            var x509KeyUsageFlags = X509KeyUsageFlags.DigitalSignature;

            // only if certification authentication is used
            var enhancedKeyUsages = new OidCollection
            {
                new Oid("1.3.6.1.5.5.7.3.1"),  // TLS Server auth
                new Oid("1.3.6.1.5.5.7.3.2"),  // TLS Client auth
            };

            var certificate = _cc.NewRsaSelfSignedCertificate(
                new DistinguishedName { CommonName = dnsName },
                basicConstraints,
                new ValidityPeriod
                {
                    ValidFrom = DateTimeOffset.UtcNow,
                    ValidTo = DateTimeOffset.UtcNow.AddDays(validityPeriodInDays)
                },
                subjectAlternativeName,
                enhancedKeyUsages,
                x509KeyUsageFlags,
                new RsaConfiguration { KeySize = 2048 }
            );

            return certificate;
        }

        private static X509Certificate2 CreateECDsaCertificate(string dnsName, int validityPeriodInDays)
        {
            var basicConstraints = new BasicConstraints
            {
                CertificateAuthority = false,
                HasPathLengthConstraint = false,
                PathLengthConstraint = 0,
                Critical = false
            };

            var san = new SubjectAlternativeName
            {
                DnsName = new List<string> { dnsName }
            };

            var x509KeyUsageFlags = X509KeyUsageFlags.DigitalSignature;

            // only if certification authentication is used
            var enhancedKeyUsages = new OidCollection {
                new Oid("1.3.6.1.5.5.7.3.1"),  // TLS Server auth
                new Oid("1.3.6.1.5.5.7.3.2"),  // TLS Client auth
            };

            var certificate = _cc.NewECDsaSelfSignedCertificate(
                new DistinguishedName { CommonName = dnsName },
                basicConstraints,
                new ValidityPeriod
                {
                    ValidFrom = DateTimeOffset.UtcNow,
                    ValidTo = DateTimeOffset.UtcNow.AddDays(validityPeriodInDays)
                },
                san,
                enhancedKeyUsages,
                x509KeyUsageFlags,
                new ECDsaConfiguration());

            return certificate;
        }

    }
}