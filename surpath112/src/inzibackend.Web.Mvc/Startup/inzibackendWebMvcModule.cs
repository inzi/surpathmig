using Abp.AspNetZeroCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using inzibackend.Auditing;
using inzibackend.Authorization.Users.Password;
using inzibackend.Configuration;
using inzibackend.EntityFrameworkCore;
using inzibackend.MultiTenancy;
using inzibackend.Web.Areas.App.Startup;
using Abp.EntityHistory;
using inzibackend.EntityHistory;
using inzibackend.Surpath.Jobs;

namespace inzibackend.Web.Startup
{
    [DependsOn(
        typeof(inzibackendWebCoreModule)
    )]
    public class inzibackendWebMvcModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public inzibackendWebMvcModule(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "https://localhost:44302/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
            Configuration.Navigation.Providers.Add<AppSPNavigationProvider>();
            Configuration.Navigation.Providers.Add<AppModNavigationProvider>();

            IocManager.Register<DashboardViewConfiguration>();
            IocManager.Register<IConfigurationRoot>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(inzibackendWebMvcModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }

            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());

            var expiredAuditLogDeleterWorker = IocManager.Resolve<ExpiredAuditLogDeleterWorker>();
            if (Configuration.Auditing.IsEnabled && expiredAuditLogDeleterWorker.IsEnabled)
            {
                workManager.Add(expiredAuditLogDeleterWorker);
            }

            workManager.Add(IocManager.Resolve<PasswordExpirationBackgroundWorker>());

            var complianceExpireBackgoundService = IocManager.Resolve<ComplianceExpireBackgoundService>();
            workManager.Add(complianceExpireBackgoundService);

            //workManager.Add(IocManager.Resolve<IConfigurationRoot>());
            //workManager.Add(IocManager.Resolve<ComplianceExpireBackgoundService>());
        }
    }
}