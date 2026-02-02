using Abp.Modules;
using Abp.Reflection.Extensions;

namespace inzibackend
{
    [DependsOn(typeof(inzibackendXamarinSharedModule))]
    public class inzibackendXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendXamarinAndroidModule).GetAssembly());
        }
    }
}