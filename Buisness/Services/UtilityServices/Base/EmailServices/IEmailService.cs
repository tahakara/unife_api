using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using System.Net;

namespace Buisness.Services.UtilityServices.Base.EmailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendSignInOtpCode(string to, string otp, string subject = "Authentication Code for Secure Sign-In", bool isHtml = true);
        Task<bool> SendSiginCompleteMail(string ipAddress, string userAgent, string emailAddress, string firstName, string middleName, string lastName);
        Task<bool> SendForgotPasswordEmailAsync(string to, string recoveryToken);
    }
}