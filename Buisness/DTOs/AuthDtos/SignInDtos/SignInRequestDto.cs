using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.SignInDtos
{
    /// <summary>
    /// Data Transfer Object for handling sign-in requests.
    /// <para>
    /// This class is typically used to map incoming sign-in commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class SignInRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the user type ID (e.g., Admin, Staff, Student). (COMMAND PROPERTY)
        /// </summary>
        public byte UserTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the user's email address for sign-in. (COMMAND PROPERTY)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone country code for sign-in. (COMMAND PROPERTY)
        /// </summary>
        public string PhoneCountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone number for sign-in. (COMMAND PROPERTY)
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password for sign-in. (COMMAND PROPERTY)
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's unique identifier. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public Guid UserUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the session unique identifier. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public Guid SessionUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the OTP type identifier. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public byte OtpTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the OTP code for sign-in verification. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public string OtpCode { get; set; } = string.Empty;
    }
}
