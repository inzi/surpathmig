using Abp.Configuration;

namespace inzibackend.Timing.Dto;

public class GetTimezonesInput
{
    public SettingScopes DefaultTimezoneScope { get; set; }
}

