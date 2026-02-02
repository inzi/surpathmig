using inzibackend.EntityFrameworkCore;
using inzibackend.Migrations.Seed.Surpath;
using inzibackend.Surpath;
using inzibackend.SurpathSeedHelper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace inzibackend.Migrations.Seed.Host
{
    internal class CodeTypeSeeder
    {
        private readonly inzibackendDbContext _context;
        private SurpathliveSeedHelper surpathliveSeedHelper { get; set; }
        private List<string> codetypes = new List<string>() { "Lab", "Quest", "Clear Star", "FormFox" };
        private ParamHelper paramHelper;

        public CodeTypeSeeder(inzibackendDbContext context, SurpathliveSeedHelper _surpathliveSeedHelper)
        {
            _context = context;
            surpathliveSeedHelper = _surpathliveSeedHelper;
            paramHelper = new ParamHelper();
        }

        public void Create()
        {
            SeedCodeTypes();
        }

        private void SeedCodeTypes()
        {
            var seeds = _context.CodeTypes.IgnoreQueryFilters().FirstOrDefault();

            if (seeds == null)
            {
                foreach (var ct in codetypes)
                {
                    seeds = _context.CodeTypes.Add(new CodeType()
                    {
                        Name = ct
                    }).Entity;
                }
                _context.SaveChanges();
            }
        }

    }

}
