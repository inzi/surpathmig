using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace inzibackend.Startup;

[DependsOn(typeof(inzibackendCoreModule))]
public class inzibackendGraphQLModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(inzibackendGraphQLModule).GetAssembly());
    }

    public override void PreInitialize()
    {
        base.PreInitialize();

        //Adding custom AutoMapper configuration
        Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
    }
}

