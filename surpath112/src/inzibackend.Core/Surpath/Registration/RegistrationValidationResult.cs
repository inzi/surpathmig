using System.Collections.Generic;
using System.Linq;

namespace inzibackend.Surpath.Registration
{
    public class RegistrationValidationResult
    {
        private readonly List<string> _errors = new List<string>();

        public bool EmailAvailable { get; set; }
        public string EmailError { get; set; }

        public bool UsernameAvailable { get; set; }
        public string UsernameError { get; set; }

        public bool DepartmentValid { get; set; }
        public string DepartmentError { get; set; }

        public bool CohortValid { get; set; }
        public string CohortError { get; set; }

        public IReadOnlyList<string> Errors => _errors;

        public bool IsValid => EmailAvailable && UsernameAvailable && DepartmentValid && CohortValid && !_errors.Any();

        public void AddError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _errors.Add(message);
            }
        }
    }
}
