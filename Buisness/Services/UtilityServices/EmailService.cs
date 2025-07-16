using Buisness.Abstract.ServicesBase;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Buisness.Services.UtilityServices
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? throw new InvalidOperationException("SMTP Host is not configured.");
                var smtpPort = int.TryParse(_configuration["EmailSettings:SmtpPort"], out var port) ? port : throw new InvalidOperationException("SMTP Port is not configured or invalid.");
                var smtpUsername = _configuration["EmailSettings:Username"] ?? throw new InvalidOperationException("SMTP Username is not configured.");
                var smtpPassword = _configuration["EmailSettings:Password"] ?? throw new InvalidOperationException("SMTP Password is not configured.");
                var enableSsl = bool.TryParse(_configuration["EmailSettings:EnableSsl"], out var ssl) ? ssl : throw new InvalidOperationException("EnableSsl is not configured or invalid.");
                var fromAddress = _configuration["EmailSettings:FromAddress"] ?? throw new InvalidOperationException("FromAddress is not configured.");

                var smtpClient = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword),
                    UseDefaultCredentials = false,
                    EnableSsl = enableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {Recipient}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email to {Recipient}", to);
                throw;
            }
        }
    }
}