namespace inzibackend.Maui.Services.Tenants;

public interface ITenantCustomizationService
{
    Task<string> GetTenantLogo();
}