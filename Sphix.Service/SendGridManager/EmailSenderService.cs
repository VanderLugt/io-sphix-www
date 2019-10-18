using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Sphix.Service.SendGridManager
{
    public class EmailSenderService : IEmailSenderService
    {
        private AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        public EmailSenderService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public Task<Response> SendEmailAsync(string subject, string message, string toEmailAddress, string fromEmailAddress, string fromName)
        {
            return Execute(Options.SendGridKey, subject, message, toEmailAddress, fromEmailAddress, fromName);
        }
        internal Task<Response> Execute(string apiKey, string subject, string message, string toEmail,string fromEmail,string fromName)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));
            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var _data = client.SendEmailAsync(msg);
            return _data;
        }
    }
}
