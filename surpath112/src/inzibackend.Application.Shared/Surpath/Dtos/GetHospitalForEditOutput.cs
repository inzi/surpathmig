using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetHospitalForEditOutput
    {
        public CreateOrEditHospitalDto Hospital { get; set; }

    }
}