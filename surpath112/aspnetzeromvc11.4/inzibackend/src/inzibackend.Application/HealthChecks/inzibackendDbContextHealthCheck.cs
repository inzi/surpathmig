using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using inzibackend.EntityFrameworkCore;

namespace inzibackend.HealthChecks
{
    public class inzibackendDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public inzibackendDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("inzibackendDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("inzibackendDbContext could not connect to database"));
        }
    }
}
