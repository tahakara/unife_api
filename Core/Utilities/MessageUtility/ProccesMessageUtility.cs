namespace Core.Utilities.MessageUtility
{
    public abstract class ProccesMessageUtility
    {
        /// <summary>
        /// Generates a log message indicating that a process has started.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="processType">The type of the proccess.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process start.</returns>
        public static string ProccessStarted(string processName, string processType, object? additionalData = null)
        {
            if (additionalData == null)
                return $"{processName} {processType} has started.";

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

            return $"{processName} {processType} has started." + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }

        /// <summary>
        /// Generates a log message indicating that a process has completed successfully.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="processType">The type of the proccess.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process completion.</returns>
        public static string ProccessCompleted(string processName, string processType, object? additionalData = null)
        {
            if (additionalData == null)
                return $"{processName} {processType} has completed successfully.";
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
            return $"{processName} {processType} has completed successfully." + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }

        /// <summary>
        /// Generates a log message indicating that a process has failed.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="processType">The type of the process.</param>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="additionalData">Optional additional data to include in the message.</param>
        /// <returns>A formatted log message for process failure.</returns>
        public static string ProccessFailed(string processName, string processType, string? errorMessage, object? additionalData = null)
        {
            var error = string.IsNullOrWhiteSpace(errorMessage) ? "Unknown" : errorMessage;
            if (additionalData == null)
                return $"{processName} {processType} has failed. Error: {error}";
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
            return $"{processName} {processType} has failed. Error: {error}" + (string.IsNullOrWhiteSpace(additionalInfo) ? "" : $" Additional Data: {additionalInfo}");
        }
    }
}
