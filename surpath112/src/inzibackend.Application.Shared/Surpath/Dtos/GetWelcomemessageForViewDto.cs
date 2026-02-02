using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetWelcomemessageForViewDto
    {
        public WelcomemessageDto Welcomemessage { get; set; }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }
    }
}