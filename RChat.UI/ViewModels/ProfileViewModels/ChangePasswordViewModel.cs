using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RChat.UI.ViewModels.ProfileViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [PasswordPropertyText]
        public string CurrentPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        public string NewPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        [Compare("NewPassword")]
        [JsonIgnore]
        public string ConfirmNewPassword { get; set; }

    }
}
