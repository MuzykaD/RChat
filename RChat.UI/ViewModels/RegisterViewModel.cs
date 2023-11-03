using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RChat.UI.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [JsonIgnore]
        public string ConfirmPassword { get; set; }
    }
}
