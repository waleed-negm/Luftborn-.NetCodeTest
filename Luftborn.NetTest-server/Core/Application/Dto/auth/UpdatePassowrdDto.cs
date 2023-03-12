using System.ComponentModel.DataAnnotations;

namespace Core.Application.Dto.auth
{
    public class UpdatePassowrdDto
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "confirm password dont match password")]
        public string ConfirmPassword { get; set; }
    }
}
