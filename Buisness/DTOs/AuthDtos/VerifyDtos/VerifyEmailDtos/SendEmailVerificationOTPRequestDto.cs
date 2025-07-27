using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos
{
    /// <summary>
    /// Data Transfer Object for sending an email verification OTP request.
    /// <para>
    /// This class is typically used to map incoming send-email-verification-OTP commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class SendEmailVerificationOTPRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the access token used to identify the user for whom the email verification OTP will be sent. (COMMAND PROPERTY)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        // INTERNAL USAGE PROPERTIES
        /// <summary>
        /// Gets or sets the user's unique identifier. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public Guid UserUid { get; set; } = Guid.Empty;
    }
}
