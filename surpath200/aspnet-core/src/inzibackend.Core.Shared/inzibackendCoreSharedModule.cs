using Abp.Modules;
using Abp.Reflection.Extensions;

namespace inzibackend;

public class inzibackendCoreSharedModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(inzibackendCoreSharedModule).GetAssembly());
    }
}

