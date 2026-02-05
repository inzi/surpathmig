using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class WelcomemessageDto : EntityDto
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsDefault { get; set; }

        public DateTime DisplayStart { get; set; }

        public DateTime DisplayEnd { get; set; }

    }
}