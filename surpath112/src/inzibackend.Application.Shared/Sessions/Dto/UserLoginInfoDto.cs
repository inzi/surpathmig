using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using System;
using System.Collections.Generic;

namespace inzibackend.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }

        public bool IsPaid { get; set; } = false;
        public bool IsAlwaysDonor { get; set; } = false;
        public bool IsCohortUser { get; set; } = false;
        public bool IsInvoiced { get; set; } = false;
        public Guid? CohortUserId { get; set; }
        public List<Guid> Departments { get; set; } = new List<Guid>();
        public List<Guid> DepartmentsAuthed { get; set; } = new List<Guid>();
        public Guid? CohortId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
