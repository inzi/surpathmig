using Microsoft.Extensions.Configuration;

namespace inzibackend.Configuration;

public interface IAppConfigurationAccessor
{
    IConfigurationRoot Configuration { get; }
}

