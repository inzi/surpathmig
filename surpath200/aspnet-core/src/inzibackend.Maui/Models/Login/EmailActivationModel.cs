using System.ComponentModel.DataAnnotations;

namespace inzibackend.Maui.Models.Login;

public class EmailActivationModel
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
}