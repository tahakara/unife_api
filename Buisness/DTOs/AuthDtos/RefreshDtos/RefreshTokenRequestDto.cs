using Buisness.Concrete.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.RefreshDtos
{
    public class RefreshTokenRequestDto : DtoBase
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string UserUuid { get; set; } = string.Empty;
        public string SessionUuid { get; set; } = string.Empty;
    }

    public class RefreshTokenResponseDto : DtoBase
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string UserUuid { get; set; } = string.Empty;
        public string SessionUuid { get; set; } = string.Empty;
    }
}
