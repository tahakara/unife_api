using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using Npgsql.TypeMapping;

namespace Buisness.DTOs.AuthDtos.SignUpDtos.Request
{
    /// <summary>
    /// Data Transfer Object for handling user sign-up requests.
    /// <para>
    /// This class is typically used to map incoming sign-up commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class SignUpRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the user type ID (e.g., Admin, Staff, Student). (COMMAND PROPERTY)
        /// </summary>
        public byte UserTypeId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the university unique identifier. (COMMAND PROPERTY)
        /// </summary>
        public Guid UniversityUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the user's first name. (COMMAND PROPERTY)
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's middle name. (COMMAND PROPERTY)
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's last name. (COMMAND PROPERTY)
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address. (COMMAND PROPERTY)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone country code. (COMMAND PROPERTY)
        /// </summary>
        public string PhoneCountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone number. (COMMAND PROPERTY)
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password. (COMMAND PROPERTY)
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
