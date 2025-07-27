using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.RefreshDtos
{
    /// <summary>
    /// Data Transfer Object for the response of a refresh token operation.
    /// <para>
    /// Contains the new refresh token, access token, session and user identifiers, and user type information.
    /// </para>
    /// </summary>
    public class RefreshTokenResponseDto : ResponseDtoBase
    {
        /// <summary>
        /// Gets or sets the new refresh token issued to the client.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the new access token issued to the client.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the session associated with the tokens.
        /// </summary>
        public string SessionUuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the user associated with the tokens.
        /// </summary>
        public string UserUuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user type identifier (e.g., Admin, Staff, Student).
        /// </summary>
        public byte UserTypeId { get; set; } = 0;
    }
}
