using inzibackend.EntityFrameworkCore;
using inzibackend.Surpath;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace inzibackend.Migrations.Seed.Host
{
    internal class CodeTypeSeeder
    {
        private readonly inzibackendDbContext _context;
        private List<string> codetypes = new List<string>() { "Lab", "Quest", "Clear Star", "FormFox" };

        public CodeTypeSeeder(inzibackendDbContext context)
        {
            _context = context;
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
