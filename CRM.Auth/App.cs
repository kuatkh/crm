using CRM.Auth.Auth;
using CRM.Auth.AuthProvider;
using CRM.Auth.Configuration;
using CRM.Auth.IdentityProvider;
using CRM.DataModel.Data;
using CRM.DataModel.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace CRM.Auth
{
    public static class App
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            CrmAuthConfiguration _configuration = configuration.GetSection("CrmAuth").Get<CrmAuthConfiguration>();

            var cert = new X509Certificate2(_configuration.CertPath, _configuration.CertSecret);
            var key = new X509SecurityKey(cert);
            SigningCredentials credentials = new(key, "RS256");

            services.AddIdentityServer()
                .AddApiResources()
                .AddClients(_configuration)
                .AddInMemoryApiScopes(AppExtension.GetApiScopes())
                .AddProfileService<ProfileService>()
                .AddExtensionGrantValidator<SilentGrantValidator>()
                .AddExtensionGrantValidator<DbGrantValidator>()
                .AddSigningCredential(credentials);

            services.AddDbContext<CrmDbContext>(options =>
                options.UseNpgsql(_configuration.ConnectionString));

            services.AddIdentity<CrmUsers, CrmRoles>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<CrmDbContext>();

            services.AddTransient<IIdentityProvider, IdentityProvider.IdentityProvider>();
            services.AddTransient<IIdentityProvider, IdentityProvider.WindowsIdentityProvider>();
            services.AddScoped<IAuthProvider, AuthProvider.AuthProvider>();
            services.AddSingleton(_configuration);
            services.AddScoped<IUserClaimsPrincipalFactory<CrmUsers>, MyUserClaimsPrincipalFactory>();

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

        public static WebApplication Configure(this WebApplication app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.EnvironmentName == Environments.Development)
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

            using var serviceScope = app.Services.CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<CrmDbContext>();
            context.Database.Migrate();

            return app;
        }
    }
}
