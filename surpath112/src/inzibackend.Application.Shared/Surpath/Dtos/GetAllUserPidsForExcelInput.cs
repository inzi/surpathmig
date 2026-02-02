using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllUserPidsForExcelInput
    {
        public string Filter { get; set; }

        public string PidFilter { get; set; }

        public int? ValidatedFilter { get; set; }

        public string PidTypeNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}