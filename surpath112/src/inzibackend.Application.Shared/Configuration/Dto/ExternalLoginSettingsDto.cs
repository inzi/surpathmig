using System.Collections.Generic;

namespace inzibackend.Configuration.Dto
{
    public class ExternalLoginSettingsDto
    {
        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
    }
}