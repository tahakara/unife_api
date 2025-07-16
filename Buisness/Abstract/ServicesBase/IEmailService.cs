using Buisness.Abstract.ServicesBase.Base;

namespace Buisness.Abstract.ServicesBase
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    }
}