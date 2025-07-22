using Buisness.Concrete.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos
{
    public class ChangePasswordRequestDto : DtoBase
    {
        public string? AccessToken { get; set; } = string.Empty;
        public string? SessionUuid { get; set; } = string.Empty;
        public byte UserTypeId { get; set; } = 0;
        public string? UserUuid { get; set; } = string.Empty;
        public string? OldPassword { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
        public string? ConfirmPassword { get; set; } = string.Empty;

    }
}
