using System.ComponentModel.DataAnnotations;

namespace inzibackend.Authorization.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}

