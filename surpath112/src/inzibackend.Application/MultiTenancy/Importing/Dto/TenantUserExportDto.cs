namespace inzibackend.MultiTenancy.Importing.Dto
{
    public class TenantUserExportDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string AssignedRoleNames { get; set; }

        public string DepartmentName { get; set; }

        public string CohortName { get; set; }

        public string Address { get; set; }

        public string SuiteApt { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string DateOfBirth { get; set; }

        public string SSN { get; set; }
    }
}
