//using Abp.Application.Services.Dto;
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
//using System.Threading;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace inzibackend.Surpath.Importing
//{
//    public class ClientCreatorImporter
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

//        public ClientCreatorImporter(SurpathliveSeedHelper surpathliveSeedHelper, IUnitOfWorkManager unitOfWorkManager)
//        {
//            //_context = context;
//            _surpathliveSeedHelper = surpathliveSeedHelper;
//            paramHelper = new ParamHelper();

//            _unitOfWorkManager = unitOfWorkManager;
//            //IocManager.Instance = _ IocManager.Instance;
//        }


//        public async Task<ImportFromSurscanLiveJobArgs> CreateClient(ImportFromSurscanLiveJobArgs _args)
//        {
//            args = _args;

//            clientRecordCategoryClasses = GetClientRecordCategories();
//            clientAdmins = GetTenantAdminUsers();
            

//            paramHelper.reset();
//            paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//            var sql = "select * from clients where is_client_active = 1 and client_id = @client_id;";
//            if (_surpathliveSeedHelper.conn.State == ConnectionState.Closed) _surpathliveSeedHelper.conn.Open();
//            MySqlCommand _cmd = new MySqlCommand(sql, _surpathliveSeedHelper.conn);
//            _cmd.Parameters.Clear();
//            foreach (MySqlParameter parameter in paramHelper.ParmList)
//            {
//                _cmd.Parameters.Add(parameter);
//            }
//            using (MySqlDataReader reader = _cmd.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    args.ClientCode = reader.GetString("client_code");
//                    args.NewTenantName = reader.GetString("client_name");
//                    args.NewTenancyName = Regex.Replace(args.ClientCode, "[^a-zA-Z0-9._]", string.Empty).ToLower();


//                }
//            }

//            using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//            {
                
//                using (var mainUow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                {
//                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
//                    using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                    {
//                        _session = _sessionObj.Object;
//                        _session.Use(args.TenantId, args.UserId);


//                        var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
//                        var input = new CreateTenantInput
//                        {
//                            TenancyName = args.NewTenancyName,
//                            Name = args.NewTenantName,
//                            AdminEmailAddress = "chris@borps.com",
//                            ClientCode = args.ClientCode,
//                            AdminPassword = "123qwe",
//                            IsActive = true,
//                            EditionId = defaultEdition.Id
//                        };
//                        args.NewAdminEmail = input.AdminEmailAddress;
//                        using (var _tenantAppService = IocManager.Instance.ResolveAsDisposable<ITenantAppService>())
//                        {
//                            await _tenantAppService.Object.CreateTenant(input);

//                        }
//                        args.Created = true;


//                        await uowManager.Object.Current.SaveChangesAsync();
//                    }
//                }
//            }

//            return args;
//        }

//        public async Task<ImportFromSurscanLiveJobArgs> GetAdminId(ImportFromSurscanLiveJobArgs _args)
//        {
//            args = _args;

//            using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//            {
//                using (var permUow = uowManager.Object.Begin())
//                {
//                    using (var um = IocManager.Instance.ResolveAsDisposable<UserManager>())
//                    {
//                        using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                        {
//                            _session = _sessionObj.Object;
//                            _session.Use(args.TenantId, args.UserId);
                            
//                            //var o = um.Object.FindAsync(args.NewTenantId, )

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


//                clientRecordCategoryClasses = GetClientRecordCategories();
//                clientAdmins = GetTenantAdminUsers();

//                paramHelper.reset();
//                paramHelper.Param = new MySqlParameter("@client_id", args.client_id);
//                var sql = "select * from clients where is_client_active = 1 and client_id = @client_id;";
//                if (_surpathliveSeedHelper.conn.State == ConnectionState.Closed) _surpathliveSeedHelper.conn.Open();
//                MySqlCommand _cmd = new MySqlCommand(sql, _surpathliveSeedHelper.conn);
//                _cmd.Parameters.Clear();
//                foreach (MySqlParameter parameter in paramHelper.ParmList)
//                {
//                    _cmd.Parameters.Add(parameter);
//                }
//                using (MySqlDataReader reader = _cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        args.ClientCode = reader.GetString("client_code");
//                        args.NewTenantName = reader.GetString("client_name");
//                        args.NewTenancyName = Regex.Replace(args.ClientCode, "[^a-zA-Z0-9._]", string.Empty).ToLower();


//                    }
//                }


//                using (var uowManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
//                {
//                    using (var mainUow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
//                    {
//                        _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
//                        using (var _sessionObj = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
//                        {
//                            _session = _sessionObj.Object;
//                            _session.Use(args.TenantId, args.UserId);

//                            var _tenentexists = await _context.Tenants.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(t => t.IsDeleted == false && t.ClientCode == args.ClientCode && t.TenancyName == args.NewTenancyName);

//                            if (_tenentexists == null)
//                            {
//                                var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
//                                var input = new CreateTenantInput
//                                {
//                                    TenancyName = args.NewTenancyName,
//                                    Name = args.NewTenantName,
//                                    AdminEmailAddress = "chris@borps.com",
//                                    ClientCode = args.ClientCode,
//                                    AdminPassword = "123qwe",
//                                    IsActive = true,
//                                    EditionId = defaultEdition.Id
//                                };
//                                args.NewAdminEmail = input.AdminEmailAddress;
//                                using (var _tenantAppService = IocManager.Instance.ResolveAsDisposable<ITenantAppService>())
//                                {
//                                    await _tenantAppService.Object.CreateTenant(input);

//                                }
//                                args.Created = true;
//                            }
//                            else
//                            {
//                                // client exists, get the id
//                                args.NewTenantId = _tenentexists.Id;
//                                var user = _context.Users.IgnoreQueryFilters().First(u => u.TenantId == args.NewTenantId && u.Name.ToLower().Equals("admin".ToLower()));
//                                args.NewTeantAdminId = user.Id;
//                                args.Created = true;
//                            }
//                        }

//                        await mainUow.CompleteAsync();
//                    }
//                }



//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//            return args;
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