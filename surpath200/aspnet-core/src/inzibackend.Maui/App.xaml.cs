using inzibackend.ApiClient;
using inzibackend.Maui.Core;
using inzibackend.Maui.Services.Account;
using inzibackend.Maui.Services.Navigation;
using inzibackend.Maui.Services.Storage;

namespace inzibackend.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }

    public static async Task OnSessionTimeout()
    {
        await DependencyResolver.Resolve<IAccountService>().LogoutAsync();
        DependencyResolver.Resolve<INavigationService>().NavigateTo(NavigationUrlConsts.Login);
    }

    public static async Task OnAccessTokenRefresh(string newAccessToken, string newEncryptedAccessToken)
    {
        await DependencyResolver.Resolve<IDataStorageService>().StoreAccessTokenAsync(newAccessToken, newEncryptedAccessToken);
    }

    public static void LoadPersistedSession()
    {
        var accessTokenManager = DependencyResolver.Resolve<IAccessTokenManager>();
        var dataStorageService = DependencyResolver.Resolve<IDataStorageService>();
        var applicationContext = DependencyResolver.Resolve<IApplicationContext>();

        accessTokenManager.AuthenticateResult = dataStorageService.RetrieveAuthenticateResult();
        applicationContext.Load(dataStorageService.RetrieveTenantInfo(), dataStorageService.RetrieveLoginInfo());
    }
}