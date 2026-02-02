using inzibackend.Common;
using inzibackend.Editions.Dto;
using inzibackend.Maui.Core.Components;
using inzibackend.Maui.Core.Threading;
using inzibackend.Maui.Models.Tenants;
using inzibackend.MultiTenancy;

namespace inzibackend.Maui.Pages.Tenant;

public partial class CreateTenantModal : inzibackendMainLayoutPageComponentBase
{
    private readonly ITenantAppService _tenantAppService;
    private readonly ICommonLookupAppService _commonLookupAppService;

    public CreateTenantModal()
    {
        _tenantAppService = Resolve<ITenantAppService>();
        _commonLookupAppService = Resolve<ICommonLookupAppService>();
    }

    private CreateTenantModel CreateTenantModel { get; set; } = new()
    {
        IsActive = true
    };

    protected override async Task OnInitializedAsync()
    {
        CreateTenantModel = new CreateTenantModel
        {
            IsActive = true
        };

        await PopulateEditionsCombobox();

        await SetPageHeader(L("CreateNewTenant"));
    }

    private async Task CreateTenantAsync()
    {
        await SetBusyAsync(async () =>
        {
            await WebRequestExecuter.Execute(async () =>
            {
                CreateTenantModel.NormalizeCreateTenantInputModel();

                await _tenantAppService.CreateTenant(CreateTenantModel);
            }, async () => { await UserDialogsService.AlertSuccess(L("SuccessfullySaved")); });
        });
    }

    private async Task PopulateEditionsCombobox()
    {
        var editions = await _commonLookupAppService.GetEditionsForCombobox();
        CreateTenantModel.Editions = editions.Items.ToList();

        CreateTenantModel.Editions.Insert(0, new SubscribableEditionComboboxItemDto(CreateTenantModel.NotAssignedValue,
            $"- {L("NotAssigned")} -", null));
    }
}