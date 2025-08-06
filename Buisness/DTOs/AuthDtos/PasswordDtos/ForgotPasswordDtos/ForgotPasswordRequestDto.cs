using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos
{
    /// <summary>
    /// Data Transfer Object for handling forgot password requests.
    /// <para>
    /// This class is typically used to map incoming forgot password commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class ForgotPasswordRequestDto : RequestDtoBase
    {
        // COMMAND PROPERTIES
        /// <summary>
        /// Gets or sets the user type ID (e.g., Admin, Staff, Student). (COMMAND PROPERTY)
        /// </summary>
        public byte UserTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the recovery method ID (e.g., 1 for email, 2 for phone). (COMMAND PROPERTY)
        /// </summary>
        public byte RecoveryMethodId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the user's email address for password recovery. (COMMAND PROPERTY)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone country code for password recovery. (COMMAND PROPERTY)
        /// </summary>
        public string PhoneCountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone number for password recovery. (COMMAND PROPERTY)
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;


        // INTERNAL USAGE PROPERTIES

        /// <summary>
        /// Gets or sets the recovery token generated for the forgot password process (internal usage).
        /// </summary>
        public string RecoveryToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's unique identifier (INTERNAL USAGE PROPERTY).
        /// </summary>
        public Guid UserUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the recovery session unique identifier (INTERNAL USAGE PROPERTY).
        /// </summary>
        public Guid RecoverySessionUuid { get; set; } = Guid.Empty;
    }
}
