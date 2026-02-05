using Abp.Modules;
using Abp.Reflection.Extensions;

namespace inzibackend
{
    [DependsOn(typeof(inzibackendXamarinSharedModule))]
    public class inzibackendXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendXamarinIosModule).GetAssembly());
        }
    }
}