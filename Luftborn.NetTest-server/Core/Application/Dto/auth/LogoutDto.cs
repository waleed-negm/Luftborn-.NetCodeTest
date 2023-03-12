using System.ComponentModel.DataAnnotations;

namespace Core.Application.Dto.auth
{
    public class LogoutDto
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string token { get; set; }
    }
}
