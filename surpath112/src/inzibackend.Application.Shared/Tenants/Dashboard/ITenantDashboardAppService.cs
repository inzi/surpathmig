using Abp.Application.Services;
using inzibackend.Tenants.Dashboard.Dto;

namespace inzibackend.Tenants.Dashboard
{
    // Surscan Working Filter
    public class GetSurpathDeptInput
    {
        public string Name { get; set; }
    }
    // Surscan Working Filter
    public class GetSurpathDeptOutput
    {
        public string OutPutName { get; set; }
    }

    //public class GetSurpathDeptListOutput
    //{
    //    public GetSurpathDeptListOutput(List<MemberActivity> memberActivities)
    //    {
    //        MemberActivities = memberActivities;
    //    }

    //    public List<MemberActivity> MemberActivities { get; set; }

    //}

    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();

        GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input);

        GetDailySalesOutput GetDailySales();

        GetProfitShareOutput GetProfitShare();

        GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input);

        GetTopStatsOutput GetTopStats();

        GetRegionalStatsOutput GetRegionalStats();

        GetGeneralStatsOutput GetGeneralStats();
        // Surscan Working Filter
        GetSurpathDeptOutput GetSurpathDeptData(GetSurpathDeptInput input);
        //GetSurpathDeptListOutput GetSurpathDeptListData(GetDashboardDataInput input);
    }
}
