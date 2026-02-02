using System.ComponentModel.DataAnnotations;

namespace inzibackend.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}