using System.Threading.Tasks;

namespace inzibackend.Security.Recaptcha;

public interface IRecaptchaValidator
{
    Task ValidateAsync(string captchaResponse);
}
