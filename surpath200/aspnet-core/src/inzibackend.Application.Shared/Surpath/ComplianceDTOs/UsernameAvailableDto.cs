using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class UsernameAvailableDto 
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string EmailAddress { get; set; }
        public int? TenantId { get; set; }

    }
}