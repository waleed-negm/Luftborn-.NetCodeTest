using System.ComponentModel.DataAnnotations;

namespace Core.Application.Dto.auth
{
    public class AuthDto
    {
        public string? UserId { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required, MaxLength(100)]
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public List<string>? Roles { get; set; }
    }
}
