using Microsoft.AspNetCore.Components;
using inzibackend.Authorization.Accounts;
using inzibackend.Authorization.Accounts.Dto;
using inzibackend.Maui.Core.Components;
using inzibackend.Maui.Core.Threading;
using inzibackend.Maui.Models.Login;

namespace inzibackend.Maui.Pages.Login;

public partial class ForgotPasswordModal : ModalBase
{
    public override string ModalId => "forgot-password-modal";

    [Parameter] public EventCallback OnSave { get; set; }

    public ForgotPasswordModel ForgotPasswordModel { get; } = new();

    private readonly IAccountAppService _accountAppService;

    public ForgotPasswordModal()
    {
        _accountAppService = Resolve<IAccountAppService>();
    }

    protected virtual async Task Save()
    {
        await SetBusyAsync(async () =>
        {
            await WebRequestExecuter.Execute(
                async () =>
                    await _accountAppService.SendPasswordResetCode(new SendPasswordResetCodeInput { EmailAddress = ForgotPasswordModel.EmailAddress }),
                async () =>
                {
                    await OnSave.InvokeAsync();
                }
            );
        });
    }

    protected virtual async Task Cancel()
    {
        await Hide();
    }
}