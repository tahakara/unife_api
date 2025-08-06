using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;

namespace Buisness.DTOs.AuthDtos.SignUpDtos
{
    /// <summary>
    /// Data Transfer Object for the response of a user sign-up operation.
    /// <para>
    /// Contains user and university information returned after a successful sign-up.
    /// </para>
    /// </summary>
    public class SignUpResponseDto : ResponseDtoBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the university associated with the user.
        /// </summary>
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UniversityUuid { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's middle name.
        /// </summary>
        public string? MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone country code.
        /// </summary>
        public string PhoneCountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
