using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models
{

    public class RegisterModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;    

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;    

        [Required]
        public string Role { get; set; } = string.Empty; // Admin, User, Guest
    }
}