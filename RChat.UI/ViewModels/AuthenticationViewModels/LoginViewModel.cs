using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RChat.UI.ViewModels.AuthenticationViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
