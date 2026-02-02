using System.ComponentModel.DataAnnotations;

namespace inzibackend.Maui.Models.Login;

public class ForgotPasswordModel
{
    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; }
}