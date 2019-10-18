using SendGrid;
using System.Threading.Tasks;

namespace Sphix.Service.SendGridManager
{
   public interface IEmailSenderService
    {
        Task<Response> SendEmailAsync(string subject, string message, string toEmail, string fromEmail, string fromName);
    }
}
