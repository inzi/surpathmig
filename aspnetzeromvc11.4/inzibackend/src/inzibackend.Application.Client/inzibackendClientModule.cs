using Abp.Modules;
using Abp.Reflection.Extensions;

namespace inzibackend
{
    public class inzibackendClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendClientModule).GetAssembly());
        }
    }
}
