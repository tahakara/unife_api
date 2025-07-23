using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos
{
    public class LogoutAllRequestDto : DtoBase
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
