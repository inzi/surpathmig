using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace inzibackend.EntityFrameworkCore
{
    public static class inzibackendDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<inzibackendDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<inzibackendDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}