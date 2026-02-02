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
    public class SPLImporter
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

        public SPLImporter(inzibackendDbContext context, SurpathliveSeedHelper surpathliveSeedHelper, ImportFromSurscanLiveJobArgs _args)
        {
            _surpathliveSeedHelper = surpathliveSeedHelper;
            _context = context;
            paramHelper = new ParamHelper();
            args = _args;
            _passwordHasher = new PasswordHasher<User>();
            string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory(), _environment, true);
            //using (var appConfigurationAccessor = IocManager.Instance.ResolveAsDisposable<IAppConfigurationAccessor>())
            //{
            //    _appConfiguration = appConfigurationAccessor.Object.Configuration;
            SurpathRecordsRootFolder = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
            //}
            using (var binaryObjectManager = IocManager.Instance.ResolveAsDisposable<IBinaryObjectManager>())
            {
                _binaryObjectManager = binaryObjectManager.Object;
            }
            Log = SeedHelper.Log;
        }


        public async Task<ImportFromSurscanLiveJobArgs> ImportClient(ImportFromSurscanLiveJobArgs _args)
        {
            args = _args;


            clientRecordCategoryClasses = GetClientRecordCategories();
            clientAdmins = GetTenantAdminUsers();


            paramHelper.reset();
            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
            var sql = "select * from clients where is_client_active = 1 and client_id = @client_id;";
            if (_surpathliveSeedHelper.conn.State == ConnectionState.Closed) _surpathliveSeedHelper.conn.Open();
            MySqlCommand _cmd = new MySqlCommand(sql, _surpathliveSeedHelper.conn);
            _cmd.Parameters.Clear();
            foreach (MySqlParameter parameter in paramHelper.ParmList)
            {
                _cmd.Parameters.Add(parameter);
            }
            using (MySqlDataReader reader = _cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    args.ClientCode = reader.GetString("client_code");
                    args.NewTenantName = reader.GetString("client_name");
                    args.NewTenancyName = Regex.Replace(args.ClientCode, "[^a-zA-Z0-9._]", string.Empty).ToLower();



                }
            }




            //using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            //{
            //    using (var permUow = uowManager.Object.Begin())
            //    {
            //        _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
            //        if (_context.Tenants.AsNoTracking().IgnoreQueryFilters().Any(t => t.TenancyName.Equals(args.NewTenancyName)))
            //            return args;


            //        using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            //        {
            //            _session = _sessionObj.Object;
            //            _session.Use(args.TenantId, args.UserId);
            var _client = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == args.NewTenancyName);


            if (_client == null)
            {
                Log.Write($"Creating {args.NewTenancyName}");

                _client = new Tenant(args.NewTenancyName, args.NewTenantName);

                var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    _client.EditionId = defaultEdition.Id;
                    _client.ClientCode = args.ClientCode;
                }

                _context.Tenants.Add(_client);
                _context.SaveChanges();

                args.NewTenantId = _client.Id;
            }
            else
            {
                Log.Write($"Tenant {args.NewTenancyName} exists");
                args.NewTenantId = _client.Id;
            }
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == args.NewTenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(args.NewTenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            //User role

            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == args.NewTenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(args.NewTenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }


            //CreateClientDepartments();
            CreateMissingClientDepartments();
            //CreateTenantAdminUser(adminRole, "David", "Leonard", "", "david@surscan.com", "surscan");
            //CreateTenantAdminUser(adminRole, "Chris", "Norman", "", "chris@surscan.com");

            CreateTenantAdminUser(adminRole);
            if (args.client_id == 117)
            {
                //CreateTenantAdminUser(Role adminRole, string fname, string lname, string phone, string email)
                CreateTenantAdminUser(adminRole, "Emilie", "Allen", "", "emilieallen@dcccd.edu");
                CreateTenantAdminUser(adminRole, "Christi", "Carter", "", "CCarter@dcccd.edu");
            }

            if (!_context.Cohorts.IgnoreQueryFilters().Any(c => c.DefaultCohort == true && c.TenantId == args.NewTenantId))
            {
                // Create a default cohort
                var _cohort = new Cohort()
                {
                    Name = "Unassigned Cohort",
                    Description = "This is the default cohort new users will be assigned to when they join the system",
                    TenantId = args.NewTenantId,
                    DefaultCohort = true
                };
                _context.Cohorts.Add(_cohort);
                _context.SaveChanges();


                var _cohorts = new List<string> { "2022 Fall", "2023 Sprint", "2023 Fall", "2024 Sprint" };

                _cohorts.ForEach(s =>
                {
                    _context.Cohorts.Add(new Cohort()
                    {
                        Name = s,
                        Description = $"Graduating in {s}",
                        TenantId = args.NewTenantId,
                        DefaultCohort = false
                    }
                    );
                    _context.SaveChanges();

                });

            }



            if (!_context.RecordStatuses.IgnoreQueryFilters().Any(r => r.TenantId == args.NewTenantId && r.IsDefault == true))
            {
                // Create a default record Status
                var _defRecStat = new RecordStatus()
                {
                    HtmlColor = "#bd9a93",
                    IsDefault = true,
                    StatusName = "Unknown (Default)",
                    TenantId = args.NewTenantId,
                    TenantDepartmentId = null,
                    IsSurpathServiceStatus = false,
                };
                _context.RecordStatuses.Add(_defRecStat);
                _context.SaveChanges();

                var _statusesDefauls = new List<RecStatDefault>()
                    {
                        new RecStatDefault(){StatusName = "Needs Review", HtmlColor ="#bfb77e"},
                        new RecStatDefault(){StatusName = "For Approval", HtmlColor ="#93c967"},
                        new RecStatDefault(){StatusName = "Accepted", HtmlColor ="#67c777"},
                        new RecStatDefault(){StatusName = "Not Accepted", HtmlColor ="#ba323b"},
                        new RecStatDefault(){StatusName = "Need more info", HtmlColor ="#7bced4"},
                        new RecStatDefault(){StatusName = "Under Review", HtmlColor ="#4b71c9"},
                        new RecStatDefault(){StatusName = "Needs Signature", HtmlColor ="#7856dd"},
                    };

                _statusesDefauls.ForEach(s =>
                {
                    _context.RecordStatuses.Add(new RecordStatus()
                    {
                        HtmlColor = s.HtmlColor,
                        IsDefault = false,
                        StatusName = s.StatusName,
                        TenantId = args.NewTenantId,
                        TenantDepartmentId = null,
                        IsSurpathServiceStatus=false,
                    });
                    _context.SaveChanges();
                });
            }

            if (!_context.PidTypes.IgnoreQueryFilters().Any(r => r.TenantId == args.NewTenantId))
            {
                // create default pids from host
                var _HostPidTypes = new List<PidType>();
                if (!_context.PidTypes.IgnoreQueryFilters().Any(r => r.TenantId == null))
                {
                    // host doesn't have defaults - create them.
                    _HostPidTypes = new List<PidType>()
                    {
                        new PidType(){Name="SSN",Description="Social Security Number",MaskPid=true, CreatedBy = 1, CreatedOn = DateTime.UtcNow, IsActive = true, PidRegex = "", TenantId = null},
                        new PidType(){Name="DL",Description="Driver's License",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.UtcNow, IsActive = true, PidRegex = "", TenantId = null},
                        new PidType(){Name="Passport",Description="Passport Number",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.UtcNow, IsActive = true, PidRegex = "", TenantId = null},
                        new PidType(){Name="StudentID",Description="Student Id",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.UtcNow, IsActive = true, PidRegex = "", TenantId = null},
                        //new PidType(){Name="StudentID",Description="Student Id",MaskPid=false, CreatedBy = 1, CreatedOn = DateTime.UtcNow, IsActive = true, PidRegex = "", TenantId = null},

                    };
                    _HostPidTypes.ForEach(p =>
                    {
                        _context.PidTypes.Add(p);
                    });
                    _context.SaveChanges();
                }
                _HostPidTypes.ForEach(p =>
                {
                    _context.PidTypes.Add(new PidType()
                    {
                        Name = p.Name,
                        Description = p.Description,
                        MaskPid = p.MaskPid,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        IsActive = p.IsActive,
                        PidRegex = p.PidRegex,
                        TenantId = args.NewTenantId
                    });
                });
                _context.SaveChanges();

            }


            /// Client in 
            /// User Start


            clientUsers = GetDonors();

            var _totalcClientUsers = clientUsers.Count();
            //var ulist = _context.Users.IgnoreQueryFilters().AsNoTracking().ToList();
            //User role
            var _LabCodeId = _context.CodeTypes.AsNoTracking().IgnoreQueryFilters().Where(c => c.Name.ToUpper().Equals("LAB")).FirstOrDefault().Id;
            var _userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == args.NewTenantId && r.Name == StaticRoleNames.Tenants.User);

            Log.Write($"{clientUsers.Count} users to create");
            using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
            {
                var userManagerObject = userManager.Object;
                var _user_number = 0;

                // for adding into cohors
                var _cohortlist = _context.Cohorts.AsNoTracking().IgnoreQueryFilters().ToList();

                foreach (var _user in clientUsers)
                {
                    _user_number++;

                    var _tenant = _context.Tenants.IgnoreQueryFilters().AsNoTracking().Where(t => t.ClientCode.ToUpper().Equals(_user.client_code)).FirstOrDefault();
                    if (_tenant != null)
                    {
                        var _code = _context.DeptCodes.IgnoreQueryFilters().AsNoTracking().Where(t => t.TenantId == _tenant.Id && t.CodeTypeId == _LabCodeId).FirstOrDefault();
                        if (_code != null)
                        {
                            //var _existingUser = _context.UserAccounts.FirstOrDefault(u=>u.TenantId == _tenant.Id && u.)
                            //var _existUser = userManagerObject.GetUser()
                            //var _tenantId = _code.TenantId;
                            var _deptid = _code.TenantDepartmentId;
                            var _e = _context.Users.IgnoreQueryFilters().AsNoTracking().Where(u => u.TenantId == _tenant.Id && u.EmailAddress.ToLower().Equals(_user.user_email.ToLower())).FirstOrDefault();
                            if (_e != null)
                            {
                                Log.Write($"User {_e.EmailAddress} ({_e.FullName})  for client {_code.Code} alread exists");
                                _user.Id = _e.Id;
                            }
                            if (_e == null)
                            {



                                //var _existingUser = _context.Users.AsNoTracking().Where(u => u.EmailAddress.ToUpper().Equals(_user.user_email.ToUpper())).FirstOrDefault();
                                //if (_existingUser != null)
                                //{
                                //    Log.Write($"User: {_user.user_email} for client {_code.Code} exists");
                                //    // await GetUserDocuments(_user, _tenantId, _deptid);
                                //    //continue;
                                //}
                                //else
                                //{
                                Log.Write($"Creating user {clientUsers.IndexOf(_user)}/{_totalcClientUsers}: {_user.user_email} for client {_code.Code}");
                                var user = new User
                                {
                                    TenantId = _tenant.Id,
                                    UserName = _user.user_email,
                                    Name = _user.user_first_name,
                                    Surname = _user.user_last_name,
                                    EmailAddress = _user.user_email,
                                    IsEmailConfirmed = true,
                                    ShouldChangePasswordOnNextLogin = true,
                                    IsActive = true
                                };

                                var randomPassword = userManagerObject.CreateRandomPassword().GetAwaiter().GetResult();
                                user.Password = _passwordHasher.HashPassword(user, randomPassword);

                                user.SetNormalizedNames();

                                //var _thisuser = _context.Users.Add(user).Entity;
                                //user.Id = _thisuser.Id;
                                _context.Users.Add(user);
                                _context.SaveChanges();

                                _context.UserRoles.Add(new UserRole(args.NewTenantId, user.Id, _userRole.Id));
                                _context.SaveChanges();

                                _context.UserAccounts.Add(new UserAccount
                                {
                                    TenantId = _tenant.Id,
                                    UserId = user.Id,
                                    UserName = user.UserName,
                                    EmailAddress = user.EmailAddress
                                });

                                _context.SaveChanges();
                                _user.Id = user.Id;


                                if (_user_number >= _cohortlist.Count) _user_number = 0;
                                // Add to default cohort
                                var _cohortUser = new CohortUser()
                                {
                                    CohortId = _cohortlist[_user_number].Id,
                                    TenantId = _tenant.Id,
                                    UserId = user.Id
                                };
                                _context.CohortUsers.Add(_cohortUser);
                                //}

                            };
                            await GetUserDocuments(_user, _tenant.Id, _deptid);


                        }
                    }
                }

            }



            /// User End


            //}





            //        };
            //    };
            //}
            return args;
        }

        public async Task<bool> GetUserDocuments(ClientUserOrAdmin _user, int? _tenantId, Guid? _tenantDeptId)
        {

            paramHelper.reset();
            paramHelper.Param = new MySqlParameter("@donor_id", _user.donor_id);

            var sql = @"
            select * from donor_documents
            where donor_id = @donor_id;
            ";

            Log.Write($"Getting documents for {_user.Id}");

            SurpathliveSeedHelper OldDataHelper = new(_surpathliveSeedHelper.ConnectionString);
            if (OldDataHelper.conn.State == ConnectionState.Closed) OldDataHelper.conn.Open();
            MySqlCommand _cmd = new MySqlCommand(sql, OldDataHelper.conn);
            _cmd.Parameters.Clear();
            foreach (MySqlParameter parameter in paramHelper.ParmList)
            {
                _cmd.Parameters.Add(parameter);
            }
            var _deptCodes = _context.DeptCodes.AsNoTracking().ToList();
            var _deptss = _context.TenantDepartments.AsNoTracking().ToList();
            var recCats = _context.RecordCategories.AsNoTracking().IgnoreQueryFilters().ToList();

            var _defStat = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.IsDefault == true && s.IsSurpathServiceStatus == false);
            var _needsapprovalstate = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.StatusName.Equals("Needs Review")) ?? _defStat;
            var _acceptedstate = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.StatusName.Equals("Accepted")) ?? _defStat;
            var _rejectedState = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.StatusName.Equals("Not Accepted")) ?? _defStat;

            using (var reader = _cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    var _titleCat = reader.SafeGetString("document_title");
                    var _filename = reader.SafeGetString("file_name");

                    // get category
                    var recCat = _context.RecordCategories.AsNoTracking().IgnoreQueryFilters().Where(rc => rc.Name.ToLower().Trim().Equals(_titleCat.ToLower().Trim()) && rc.TenantId == _tenantId).FirstOrDefault(); // && rc.TenantDepartmentId == _tenantDeptId);


                    if (recCat == null)
                    {
                        Log.Write($"Creating Record Category: {_titleCat} ");
                        // create this record category
                        recCat = new RecordCategory()
                        {
                            //TenantDepartmentId = _tenantDeptId,
                            TenantId = _tenantId,
                            Name = _titleCat,
                            Instructions = "! This category was generated from an import"

                        };

                        _context.RecordCategories.Add(recCat);
                        _context.SaveChanges();
                    }
                    if (recCat != null)
                    {

                        var _recordExisting = (from r in _context.Records
                                               join rs in _context.RecordStates on r.Id equals rs.RecordId
                                               where rs.UserId == _user.Id
                                               && r.TenantId == args.NewTenantId
                                               && rs.TenantId == args.NewTenantId
                                               && r.filename == _filename
                                               select rs).IgnoreQueryFilters().FirstOrDefault();
                        //var _rExisting = _context.RecordStates.IgnoreQueryFilters()


                        //if (!_context.BinaryObjects.IgnoreQueryFilters().Any(b => b.TenantId == args.NewTenantId && b.FileName.ToLower().Equals(_filename.ToLower())))
                        if (_recordExisting == null)
                        {
                            //                        //var _bytes = (byte[])reader.GetBytes("document_content"));
                            var _bytes = (byte[])reader["document_content"];
                            //                        // add the record
                            var _folder = GetDestFolder(_tenantId, _user.Id);
                            //                        // var bino = new BinaryObject(_tenantId, _bytes, _titleCat, true, _folder);
                            var storedFile = new BinaryObject(_tenantId, _bytes, _filename, true, _folder, _filename);
                            _binaryObjectManager.SaveAsync(storedFile).GetAwaiter().GetResult();

                            var rec = new Record()
                            {
                                filedata = storedFile.Id,
                                filename = _filename,
                                physicalfilepath = storedFile.FileName,
                                TenantId = _tenantId,
                                BinaryObjId = storedFile.Id
                            };
                            _context.Records.Add(rec);
                            _context.SaveChanges();

                            //var _statuses = new List<string> { "Needs Review", "For Approval", "Accepted", "Not Accepted", "Need more info", "Under Review", "Needs Signature" };

                            var isrej = reader["is_Rejected"] == DBNull.Value ? false : reader.GetBoolean("is_Rejected");
                            var isapp = reader["is_Approved"] == DBNull.Value ? false : reader.GetBoolean("is_Approved");
                            var _estate = EnumRecordState.NeedsApproval;
                            if (isrej) _estate = EnumRecordState.Rejected;
                            if (isapp) _estate = EnumRecordState.Approved;

                            //var _status = _context.RecordStatuses.FirstOrDefaultAsync()


                            var _state = _needsapprovalstate;
                            if (isrej) _state = _rejectedState;
                            if (isapp) _state = _acceptedstate;
                            try
                            {
                                // this is dying because it doesn't use the enum - it uses the RecordStatus
                                var recstate = new RecordState()
                                {
                                    Id = SequentialGuidGenerator.Instance.Create(),
                                    RecordCategoryId = recCat.Id,
                                    Notes = String.Empty,
                                    TenantId = _tenantId,
                                    UserId = _user.Id,
                                    RecordId = rec.Id,
                                    RecordStatusId = _state.Id,
                                    State = _estate
                                };
                                _context.RecordStates.Add(recstate);
                                _context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                        else
                        {
                            Log.Write($"File: {_filename} {_titleCat} already exists for this user");
                        }
                    }
                }
            }
            return true;
        }

        //public async Task<bool> GetUserDocumentJob(ImportFromSurscanLiveJobArgs _args, ClientUserOrAdmin _user, int? _tenantId, Guid? _tenantDeptId)
        //{

        //    paramHelper.reset();
        //    paramHelper.Param = new MySqlParameter("@donor_id", _user.donor_id);

        //    var sql = @"
        //    select * from donor_documents
        //    where donor_id = @donor_id;
        //    ";


        //    SurpathliveSeedHelper OldDataHelper = new(_surpathliveSeedHelper.ConnectionString);
        //    if (OldDataHelper.conn.State == ConnectionState.Closed) OldDataHelper.conn.Open();
        //    MySqlCommand _cmd = new MySqlCommand(sql, OldDataHelper.conn);
        //    _cmd.Parameters.Clear();
        //    foreach (MySqlParameter parameter in paramHelper.ParmList)
        //    {
        //        _cmd.Parameters.Add(parameter);
        //    }
        //    var _deptCodes = _context.DeptCodes.AsNoTracking().ToList();
        //    var _deptss = _context.TenantDepartments.AsNoTracking().ToList();
        //    var recCats = _context.RecordCategories.AsNoTracking().IgnoreQueryFilters().ToList();

        //    var _defStat = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.Default == true);
        //    var _needsapprovalstate = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.StatusName.Equals("Needs Review")) ?? _defStat;
        //    var _acceptedstate = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.StatusName.Equals("Accepted")) ?? _defStat;
        //    var _rejectedState = _context.RecordStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefault(s => s.StatusName.Equals("Not Accepted")) ?? _defStat;

        //    using (var reader = _cmd.ExecuteReader())
        //    {

        //        while (reader.Read())
        //        {
        //            var _titleCat = reader.SafeGetString("document_title");
        //            var _filename = reader.SafeGetString("file_name");
        //            // get category
        //            var recCat = _context.RecordCategories.AsNoTracking().IgnoreQueryFilters().Where(rc => rc.Name.ToLower().Trim().Equals(_titleCat.ToLower().Trim()) && rc.TenantId == _tenantId).FirstOrDefault(); // && rc.TenantDepartmentId == _tenantDeptId);


        //            //if (recCat == null)
        //            //{
        //            //    Log.Write($"Creating Record Category: {_titleCat} ");
        //            //    // create this record category
        //            //    recCat = new RecordCategory()
        //            //    {
        //            //        //TenantDepartmentId = _tenantDeptId,
        //            //        TenantId = _tenantId,
        //            //        Name = _titleCat,
        //            //        Instructions = "! This category was generated from an import"

        //            //    };

        //            //    _context.RecordCategories.Add(recCat);
        //            //    _context.SaveChanges();
        //            //}
        //            if (recCat != null)
        //            {
        //                //                        //var _bytes = (byte[])reader.GetBytes("document_content"));
        //                var _bytes = (byte[])reader["document_content"];
        //                //                        // add the record
        //                var _folder = GetDestFolder(_tenantId, _user.Id);
        //                //                        // var bino = new BinaryObject(_tenantId, _bytes, _titleCat, true, _folder);

        //                var recId = Guid.Empty;
        //                //using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
        //                //{
        //                //    using (var permUow = uowManager.Object.Begin())
        //                //    {
        //                //        _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
        //                if (_context.Tenants.AsNoTracking().IgnoreQueryFilters().Any(t => t.TenancyName.Equals(_args.NewTenancyName)))
        //                    return false;


        //                //using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
        //                //{
        //                //    _session = _sessionObj.Object;
        //                //    _session.Use(args.TenantId, args.UserId);
        //                var storedFile = new BinaryObject(_tenantId, _bytes, _filename, true, _folder);
        //                _binaryObjectManager.SaveAsync(storedFile).GetAwaiter().GetResult();
        //                var rec = new Record()
        //                {
        //                    filedata = storedFile.Id,
        //                    filename = _filename,
        //                    physicalfilepath = storedFile.FileName,
        //                    TenantId = _tenantId
        //                };
        //                _context.Records.Add(rec);
        //                _context.SaveChanges();
        //                recId = rec.Id;
        //                //        }
        //                //    //    await permUow.CompleteAsync();
        //                //    //}
        //                //}



        //                if (recId != Guid.Empty)
        //                {
        //                    //var _statuses = new List<string> { "Needs Review", "For Approval", "Accepted", "Not Accepted", "Need more info", "Under Review", "Needs Signature" };

        //                    var isrej = reader["is_Rejected"] == DBNull.Value ? false : reader.GetBoolean("is_Rejected");
        //                    var isapp = reader["is_Approved"] == DBNull.Value ? false : reader.GetBoolean("is_Approved");
        //                    var _estate = EnumRecordState.NeedsApproval;
        //                    if (isrej) _estate = EnumRecordState.Rejected;
        //                    if (isapp) _estate = EnumRecordState.Approved;

        //                    //var _status = _context.RecordStatuses.FirstOrDefaultAsync()


        //                    var _state = _needsapprovalstate;
        //                    if (isrej) _state = _rejectedState;
        //                    if (isapp) _state = _acceptedstate;
        //                    try
        //                    {
        //                        // this is dying because it doesn't use the enum - it uses the RecordStatus
        //                        var recstate = new RecordState()
        //                        {
        //                            Id = SequentialGuidGenerator.Instance.Create(),
        //                            Notes = String.Empty,
        //                            TenantId = _tenantId,
        //                            UserId = _user.Id,
        //                            RecordId = recId,
        //                            RecordStatusId = _state.Id,
        //                            State = _estate
        //                        };
        //                        _context.RecordStates.Add(recstate);
        //                        _context.SaveChanges();
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        throw;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    return true;
        //}


        public async Task<bool> GetDoc(int donor_document_id)
        {
            return true;
        }

        private void CreateClientDepartments()
        {
            // Create Departments for tenant
            var _depts = _context.TenantDepartments.IgnoreQueryFilters().FirstOrDefault(d => d.TenantId == args.NewTenantId);
            if (_depts == null)
            {
                _deptCodes = _context.CodeTypes.AsNoTracking().IgnoreQueryFilters();

                paramHelper.reset();

                paramHelper.Param = new MySqlParameter("@client_code", args.ClientCode);
                SurpathliveSeedHelper surpathliveSeedHelperDepts = new(_surpathliveSeedHelper.ConnectionString);
                var sqlDepts = @"select 
cd.*,
c.client_code
from 
client_departments cd
left outer join clients c on c.client_id = cd.client_id
where c.client_code = @client_code;";
                if (surpathliveSeedHelperDepts.conn.State == ConnectionState.Closed) surpathliveSeedHelperDepts.conn.Open();
                MySqlCommand cmdDepts = new MySqlCommand(sqlDepts, surpathliveSeedHelperDepts.conn);
                foreach (MySqlParameter parameter in paramHelper.ParmList)
                {
                    cmdDepts.Parameters.Add(parameter);
                }





                using (MySqlDataReader reader = cmdDepts.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Log.Write($"Creating department for {args.NewTenantName} -> {reader.GetString("department_name")}");

                        var _oldDeptId = reader.GetInt32("client_department_id");
                        var _newDept = new TenantDepartment()
                        {
                            Name = reader.GetString("department_name"),
                            TenantId = args.NewTenantId,
                            MROType = (EnumClientMROTypes)reader.GetInt32("mro_type_id")
                        };
                        _depts = _context.TenantDepartments.Add(_newDept).Entity;
                        _context.SaveChanges();




                        // create this depts codes
                        if (reader["lab_code"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(reader.GetString("lab_code")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("lab_code"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Lab").FirstOrDefault().Id,
                                    TenantDepartmentId = _depts.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }
                        }

                        if (reader["ClearStarCode"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(reader.GetString("ClearStarCode")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("ClearStarCode"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Clear Star").FirstOrDefault().Id,
                                    TenantDepartmentId = _depts.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }
                        }

                        if (reader["QuestCode"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(reader.GetString("QuestCode")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("QuestCode"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Quest").FirstOrDefault().Id,
                                    TenantDepartmentId = _depts.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }
                        }

                        if (reader["FormFoxCode"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(reader.GetString("FormFoxCode")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("FormFoxCode"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "FormFox").FirstOrDefault().Id,
                                    TenantDepartmentId = _depts.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }
                        }

                        // Add a dept default record cat rule
                        var rcr = new RecordCategoryRule()
                        {
                            TenantId = args.NewTenantId,
                            Name = $"{_newDept.Name} Example Record Rule",
                            Description = "An example record rule",
                            Notify = false,
                            ExpireInDays = 90,
                            WarnDaysBeforeFirst = 45,
                            WarnDaysBeforeSecond = 25,
                            WarnDaysBeforeFinal = 10,
                            Expires = true,
                            Required = true

                        };
                        _context.RecordCategoryRules.Add(rcr);
                        _context.SaveChanges();
                        var rr = new RecordRequirement()
                        {
                            TenantId = args.NewTenantId,
                            Name = $"{_newDept.Name} Example Record Requirement",
                            Description = "An example record requirement",
                            Metadata = "{}"
                        };
                        _context.RecordRequirements.Add(rr);
                        _context.SaveChanges();
                        // Add the depts record category

                        clientRecordCategoryClasses.Where(rc => rc.client_department_id == _oldDeptId).ToList().ForEach(rc =>
                        {
                            var _rc = new RecordCategory();
                            _rc.TenantId = args.NewTenantId;
                            _rc.Name = rc.description.Trim();
                            _rc.Instructions = rc.instructions;
                            _rc.RecordRequirementId = rr.Id;
                            _rc.RecordCategoryRuleId = rcr.Id;
                            _context.RecordCategories.Add(_rc);

                        });
                        _context.SaveChanges();
                    }
                }
                // _context.SaveChanges();
                surpathliveSeedHelperDepts.conn.Close();
                surpathliveSeedHelperDepts = null;
            }

        }

        private void CreateMissingClientDepartments()
        {


            _deptCodes = _context.CodeTypes.AsNoTracking().IgnoreQueryFilters();

            //var _depts = _context.TenantDepartments.IgnoreQueryFilters().FirstOrDefault(d => d.TenantId == args.NewTenantId);

            paramHelper.reset();

            paramHelper.Param = new MySqlParameter("@client_code", args.ClientCode);
            SurpathliveSeedHelper surpathliveSeedHelperDepts = new(_surpathliveSeedHelper.ConnectionString);
            var sqlDepts = @"select 
cd.*,
c.client_code
from 
client_departments cd
left outer join clients c on c.client_id = cd.client_id
where c.client_code = @client_code;";
            if (surpathliveSeedHelperDepts.conn.State == ConnectionState.Closed) surpathliveSeedHelperDepts.conn.Open();
            MySqlCommand cmdDepts = new MySqlCommand(sqlDepts, surpathliveSeedHelperDepts.conn);
            foreach (MySqlParameter parameter in paramHelper.ParmList)
            {
                cmdDepts.Parameters.Add(parameter);
            }




            using (MySqlDataReader reader = cmdDepts.ExecuteReader())
            {
                while (reader.Read())
                {
                    var _DeptName = reader.GetString("department_name");
                    var _TenantDepartment = _context.TenantDepartments.IgnoreQueryFilters().FirstOrDefault(t => t.TenantId == args.NewTenantId && t.Name.ToLower().Equals(_DeptName.ToLower()));
                    //var _TenantDepartmentExists = _context.TenantDepartments.Any(t => t.TenantId==args.NewTenantId &&  t.Name.ToLower().Equals(_DeptName.ToLower()));
                    var _oldDeptId = reader.GetInt32("client_department_id");
                    //var tlist = _context.TenantDepartments.IgnoreQueryFilters().Where(t=>t.TenantId==args.NewTenantId).ToList();
                    if (_TenantDepartment != null)
                    {
                        Log.Write($"{_DeptName} exists");
                    }
                    else
                    {

                        Log.Write($"Creating department for {args.NewTenantName} -> {reader.GetString("department_name")}");

                        var _newDept = new TenantDepartment()
                        {
                            Name = reader.GetString("department_name"),
                            TenantId = args.NewTenantId,
                            MROType = (EnumClientMROTypes)reader.GetInt32("mro_type_id")
                        };
                        _TenantDepartment = _context.TenantDepartments.Add(_newDept).Entity;
                        _context.SaveChanges();
                    }


                    // create this depts codes
                    if (reader["lab_code"] != DBNull.Value)
                    {
                        if (!string.IsNullOrEmpty(reader.GetString("lab_code")))
                        {
                            if (!_context.DeptCodes.IgnoreQueryFilters().Any(d => d.TenantDepartmentId == _TenantDepartment.Id && d.Code == reader.GetString("lab_code")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("lab_code"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Lab").FirstOrDefault().Id,
                                    TenantDepartmentId = _TenantDepartment.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }

                        }
                    }

                    if (reader["ClearStarCode"] != DBNull.Value)
                    {
                        if (!string.IsNullOrEmpty(reader.GetString("ClearStarCode")))
                        {
                            if (!_context.DeptCodes.IgnoreQueryFilters().Any(d => d.TenantDepartmentId == _TenantDepartment.Id && d.Code == reader.GetString("ClearStarCode")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("ClearStarCode"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Clear Star").FirstOrDefault().Id,
                                    TenantDepartmentId = _TenantDepartment.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }
                        }
                    }

                    if (reader["QuestCode"] != DBNull.Value)
                    {
                        if (!string.IsNullOrEmpty(reader.GetString("QuestCode")))
                        {
                            if (!_context.DeptCodes.IgnoreQueryFilters().Any(d => d.TenantDepartmentId == _TenantDepartment.Id && d.Code == reader.GetString("QuestCode")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("QuestCode"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "Quest").FirstOrDefault().Id,
                                    TenantDepartmentId = _TenantDepartment.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }

                        }
                    }

                    if (reader["FormFoxCode"] != DBNull.Value)
                    {
                        if (!string.IsNullOrEmpty(reader.GetString("FormFoxCode")))
                        {
                            if (!_context.DeptCodes.IgnoreQueryFilters().Any(d => d.TenantDepartmentId == _TenantDepartment.Id && d.Code == reader.GetString("FormFoxCode")))
                            {
                                var newcode = _context.DeptCodes.Add(new DeptCode()
                                {
                                    Code = reader.GetString("FormFoxCode"),
                                    CodeTypeId = _deptCodes.Where(c => c.Name == "FormFox").FirstOrDefault().Id,
                                    TenantDepartmentId = _TenantDepartment.Id,
                                    TenantId = args.NewTenantId

                                }).Entity;
                                _context.SaveChanges();
                            }

                        }
                    }


                    if (!_context.RecordCategoryRules.IgnoreQueryFilters().Any(r => r.TenantId == args.NewTenantId))
                    {
                        // Add a dept default record cat rule
                        var rcr = new RecordCategoryRule()
                        {
                            TenantId = args.NewTenantId,
                            Name = $"{_TenantDepartment.Name} Example Record Rule",
                            Description = "An example record rule",
                            Notify = false,
                            ExpireInDays = 90,
                            WarnDaysBeforeFirst = 45,
                            WarnDaysBeforeSecond = 25,
                            WarnDaysBeforeFinal = 10,
                            Expires = true,
                            Required = true
                        };
                        _context.RecordCategoryRules.Add(rcr);
                        _context.SaveChanges();
                        var rr = new RecordRequirement()
                        {
                            TenantId = args.NewTenantId,
                            Name = $"{_TenantDepartment.Name} Example Record Requirement",
                            Description = "An example record requirement",
                            Metadata = "{}"
                        };
                        _context.RecordRequirements.Add(rr);
                        _context.SaveChanges();
                        // Add the depts record category

                        clientRecordCategoryClasses.Where(rc => rc.client_department_id == _oldDeptId).ToList().ForEach(rc =>
                        {
                            var _rc = new RecordCategory();
                            _rc.TenantId = args.NewTenantId;
                            _rc.Name = rc.description.Trim();
                            _rc.Instructions = rc.instructions;
                            _rc.RecordRequirementId = rr.Id;
                            _rc.RecordCategoryRuleId = rcr.Id;
                            _context.RecordCategories.Add(_rc);

                        });
                        _context.SaveChanges();
                    }
                }
            }


        }


        private void CreateTenantAdminUser(Role adminRole)
        {
            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == args.NewTenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser((int)args.NewTenantId, "chris@surscan.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = false;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(args.NewTenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                _context.UserAccounts.Add(new UserAccount
                {
                    TenantId = args.NewTenantId,
                    UserId = adminUser.Id,
                    UserName = AbpUserBase.AdminUserName,
                    EmailAddress = adminUser.EmailAddress
                });
                _context.SaveChanges();

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), args.NewTenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }
        }
        private void CreateTenantAdminUser(Role adminRole, string fname, string lname, string phone, string email, string password = "123qwe")
        {
            var custadminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == args.NewTenantId && u.UserName.ToLower().Equals(email));
            if (custadminUser == null)
            {
                custadminUser = User.CreateTenantAdminUser((int)args.NewTenantId, email);
                custadminUser.UserName = email;
                custadminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(custadminUser, password);
                custadminUser.IsEmailConfirmed = true;
                custadminUser.ShouldChangePasswordOnNextLogin = false;
                custadminUser.IsActive = true;
                //custadminUser.FullName = $"{fname} {lname}";
                custadminUser.Name = fname;
                custadminUser.Surname = lname;
                custadminUser.EmailAddress = email;
                custadminUser.PhoneNumber = phone;

                _context.Users.Add(custadminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(args.NewTenantId, custadminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                _context.UserAccounts.Add(new UserAccount
                {
                    TenantId = args.NewTenantId,
                    UserId = custadminUser.Id,
                    UserName = email,
                    EmailAddress = custadminUser.EmailAddress
                });
                _context.SaveChanges();

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), args.NewTenantId, custadminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }
        }


        public List<ClientUserOrAdmin> GetDonors()
        {
            var users = new List<ClientUserOrAdmin>();
            paramHelper.reset();
            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
            paramHelper.Param = new MySqlParameter("@max_donors", args.max_donors);
            paramHelper.Param = new MySqlParameter("@days_old", args.days_old);

            var sql = @"
SELECT DISTINCT max(u.user_id) AS user_id,
                d.donor_id,
                u.user_email,
                u.user_first_name,
                u.user_last_name,
                u.user_phone_number,
                c.client_code,
                c.client_id,
                c.client_name,
                cd.client_department_id,
                cd.department_name,
                cd.lab_code
FROM users    u
     LEFT OUTER JOIN donors d ON d.donor_id = u.donor_id
     LEFT OUTER JOIN donor_test_info dti ON dti.donor_id = d.donor_id
     LEFT OUTER JOIN clients c ON c.client_id = dti.client_id
     LEFT OUTER JOIN client_departments cd
        ON dti.client_department_id = cd.client_department_id
	 LEFT OUTER JOIN donor_documents dd on dd.donor_id = d.donor_id 
WHERE 
d.donor_id is not null AND
c.client_id is not null AND
cd.client_department_id is not null AND
u.created_on >= now() - INTERVAL @days_old DAY AND u.is_archived = 0
-- and dd.donor_document_id is not null
and c.client_id = @client_id
GROUP BY u.user_email
limit @max_donors;
";

            SurpathliveSeedHelper OldDataHelper = new(_surpathliveSeedHelper.ConnectionString);
            if (OldDataHelper.conn.State == ConnectionState.Closed) OldDataHelper.conn.Open();
            MySqlCommand _cmd = new MySqlCommand(sql, OldDataHelper.conn);
            _cmd.Parameters.Clear();

            foreach (MySqlParameter parameter in paramHelper.ParmList)
            {
                _cmd.Parameters.Add(parameter);
            }
            var _deptCodes = _context.DeptCodes.AsNoTracking().ToList();
            var _deptss = _context.TenantDepartments.AsNoTracking().ToList();

            using (var reader = _cmd.ExecuteReader())
            {

                while (reader.Read())
                {

                    var _user = new ClientUserOrAdmin();

                    _user.client_code = reader.SafeGetString("client_code");
                    _user.client_department_id = reader.GetInt32("client_department_id");
                    _user.client_id = reader.GetInt32("client_id");
                    _user.user_id = reader.GetInt32("user_id");
                    _user.donor_id = reader.GetInt32("donor_id");
                    _user.user_email = reader.SafeGetString("user_email");
                    _user.user_first_name = reader.SafeGetString("user_first_name");
                    _user.user_last_name = reader.SafeGetString("user_last_name");
                    _user.lab_code = reader.SafeGetString("lab_code");
                    _user.phone_number = reader.SafeGetString("user_phone_number");

                    users.Add(_user);
                }


            }
            return users;
        }

        private List<ClientRecordCategoryClass> GetClientRecordCategories()
        {
            paramHelper.reset();
            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
            var retlist = new List<ClientRecordCategoryClass>();
            var sql = @"
select distinct
c.client_id,
cd.client_department_id,
cd.lab_code,
c.client_code
,ccd.description
,ccd.instructions
from clients c
inner join client_departments cd on cd.client_id = c.client_id
inner join client_dept_doctypes ccd on ccd.client_department_id = cd.client_department_id
where c.client_id = @client_id
order by c.client_id, cd.client_department_id;
        ";

            if (_surpathliveSeedHelper.conn.State == ConnectionState.Closed) _surpathliveSeedHelper.conn.Open();
            MySqlCommand _cmd = new MySqlCommand(sql, _surpathliveSeedHelper.conn);
            _cmd.Parameters.Clear();

            foreach (MySqlParameter parameter in paramHelper.ParmList)
            {
                _cmd.Parameters.Add(parameter);
            }

            using (var reader = _cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var nr = new ClientRecordCategoryClass();
                    nr.client_code = reader.SafeGetString("client_code");
                    nr.lab_code = reader.SafeGetString("lab_code");
                    nr.client_department_id = reader.GetInt32("client_department_id");
                    nr.client_id = reader.GetInt32("client_id");

                    nr.description = reader.SafeGetString("description");
                    nr.instructions = reader.SafeGetString("instructions");
                    retlist.Add(nr);
                }
            }
            return retlist;
        }

        private List<ClientUserOrAdmin> GetTenantAdminUsers()
        {
            try
            {
                var admins = new List<ClientUserOrAdmin>();

                var sql = @"
select distinct
max(u.user_id) as user_id,
u.user_email,
u.user_first_name,
u.user_last_name,
u.user_password,
c.client_id,
c.client_code,
c.client_name,
c.is_client_active,
cd.client_department_id,
cd.is_contact_info_as_client,
cd.lab_code
from users u
left outer join user_departments ud on ud.user_id = u.user_id
left outer join client_departments cd on cd.client_department_id = ud.client_department_id
left outer join clients c on c.client_id = cd.client_id
where
u.is_user_active = 1
and u.user_email not like '%@surscan.com'
and u.user_email not like '%@tx.rr.com'
and c.client_id = @client_id
and cd.lab_code is not null and cd.lab_code != ''
and c.is_client_active = 1
group by u.user_email;
";

                SurpathliveSeedHelper ClientAdminsHelper = new(_surpathliveSeedHelper.ConnectionString);
                if (ClientAdminsHelper.conn.State == ConnectionState.Closed) ClientAdminsHelper.conn.Open();
                MySqlCommand cmdDepts = new MySqlCommand(sql, ClientAdminsHelper.conn);
                cmdDepts.Parameters.Clear();
                paramHelper.reset();
                paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
                foreach (MySqlParameter parameter in paramHelper.ParmList)
                {
                    cmdDepts.Parameters.Add(parameter);
                }

                using (var reader = cmdDepts.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _ClientAdmin = new ClientUserOrAdmin()
                        {
                            client_code = reader.SafeGetString("client_code"),
                            client_department_id = reader.GetInt32("client_department_id"),
                            is_client_active = reader.GetInt32("is_client_active"),
                            is_contact_info_as_client = reader.GetInt32("is_contact_info_as_client"),
                            client_id = reader.GetInt32("client_id"),
                            user_id = reader.GetInt32("user_id"),
                            user_email = reader.SafeGetString("user_email"),
                            user_first_name = reader.SafeGetString("user_first_name"),
                            user_last_name = reader.SafeGetString("user_last_name"),
                            user_password = reader.SafeGetString("user_password"),
                            lab_code = reader.SafeGetString("lab_code")
                        };
                        admins.Add(_ClientAdmin);
                    }
                }
                return admins;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetDestFolder(int? TenantId, long? UserId)
        {
            // var destfolder = $"{appFolders.SurpathRootFolder}";
            var _tenantid = TenantId == null ? "surscan" : TenantId.Value.ToString();
            var destFolder = Path.Combine(SurpathRecordsRootFolder, _tenantid, UserId.ToString());
            destFolder = destFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            return destFolder;
        }

    }

    internal class RecStatDefault
    {
        public string StatusName { get; set; }
        public string HtmlColor { get; set; }
        public bool IsDefault { get; set; } = false;
    }

}