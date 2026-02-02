using System.Collections.Generic;

namespace inzibackend.Authorization.Users.Profile.Dto;

public class UpdateGoogleAuthenticatorKeyOutput
{
    public IEnumerable<string> RecoveryCodes { get; set; }
}

