using System;

namespace inzibackend.MultiTenancy.Importing.Dto
{
    public class ImportTenantUserDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// comma separated list
        /// </summary>
        public string[] AssignedRoleNames { get; set; }

        /// <summary>
        /// Optional department name (case-insensitive lookup)
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Optional cohort name (case-insensitive lookup)
        /// </summary>
        public string CohortName { get; set; }

        /// <summary>
        /// Street address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Apartment or suite number
        /// </summary>
        public string SuiteApt { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State abbreviation (e.g., TX, CA)
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// ZIP code
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Social Security Number (SSN) - will be stored as UserPid
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing user
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
