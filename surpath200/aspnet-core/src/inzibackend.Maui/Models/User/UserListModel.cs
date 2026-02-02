using Abp.AutoMapper;
using inzibackend.Authorization.Users.Dto;

namespace inzibackend.Maui.Models.User;

[AutoMapFrom(typeof(UserListDto))]
public class UserListModel : UserListDto
{
    public string Photo { get; set; }

    public string FullName => Name + " " + Surname;
}