using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetTestCategoryForEditOutput
    {
        public CreateOrEditTestCategoryDto TestCategory { get; set; }

    }
}