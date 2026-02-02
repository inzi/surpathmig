using System.Threading.Tasks;

namespace inzibackend.Net.Emailing;

public interface IEmailSettingsChecker
{
    bool EmailSettingsValid();

    Task<bool> EmailSettingsValidAsync();
}

