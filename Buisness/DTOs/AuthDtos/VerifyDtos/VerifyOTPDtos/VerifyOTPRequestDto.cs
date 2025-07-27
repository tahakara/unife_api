using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos
{
    /// <summary>
    /// Data Transfer Object for verifying a user's OTP (One-Time Password) during authentication flows.
    /// <para>
    /// This class is typically used to map incoming verify-OTP commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class VerifyOTPRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the user type ID (e.g., Admin, Staff, Student). (COMMAND PROPERTY)
        /// </summary>
        public byte UserTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the unique identifier of the session associated with the OTP. (COMMAND PROPERTY)
        /// </summary>
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid SessionUuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user associated with the OTP. (COMMAND PROPERTY)
        /// </summary>
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid UserUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the OTP type identifier. (COMMAND PROPERTY)
        /// </summary>
        public byte OtpTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the OTP code provided by the user for verification. (COMMAND PROPERTY)
        /// </summary>
        public string OtpCode { get; set; } = string.Empty;
    }
}
