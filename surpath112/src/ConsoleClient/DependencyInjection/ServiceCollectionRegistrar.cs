using Abp.Dependency;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inzibackend.Identity;
using inzibackend.Surpath;
using inzibackend.MultiTenancy;


namespace ConsoleClient.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            // Register TenantManager if it's not already registered
            if (!iocManager.IsRegistered<TenantManager>())
            {
                iocManager.Register<TenantManager>(DependencyLifeStyle.Transient);
            }

            WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
        }
    }
}
