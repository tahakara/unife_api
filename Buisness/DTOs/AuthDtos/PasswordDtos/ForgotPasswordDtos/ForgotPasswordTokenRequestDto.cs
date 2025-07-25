using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos
{
    public class ForgotPasswordRecoveryTokenRequestDto : DtoBase
    {
        // For Command
        public string? RecoveryToken { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
        public string? ConfirmPassword { get; set; } = string.Empty;


        // For Internal
        public Guid UserUuid { get; set; } = Guid.Empty;
        public Guid RecoverySessionUuid { get; set; } = Guid.Empty;
        public byte UserTypeId { get; set; } = 0; // 0: Student, 1: Staff, 2: Admin
    }
}
