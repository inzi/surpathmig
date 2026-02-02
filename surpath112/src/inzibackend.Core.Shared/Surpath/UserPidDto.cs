using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class UserPidDto : EntityDto<Guid>
    {
        public string Pid { get; set; }

        public bool Validated { get; set; }

        public Guid PidTypeId { get; set; }

        public long? UserId { get; set; }

        //public bool MaskPid { get; set; } = true;
        //public string PidTypeName { get; set; }
        //public string PidTypeDescription { get; set; }
        public PidTypeDto PidType { get; set; }
    }
}