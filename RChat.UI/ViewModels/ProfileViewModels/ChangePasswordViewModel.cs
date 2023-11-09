using RChat.UI.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RChat.UI.ViewModels.ProfileViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [PasswordPropertyText]
        [IdentityPassword]
        public string CurrentPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        [IdentityPassword]
        public string NewPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        [Compare("NewPassword")]
        [IdentityPassword]
        [JsonIgnore]
        public string ConfirmNewPassword { get; set; }

    }
}
