using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace inzibackend.EntityFrameworkCore
{
    public static class inzibackendDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<inzibackendDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }

        public static void Configure(DbContextOptionsBuilder<inzibackendDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection.ConnectionString, ServerVersion.AutoDetect(connection.ConnectionString), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }
    }
}