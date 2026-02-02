using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllCohortUsersForExcelInput
    {
        public string Filter { get; set; }

        public string CohortDescriptionFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string CohortId { get; set; }

    }
}