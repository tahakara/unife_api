using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos
{
    /// <summary>
    /// Data Transfer Object for changing a user's password.
    /// <para>
    /// This class is typically used to map incoming change password commands from the client.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in this class for internal processing.
    /// </para>
    /// </summary>
    public class ChangePasswordRequestDto : RequestDtoBase
    {
        /// <summary>
        /// Gets or sets the access token used to identify the user whose password will be changed. (COMMAND PROPERTY)
        /// </summary>
        public string? AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's current password. (COMMAND PROPERTY)
        /// </summary>
        public string? OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the new password to be set for the user. (COMMAND PROPERTY)
        /// </summary>
        public string? NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirmation of the new password. (COMMAND PROPERTY)
        /// </summary>
        public string? ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to log out other sessions after the password change. (COMMAND PROPERTY)
        /// </summary>
        public bool LogoutOtherSessions { get; set; } = true;
    }
}
