using Abp.Modules;
using Abp.Reflection.Extensions;

namespace inzibackend
{
    [DependsOn(typeof(inzibackendCoreSharedModule))]
    public class inzibackendApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendApplicationSharedModule).GetAssembly());
        }
    }
}