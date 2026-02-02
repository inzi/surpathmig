//using Abp.Dependency;
//using Abp.Domain.Uow;
//using Abp.EntityFrameworkCore.Uow;
//using Abp.MultiTenancy;
//using Abp.Runtime.Session;
//using inzibackend.Authorization.Accounts;
//using inzibackend.Authorization.Roles;
//using inzibackend.Authorization.Users;
//using inzibackend.Editions;
//using inzibackend.EntityFrameworkCore;
//using inzibackend.MultiTenancy;
//using inzibackend.MultiTenancy.Dto;
//using inzibackend.SurpathSeedHelper;
//using Microsoft.EntityFrameworkCore;
//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Transactions;
//using Microsoft.AspNetCore.Identity;
//using Abp.UI;
//using Abp.Application.Services.Dto;

//namespace inzibackend.Surpath.Importing
//{
//    public class ClientImporter
//    {
//        private ParamHelper paramHelper;
//        private inzibackendDbContext _context;
//        private ImportFromSurscanLiveJobArgs args;
//        private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
//        private List<ClientRecordCategoryClass> clientRecordCategoryClasses { get; set; }
//        private List<ClientUserOrAdmin> clientAdmins { get; set; }


//        private IUnitOfWorkManager _unitOfWorkManager;
//        //private I IocManager.Instance  IocManager.Instance;

//        private IAbpSession _session;
//        private IQueryable<CodeType> _deptCodes;

//        public ClientImporter(SurpathliveSeedHelper surpathliveSeedHelper, IUnitOfWorkManager unitOfWorkManager)
//        {
//            //_context = context;
//            _surpathliveSeedHelper = surpathliveSeedHelper;
//            paramHelper = new ParamHelper();

//            _unitOfWorkManager = unitOfWorkManager;
//            //IocManager.Instance = _ IocManager.Instance;
//        }


//        public async Task<ImportFromSurscanLiveJobArgs> PrepUser(ImportFromSurscanLiveJobArgs _args)
//        {
//            args = _args;

//            using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//            {
//                using (var permUow = uowManager.Object.Begin())
//                {
//                    using (var um = IocManager.Instance.Resolve<UserManager>())
//                    {
//                        _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
//                        var _t = _context.Tenants.First(t => t.TenancyName == args.NewTenancyName);
//                        args.NewTenantId = _t.Id;
//                        using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                        {
//                            _session = _sessionObj.Object;
//                            _session.Use(args.TenantId, args.UserId);
//                            //var user = await um.GetUserByIdAsync((long)args.NewTeantAdminId);
//                            var user = _context.Users.IgnoreQueryFilters().First(u => u.TenantId == args.NewTenantId && u.EmailAddress.ToLower().Equals(args.NewAdminEmail.ToLower()));

//                            using (var _service = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
//                            {

//                                var p1 = await _service.Object.GetUserPermissionsForEdit(new EntityDto<long>() { Id = user.Id });

//                                await _service.Object.ResetUserSpecificPermissions(new EntityDto<long>()
//                                {
//                                    Id = user.Id,

//                                });

//                                var p2 = await _service.Object.GetUserPermissionsForEdit(new EntityDto<long>() { Id = user.Id });
//                            }

//                            args.NewTeantAdminId = user.Id;

//                        };
//                    };
//                    await permUow.CompleteAsync();
//                };
//            }
//            return args;
//        }

//        public async Task<ImportFromSurscanLiveJobArgs> DoImport(ImportFromSurscanLiveJobArgs _args)
//        {
//            args = _args;
//            try
//            {
//                using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//                {
//                    using (var mainUow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                    {
//                        _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
//                        using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                        {
//                            _session = _sessionObj.Object;
//                            _session.Use(args.TenantId, args.UserId);


//                            clientRecordCategoryClasses = GetClientRecordCategories();
//                            clientAdmins = GetTenantAdminUsers();

//                            //int _tenantId = 0;
//                            //using (var _tenantAppService = IocManager.Instance.ResolveAsDisposable<ITenantAppService>())
//                            //{
//                            //    var _tenant = await _tenantAppService.Object.GetTenants(new GetTenantsInput()
//                            //    {
//                            //        EditionId = -1,
//                            //        EditionIdSpecified = false,
//                            //        Filter = args.NewTenancyName,
//                            //        MaxResultCount = 1,
//                            //        Sorting = "TenancyName"
//                            //    });
//                            //    _tenantId = _tenant.Items[0].Id;
//                            //    args.NewTenantId = _tenantId;
//                            //}
//                            //_session.Use(args.NewTenantId, args.UserId);


//                            //var newAdmin = await _context.Users.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == args.NewTeantAdminId);
//                            //if (newAdmin == null) throw new UserFriendlyException("Can't find admin!");

//                            //args.NewTeantAdminId = newAdmin.Id;




//                            //_session.Use(null, args.UserId);
//                            //using (var um = IocManager.Instance.Resolve<UserManager>())
//                            //{
//                            //    var user = await um.GetUserByIdAsync((long)args.NewTeantAdminId);
//                            //    await um.ResetAllPermissionsAsync(user);
//                            //};
//                            //_session.Use(args.NewTenantId, args.UserId);

//                            //using (var um = IocManager.Instance.Resolve<UserManager>())
//                            //{
//                            //    //var uli = new UserLoginInfo()


//                            //    //var u = um.Find(args.NewTenantId,new UserLoginInfo()
//                            //    //{

//                            //    //})
//                            //    var u = await um.FindByNameAsync("Admin");
//                            //    args.NewTeantAdminId = u.Id;
//                            //};

//                            //using (var _service = IocManager.Instance.ResolveAsDisposable<IAccountAppService>())
//                            //{
//                            //    var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => args.NewTenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);


//                            //    //var _adminUserResult = await _service.Object.
//                            //    //var _adminUserResult = await _service.Object.GetUsers(new Authorization.Users.Dto.GetUsersInput()
//                            //    //{
//                            //    //    Role = adminRole.Id,
//                            //    //    Filter = "Admin",
//                            //    //    Sorting = "Name"
//                            //    //});
//                            //    var _newadmin = _adminUserResult.Items[0].Id;
//                            //    args.NewTeantAdminId = _newadmin;
//                            //}

//                            //using (var _service = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
//                            //{
//                            //    var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => args.NewTenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
//                            //    var _adminUserResult = await _service.Object.GetUsers(new Authorization.Users.Dto.GetUsersInput()
//                            //    {
//                            //        Role = adminRole.Id,
//                            //        Filter = "Admin",
//                            //        Sorting = "Name"
//                            //    });
//                            //    var _newadmin = _adminUserResult.Items[0].Id;
//                            //    args.NewTeantAdminId = _newadmin;
//                            //}
//                            if (args.Created)
//                            {
//                                //args.TenantId = args.NewTenantId;
//                                //args.UserId = args.NewTeantAdminId;
//                                _session.Use(args.NewTenantId, args.NewTeantAdminId);
//                                // now do the rest with this tenant's ids

//                                //using (var mainUow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                                //{



//                                //var _tenant = await _context.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.TenancyName.Equals(args.NewTenancyName));
//                                await CreateDefaultCohort();

//                                _deptCodes = _context.CodeTypes.AsNoTracking().IgnoreQueryFilters();
//                                await CreateDepartments();

//                                mainUow.Complete();
//                                //}
//                            }
//                        }
//                    }
//                }


//                //paramHelper.reset();
//                //paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//                //var sql = "select * from clients where is_client_active = 1 and client_id = @client_id;";
//                //if (_surpathliveSeedHelper.conn.State == ConnectionState.Closed) _surpathliveSeedHelper.conn.Open();
//                //MySqlCommand _cmd = new MySqlCommand(sql, _surpathliveSeedHelper.conn);
//                //_cmd.Parameters.Clear();

//                //foreach (MySqlParameter parameter in paramHelper.ParmList)
//                //{
//                //    _cmd.Parameters.Add(parameter);
//                //}


//                //using (MySqlDataReader reader = _cmd.ExecuteReader())
//                //{
//                //    while (reader.Read())
//                //    {
//                //        var client_code = reader.GetString("client_code");
//                //        var client_name = reader.GetString("client_name");
//                //        var _client_code = Regex.Replace(client_code, "[^a-zA-Z0-9._]", string.Empty).ToLower();
//                //        using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//                //        {
//                //            using (var mainUow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                //            {
//                //                using (var _Uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                //                {
//                //                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
//                //                    using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                //                    {
//                //                        _session = _sessionObj.Object;
//                //                        _session.Use(args.TenantId, args.UserId);

//                //                        var _tenentexists = _context.Tenants.AsNoTracking().IgnoreQueryFilters().Any(t => t.IsDeleted == false && (t.ClientCode == _client_code || t.TenancyName == client_name));

//                //                        if (!_tenentexists)
//                //                        {
//                //                            var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
//                //                            var input = new CreateTenantInput
//                //                            {
//                //                                TenancyName = _client_code,
//                //                                Name = client_name,
//                //                                AdminEmailAddress = "chris@borps.com",
//                //                                ClientCode = _client_code,
//                //                                AdminPassword = "123qwe",
//                //                                IsActive = true,
//                //                                EditionId = defaultEdition.Id
//                //                            };
//                //                            using (var _tenantAppService = IocManager.Instance.ResolveAsDisposable<ITenantAppService>())
//                //                            {
//                //                                await _tenantAppService.Object.CreateTenant(input);
//                //                            }
//                //                        }
//                //                        _context.SaveChanges();
//                //                        _Uow.Complete();
//                //                    }
//                //                }
//                //                _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
//                //                mainUow.Complete();
//                //            }
//                //        }


//                //        // get our ids'

//                //    }
//                //}



//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//            return args;
//        }



//        //private async Task<bool> CreateClient(string client_name, string client_code)
//        //{
//        //    try
//        //    {
//        //        var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
//        //        var input = new CreateTenantInput
//        //        {
//        //            TenancyName = client_code,
//        //            Name = client_name,
//        //            AdminEmailAddress = "chris@borps.com",
//        //            ClientCode = client_code,
//        //            AdminPassword = "123qwe",
//        //            IsActive = true,
//        //            EditionId = defaultEdition.Id
//        //        };
//        //        using (var _tenantAppService = IocManager.Instance.ResolveAsDisposable<ITenantAppService>())
//        //        {
//        //            await _tenantAppService.Object.CreateTenant(input);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw;
//        //    }
//        //    return true;
//        //}

//        private async Task<bool> CreateDepartments()
//        {

//            paramHelper.reset();

//            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//            SurpathliveSeedHelper surpathliveSeedHelperDepts = new(_surpathliveSeedHelper.ConnectionString);

//            var sqlDepts = @"select
//        cd.*,
//        c.client_code
//        from
//        client_departments cd
//        left outer join clients c on c.client_id = cd.client_id
//        where c.client_id = @client_id;";

//            if (surpathliveSeedHelperDepts.conn.State == ConnectionState.Closed) surpathliveSeedHelperDepts.conn.Open();
//            MySqlCommand cmdDepts = new MySqlCommand(sqlDepts, surpathliveSeedHelperDepts.conn);
//            foreach (MySqlParameter parameter in paramHelper.ParmList)
//            {
//                cmdDepts.Parameters.Add(parameter);
//            }
//            using (var uowmCreateDept = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//            {
//                using (MySqlDataReader reader = cmdDepts.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        Debug.WriteLine($"Creating department for {args.NewTenancyName} -> {reader.SafeGetString("department_name")}");

//                        var _oldDeptId = reader.GetInt32("client_department_id");
//                        var _deptName = reader.GetString("department_name");

//                        using (var _departmentsAppService = IocManager.Instance.ResolveAsDisposable<ITenantDepartmentsAppService>())
//                        {
//                            //_session.Use(args.NewTenantId, args.NewTeantAdminId);
//                            var _dept = new TenantDepartment() { Active = true, Name = _deptName, Description = "Imported", Id = Guid.NewGuid(), MROType = (EnumClientMROTypes)reader.GetInt32("mro_type_id"), TenantId = args.NewTenantId };
//                            using (var uowCreateDept = uowmCreateDept.Object.Begin(TransactionScopeOption.Suppress))
//                            {
//                                //await _departmentsAppService.Object.CreateOrEdit(new Dtos.CreateOrEditTenantDepartmentDto()
//                                //{
//                                //    Active = true,
//                                //    Name = _deptName,
//                                //    MROType = (EnumClientMROTypes)reader.GetInt32("mro_type_id")
//                                //});
//                                var tcontext = uowmCreateDept.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);


//                                tcontext.TenantDepartments.Add(_dept);
//                                tcontext.SaveChanges(true);
//                                uowCreateDept.Complete();
//                            }


//                            //var _deptres = await _departmentsAppService.Object.GetAll(new Dtos.GetAllTenantDepartmentsInput()
//                            //{
//                            //    Filter = _deptName
//                            //});
//                            if (_dept.Id != Guid.Empty)
//                            {
//                                //var _dept = _deptres.Items[0].TenantDepartment;
//                                using (var uowDeptCodes = uowmCreateDept.Object.Begin(TransactionScopeOption.Suppress))
//                                {
//                                    using (var _deptCodesAppService = IocManager.Instance.ResolveAsDisposable<IDeptCodesAppService>())
//                                    {
//                                        // create this depts codes
//                                        if (reader["lab_code"] != DBNull.Value)
//                                        {
//                                            if (!string.IsNullOrEmpty(reader.GetString("lab_code")))
//                                            {
//                                                await _deptCodesAppService.Object.CreateOrEdit(new Dtos.CreateOrEditDeptCodeDto()
//                                                {
//                                                    Code = reader.GetString("lab_code"),
//                                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Lab").FirstOrDefault().Id,
//                                                    TenantDepartmentId = _dept.Id,
//                                                    // TenantId = _tenantId
//                                                });
//                                            }
//                                            if (reader["ClearStarCode"] != DBNull.Value)
//                                            {
//                                                if (!string.IsNullOrEmpty(reader.GetString("ClearStarCode")))
//                                                {
//                                                    await _deptCodesAppService.Object.CreateOrEdit(new Dtos.CreateOrEditDeptCodeDto()
//                                                    {
//                                                        Code = reader.GetString("ClearStarCode"),
//                                                        CodeTypeId = _deptCodes.Where(c => c.Name == "Clear Star").FirstOrDefault().Id,
//                                                        TenantDepartmentId = _dept.Id,
//                                                        // TenantId = _tenantId
//                                                    });
//                                                }
//                                            }

//                                            if (reader["QuestCode"] != DBNull.Value)
//                                            {
//                                                if (!string.IsNullOrEmpty(reader.GetString("QuestCode")))
//                                                {
//                                                    await _deptCodesAppService.Object.CreateOrEdit(new Dtos.CreateOrEditDeptCodeDto()
//                                                    {
//                                                        Code = reader.GetString("QuestCode"),
//                                                        CodeTypeId = _deptCodes.Where(c => c.Name == "Quest").FirstOrDefault().Id,
//                                                        TenantDepartmentId = _dept.Id,
//                                                        // TenantId = _tenantId
//                                                    });
//                                                }
//                                            }

//                                            if (reader["FormFoxCode"] != DBNull.Value)
//                                            {
//                                                if (!string.IsNullOrEmpty(reader.GetString("FormFoxCode")))
//                                                {
//                                                    await _deptCodesAppService.Object.CreateOrEdit(new Dtos.CreateOrEditDeptCodeDto()
//                                                    {
//                                                        Code = reader.GetString("FormFoxCode"),
//                                                        CodeTypeId = _deptCodes.Where(c => c.Name == "FormFox").FirstOrDefault().Id,
//                                                        TenantDepartmentId = _dept.Id,
//                                                        // TenantId = _tenantId
//                                                    });
//                                                }
//                                            }
//                                        }
//                                    }
//                                    uowDeptCodes.Complete();
//                                }
//                                var rcrId = Guid.Empty;
//                                using (var uowRecCatRules = uowmCreateDept.Object.Begin(TransactionScopeOption.Suppress))
//                                {
//                                    using (var _rulesAppService = IocManager.Instance.ResolveAsDisposable<IRecordCategoryRulesAppService>())
//                                    {
//                                        var rcr = new RecordCategoryRule()
//                                        {
//                                            TenantId = args.NewTenantId,
//                                            Name = $"{_dept.Name} Example Record Rule",
//                                            Description = "An example record rule",
//                                            Notify = false,
//                                            ExpireInDays = 90,
//                                            WarnDaysBeforeFirst = 45,
//                                            WarnDaysBeforeSecond = 25,
//                                            WarnDaysBeforeFinal = 10,
//                                            Expires = true,
//                                            Required = true
//                                        };
//                                        _context.RecordCategoryRules.Add(rcr);
//                                        _context.SaveChanges();
//                                        rcrId = rcr.Id;
//                                    }
//                                    uowRecCatRules.Complete();
//                                }
//                                var rrId = Guid.Empty;
//                                using (var uowRecCatReg = uowmCreateDept.Object.Begin(TransactionScopeOption.Suppress))
//                                {

//                                    var rr = new RecordRequirement()
//                                    {
//                                        TenantId = args.NewTenantId,
//                                        Name = $"{_dept.Name} Example Record Requirement",
//                                        Description = "An example record requirement",
//                                        Metadata = "{}"
//                                    };
//                                    _context.RecordRequirements.Add(rr);
//                                    _context.SaveChanges();
//                                    rrId = rr.Id;
//                                }
//                                using (var uowCatClasses = uowmCreateDept.Object.Begin(TransactionScopeOption.Suppress))
//                                {
//                                    var l = clientRecordCategoryClasses.Where(rc => rc.client_department_id == _oldDeptId).ToList();

//                                    l.ForEach(rc =>
//                                    {
//                                        var _rc = new RecordCategory();
//                                        _rc.TenantId = args.NewTenantId;
//                                        _rc.Name = rc.description.Trim();
//                                        _rc.Instructions = rc.instructions;
//                                        _rc.RecordRequirementId = rrId;
//                                        _rc.RecordCategoryRuleId = rcrId;
//                                        _context.RecordCategories.Add(_rc);
//                                        _context.SaveChanges();
//                                    });


//                                    uowCatClasses.Complete();
//                                }
//                            }
//                        }
//                    }


//                }
//            }
//            return true;
//        }

//        //private async Task<bool> CreateClientDepartments()
//        //{
//        //    // Create Departments for tenant
//        //    var _depts = _context.TenantDepartments.IgnoreQueryFilters().FirstOrDefault(d => d.TenantId == args.NewTenantId);
//        //    if (_depts == null)
//        //    {
//        //        var _deptCodes = _context.CodeTypes.AsNoTracking().IgnoreQueryFilters();

//        //        paramHelper.reset();

//        //        paramHelper.Param = new MySqlParameter("@client_code", client_code);
//        //        SurpathliveSeedHelper surpathliveSeedHelperDepts = new(_surpathliveSeedHelper.ConnectionString);
//        //        var sqlDepts = @"select
//        //cd.*,
//        //c.client_code
//        //from
//        //client_departments cd
//        //left outer join clients c on c.client_id = cd.client_id
//        //where c.client_code = @client_code;";
//        //        if (surpathliveSeedHelperDepts.conn.State == ConnectionState.Closed) surpathliveSeedHelperDepts.conn.Open();
//        //        MySqlCommand cmdDepts = new MySqlCommand(sqlDepts, surpathliveSeedHelperDepts.conn);
//        //        foreach (MySqlParameter parameter in paramHelper.ParmList)
//        //        {
//        //            cmdDepts.Parameters.Add(parameter);
//        //        }
//        //        using (MySqlDataReader reader = cmdDepts.ExecuteReader())
//        //        {
//        //            while (reader.Read())
//        //            {
//        //                Debug.WriteLine($"Creating department for {client_name} -> {reader.GetString("department_name")}");

//        //                var _oldDeptId = reader.GetInt32("client_department_id");
//        //                var _newDept = new TenantDepartment()
//        //                {
//        //                    Name = reader.GetString("department_name"),
//        //                    TenantId = _tenantId,
//        //                    MROType = (EnumClientMROTypes)reader.GetInt32("mro_type_id")
//        //                };

//        //                _depts = _context.TenantDepartments.Add(_newDept).Entity;
//        //                _context.SaveChanges();

//        //                // Add a dept default record cat rule
//        //                var rcr = new RecordCategoryRule()
//        //                {
//        //                    TenantId = _tenantId,
//        //                    Name = $"{_newDept.Name} Example Record Rule",
//        //                    Description = "An example record rule",
//        //                    Notify = false,
//        //                    ExpireInDays = 90,
//        //                    WarnDaysBeforeFirst = 45,
//        //                    WarnDaysBeforeSecond = 25,
//        //                    WarnDaysBeforeFinal = 10,
//        //                    Expires = true,
//        //                    Required = true
//        //                };
//        //                _context.RecordCategoryRules.Add(rcr);
//        //                _context.SaveChanges();
//        //                var rr = new RecordRequirement()
//        //                {
//        //                    TenantId = _tenantId,
//        //                    Name = $"{_newDept.Name} Example Record Requirement",
//        //                    Description = "An example record requirement",
//        //                    Metadata = "{}"
//        //                };
//        //                _context.RecordRequirements.Add(rr);
//        //                _context.SaveChanges();
//        //                // Add the depts record category

//        //                clientRecordCategoryClasses.Where(rc => rc.client_department_id == _oldDeptId).ToList().ForEach(rc =>
//        //                {
//        //                    var _rc = new RecordCategory();
//        //                    _rc.TenantId = _tenantId;
//        //                    _rc.Name = rc.description.Trim();
//        //                    _rc.Instructions = rc.instructions;
//        //                    _rc.RecordRequirementId = rr.Id;
//        //                    _rc.RecordCategoryRuleId = rcr.Id;
//        //                    _context.RecordCategories.Add(_rc);
//        //                });
//        //                _context.SaveChanges();
//        //            }
//        //        }
//        //        // _context.SaveChanges();
//        //        surpathliveSeedHelperDepts.conn.Close();
//        //        surpathliveSeedHelperDepts = null;
//        //    }
//        //    return true;
//        //}

//        private async Task<bool> CreateDefaultCohort()
//        {
//            var _cohort = new Cohort()
//            {
//                Name = "Unassigned Cohort",
//                Description = "This is the default cohort new users will be assigned to when they join the system",
//                TenantId = args.NewTenantId,
//                DefaultCohort = true
//            };
//            _context.Cohorts.Add(_cohort);
//            _context.SaveChanges();
//            return true;
//        }

//        private List<ClientUserOrAdmin> GetTenantAdminUsers()
//        {
//            try
//            {
//                var admins = new List<ClientUserOrAdmin>();

//                var sql = @"
//select distinct
//max(u.user_id) as user_id,
//u.user_email,
//u.user_first_name,
//u.user_last_name,
//u.user_password,
//c.client_id,
//c.client_code,
//c.client_name,
//c.is_client_active,
//cd.client_department_id,
//cd.is_contact_info_as_client,
//cd.lab_code
//from users u
//left outer join user_departments ud on ud.user_id = u.user_id
//left outer join client_departments cd on cd.client_department_id = ud.client_department_id
//left outer join clients c on c.client_id = cd.client_id
//where
//u.is_user_active = 1
//and u.user_email not like '%@surscan.com'
//and u.user_email not like '%@tx.rr.com'
//and c.client_id = @client_id
//and cd.lab_code is not null and cd.lab_code != ''
//and c.is_client_active = 1
//group by u.user_email;
//";

//                SurpathliveSeedHelper ClientAdminsHelper = new(_surpathliveSeedHelper.ConnectionString);
//                if (ClientAdminsHelper.conn.State == ConnectionState.Closed) ClientAdminsHelper.conn.Open();
//                MySqlCommand cmdDepts = new MySqlCommand(sql, ClientAdminsHelper.conn);
//                cmdDepts.Parameters.Clear();
//                paramHelper.reset();
//                paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//                foreach (MySqlParameter parameter in paramHelper.ParmList)
//                {
//                    cmdDepts.Parameters.Add(parameter);
//                }

//                using (var reader = cmdDepts.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var _ClientAdmin = new ClientUserOrAdmin()
//                        {
//                            client_code = reader.SafeGetString("client_code"),
//                            client_department_id = reader.GetInt32("client_department_id"),
//                            is_client_active = reader.GetInt32("is_client_active"),
//                            is_contact_info_as_client = reader.GetInt32("is_contact_info_as_client"),
//                            client_id = reader.GetInt32("client_id"),
//                            user_id = reader.GetInt32("user_id"),
//                            user_email = reader.SafeGetString("user_email"),
//                            user_first_name = reader.SafeGetString("user_first_name"),
//                            user_last_name = reader.SafeGetString("user_last_name"),
//                            user_password = reader.SafeGetString("user_password"),
//                            lab_code = reader.SafeGetString("lab_code")
//                        };
//                        admins.Add(_ClientAdmin);
//                    }
//                }
//                return admins;
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        private List<ClientRecordCategoryClass> GetClientRecordCategories()
//        {
//            paramHelper.reset();
//            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//            var retlist = new List<ClientRecordCategoryClass>();
//            var sql = @"
//select distinct
//c.client_id,
//cd.client_department_id,
//cd.lab_code,
//c.client_code
//,ccd.description
//,ccd.instructions
//from clients c
//inner join client_departments cd on cd.client_id = c.client_id
//inner join client_dept_doctypes ccd on ccd.client_department_id = cd.client_department_id
//where c.client_id = @client_id
//order by c.client_id, cd.client_department_id;
//        ";

//            if (_surpathliveSeedHelper.conn.State == ConnectionState.Closed) _surpathliveSeedHelper.conn.Open();
//            MySqlCommand _cmd = new MySqlCommand(sql, _surpathliveSeedHelper.conn);
//            _cmd.Parameters.Clear();

//            foreach (MySqlParameter parameter in paramHelper.ParmList)
//            {
//                _cmd.Parameters.Add(parameter);
//            }

//            using (var reader = _cmd.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    var nr = new ClientRecordCategoryClass();
//                    nr.client_code = reader.SafeGetString("client_code");
//                    nr.lab_code = reader.SafeGetString("lab_code");
//                    nr.client_department_id = reader.GetInt32("client_department_id");
//                    nr.client_id = reader.GetInt32("client_id");

//                    nr.description = reader.SafeGetString("description");
//                    nr.instructions = reader.SafeGetString("instructions");
//                    retlist.Add(nr);
//                }
//            }
//            return retlist;
//        }
//    }
//}