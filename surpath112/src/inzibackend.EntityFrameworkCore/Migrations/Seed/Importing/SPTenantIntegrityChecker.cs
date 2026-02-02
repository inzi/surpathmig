using Abp;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Runtime.Session;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Configuration;
using inzibackend.Editions;
using inzibackend.EntityFrameworkCore;
using inzibackend.MultiTenancy;
using inzibackend.Notifications;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Surpath.Importing;
using inzibackend.SurpathSeedHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace inzibackend.Migrations.Seed.Host
{
    public class SPTenantIntegrityChecker
    {
        public Log Log { get; set; }

        private ParamHelper paramHelper;
        private inzibackendDbContext _context;
        private ImportFromSurscanLiveJobArgs args;
        private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
        private List<ClientRecordCategoryClass> clientRecordCategoryClasses { get; set; }
        private List<ClientUserOrAdmin> clientAdmins { get; set; }


        private IUnitOfWorkManager _unitOfWorkManager;
        private List<ClientUserOrAdmin> clientUsers { get; set; }


        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly string SurpathRecordsRootFolder;
        //private readonly IConfigurationRoot _appConfiguration;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private IAbpSession _session;
        private IQueryable<CodeType> _deptCodes;

        public SPTenantIntegrityChecker(inzibackendDbContext context)
        {
            _context = context;
            string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory(), _environment, true);
            SurpathRecordsRootFolder = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
            Log = SeedHelper.Log;
        }


        public async Task CheckIntegrity()
        {

        }



        public async Task CheckHost()
        {
            // Record statuses
            await CheckHostRecordStatuses();
            // PidTypes
            await CheckHostPidTypes();

            await CheckHostSurpathServices();
        }

        public async Task CheckHostSurpathServices()
        {
            if (!_context.SurpathServices.IgnoreQueryFilters().Any(r => r.TenantId == null))
            {
                // add default services
                var _serviceList = new List<SurpathService>()
                {
                    new SurpathService(){Name = "Drug Testing", IsEnabledByDefault = true},
                    new SurpathService(){Name = "Background Check", IsEnabledByDefault=true},
                };
                //foreach(var _service in _serviceList)
                _serviceList.ForEach(s=> _context.SurpathServices.Add(s));
            }
        }

        public async Task CheckHostPidTypes()
        {
            if (!_context.PidTypes.IgnoreQueryFilters().Any(r => r.TenantId == null))
            {
                var _HostPidTypes = new List<PidType>()
                    {
                        new PidType(){Name="SSN",Description="Social Security Number",MaskPid=true, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null, Required = true, PidInputMask="surscan_inputmask_ssn"},
                        new PidType(){Name="DL",Description="Driver's License",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
                        new PidType(){Name="Passport",Description="Passport Number",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
                        new PidType(){Name="StudentID",Description="Student Id",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},
                        //new PidType(){Name="StudentID",Description="Student Id",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true, PidRegex = "", TenantId = null},

                    };
                _HostPidTypes.ForEach(p =>
                {
                    _context.PidTypes.Add(p);
                });
                _context.SaveChanges();
            }



        }

        public async Task CheckHostAndTenants()
        {
            await CheckHost();
            await CheckTenants();
        }


        public async Task CheckHostDepartments()
        {
            // Host has no departments
        }


        public async Task CheckHostRecordStatuses()
        {
            if (!_context.RecordStatuses.IgnoreQueryFilters().Any(r => r.TenantId == null))
            {

                var _statusesDefauls = new List<RecStatDefault>()
                    {
                        new RecStatDefault(){StatusName = "Needs Review", HtmlColor ="#bfb77e", IsDefault = true},
                        new RecStatDefault(){StatusName = "For Approval", HtmlColor ="#93c967", IsDefault = false},
                        new RecStatDefault(){StatusName = "Accepted", HtmlColor ="#67c777", IsDefault = false},
                        new RecStatDefault(){StatusName = "Not Accepted", HtmlColor ="#ba323b", IsDefault = false},
                        new RecStatDefault(){StatusName = "Need more info", HtmlColor ="#7bced4", IsDefault = false},
                        new RecStatDefault(){StatusName = "Under Review", HtmlColor ="#4b71c9", IsDefault = false},
                        new RecStatDefault(){StatusName = "Needs Signature", HtmlColor ="#7856dd", IsDefault = false},
                        new RecStatDefault(){StatusName = "Uknown", HtmlColor ="#bd9a93", IsDefault = false},
                    };

                _statusesDefauls.ForEach(s =>
                {
                    _context.RecordStatuses.Add(new RecordStatus()
                    {
                        HtmlColor = s.HtmlColor,
                        IsDefault = false,
                        StatusName = s.StatusName,
                        TenantId = null,
                        TenantDepartmentId = null,
                        IsSurpathServiceStatus = false
                    });
                    _context.SaveChanges();
                });
            }
        }




        public async Task CheckTenantRecordStatuses(int _tenantId)
        {


            if (!_context.RecordStatuses.IgnoreQueryFilters().Any(r => r.TenantId == _tenantId))
            {
                var _Defaults = _context.RecordStatuses.AsNoTracking().IgnoreQueryFilters().Where(r => r.TenantId == null).ToList();

                _Defaults.ForEach(s =>
                {
                    _context.RecordStatuses.Add(new RecordStatus()
                    {
                        HtmlColor = s.HtmlColor,
                        IsDefault = false,
                        StatusName = s.StatusName,
                        TenantId = _tenantId,
                        TenantDepartmentId = null,
                        IsSurpathServiceStatus = false
                    });
                    _context.SaveChanges();
                });
            }
        }

        public async Task CheckTenantSurpathServices(int _tenantId)
        {
            //if (!_context.TenantSurpathServices.IgnoreQueryFilters().Any(r => r.TenantId == _tenantId))
            //{
            //    var _hostServices = _context.SurpathServices.AsNoTracking().IgnoreQueryFilters().Where(p => p.TenantId == null).ToList();

            //    _hostServices.ForEach(p =>
            //    {
            //        _context.TenantSurpathServices.Add(new TenantSurpathService()
            //        {
            //            IsEnabled = p.IsEnabledByDefault,
            //            SurpathServiceId = p.Id,
            //            TenantId= _tenantId,
            //        });
            //    });
            //    _context.SaveChanges();

            //}
        }

        public async Task CheckTenantPidTypes(int _tenantId)
        {

            if (!_context.PidTypes.IgnoreQueryFilters().Any(r => r.TenantId == _tenantId))
            {
                var _HostPidTypes = _context.PidTypes.AsNoTracking().IgnoreQueryFilters().Where(p => p.TenantId == null).ToList();

                _HostPidTypes.ForEach(p =>
                {
                    _context.PidTypes.Add(new PidType()
                    {
                        Name = p.Name,
                        Description = p.Description,
                        CreatedBy = 1,
                        CreatedOn = DateTime.UtcNow,
                        MaskPid = p.MaskPid,
                        PidRegex = p.PidRegex,
                        IsActive = p.IsActive,
                        TenantId = _tenantId
                    });
                });
                _context.SaveChanges();

            }
            //// check ssn
            ////var _ssnPidTypes = _context.PidTypes.Where(p => p.Name.ToLower() == "ssn" && p.TenantId == _tenantId).ToList();
            //var _ssnPidTypes = _context.PidTypes.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId).ToList();
            //_ssnPidTypes.ForEach(p =>
            //{
            //    if (p.Name.ToLower() == "ssn")
            //    {
            //        p.PidInputMask = "surscan_inputmask_ssns";
            //        _context.SaveChanges();

            //    }

            //});


        }

        public async Task SetTenantPidTypeInputMasks()
        {



            //var _ssnPidTypes = _context.PidTypes.Where(p => p.Name.ToLower() == "ssn" && p.TenantId == _tenantId).ToList();
            var _ssnPidTypes = _context.PidTypes.IgnoreQueryFilters().ToList();
            _ssnPidTypes.ForEach(p =>
            {
                // check ssn
                if (p.Name.ToLower() == "ssn")
                {
                    p.PidInputMask = "surscan_inputmask_ssn";
                    _context.SaveChanges();

                }

            });


        }


        public async Task CheckTenantDepartments(int _tenantId)
        {
            if (!_context.TenantDepartments.IgnoreQueryFilters().AsNoTracking().Any(d => d.TenantId == _tenantId))
            {
                _context.TenantDepartments.Add(new TenantDepartment()
                {
                    Name = "Generic Department",
                    Active = true,
                    Description = "This is an example department",
                    MROType = EnumClientMROTypes.MPOS,
                    TenantId = _tenantId
                });
            }
        }

        public async Task CheckTenantCohorts(int _tenantId)
        {
            if (!_context.Cohorts.IgnoreQueryFilters().AsNoTracking().Any(d => d.TenantId == _tenantId))
            {
                var _dept = _context.TenantDepartments.AsNoTracking().IgnoreQueryFilters().FirstOrDefault();
                _context.Cohorts.Add(new Cohort()
                {
                    Name = "Unassigned",
                    DefaultCohort = true,
                    Description = "This is a generic cohort, users will automatically be assigned to this cohort if they're not assigned manually or during registration",
                    TenantDepartmentId = _dept.Id,
                    TenantId = _tenantId

                });

            }
        }


        public async Task CheckTenants()
        {
            var _tenants = _context.Tenants.AsNoTracking().IgnoreQueryFilters().Where(t => t.IsDeleted == false).ToList();

            foreach (var t in _tenants)
            {
                var _thisTenantId = t.Id;
                await CheckTenantPidTypes(_thisTenantId);
                await CheckTenantDepartments(_thisTenantId);
                await CheckTenantCohorts(_thisTenantId);
                await CheckTenantSurpathServices(_thisTenantId);

            }
            await SetTenantPidTypeInputMasks();
        }
    }
}