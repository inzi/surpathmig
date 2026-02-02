//using Abp.Dependency;
//using Abp.Domain.Uow;
//using Abp.EntityFrameworkCore.Uow;
//using Abp.MultiTenancy;
//using Abp.Runtime.Session;
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
//using inzibackend.Authorization.Users.Dto;
//using System.IO;
//using inzibackend.Configuration;
//using inzibackend.Storage;
//using Microsoft.Extensions.Configuration;

//namespace inzibackend.Surpath.Importing
//{
//    public class UserImporter
//    {
//        private ParamHelper paramHelper;
//        private inzibackendDbContext _context;
//        private ImportFromSurscanLiveJobArgs args;
//        private SurpathliveSeedHelper _surpathliveSeedHelper { get; set; }
//        private List<ClientRecordCategoryClass> clientRecordCategoryClasses { get; set; }
//        private List<ClientUserOrAdmin> clientAdmins { get; set; }
//        private List<ClientUserOrAdmin> clientUsers { get; set; }


//        private IUnitOfWorkManager _unitOfWorkManager;

//        private IAbpSession _session;
//        private IQueryable<CodeType> _deptCodes;

//        //private readonly IPasswordHasher<User> _passwordHasher;
//        private readonly string SurpathRecordsRootFolder;
//        private readonly IConfigurationRoot _appConfiguration;
//        private readonly IBinaryObjectManager _binaryObjectManager;

//        public UserImporter(SurpathliveSeedHelper surpathliveSeedHelper, IUnitOfWorkManager unitOfWorkManager)
//        {
//            //_context = context;
//            _surpathliveSeedHelper = surpathliveSeedHelper;
//            paramHelper = new ParamHelper();

//            _unitOfWorkManager = unitOfWorkManager;

//            using (var appConfigurationAccessor = IocManager.Instance.ResolveAsDisposable<IAppConfigurationAccessor>())
//            {
//                _appConfiguration = appConfigurationAccessor.Object.Configuration;
//                SurpathRecordsRootFolder = _appConfiguration["Surpath:SurpathRecordsRootFolder"];
//            }
//            using (var binaryObjectManager = IocManager.Instance.ResolveAsDisposable<IBinaryObjectManager>())
//            {
//                _binaryObjectManager = binaryObjectManager.Object;
//            }
//        }

//        public async Task<bool> DoImport(ImportFromSurscanLiveJobArgs _args)
//        {
//            args = _args;

//            try
//            {
//                using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//                {
//                    using (var mainUow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                    {
//                        using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                        {
//                            _session = _sessionObj.Object;
//                            _session.Use(args.NewTenantId, args.NewTeantAdminId);

//                            _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);

//                            clientUsers = GetDonors();
//                            var _LabCodeId = _context.CodeTypes.AsNoTracking().IgnoreQueryFilters().Where(c => c.Name.ToUpper().Equals("LAB")).FirstOrDefault().Id;
//                            using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
//                            {
//                                var userManagerObject = userManager.Object;

//                                foreach (var _user in clientUsers)
//                                {
//                                    var _tenant = _context.Tenants.IgnoreQueryFilters().AsNoTracking().Where(t => t.TenancyName.ToUpper().Equals(args.NewTenancyName)).FirstOrDefault();
//                                    if (_tenant != null)
//                                    {
//                                        var _code = _context.DeptCodes.IgnoreQueryFilters().AsNoTracking().Where(t => t.TenantId == _tenant.Id && t.CodeTypeId == _LabCodeId).FirstOrDefault();
//                                        if (_code != null)
//                                        {
//                                            var _e = _context.Users.IgnoreQueryFilters().AsNoTracking().Where(u => u.TenantId == _tenant.Id && u.EmailAddress.ToLower().Equals(_user.user_email.ToLower())).FirstOrDefault();
//                                            if (_e == null)
//                                            {

//                                                var _tenantId = _code.TenantId;
//                                                var _deptid = _code.TenantDepartmentId;
//                                                var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
//                                                var _assignedRoleNames = new List<string> { userRole.Name };
//                                                var _existingUser = _context.Users.AsNoTracking().Where(u => u.EmailAddress.ToUpper().Equals(_user.user_email.ToUpper())).FirstOrDefault();
//                                                if (_existingUser != null)
//                                                {
//                                                    Debug.WriteLine($"User: {_user.user_email} for client {_code.Code} exists, skipping");

//                                                    continue;
//                                                }
//                                                Debug.WriteLine($"Creating user: {_user.user_email} for client {_code.Code}");
//                                                var newUserId = (long)0;
//                                                using (var _service = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
//                                                {
//                                                    await _service.Object.CreateOrUpdateUser(new CreateOrUpdateUserInput()
//                                                    {
//                                                        AssignedRoleNames = _assignedRoleNames.ToArray(),
//                                                        OrganizationUnits = new List<long>(),
//                                                        SendActivationEmail = false,
//                                                        SetRandomPassword = true,
//                                                        User = new UserEditDto()
//                                                        {
//                                                            Id = null,
//                                                            IsActive = true,
//                                                            IsLockoutEnabled = true,
//                                                            IsTwoFactorEnabled = false,
//                                                            EmailAddress = _user.user_email,
//                                                            Name = _user.user_first_name,
//                                                            Surname = _user.user_last_name,
//                                                            ShouldChangePasswordOnNextLogin = false,
//                                                            Password = "",
//                                                            PhoneNumber = _user.phone_number,
//                                                            UserName = _user.user_email

//                                                        }
//                                                    });

//                                                    var newuser = await _service.Object.GetUsers(new GetUsersInput()
//                                                    {
//                                                        Filter = _user.user_email
//                                                    });

//                                                    if (newuser.Items.Count == 1)
//                                                    {
//                                                        newUserId = newuser.Items[0].Id;
//                                                    }
//                                                };

//                                                GetUserDocuments(_user, _tenantId, _deptid);

//                                                // Add to default cohort
//                                                var _cohort = _context.Cohorts.AsNoTracking().Where(c => c.TenantId == _tenantId && c.DefaultCohort == true).FirstOrDefault();
//                                                if (_cohort != null)
//                                                {
//                                                    var _cohortUser = new CohortUser()
//                                                    {
//                                                        CohortId = _cohort.Id,
//                                                        TenantId = _tenantId,
//                                                        UserId = newUserId
//                                                    };
//                                                    _context.CohortUsers.Add(_cohortUser);
//                                                }
//                                            };
//                                        };
//                                    };
//                                };
//                            };
//                        };


//                        mainUow.Complete();
//                    }
//                };
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }
//            return true;
//        }

//        public async Task<bool> GetUserDocuments(ClientUserOrAdmin _user, int? _tenantId, Guid? _tenantDeptId)
//        {

//            paramHelper.reset();
//            paramHelper.Param = new MySqlParameter("@donor_id", _user.donor_id);

//            var sql = @"
//            select * from donor_documents
//            where donor_id = @donor_id;
//            ";


//            SurpathliveSeedHelper OldDataHelper = new(_surpathliveSeedHelper.ConnectionString);
//            if (OldDataHelper.conn.State == ConnectionState.Closed) OldDataHelper.conn.Open();
//            MySqlCommand _cmd = new MySqlCommand(sql, OldDataHelper.conn);
//            _cmd.Parameters.Clear();
//            foreach (MySqlParameter parameter in paramHelper.ParmList)
//            {
//                _cmd.Parameters.Add(parameter);
//            }
//            var _deptCodes = _context.DeptCodes.AsNoTracking().ToList();
//            var _deptss = _context.TenantDepartments.AsNoTracking().ToList();
//            var recCats = _context.RecordCategories.AsNoTracking().IgnoreQueryFilters().ToList();
//            using (var reader = _cmd.ExecuteReader())
//            {

//                while (reader.Read())
//                {
//                    var _titleCat = reader.SafeGetString("document_title");
//                    var _filename = reader.SafeGetString("file_name");
//                    // get category
//                    var recCat = _context.RecordCategories.AsNoTracking().IgnoreQueryFilters().Where(rc => rc.Name.ToLower().Trim().Equals(_titleCat.ToLower().Trim()) && rc.TenantId == _tenantId).FirstOrDefault(); // && rc.TenantDepartmentId == _tenantDeptId);


//                    if (recCat == null)
//                    {
//                        Debug.WriteLine($"Creating Record Category: {_titleCat} ");
//                        // create this record category
//                        recCat = new RecordCategory()
//                        {
//                            //TenantDepartmentId = _tenantDeptId,
//                            TenantId = _tenantId,
//                            Name = _titleCat,
//                            Instructions = "! This category was generated from an import"

//                        };

//                        _context.RecordCategories.Add(recCat);
//                        _context.SaveChanges();
//                    }
//                    if (recCat != null)
//                    {
//                        //                        //var _bytes = (byte[])reader.GetBytes("document_content"));
//                        var _bytes = (byte[])reader["document_content"];
//                        //                        // add the record
//                        var _folder = GetDestFolder(_tenantId, _user.Id);
//                        //                        // var bino = new BinaryObject(_tenantId, _bytes, _titleCat, true, _folder);
//                        var storedFile = new BinaryObject(_tenantId, _bytes, _filename, true, _folder);
//                        //                        _binaryObjectManager.SaveAsync(storedFile).GetAwaiter().GetResult();

//                        var rec = new Record()
//                        {
//                            filedata = storedFile.Id,
//                            filename = _filename,
//                            physicalfilepath = storedFile.FileName,
//                            TenantId = _tenantId
//                        };
//                        _context.Records.Add(rec);
//                        _context.SaveChanges();


//                        var isrej = reader["is_Rejected"] == DBNull.Value ? false : reader.GetBoolean("is_Rejected");
//                        var isapp = reader["is_Approved"] == DBNull.Value ? false : reader.GetBoolean("is_Approved");
//                        var _state = EnumRecordState.NeedsApproval;
//                        if (isrej) _state = EnumRecordState.Rejected;
//                        if (isapp) _state = EnumRecordState.Approved;

//                        var recstate = new RecordState()
//                        {
//                            Notes = String.Empty,
//                            State = _state,
//                            TenantId = _tenantId,
//                            UserId = _user.Id,
//                            RecordId = rec.Id
//                        };
//                        _context.RecordStates.Add(recstate);
//                        _context.SaveChanges();
//                    }
//                }
//            }
//            return true;
//        }


//        public List<ClientUserOrAdmin> GetDonors()
//        {
//            var users = new List<ClientUserOrAdmin>();
//            paramHelper.reset();
//            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//            paramHelper.Param = new MySqlParameter("@max_donors", args.max_donors);
//            paramHelper.Param = new MySqlParameter("@days_old", args.days_old);

//            var sql = @"
//SELECT DISTINCT max(u.user_id) AS user_id,
//                d.donor_id,
//                u.user_email,
//                u.user_first_name,
//                u.user_last_name,
//                u.user_phone_number,
//                c.client_code,
//                c.client_id,
//                c.client_name,
//                cd.client_department_id,
//                cd.department_name,
//                cd.lab_code
//FROM users    u
//     LEFT OUTER JOIN donors d ON d.donor_id = u.donor_id
//     LEFT OUTER JOIN donor_test_info dti ON dti.donor_id = d.donor_id
//     LEFT OUTER JOIN clients c ON c.client_id = dti.client_id
//     LEFT OUTER JOIN client_departments cd
//        ON dti.client_department_id = cd.client_department_id
//	 LEFT OUTER JOIN donor_documents dd on dd.donor_id = d.donor_id 
//WHERE 
//d.donor_id is not null AND
//c.client_id is not null AND
//cd.client_department_id is not null AND
//u.created_on >= now() - INTERVAL @days_old DAY AND u.is_archived = 0
//-- and dd.donor_document_id is not null
//and c.client_id = @client_id
//GROUP BY u.user_email
//limit @max_donors;
//";

//            SurpathliveSeedHelper OldDataHelper = new(_surpathliveSeedHelper.ConnectionString);
//            if (OldDataHelper.conn.State == ConnectionState.Closed) OldDataHelper.conn.Open();
//            MySqlCommand _cmd = new MySqlCommand(sql, OldDataHelper.conn);
//            _cmd.Parameters.Clear();

//            foreach (MySqlParameter parameter in paramHelper.ParmList)
//            {
//                _cmd.Parameters.Add(parameter);
//            }
//            var _deptCodes = _context.DeptCodes.AsNoTracking().ToList();
//            var _deptss = _context.TenantDepartments.AsNoTracking().ToList();

//            using (var reader = _cmd.ExecuteReader())
//            {

//                while (reader.Read())
//                {

//                    var _user = new ClientUserOrAdmin();

//                    _user.client_code = reader.SafeGetString("client_code");
//                    _user.client_department_id = reader.GetInt32("client_department_id");
//                    _user.client_id = reader.GetInt32("client_id");
//                    _user.user_id = reader.GetInt32("user_id");
//                    _user.donor_id = reader.GetInt32("donor_id");
//                    _user.user_email = reader.SafeGetString("user_email");
//                    _user.user_first_name = reader.SafeGetString("user_first_name");
//                    _user.user_last_name = reader.SafeGetString("user_last_name");
//                    _user.lab_code = reader.SafeGetString("lab_code");
//                    _user.phone_number = reader.SafeGetString("user_phone_number");

//                    users.Add(_user);
//                }


//            }
//            return users;
//        }


//        private string GetDestFolder(int? TenantId, long? UserId)
//        {
//            // var destfolder = $"{appFolders.SurpathRootFolder}";
//            var _tenantid = TenantId == null ? "surscan" : TenantId.Value.ToString();
//            var destFolder = Path.Combine(SurpathRecordsRootFolder, _tenantid, UserId.ToString());
//            destFolder = destFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
//            if (!Directory.Exists(destFolder))
//            {
//                Directory.CreateDirectory(destFolder);
//            }
//            return destFolder;
//        }


//    }
//}
