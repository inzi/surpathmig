using inzibackend.Authorization.Users.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class GetCohortUserForBGCExportDto
    {
        public CohortUserDto CohortUser { get; set; }

        public string CohortDescription { get; set; }
        public string CohortName { get; set; }

        public UserEditDto UserEditDto { get; set; }

        public List<UserPidDto> UserPids { get; set; } = new List<UserPidDto>();
    }
}