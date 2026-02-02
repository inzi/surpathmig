using System;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos.Registration
{
    public class RegistrationValidationInputDto
    {
        public int? TenantId { get; set; }

        [StringLength(256)]
        public string EmailAddress { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }
    }
}

