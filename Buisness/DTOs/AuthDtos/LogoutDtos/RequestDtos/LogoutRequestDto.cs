using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos
{
    /// <summary>
    /// Data Transfer Object for logging out the current user session.
    /// <para>
    /// This class is typically used to map incoming logout commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class LogoutRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the access token used to identify the user and the session to be logged out. (COMMAND PROPERTY)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}
