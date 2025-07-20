namespace Buisness.Services.UtilityServices.Base.EmailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendSignInOtpCode(string to, string otp, string subject = "Authentication Code for Secure Sign-In", bool isHtml = true);
    }
}