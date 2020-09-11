using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BigProject.Auth.Auth;
using BigProject.Auth.AuthProvider;
using BigProject.Auth.Configuration;
using BigProject.Auth.IdentityProvider;
using BigProject.DataModel.Data;
using BigProject.DataModel.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NLog.Extensions.Logging;

namespace BigProject.Auth
{
    public class Startup
    {
        private IConfigurationRoot ConfigurationRoot { get; }
        private readonly BpAuthConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            ConfigurationRoot = builder.Build();

            _configuration = ConfigurationRoot.GetSection("AbAuth").Get<BpAuthConfiguration>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2(_configuration.CertPath, _configuration.CertSecret);
            var key = new X509SecurityKey(cert);
            SigningCredentials credentials = new SigningCredentials(key, "RS256");

            services.AddIdentityServer()
                .AddApiResources()
                .AddClients(_configuration)
                .AddInMemoryApiScopes(StartupExtension.GetApiScopes())
                .AddProfileService<ProfileService>()
                .AddExtensionGrantValidator<SilentGrantValidator>()
                .AddExtensionGrantValidator<DbGrantValidator>()
                .AddSigningCredential(credentials);

            services.AddDbContext<BpDbContext>(options =>
                options.UseNpgsql(_configuration.ConnectionString));

            services.AddIdentity<BpUsers, BpRoles>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<BpDbContext>();

            services.AddTransient<IIdentityProvider, IdentityProvider.IdentityProvider>();
            services.AddTransient<IIdentityProvider, IdentityProvider.WindowsIdentityProvider>();
            services.AddScoped<IAuthProvider, AuthProvider.AuthProvider>();
            services.AddSingleton(_configuration);
            services.AddScoped<IUserClaimsPrincipalFactory<BpUsers>, MyUserClaimsPrincipalFactory>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddMemoryCache();
            services.AddMvc().AddNewtonsoftJson();

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

            NLog.LogManager.LoadConfiguration("nlog.config");

            app.UseRouting();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
