using System.ComponentModel.DataAnnotations;

namespace Core.Application.Dto.auth
{
    public class ResetPassowrdDto
    {
        [Required]
        public string Token { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "confirm password dont match password")]
        public string ConfirmPassword { get; set; }
    }
}
