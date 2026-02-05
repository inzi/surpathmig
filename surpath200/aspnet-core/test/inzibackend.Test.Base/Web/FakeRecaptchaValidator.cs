using System.Threading.Tasks;
using inzibackend.Security.Recaptcha;

namespace inzibackend.Test.Base.Web;

public class FakeRecaptchaValidator : IRecaptchaValidator
{
    public Task ValidateAsync(string captchaResponse)
    {
        return Task.CompletedTask;
    }
}
