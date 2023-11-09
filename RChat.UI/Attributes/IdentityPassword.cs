using System.ComponentModel.DataAnnotations;

namespace RChat.UI.Attributes
{
    public class IdentityPassword : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 6;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = true;

        public IdentityPassword()
        {
            ErrorMessage = "Identity requires that passwords contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character. Passwords must be at least six characters long.";
        }

        public override bool IsValid(object value)
        {
            if (value is not string password)
            {
                return false;
            }

            if (password.Length < MinimumLength)
            {
                ErrorMessage = $"The password must be at least {MinimumLength} characters long.";
                return false;
            }

            if (RequireUppercase && !password.Any(char.IsUpper))
            {
                ErrorMessage = "The password must contain at least one uppercase letter.";
                return false;
            }

            if (RequireLowercase && !password.Any(char.IsLower))
            {
                ErrorMessage = "The password must contain at least one lowercase letter.";
                return false;
            }

            if (RequireDigit && !password.Any(char.IsDigit))
            {
                ErrorMessage = "The password must contain at least one digit.";
                return false;
            }

            if (RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
            {
                ErrorMessage = "The password must contain at least one non-alphanumeric character.";
                return false;
            }

            return true;
        }
    }
}
