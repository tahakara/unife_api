using Core.Utilities.MessageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Common
{
    /// <summary>
    /// Provides a set of standardized CQRS log message templates for process tracking and error reporting.
    /// </summary>
    public class CQRSLogMessages : IMessageUtility
    {
        /// <summary>
        /// Gets the default message for an unknown state.
        /// </summary>
        public static string Unknown { get; } = "Unknown";

        /// <summary>
        /// Gets the default message for a successful operation.
        /// </summary>
        public static string Success { get; } = "Success";

        /// <summary>
        /// Generates a log message indicating that a process has started.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process start.</returns>
        public static string ProccessStarted(string processName, object? additionalData = null)
        {
            if (additionalData == null)
                return $"{processName} process has started.";

            string additionalInfo;
            if (additionalData is string str)
            {
                additionalInfo = str;
            }
            else if (additionalData is IEnumerable<object> list && !(additionalData is string))
            {
                additionalInfo = string.Join(", ", list.Select(x => x?.ToString()));
            }
            else
            {
                additionalInfo = System.Text.Json.JsonSerializer.Serialize(additionalData);
            }

            return $"{processName} process has started." + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }

        /// <summary>
        /// Generates a log message indicating that a process has completed successfully.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process completion.</returns>
        public static string ProccessCompleted(string processName, object? additionalData = null)
        {
            if (additionalData == null)
                return $"{processName} process has completed successfully.";
            string additionalInfo;
            if (additionalData is string str)
            {
                additionalInfo = str;
            }
            else if (additionalData is IEnumerable<object> list && !(additionalData is string))
            {
                additionalInfo = string.Join(", ", list.Select(x => x?.ToString()));
            }
            else
            {
                additionalInfo = System.Text.Json.JsonSerializer.Serialize(additionalData);
            }
            return $"{processName} process has completed successfully." + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }

        /// <summary>
        /// Generates a log message indicating that a process has failed.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process failure.</returns>
        public static string ProccessFailed(string processName, string? errorMessage, object? additionalData = null)
        {
            var error = string.IsNullOrWhiteSpace(errorMessage) ? "Unknown" : errorMessage;
            if (additionalData == null)
                return $"{processName} process has failed. Error: {error}";
            string additionalInfo;
            if (additionalData is string str)
            {
                additionalInfo = str;
            }
            else if (additionalData is IEnumerable<object> list && !(additionalData is string))
            {
                additionalInfo = string.Join(", ", list.Select(x => x?.ToString()));
            }
            else
            {
                additionalInfo = System.Text.Json.JsonSerializer.Serialize(additionalData);
            }
            return $"{processName} process has failed. Error: {error}" + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }
    }
}
