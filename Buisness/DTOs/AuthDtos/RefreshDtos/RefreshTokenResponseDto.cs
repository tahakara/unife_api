using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.RefreshDtos
{
    public class RefreshTokenResponseDto : DtoBase, IDtoBase
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string SessionUuid { get; set; } = string.Empty;
        public string UserUuid { get; set; } = string.Empty;
        public byte UserTypeId { get; set; } = 0;
    }
}
