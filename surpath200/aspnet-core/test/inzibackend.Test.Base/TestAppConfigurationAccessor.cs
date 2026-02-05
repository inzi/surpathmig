using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using inzibackend.Configuration;

namespace inzibackend.Test.Base;

public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
{
    public IConfigurationRoot Configuration { get; }

    public TestAppConfigurationAccessor()
    {
        Configuration = AppConfigurations.Get(
            typeof(inzibackendTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }
}
