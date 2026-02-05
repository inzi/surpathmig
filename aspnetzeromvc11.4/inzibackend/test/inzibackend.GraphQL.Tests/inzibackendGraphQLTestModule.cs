using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using inzibackend.Configure;
using inzibackend.Startup;
using inzibackend.Test.Base;

namespace inzibackend.GraphQL.Tests
{
    [DependsOn(
        typeof(inzibackendGraphQLModule),
        typeof(inzibackendTestBaseModule))]
    public class inzibackendGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendGraphQLTestModule).GetAssembly());
        }
    }
}