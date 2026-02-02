using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos.Registration
{
    public class RegistrationValidationResultDto
    {
        public bool IsValid { get; set; }

        public bool EmailAvailable { get; set; }
        public string EmailError { get; set; }

        public bool UsernameAvailable { get; set; }
        public string UsernameError { get; set; }

        public bool DepartmentValid { get; set; }
        public string DepartmentError { get; set; }

        public bool CohortValid { get; set; }
        public string CohortError { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
