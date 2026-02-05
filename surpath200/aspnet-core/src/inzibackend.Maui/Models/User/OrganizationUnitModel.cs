using Abp.AutoMapper;
using inzibackend.Organizations.Dto;

namespace inzibackend.Maui.Models.User;

[AutoMapFrom(typeof(OrganizationUnitDto))]
public class OrganizationUnitModel : OrganizationUnitDto
{
    public bool IsAssigned { get; set; }
}