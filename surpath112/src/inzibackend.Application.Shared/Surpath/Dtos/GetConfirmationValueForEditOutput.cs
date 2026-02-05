using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetConfirmationValueForEditOutput
    {
        public CreateOrEditConfirmationValueDto ConfirmationValue { get; set; }

        public string DrugName { get; set; }

        public string TestCategoryName { get; set; }

    }
}