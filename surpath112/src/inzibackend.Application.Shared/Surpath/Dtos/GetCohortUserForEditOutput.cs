using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetCohortUserForEditOutput
    {
        public CreateOrEditCohortUserDto CohortUser { get; set; }

        public string CohortDescription { get; set; }

        public string UserName { get; set; }

    }
}