using Core.Utilities.MessageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Base.Generic
{
    public class HelperLogMessages : ProccesMessageUtility, IMessageUtility
    {
        /// <summary>
        /// The process type name used in Helper Messages.
        /// </summary>
        public const string ProcessTypeName = "Helper";

        /// <summary>
        /// Gets the default message for an unknown state.
        /// </summary>
        public const string Unknown = "Unknown";

        /// <summary>
        /// Gets the default message for a successful operation.
        /// </summary>
        public const string Success = "Success";

        /// <summary>
        /// Generates a log message indicating that a HerperMessages process has started.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process start.</returns>
        public static string ProccessStarted(string processName, object? additionalData = null)
            => ProccessStarted(processName, ProcessTypeName, additionalData);

        /// <summary>
        /// Generates a log message indicating that a HerperMessages process has completed successfully.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process completion.</returns>
        public static string ProccessCompleted(string processName, object? additionalData = null)
            => ProccessCompleted(processName, ProcessTypeName, additionalData);

        /// <summary>
        /// Generates a log message indicating that a HerperMessages process has failed.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process failure.</returns>
        public static string ProccessFailed(string processName, string? errorMessage, object? additionalData = null)
            => ProccessFailed(processName, ProcessTypeName, errorMessage, additionalData);
    }
}
