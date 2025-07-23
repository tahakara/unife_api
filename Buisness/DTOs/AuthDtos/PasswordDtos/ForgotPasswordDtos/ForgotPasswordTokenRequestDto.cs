using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos
{
    public class ForgotPasswordRecoveryTokenRequestDto : DtoBase
    {
        public string? RecoveryToken { get; set; } = string.Empty;
    }
}
