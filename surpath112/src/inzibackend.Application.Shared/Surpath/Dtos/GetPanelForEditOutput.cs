using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetPanelForEditOutput
    {
        public CreateOrEditPanelDto Panel { get; set; }

        public string TestCategoryName { get; set; }

    }
}