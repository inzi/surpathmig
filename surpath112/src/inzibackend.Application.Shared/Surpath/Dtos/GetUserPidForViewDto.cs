using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetUserPidForViewDto
    {
        public UserPidDto UserPid { get; set; }

        public string PidTypeName { get; set; }

        public string UserName { get; set; }
        public PidTypeDto PidType { get; set; }

    }
}