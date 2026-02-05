using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace inzibackend.Web.Authentication.JwtBearer;

public class AsyncJwtBearerOptions : JwtBearerOptions
{
    public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;

    private readonly inzibackendAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new inzibackendAsyncJwtSecurityTokenHandler();

    public AsyncJwtBearerOptions()
    {
        AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() { _defaultAsyncHandler };
    }
}


