using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CRM.DataModel.Data;
using CRM.DataModel.Models;
using CRM.Services.Common;
using CRM.Services.Interfaces;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NLog.Extensions.Logging;

namespace CRM.Admin
{
    public class Startup
    {
        private readonly CrmConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _configuration = Configuration.GetSection("AppSettings").Get<CrmConfiguration>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddDbContext<CrmDbContext>(options =>
                options.UseNpgsql(_configuration.ConnectionString));

            services.AddIdentity<CrmUsers, CrmRoles>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<CrmDbContext>()
                .AddDefaultTokenProviders();

            var cert = new X509Certificate2(_configuration.CertPath, _configuration.CertSecret);
            var key = new X509SecurityKey(cert);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudiences = new[] { "crm.full", "crm.adm.full" },

                        ValidateIssuer = true,
                        ValidIssuers = new[] { "http://localhost:5000", "https://localhost:5001", _configuration.AuthServerUrl },

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
                    jwtBearerOptions.Authority = _configuration.AuthServerUrl;
                    jwtBearerOptions.RequireHttpsMetadata = false;
                }, introspectionOptions =>
                {
                    introspectionOptions.Authority = _configuration.AuthServerUrl;
                    introspectionOptions.AuthenticationType = JwtBearerDefaults.AuthenticationScheme;
                    introspectionOptions.ClientId = _configuration.Client.Id;
                    introspectionOptions.ClientSecret = _configuration.Client.Secret;
                    introspectionOptions.DiscoveryPolicy = new DiscoveryPolicy()
                    {
                        RequireHttps = false,
                        Authority = _configuration.AuthServerUrl,
                        ValidateEndpoints = false,
                        ValidateIssuerName = true
                    };
                });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });

            services.AddHttpClient();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

#if DEBUG
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:9000", "http://localhost:5000", "https://localhost:44380")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            });
#else
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
            });
#endif

            services.AddSingleton(_configuration);

            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, CacheManager>();

            services.AddMvc().AddNewtonsoftJson();

            services.AddControllers();

            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
                logging.AddNLog();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            NLog.LogManager.LoadConfiguration("nlog.config");

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    if (context.File.Name.EndsWith(".js.gz"))
                    {
                        context.Context.Response.Headers[HeaderNames.ContentType] = "text/javascript";
                        context.Context.Response.Headers[HeaderNames.ContentEncoding] = "gzip";
                    }
                    const int timeInSeconds = 60 * 60 * 24;
                    context.Context.Response.Headers[HeaderNames.CacheControl] = "public, max-age=" + timeInSeconds;
                }
            });
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "wwwroot");
            });

            app.Use(async (context, next) =>
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    await context.ChallengeAsync();
                }
                else
                {
                    await next();
                }
            });
        }
    }
}
