using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetCodeTypeForEditOutput
    {
        public CreateOrEditCodeTypeDto CodeType { get; set; }

    }
}