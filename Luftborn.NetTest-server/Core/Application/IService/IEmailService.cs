
using Microsoft.AspNetCore.Http;

namespace Core.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachments = null);
    }
}
