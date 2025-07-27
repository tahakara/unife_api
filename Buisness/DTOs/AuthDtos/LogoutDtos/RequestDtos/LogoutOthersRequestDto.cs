using Buisness.DTOs.Base;

namespace Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos
{
    /// <summary>
    /// Data Transfer Object for logging out all user sessions except the current one.
    /// <para>
    /// This class is typically used to map incoming logout-others commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class LogoutOthersRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the access token used to identify the user and the current session to keep active. (COMMAND PROPERTY)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}
