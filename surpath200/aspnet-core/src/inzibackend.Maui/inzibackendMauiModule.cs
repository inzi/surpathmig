using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using inzibackend.ApiClient;
using inzibackend.Maui.Core;

namespace inzibackend.Maui;

[DependsOn(typeof(inzibackendClientModule), typeof(AbpAutoMapperModule))]
public class inzibackendMauiModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Localization.IsEnabled = false;
        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

        Configuration.ReplaceService<IApplicationContext, MauiApplicationContext>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(inzibackendMauiModule).GetAssembly());
    }
}