using System.ComponentModel.DataAnnotations;

namespace Core.Application.Dto.auth
{
    public class RegisterDto
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, StringLength(50)]
        public string Username { get; set; }
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string Password { get; set; }
        [Required,Compare("Password", ErrorMessage = "confirm password dont match password")]
        public string ConfirmPassword { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
    }
}