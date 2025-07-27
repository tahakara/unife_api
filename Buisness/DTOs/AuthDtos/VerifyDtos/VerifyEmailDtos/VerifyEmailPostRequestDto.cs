using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos
{
    /// <summary>
    /// Data Transfer Object for verifying a user's email address using an OTP code.
    /// <para>
    /// This class is typically used to map incoming verify-email commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class VerifyEmailRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the access token used to identify the user whose email is being verified. (COMMAND PROPERTY)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the OTP code provided by the user for email verification. (COMMAND PROPERTY)
        /// </summary>
        public string OtpCode { get; set; } = string.Empty;
    }
}
