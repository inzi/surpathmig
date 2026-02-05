using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.EntityHistory;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using inzibackend.Authorization;
using inzibackend.EntityFrameworkCore;
using inzibackend.EntityHistory;
using inzibackend.Surpath;
using System;

namespace inzibackend
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(inzibackendApplicationSharedModule),
        typeof(inzibackendCoreModule)
        )]
    public class inzibackendApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendApplicationModule).GetAssembly());
        }
    }
}