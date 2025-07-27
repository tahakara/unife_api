using System;

namespace Buisness.Helpers.Common.HelperEnums
{
    /// <summary>
    /// Specifies the mode for blacklisting user sessions.
    /// </summary>
    public enum BlacklistMode
    {
        /// <summary>
        /// Blacklist only the current session.
        /// </summary>
        Single,

        /// <summary>
        /// Blacklist all sessions for the user.
        /// </summary>
        All,

        /// <summary>
        /// Blacklist all sessions except the current one.
        /// </summary>
        AllExceptOne
    }
}
