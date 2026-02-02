using System;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using inzibackend.Configuration;
using inzibackend.Identity;
using inzibackend.Web.HealthCheck;
using inzibackend.Surpath;
using inzibackend.Web.Surpath;

namespace inzibackend.Web.Public.Startup
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();

            SurpathSettings.SurpathRecordsRootFolder = _appConfiguration.GetValue<string>(SurpathSettings.SettingsPathRecordsRootFolder);
            SurpathSettings.MaxfiledataLength = _appConfiguration.GetValue<int>(SurpathSettings.SettingsPathSurpathMaxfiledataLength, 5242880);
            SurpathSettings.MaxfiledataLengthUserFriendlyValue = _appConfiguration.GetValue<string>(SurpathSettings.SettingsPathMaxfiledataLengthUserFriendlyValue, "5MB");
            SurpathSettings.EnableInSessionPayment = _appConfiguration.GetValue<bool>(SurpathSettings.SettingsEnableInSessionPayment, false);
            SurpathSettings.MaxProfilefiledataLength = _appConfiguration.GetValue<int>(SurpathSettings.SettingsPathMaxProfilefiledataLength, 5242880);
            SurpathSettings.MaxProfilefiledataLengthUserFriendlyValue = _appConfiguration.GetValue<string>(SurpathSettings.SettingsPathMaxProfilefiledataLengthUserFriendlyValueValue, "5MB");
            SurpathSettings.AllowedFileExtensions = _appConfiguration.GetValue<string>(SurpathSettings.SettingsPathAllowedFileExtensions, "jpeg, jpg, png, pdf, txt, hl7");

        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            //MVC
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(typeof(SurpathWatchdogValidation));
            }).AddNewtonsoftJson();

            if (bool.Parse(_appConfiguration["KestrelServer:IsEnabled"]))
            {
                ConfigureKestrel(services);
            }

            IdentityRegistrar.Register(services);
            services.AddSignalR();

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                services.AddAbpZeroHealthCheck();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<Settings>(settings =>
                    {
                        healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true);
                    });
                    services.AddHealthChecksUI()
                        .AddInMemoryStorage();
                }
            }

            //Configure Abp and Dependency Injection
            return services.AddAbp<inzibackendWebFrontEndModule>(options =>
            {
                var isProd = true;
#if DEBUG
                isProd = false;
#endif

                //Configure Log4Net logging
                //options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                //    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                //        ? "log4net.config"
                //        : "log4net.Production.config")
                //);

                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(isProd == false
                        ? "log4net.config"
                        : "log4net.Production.config")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.
            app.UseSurpathExceptionLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                
                app.ApplicationServices.GetRequiredService<IAbpAspNetCoreConfiguration>().EndpointConfiguration.ConfigureAllEndpoints(endpoints);
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                app.UseHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                
                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                {
                    app.UseHealthChecksUI();
                }
            }
        }

        private void ConfigureKestrel(IServiceCollection services)
        {
            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.Listen(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 443),
                    listenOptions =>
                    {
                        var certPassword = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Password");
                        var certPath = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Path");
                        var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword);
                        listenOptions.UseHttps(new HttpsConnectionAdapterOptions()
                        {
                            ServerCertificate = cert
                        });
                    });
            });
        }
    }
}
