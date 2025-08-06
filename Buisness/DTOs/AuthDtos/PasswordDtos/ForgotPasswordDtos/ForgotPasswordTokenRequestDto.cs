using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos
{
    /// <summary>
    /// Data Transfer Object for handling password reset requests using a recovery token.
    /// <para>
    /// This class is typically used to map incoming reset password commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class ForgotPasswordRecoveryTokenRequestDto : RequestDtoBase
    {
        // COMMAND PROPERTIES
        /// <summary>
        /// Gets or sets the recovery token provided by the user for password reset. (COMMAND PROPERTY)
        /// </summary>
        public string? RecoveryToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the new password to be set for the user. (COMMAND PROPERTY)
        /// </summary>
        public string? NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirmation of the new password. (COMMAND PROPERTY)
        /// </summary>
        public string? ConfirmPassword { get; set; } = string.Empty;


        // INTERNAL USAGE PROPERTIES
        /// <summary>
        /// Gets or sets the user's unique identifier (INTERNAL USAGE PROPERTIES).
        /// </summary>
        public Guid UserUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the recovery session unique identifier (INTERNAL USAGE PROPERTIES).
        /// </summary>
        public Guid RecoverySessionUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the user type ID (INTERNAL USAGE PROPERTIES). 0: Student, 1: Staff, 2: Admin.
        /// </summary>
        public byte UserTypeId { get; set; } = 0;
    }
}
