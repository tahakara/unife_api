using Buisness.Services.UtilityServices.Base.EmailServices;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Buisness.Services.UtilityServices.EmailServices
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

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
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
                //mailMessage.To.Add(to);
                mailMessage.To.Add("noreply@tahakara.dev");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogDebug("Email sent successfully to {Recipient}", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email to {Recipient}", to);
                return false;
            }
        }

        public async Task<bool> SendForgotPasswordEmailAsync(string to, string recoveryToken)
        {
            try
            {
                _logger.LogDebug("Sending forgot password email to {Recipient}", to);
                var subject = "Password Recovery Request";
                var body = $@"
                    <div style='font-family:Segoe UI,Arial,sans-serif;max-width:600px;margin:auto;border:1px solid #e0e0e0;border-radius:8px;box-shadow:0 2px 8px #f0f0f0;padding:32px;background:#fafbfc;'>
                        <div style='text-align:center;margin-bottom:24px;'>
                            <img src='https://cdn-icons-png.flaticon.com/512/561/561127.png' alt='Password Recovery' width='64' height='64' style='margin-bottom:12px;' />
                            <h2 style='color:#d32f2f;margin:0;'>Password Recovery Request</h2>
                        </div>
                        <p style='font-size:16px;color:#333;'>Hello,</p>
                        <p style='font-size:15px;color:#444;margin-bottom:18px;'>We received a request to reset your password. If you did not make this request, please ignore this email.</p>
                        <p style='font-size:15px;color:#444;margin-bottom:18px;'>To reset your password, please use the following token:</p>
                        <h2 style='letter-spacing:2px;'>http://example.com/auth/forgot-password/t/{recoveryToken}</h2>
                        <p style='font-size:14px;color:#333;'>If you have any questions, feel free to contact our support team.</p>
                        <hr style='border:none;border-top:1px solid #eee;margin:24px 0;' />
                        <p style='font-size:12px;color:#999;text-align:center;'>This is an automated message. Please do not reply directly to this email.</p>
                    </div>";

                var emailSent = await SendEmailAsync(to, subject, body, true);
                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send forgot password email to {Recipient}", to);
                    return false;
                }
                _logger.LogDebug("Forgot password email sent successfully to {Recipient}", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending forgot password email to {Recipient}", to);
                return false;
            }
        }

        public async Task<bool> SendSiginCompleteMail(string ipAddress, string userAgent, string emailAddress, string firstName, string middleName, string lastName)
        {
            try
            {
                _logger.LogDebug("Sending sign-in complete email to {EmailAddress}", emailAddress);

                var subject = "Sign-In Completed Successfully";
                var body = $@"
                    <div style='font-family:Segoe UI,Arial,sans-serif;max-width:600px;margin:auto;border:1px solid #e0e0e0;border-radius:8px;box-shadow:0 2px 8px #f0f0f0;padding:32px;background:#fafbfc;'>
                        <div style='text-align:center;margin-bottom:24px;'>
                            <img src='https://cdn-icons-png.flaticon.com/512/561/561127.png' alt='Sign-In Success' width='64' height='64' style='margin-bottom:12px;' />
                            <h2 style='color:#2e7d32;margin:0;'>Sign-In Successful</h2>
                        </div>
                        <p style='font-size:16px;color:#333;'>Hello <strong>{firstName} {middleName} {lastName}</strong>,</p>
                        <p style='font-size:15px;color:#444;margin-bottom:18px;'>We noticed a successful sign-in to your account.</p>
                        <table style='width:100%;font-size:14px;color:#555;margin-bottom:18px;'>
                            <tr>
                                <td style='padding:6px 0;width:120px;font-weight:bold;'>IP Address:</td>
                                <td style='padding:6px 0;'>{ipAddress}</td>
                            </tr>
                            <tr>
                                <td style='padding:6px 0;font-weight:bold;'>Device:</td>
                                <td style='padding:6px 0;'>{userAgent}</td>
                            </tr>
                        </table>
                        <p style='font-size:14px;color:#d32f2f;margin-bottom:18px;'>
                            If this wasn't you, please <a href='mailto:support@yourdomain.com' style='color:#1976d2;text-decoration:underline;'>contact support</a> immediately.
                        </p>
                        <p style='font-size:14px;color:#333;'>Thank you for using our service!<br/>The Support Team</p>
                        <hr style='border:none;border-top:1px solid #eee;margin:24px 0;' />
                        <p style='font-size:12px;color:#999;text-align:center;'>This is an automated message. Please do not reply directly to this email.</p>
                    </div>";

                var emailSent = await SendEmailAsync(emailAddress, subject, body, true);
                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send sign-in complete email to {EmailAddress}", emailAddress);
                    return false;
                }

                _logger.LogDebug("Sign-in complete email sent successfully to {EmailAddress}", emailAddress);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending sign-in complete email to {EmailAddress}", emailAddress);
                return false;
            }
        }

        public async Task<bool> SendSignInOtpCode(string to, string otp, string subject = "Authentication Code for Secure Sign-In", bool isHtml = true)
        {
            try
            {
                _logger.LogDebug("Sending sign-in OTP code to {Recipient}", to);
                
                var body = isHtml
                    ? $@"
                        <!-- HIDE PREVIEW TEXT -->
                        <div style='display:none; max-height:0px; overflow:hidden;'>
                        &nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;
                        </div>
                        <!-- / HIDE PREVIEW TEXT -->
                        
                        <p>Hello,</p>
                        <p>Please use the code below to continue:</p>
                        <h2 style='letter-spacing:2px;'>{otp}</h2>
                        <p>Keep this code private.</p>"
                            : $@"Hello,
                        
                        Please use the following code to continue:
                        
                        {otp}
                        
                        Keep this code private.";

                var emailSent = await SendEmailAsync(to, subject, body, isHtml);
                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send sign-in OTP code to {Recipient}", to);
                    return false;
                }

                _logger.LogDebug("Sign-in OTP code sent successfully to {Recipient}", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending sign-in OTP code to {Recipient}", to);
                return false;
            }
        }
    }
}