//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Abp.Authorization.Users;
//using Abp.BackgroundJobs;
//using Abp.Dependency;
//using Abp.Domain.Uow;
//using Abp.Extensions;
//using Abp.IdentityFramework;
//using Abp.Localization;
//using Abp.ObjectMapping;
//using Abp.UI;
//using Microsoft.AspNetCore.Identity;
//using inzibackend.Authorization.Roles;
//using inzibackend.Authorization.Users.Dto;
//using inzibackend.Authorization.Users.Importing.Dto;
//using inzibackend.Notifications;
//using inzibackend.Storage;
//using inzibackend.Authorization.Users.Importing;
//using inzibackend.Authorization.Users;
//using inzibackend.EntityFrameworkCore;
//using Abp.EntityFrameworkCore;
//using Abp.Domain.Repositories;
//using inzibackend.MultiTenancy;
//using inzibackend.SurpathSeedHelper;
//using inzibackend.Configuration;
//using System.IO;
//using Microsoft.Extensions.Configuration;
//using Abp.Runtime.Session;

//namespace inzibackend.Surpath.Importing
//{

//    // https://support.aspnetzero.com/QA/Questions/10239/Running-a-Hangfire-background-job-with-the-permissions-of-the-user-that-started-it
//    // 

//    public class importFromSurscanLiveJob : AsyncBackgroundJob<ImportFromSurscanLiveJobArgs>, ITransientDependency
//    {

//        private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
//        private IIocResolver iocResolver;
//        private IUnitOfWorkManager _unitOfWorkManager { get; set; }
//        protected readonly IBackgroundJobManager BackgroundJobManager;


//        public importFromSurscanLiveJob
//            (
//            IUnitOfWorkManager unitOfWorkManager,
//            IIocResolver _iocResolver,
//            IBackgroundJobManager backgroundJobManager)
//        {
//            iocResolver = _iocResolver;
//            var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory());
//            var _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");
//            _surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);
//            BackgroundJobManager = backgroundJobManager;

//        }

//        public override async Task ExecuteAsync(ImportFromSurscanLiveJobArgs args)
//        {
//            try
//            {
//                var _newargs = await new SPLImporter(_surpathliveSeedHelper, _unitOfWorkManager).ImportClient(args);

//                //ImportFromSurscanLiveJobArgs _newargs = await new ClientCreatorImporter(_surpathliveSeedHelper, _unitOfWorkManager).CreateClient(args);
//                //ImportFromSurscanLiveJobArgs _newargs = await new ClientCreatorImporter(_surpathliveSeedHelper, _unitOfWorkManager).CreateClient(args);

//                //await new ClientImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(_newargs);
//                //await new UserImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(args);



//                //await BackgroundJobManager.EnqueueAsync<importFromSurscanLiveJobPrep, ImportFromSurscanLiveJobArgs>(_newargs);
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }

//        }
//    }

//    //public class importFromSurscanLiveJobPrep : AsyncBackgroundJob<ImportFromSurscanLiveJobArgs>, ITransientDependency
//    //{

//    //    private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
//    //    private IIocResolver iocResolver;
//    //    private IUnitOfWorkManager _unitOfWorkManager { get; set; }
//    //    protected readonly IBackgroundJobManager BackgroundJobManager;


//    //    public importFromSurscanLiveJobPrep
//    //        (
//    //        IUnitOfWorkManager unitOfWorkManager,
//    //        IIocResolver _iocResolver,
//    //        IBackgroundJobManager backgroundJobManager)
//    //    {
//    //        iocResolver = _iocResolver;
//    //        var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory());
//    //        var _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");
//    //        _surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);
//    //        BackgroundJobManager = backgroundJobManager;

//    //    }

//    //    public override async Task ExecuteAsync(ImportFromSurscanLiveJobArgs args)
//    //    {
//    //        try
//    //        {
//    //            var _newargs = await new ClientImporter(_surpathliveSeedHelper, _unitOfWorkManager).PrepUser(args);
//    //            //await new ClientImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(_newargs);
//    //            //await new UserImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(args);

//    //            await BackgroundJobManager.EnqueueAsync<importFromSurscanLiveJobDepts, ImportFromSurscanLiveJobArgs>(_newargs);
//    //        }
//    //        catch (Exception ex)
//    //        {

//    //            throw;
//    //        }

//    //    }
//    //}

//    //public class importFromSurscanLiveJobDepts : AsyncBackgroundJob<ImportFromSurscanLiveJobArgs>, ITransientDependency
//    //{

//    //    private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
//    //    private IIocResolver iocResolver;
//    //    private IUnitOfWorkManager _unitOfWorkManager { get; set; }
//    //    protected readonly IBackgroundJobManager BackgroundJobManager;


//    //    public importFromSurscanLiveJobDepts
//    //        (
//    //        IUnitOfWorkManager unitOfWorkManager,
//    //        IIocResolver _iocResolver,
//    //        IBackgroundJobManager backgroundJobManager)
//    //    {
//    //        iocResolver = _iocResolver;
//    //        var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory());
//    //        var _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");
//    //        _surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);
//    //        BackgroundJobManager = backgroundJobManager;

//    //    }

//    //    public override async Task ExecuteAsync(ImportFromSurscanLiveJobArgs args)
//    //    {
//    //        try
//    //        {
//    //            var _newargs = await new ClientImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(args);

//    //            await BackgroundJobManager.EnqueueAsync<importFromSurscanLiveJobUsers, ImportFromSurscanLiveJobArgs>(_newargs);

//    //        }
//    //        catch (Exception ex)
//    //        {

//    //            throw;
//    //        }

//    //    }
//    //}


//    //public class importFromSurscanLiveJobUsers : AsyncBackgroundJob<ImportFromSurscanLiveJobArgs>, ITransientDependency
//    //{

//    //    private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
//    //    private IIocResolver iocResolver;
//    //    private IUnitOfWorkManager _unitOfWorkManager { get; set; }
//    //    protected readonly IBackgroundJobManager BackgroundJobManager;


//    //    public importFromSurscanLiveJobUsers
//    //        (
//    //        IUnitOfWorkManager unitOfWorkManager,
//    //        IIocResolver _iocResolver,
//    //        IBackgroundJobManager backgroundJobManager)
//    //    {
//    //        iocResolver = _iocResolver;
//    //        var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory());
//    //        var _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");
//    //        _surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);
//    //        BackgroundJobManager = backgroundJobManager;

//    //    }

//    //    public override async Task ExecuteAsync(ImportFromSurscanLiveJobArgs args)
//    //    {
//    //        try
//    //        {
//    //            var _newargs = await new UserImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(args);
//    //            //await new ClientImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(_newargs);
//    //            //await new UserImporter(_surpathliveSeedHelper, _unitOfWorkManager).DoImport(args);

//    //            //await BackgroundJobManager.EnqueueAsync<importFromSurscanLiveJob, ImportFromSurscanLiveJobArgs>(new ImportFromSurscanLiveJobArgs
//    //            //        {
//    //            //            client_id = 117,
//    //            //            TenantId = AbpSession.TenantId,
//    //            //            UserId = AbpSession.UserId,
//    //            //            max_donors = 100,
//    //            //            days_old = 365,

//    //            //        });
//    //        }
//    //        catch (Exception ex)
//    //        {

//    //            throw;
//    //        }

//    //    }
//    //}

//}
