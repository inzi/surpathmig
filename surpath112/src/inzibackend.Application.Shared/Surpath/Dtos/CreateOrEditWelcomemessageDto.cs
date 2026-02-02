using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditWelcomemessageDto : EntityDto<int?>
    {

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsDefault { get; set; }

        public DateTime DisplayStart { get; set; }

        public DateTime DisplayEnd { get; set; }

    }
}