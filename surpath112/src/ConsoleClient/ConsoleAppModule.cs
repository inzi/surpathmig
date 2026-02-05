using System.Reflection;
using Abp.AspNetZeroCore;
using Abp.Hangfire;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Zero.Configuration;
using inzibackend.Configuration;
using ConsoleClient.DependencyInjection;
using inzibackend.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using inzibackend;
using inzibackend.Surpath;
using inzibackend.MultiTenancy;
using ConsoleClient.DependencyInjection.ConsoleClient.DummyServices;
using inzibackend.Url;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;

namespace ConsoleAppDemo.ConsoleApplication
{
    [DependsOn(
        typeof(inzibackendApplicationModule),
        typeof(inzibackendEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreModule),
        //typeof(AbpUnitOfWorkModule), // Ensure this module is included
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency (and Abp.RedisCache nuget package) can be removed if not using Redis cache
        typeof(AbpHangfireAspNetCoreModule) //AbpHangfireModule dependency (and Abp.Hangfire.AspNetCore nuget package) can be removed if not using Hangfire
    )]
    public class ConsoleAppApplicationAppModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;
        public ConsoleAppApplicationAppModule()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(ConsoleAppApplicationAppModule).GetAssembly().GetDirectoryPathOrNull(),
                addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            //Set default connection string
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                inzibackendConsts.ConnectionStringName
            );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ConsoleAppApplicationAppModule).GetAssembly());
            //IocManager.Register<IUnitOfWorkManager, UnitOfWorkManager>(Abp.Dependency.DependencyLifeStyle.Transient);

            // Register the dummy WebUrlService
            IocManager.Register<IWebUrlService, DummyWebUrlService>(Abp.Dependency.DependencyLifeStyle.Transient);

            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
