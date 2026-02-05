using System;

namespace inzibackend.Surpath.Registration
{
    public class RegistrationValidationRequest
    {
        public int? TenantId { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }
    }
}
