using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetUserPidForEditOutput
    {
        public CreateOrEditUserPidDto UserPid { get; set; }

        public string PidTypeName { get; set; }

        public string UserName { get; set; }

    }
}