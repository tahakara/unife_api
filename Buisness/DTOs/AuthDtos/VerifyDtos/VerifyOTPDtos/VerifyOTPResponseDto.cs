using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;

namespace Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos
{
    /// <summary>
    /// Data Transfer Object for the response of a verify OTP (One-Time Password) operation.
    /// <para>
    /// Contains OTP type, user and session identifiers, and issued tokens after successful verification.
    /// </para>
    /// </summary>
    public class VerifyOTPResponseDto : ResponseDtoBase
    {   
        /// <summary>
        /// Gets or sets the OTP type identifier.
        /// </summary>
        public byte? OtpTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the unique identifier of the user associated with the OTP.
        /// </summary>
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UserUuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the session associated with the OTP.
        /// </summary>
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? SessionUuid { get; set; }

        /// <summary>
        /// Gets or sets the access token issued after successful OTP verification.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token issued after successful OTP verification.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
