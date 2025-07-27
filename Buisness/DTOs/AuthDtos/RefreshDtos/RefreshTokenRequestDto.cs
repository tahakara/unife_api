using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.RefreshDtos
{
    /// <summary>
    /// Data Transfer Object for handling refresh token requests.
    /// <para>
    /// This class is typically used to map incoming refresh token commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class RefreshTokenRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the refresh token provided by the client. (COMMAND PROPERTY)
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the access token provided by the client. (COMMAND PROPERTY)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's unique identifier. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public Guid UserUuid { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the session unique identifier. (INTERNAL USAGE PROPERTY)
        /// </summary>
        public Guid SessionUuid { get; set; } = Guid.Empty;
    }
}
