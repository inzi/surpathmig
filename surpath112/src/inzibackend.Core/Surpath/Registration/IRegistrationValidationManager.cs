using System.Threading.Tasks;

namespace inzibackend.Surpath.Registration
{
    public interface IRegistrationValidationManager
    {
        Task<RegistrationValidationResult> ValidateAsync(RegistrationValidationRequest request);

        Task EnsureValidAsync(RegistrationValidationRequest request);
    }
}
