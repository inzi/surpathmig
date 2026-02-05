using Abp.Dependency;
using inzibackend.EntityFrameworkCore;
using inzibackend.Migrations.Seed.Surpath;
using inzibackend.Surpath;
using inzibackend.SurpathSeedHelper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.Core.Logging;
using Abp.Domain.Uow;
using System.Transactions;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;

namespace inzibackend.Migrations.Seed.Host
{
    public class DrugSeeder
    {
        //2: Getting a logger using property injection
        public ILogger Logger { get; set; }

        private inzibackendDbContext _context;
        //private readonly string SeedDataDrugsFile = "SeedData/drugscatspanels.csv";
        private SurpathliveSeedHelper surpathliveSeedHelper { get; set; }

        private ParamHelper paramHelper;

        public List<IdMatch> idsTestCats = new List<IdMatch>();
        public List<IdMatch> idsTestPanels = new List<IdMatch>();
        public List<IdMatch> idsDrugs = new List<IdMatch>();
        public List<IdMatch> idsUnitMes = new List<IdMatch>();
        private IUnitOfWorkManager _unitOfWorkManager;
        private IIocResolver iocResolver;

        //public DrugSeeder(inzibackendDbContext context, SurpathliveSeedHelper _surpathliveSeedHelper, IUnitOfWorkManager unitOfWorkManager)
        public DrugSeeder(IIocResolver _iocResolver, SurpathliveSeedHelper _surpathliveSeedHelper, IUnitOfWorkManager unitOfWorkManager)
        {
            //_context = context;
            surpathliveSeedHelper = _surpathliveSeedHelper;
            paramHelper = new ParamHelper();

            Logger = NullLogger.Instance;
            _unitOfWorkManager = unitOfWorkManager;
            iocResolver = _iocResolver;
        }

        //[UnitOfWork(IsDisabled = true)]
        public virtual void Create()
        {
            Logger.Debug("Drug Seeder called");
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
                    SeedTestCategories();

                    uow.Complete();
                }
            }
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
                    SeedPanels();

                    uow.Complete();
                }
            }
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
                    SeedDrugsFromDb();

                    uow.Complete();
                }
            }
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
                    SeedDrugPanels();

                    uow.Complete();
                }
            }
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);
                    SeedDrugTestCategories();

                    uow.Complete();
                }
            }
        }

        public void SeedDrugsFromDb()
        {

            var drugs = _context.Drugs.IgnoreQueryFilters().FirstOrDefault();
            var confirmations = _context.ConfirmationValues.IgnoreQueryFilters().FirstOrDefault();


            if (drugs == null && confirmations == null)
            {
                Logger.Debug("Seeding Drugs");

                var sql = @"select 
                dncua.drug_name_categories_id as dncidua,
                dnchair.drug_name_categories_id as dncidhair,
                dn.*, 
                dncua.test_category_id as 'ua_test_category_id',
                dnchair.test_category_id as 'hair_test_category_id',
                tcua.internal_name as 'tcua',
                tchair.internal_name as 'tchair'
                from 
                drug_names dn 
                left outer join drug_names_categories dncua on dncua.drug_name_id = dn.drug_name_id and dncua.test_category_id = 1
                left outer join drug_names_categories dnchair on dnchair.drug_name_id = dn.drug_name_id and dnchair.test_category_id = 2
                left outer join test_categories tcua on tcua.test_category_id = dncua.test_category_id
                left outer join test_categories tchair on tchair.test_category_id = dnchair.test_category_id
                order by dn.drug_name_id;
                ";

                if (surpathliveSeedHelper.conn.State == ConnectionState.Closed) surpathliveSeedHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, surpathliveSeedHelper.conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Logger.Debug($"Seeding {(string)reader["drug_name"]}");

                        var _drugMatch = new IdMatch()
                        {
                            Id = Guid.NewGuid(),
                            OldId = reader.GetInt32("drug_name_id")
                        };

                        drugs = _context.Drugs.Add(new Drug()
                        {
                            Id = _drugMatch.Id,
                            Name = (string)reader["drug_name"],
                            Code = (string)reader["drug_code"]
                        }).Entity;

                        idsDrugs.Add(_drugMatch);


                        if (reader["dncidua"] != DBNull.Value)
                        {
                            EnumUnitOfMeasurement ua_unit_of_measurement;
                            Enum.TryParse((string)reader["ua_unit_of_measurement"], true, out ua_unit_of_measurement);

                            double.TryParse((string)reader["ua_confirmation_value"], out double uaconfv);
                            double.TryParse((string)reader["ua_screen_value"], out double uascreenv);

                            var _ummatch = new IdMatch()
                            {
                                Id = Guid.NewGuid(),
                                OldId = reader.GetInt32("dncidua")
                            };
                            var _tc = idsTestCats.Where(t => t.OldId == reader.GetInt32("ua_test_category_id")).First();
                            // create a UA confirmation
                            confirmations = _context.ConfirmationValues.Add(new ConfirmationValue()
                            {
                                Id = _ummatch.Id,
                                ConfirmValue = uaconfv,
                                ScreenValue = uascreenv,
                                UnitOfMeasurement = ua_unit_of_measurement,
                                DrugId = _drugMatch.Id, // reader.GetInt32("drug_name_id"),
                                TestCategoryId = _tc.Id
                            }).Entity;

                        }

                        if (reader["dncidhair"] != DBNull.Value)
                        {
                            EnumUnitOfMeasurement hair_unit_of_measurement;
                            Enum.TryParse((string)reader["hair_unit_of_measurement"], true, out hair_unit_of_measurement);

                            double.TryParse((string)reader["hair_confirmation_value"], out double hair_confirmation_value);
                            double.TryParse((string)reader["hair_screen_value"], out double hair_screen_value);
                            var _ummatch = new IdMatch()
                            {
                                Id = Guid.NewGuid(),
                                OldId = reader.GetInt32("dncidhair")
                            };
                            var _tc = idsTestCats.Where(t => t.OldId == reader.GetInt32("hair_test_category_id")).First();
                            // create a hair confirmation
                            confirmations = _context.ConfirmationValues.Add(new ConfirmationValue()
                            {
                                Id = _ummatch.Id,
                                ConfirmValue = hair_confirmation_value,
                                ScreenValue = hair_screen_value,
                                UnitOfMeasurement = hair_unit_of_measurement,
                                DrugId = _drugMatch.Id,
                                TestCategoryId = _tc.Id // reader.GetInt32("hair_test_category_id")
                            }).Entity;

                        }

                    }
                }
                _context.SaveChanges();

            }
        }


        public async Task SeedTestCategories()
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    _context = uowManager.Object.Current.GetDbContext<inzibackendDbContext>(MultiTenancySides.Host);

                    var testcategories = _context.TestCategories.IgnoreQueryFilters().FirstOrDefault();

                    if (testcategories == null)
                    {
                        var sql = "select * from test_categories;";
                        if (surpathliveSeedHelper.conn.State == ConnectionState.Closed) surpathliveSeedHelper.conn.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, surpathliveSeedHelper.conn);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var _match = new IdMatch()
                                {
                                    Id = Guid.NewGuid(),
                                    OldId = reader.GetInt32("test_category_id")
                                };

                                testcategories = _context.TestCategories.Add(new TestCategory()
                                {
                                    Id = _match.Id,
                                    Name = (string)reader["test_category_name"],
                                    InternalName = (string)reader["internal_name"],

                                }).Entity;
                                _context.SaveChanges();
                                await _unitOfWorkManager.Current.SaveChangesAsync();
                                idsTestCats.Add(_match);

                            }
                        }
                    }

                    uow.Complete();
                }
            }


        }

        public void SeedPanels()
        {
            var testPanels = _context.Panels.IgnoreQueryFilters().FirstOrDefault();

            if (testPanels == null)
            {
                var sql = "select * from test_panels;";
                if (surpathliveSeedHelper.conn.State == ConnectionState.Closed) surpathliveSeedHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, surpathliveSeedHelper.conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _i = idsTestCats.Where(t => t.OldId == reader.GetInt32("test_category_id")).First();
                        var _match = new IdMatch()
                        {
                            Id = Guid.NewGuid(),
                            OldId = reader.GetInt32("test_panel_id")
                        };
                        testPanels = _context.Panels.Add(new Panel()
                        {
                            Id = _match.Id,
                            Name = (string)reader["test_panel_name"],
                            Description = (string)reader["test_panel_description"],
                            Cost = (double)reader["cost"],
                            TestCategoryId = _i.Id,

                        }).Entity;
                        idsTestPanels.Add(_match);
                    }
                    _context.SaveChanges();
                }
            }
        }

        public void SeedDrugPanels()
        {
            var seedDrugPanels = _context.DrugPanels.IgnoreQueryFilters().FirstOrDefault();

            if (seedDrugPanels == null)
            {
                var sql = "select * from test_panel_drug_names;";
                if (surpathliveSeedHelper.conn.State == ConnectionState.Closed) surpathliveSeedHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, surpathliveSeedHelper.conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _dpMatch = new IdMatch()
                        {
                            Id = Guid.NewGuid(),
                            OldId = reader.GetInt32("test_panel_drug_names_id")
                        };
                        var _d = idsDrugs.Where(t => t.OldId == reader.GetInt32("drug_name_id")).First();
                        var _p = idsTestPanels.Where(t => t.OldId == reader.GetInt32("test_panel_id")).First();

                        seedDrugPanels = _context.DrugPanels.Add(new DrugPanel()
                        {
                            Id = _dpMatch.Id, // reader.GetInt32("test_panel_drug_names_id"),
                            DrugId = _d.Id,
                            PanelId = _p.Id
                        }).Entity;
                    }
                    _context.SaveChanges();
                }
            }
        }

        public void SeedDrugTestCategories()
        {


            var seeddrugtestcats = _context.DrugTestCategories.IgnoreQueryFilters().FirstOrDefault();

            //drugtestcats = _context.DrugTestCategories.Add(new DrugTestCategory()
            //{
            //    Id = reader.GetInt32("test_panel_drug_names_id"),

            //}).Entity;
            if (seeddrugtestcats == null)
            {

                var sql = @"select 
            tpdn.*,
            dncua.test_category_id as 'ua_test_category_id',
            dnchair.test_category_id as 'hair_test_category_id'
            from 
            test_panel_drug_names tpdn
            left outer join drug_names dn on dn.drug_name_id = tpdn.drug_name_id
            left outer join drug_names_categories dncua on dncua.drug_name_id = dn.drug_name_id and dncua.test_category_id = 1
            left outer join drug_names_categories dnchair on dnchair.drug_name_id = dn.drug_name_id and dnchair.test_category_id = 2
            left outer join test_categories tcua on tcua.test_category_id = dncua.test_category_id
            left outer join test_categories tchair on tchair.test_category_id = dnchair.test_category_id
            order by tpdn.test_panel_drug_names_id;";

                if (surpathliveSeedHelper.conn.State == ConnectionState.Closed) surpathliveSeedHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, surpathliveSeedHelper.conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var _d = idsDrugs.Where(t => t.OldId == reader.GetInt32("drug_name_id")).First();
                        var _p = idsTestPanels.Where(t => t.OldId == reader.GetInt32("test_panel_id")).First();
                        if (reader["ua_test_category_id"] != DBNull.Value)
                        {

                            var _dtcMatch = new IdMatch()
                            {
                                Id = Guid.NewGuid(),

                            };
                            var _t = idsTestCats.Where(t => t.OldId == reader.GetInt32("ua_test_category_id")).First();

                            seeddrugtestcats = _context.DrugTestCategories.Add(new DrugTestCategory()
                            {
                                Id = _dtcMatch.Id,
                                // Id = reader.GetInt32("test_panel_drug_names_id"),
                                DrugId = _d.Id,
                                PanelId = _p.Id,
                                TestCategoryId = _t.Id
                            }).Entity;
                        }
                        if (reader["hair_test_category_id"] != DBNull.Value)
                        {
                            var _dtcMatch = new IdMatch()
                            {
                                Id = Guid.NewGuid(),

                            };
                            var _t = idsTestCats.Where(t => t.OldId == reader.GetInt32("hair_test_category_id")).First();
                            seeddrugtestcats = _context.DrugTestCategories.Add(new DrugTestCategory()
                            {
                                Id = _dtcMatch.Id,
                                // Id = reader.GetInt32("test_panel_drug_names_id"),
                                DrugId = _d.Id,
                                PanelId = _p.Id,
                                TestCategoryId = _t.Id
                            }).Entity;
                        }
                    }
                    _context.SaveChanges();
                }
            }

        }


    }

    public class IdMatch
    {
        public Guid Id { get; set; }
        public int OldId { get; set; }
    }
}