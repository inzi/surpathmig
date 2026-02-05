using System;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using inzibackend.EntityFrameworkCore;
using inzibackend.Migrations.Seed.Host;
using inzibackend.Migrations.Seed.Tenants;
using System.Diagnostics;
using inzibackend.SurpathSeedHelper;
using inzibackend.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Abp.EntityFrameworkCore;
using inzibackend.Surpath.Importing;
using System.Collections.Generic;
using Castle.Core.Logging;

namespace inzibackend.Migrations.Seed
{
    public static class SeedHelper
    {
        //public static ILogger Logger {get;set;}
        public static Log Log { get; private set; } = new Log();

        public static string SurpathRecordsRootFolder { get; private set; }

        private static SurpathliveSeedHelper surpathliveSeedHelper { get; set; }
        private static string _surpathliveConnectionString { get; set; }
        private static IUnitOfWorkManager _unitOfWorkManager { get; set; }
        private static List<ImportFromSurscanLiveJobArgs> ImportFromSurscanLiveJobArgsList { get; set; } = new List<ImportFromSurscanLiveJobArgs>();
        public static void SeedHostDb(IIocResolver iocResolver)
        {


            string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory(), _environment,true);

            // var _dir = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
            SurpathRecordsRootFolder = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");


            var _doImport = Configuration.GetSection("Surpath:DoImport").Get<bool>();

            _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");

            //var importArgs = Configuration.GetValue<List<ImportFromSurscanLiveJobArgs>>("Surpath:SurpathLiveClientIdsToImport");
            ImportFromSurscanLiveJobArgsList = Configuration.GetSection("Surpath:SurpathLiveClientIdsToImport").Get<List<ImportFromSurscanLiveJobArgs>>();
            if (ImportFromSurscanLiveJobArgsList == null) ImportFromSurscanLiveJobArgsList = new List<ImportFromSurscanLiveJobArgs>();
            _unitOfWorkManager = iocResolver.Resolve<IUnitOfWorkManager>();

            WithDbContext<inzibackendDbContext>(iocResolver, SeedHostDb);
           // WithDbContext<inzibackendDbContext>(iocResolver, CheckTenantIntegrity);
            if (!_doImport) return;



            surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);
            new DrugSeeder(iocResolver, surpathliveSeedHelper, _unitOfWorkManager).Create();
            WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathCodeTypeData);
            WithDbContext<inzibackendDbContext>(iocResolver, SPLImporter);


            //var _dbcontextprovider = iocResolver.Resolve<IDbContextProvider<inzibackendDbContext>>();
            //var _inzibackendDbContext = iocResolver.Resolve<inzibackendDbContext>();

            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathDrugData);

            //var _context = _dbcontextprovider.GetDbContext();

            // SeedSurpathDrugData(iocResolver);

            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathClientData);
            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathDonorData);
        }

        public static void SeedHostDb(inzibackendDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            //Host seed
            new InitialHostDbBuilder(context).Create();

            //Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();

        }


        //public static void SeedHostDb(inzibackendDbContext context, IIocResolver iocResolver)
        //{
        //    var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory());
        //    _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");
        //    surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);
        //    _unitOfWorkManager = iocResolver.Resolve<IUnitOfWorkManager>();

        //    WithDbContext<inzibackendDbContext>(iocResolver, SeedHostDb);


        //    new DrugSeeder(iocResolver, surpathliveSeedHelper, _unitOfWorkManager).Create();

        //    WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathCodeTypeData);



        //    WithDbContext<inzibackendDbContext>(iocResolver, SPLImporter);


        //}

        public static void SeedSurpathCodeTypeData(inzibackendDbContext _context)
        {
            new CodeTypeSeeder(_context, surpathliveSeedHelper).Create();
        }


        public static void SPLImporter(inzibackendDbContext _context)
        {


                ImportFromSurscanLiveJobArgsList.ForEach(_args =>
            {
                Log.Write("Import started with arguments:");
                Log.Write($"client_id: {_args.client_id}");
                Log.Write($"UserId: {_args.UserId}");
                Log.Write($"TenantId: {_args.TenantId}");
                Log.Write($"max_donors: {_args.max_donors}");
                Log.Write($"days_old: {_args.days_old}");
                new SPLImporter(_context, surpathliveSeedHelper, _args).ImportClient(_args).GetAwaiter().GetResult();

            });
            //var _args = new ImportFromSurscanLiveJobArgs()
            //{
            //    client_id = 117,
            //    UserId = 1,
            //    TenantId = null,
            //    max_donors = 220,
            //    days_old = 365
            //};

        }



        public static async void CheckTenantIntegrity(inzibackendDbContext _context)
        {
            _context.SuppressAutoSetTenantId = true;

            var _i = new SPTenantIntegrityChecker(_context);
            await _i.CheckHostAndTenants();
            ////Host seed
            //new InitialHostDbBuilder(context).Create();

            ////Default tenant seed (in host database).
            //new DefaultTenantBuilder(context).Create();
            //new TenantRoleAndUserBuilder(context, 1).Create();

        }



        public static void CheckTenantIntegrity(IIocResolver iocResolver)
        {


            string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory(), _environment, true);

            // var _dir = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
            SurpathRecordsRootFolder = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");


            var _doImport = Configuration.GetSection("Surpath:DoImport").Get<bool>();
            if (!_doImport) return;

            _surpathliveConnectionString = Configuration.GetValue<string>("ConnectionStrings:surpathlive");
            surpathliveSeedHelper = new SurpathliveSeedHelper(_surpathliveConnectionString);

            //var importArgs = Configuration.GetValue<List<ImportFromSurscanLiveJobArgs>>("Surpath:SurpathLiveClientIdsToImport");
            ImportFromSurscanLiveJobArgsList = Configuration.GetSection("Surpath:SurpathLiveClientIdsToImport").Get<List<ImportFromSurscanLiveJobArgs>>();
            if (ImportFromSurscanLiveJobArgsList == null) ImportFromSurscanLiveJobArgsList = new List<ImportFromSurscanLiveJobArgs>();
            _unitOfWorkManager = iocResolver.Resolve<IUnitOfWorkManager>();

            WithDbContext<inzibackendDbContext>(iocResolver, SeedHostDb);



            //new DrugSeeder(iocResolver, surpathliveSeedHelper, _unitOfWorkManager).Create();

            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathCodeTypeData);



            //WithDbContext<inzibackendDbContext>(iocResolver, SPLImporter);



            //var _dbcontextprovider = iocResolver.Resolve<IDbContextProvider<inzibackendDbContext>>();
            //var _inzibackendDbContext = iocResolver.Resolve<inzibackendDbContext>();

            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathDrugData);

            //var _context = _dbcontextprovider.GetDbContext();

            // SeedSurpathDrugData(iocResolver);

            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathClientData);
            //WithDbContext<inzibackendDbContext>(iocResolver, SeedSurpathDonorData);
        }




        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }

        //public static string GetDestFolder(int? TenantId, long? UserId)
        //{
        //    // var destfolder = $"{appFolders.SurpathRootFolder}";
        //    var _tenantid = TenantId == null ? "surscan" : TenantId.Value.ToString();
        //    var destFolder = Path.Combine(SurpathRecordsRootFolder, _tenantid, UserId.ToString());
        //    destFolder = destFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        //    if (!Directory.Exists(destFolder))
        //    {
        //        Directory.CreateDirectory(destFolder);
        //    }
        //    return destFolder;
        //}

        //private static void WithDbContextNoUOW<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
        //    where TDbContext : DbContext
        //{
        //    using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
        //    {
        //        //using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
        //        //{
        //            var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

        //            contextAction(context);

        //        //    uow.Complete();
        //        //}
        //    }
        //}
    }
}
