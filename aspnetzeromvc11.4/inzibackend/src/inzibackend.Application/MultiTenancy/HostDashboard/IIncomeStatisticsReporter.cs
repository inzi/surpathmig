using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inzibackend.MultiTenancy.HostDashboard.Dto;

namespace inzibackend.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}