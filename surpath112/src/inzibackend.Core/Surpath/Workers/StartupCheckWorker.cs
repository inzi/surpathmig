using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using inzibackend.Surpath;
using System;

namespace inzibackend.MultiTenancy
{
    public class StartupCheckWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private const int CheckPeriodAsMilliseconds = 1 * 60 * 60 * 1000; //1 hour

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;

        public StartupCheckWorker(
                AbpTimer timer,
                IRepository<Tenant> tenantRepository,
                TenantManager tenantManager,
                IRepository<SurpathService, Guid> surpathServiceRepository,
                IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
                IUnitOfWorkManager unitOfWorkManager
            )
            : base(timer)
        {
            _tenantRepository = tenantRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantManager = tenantManager;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;

            Timer.Period = CheckPeriodAsMilliseconds;
            Timer.RunOnStart = true;

            LocalizationSourceName = inzibackendConsts.LocalizationSourceName;
            _unitOfWorkManager = unitOfWorkManager;
        }

        protected override void DoWork()
        {
            Timer.Stop();
            _unitOfWorkManager.WithUnitOfWork(() =>
            {
                var utcNow = Clock.Now.ToUniversalTime();

                // TODO - make sure every tenant has tenentsurpathservices that match system wide surpathservices

                // TODO - make sure every tenant has pidtypes
            });
        }
    }
}