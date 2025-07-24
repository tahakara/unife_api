using Core.Utilities.MessageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Common
{
    /// <summary>
    /// CQRSMessages provides a set of standardized CQRS message templates.
    /// </summary>
    public class CQRSLogMessages : IMessageUtility
    {
        public static string Unknown { get; } = "Unknown";
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




    public class CQRSResponseMessages : IMessageUtility
    {
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
