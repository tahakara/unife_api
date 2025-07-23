using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos
{
    public class LogoutOthersRequestDto : DtoBase
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
