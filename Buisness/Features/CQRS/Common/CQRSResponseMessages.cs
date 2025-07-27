using Core.Utilities.MessageUtility;
using System.Collections.Generic;
using System.Linq;

namespace Buisness.Features.CQRS.Common
{
    /// <summary>
    /// Provides standardized response message templates for CQRS operations.
    /// </summary>
    public class CQRSResponseMessages : IMessageUtility
    {
        /// <summary>
        /// Generates a success message for a completed process.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted success message.</returns>
        public static string Success(string processName, object? additionalData = null)
        {
            if (additionalData == null)
                return $"{processName} completed successfully.";
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
            return $"{processName} completed successfully." + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }

        /// <summary>
        /// Generates a failure message for a process that did not complete successfully.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="errorMessage">The error message describing the failure. Defaults to "Unknown".</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted failure message.</returns>
        public static string Fail(string processName, string? errorMessage = "Unknown", object? additionalData = null)
        {
            var error = string.IsNullOrWhiteSpace(errorMessage) ? "Unknown" : errorMessage;
            if (additionalData == null)
                return $"{processName} failed. Error: {error}";
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
            return $"{processName} failed. Error: {error}" + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }

        /// <summary>
        /// Generates an error message for a process that encountered an error.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="errorMessage">The error message describing the error. Defaults to "Unknown".</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted error message.</returns>
        public static string Error(string processName, string? errorMessage = "Unknown", object? additionalData = null)
        {
            var error = string.IsNullOrWhiteSpace(errorMessage) ? "Unknown" : errorMessage;
            if (additionalData == null)
                return $"{processName} encountered an error. Error: {error}";
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
            return $"{processName} encountered an error. Error: {error}" + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }
    }
}
