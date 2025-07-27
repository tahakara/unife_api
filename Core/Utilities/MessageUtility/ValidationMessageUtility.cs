namespace Core.Utilities.MessageUtility
{
    public abstract class ValidationMessageUtility : IMessageUtility
    {
        #region General

        /// <summary>
        /// Returns a message indicating that the property cannot be empty.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string NotEmptyFormat(string PropertyName) => "{PropertyName} cannot be empty.";

        /// <summary>
        /// Returns a message indicating that the property cannot be null.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string NotNullFormat(string PropertyName) => "{PropertyName} cannot be null.";

        /// <summary>
        /// Returns a message indicating that the property is required.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string RequiredFormat(string PropertyName) => "{PropertyName} is required.";

        /// <summary>
        /// Returns a message indicating that the property is in an invalid format.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string InvalidFormat(string PropertyName) => "{PropertyName} is in an invalid format.";

        /// <summary>
        /// Returns a message indicating that the property must be a valid byte value.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string InvalidByte(string PropertyName)
            => $"{PropertyName} must be a valid byte value (0-255).";

        /// <summary>
        /// Returns a message indicating that the property must be a valid boolean value.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string InvalidBoolean(string PropertyName)
            => $"{PropertyName} must be a valid boolean value (true or false).";

        #endregion

        #region Comparison

        /// <summary>
        /// Returns a message indicating that two property values must match.
        /// </summary>
        /// <param name="propertyName1">The name of the first property.</param>
        /// <param name="propertyName2">The name of the second property.</param>
        /// <returns>The formatted message.</returns>
        public static string MissmatchedValuesFormat(string propertyName1, string propertyName2)
            => $"{propertyName1} and {propertyName2} must match.";

        /// <summary>
        /// Returns a message indicating that the property must be greater than zero.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string GreaterThanZeroFormat(string PropertyName)
            => $"{PropertyName} must be greater than zero.";

        #endregion

        #region Range

        /// <summary>
        /// Returns a message indicating that the property must be at least a minimum length.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <returns>The formatted message.</returns>
        public static string MinLengthFormat(string PropertyName, int minLength)
            => $"{PropertyName} must be at least {minLength} characters long.";

        /// <summary>
        /// Returns a message indicating that the property cannot exceed a maximum length.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The formatted message.</returns>
        public static string MaxLengthFormat(string PropertyName, int maxLength)
            => $"{PropertyName} cannot exceed {maxLength} characters.";

        /// <summary>
        /// Returns a message indicating that the property must be between a minimum and maximum length.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The formatted message.</returns>
        public static string LengthBetweenFormat(string PropertyName, int minLength, int maxLength)
            => $"{PropertyName} must be between {minLength} and {maxLength} characters long.";

        #endregion

        #region Contains

        /// <summary>
        /// Returns a message indicating that the property must contain at least one uppercase letter.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string MustContainUppercase(string PropertyName)
            => $"{PropertyName} must contain at least one uppercase letter (A–Z).";

        /// <summary>
        /// Returns a message indicating that the property must contain at least one lowercase letter.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string MustContainLowercase(string PropertyName)
            => $"{PropertyName} must contain at least one lowercase letter (a–z).";

        /// <summary>
        /// Returns a message indicating that the property must contain at least one digit.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string MustContainDigit(string PropertyName)
            => $"{PropertyName} must contain at least one digit (0–9).";

        /// <summary>
        /// Returns a message indicating that the property must contain at least one special character.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="allowedChars">The allowed special characters.</param>
        /// <returns>The formatted message.</returns>
        public static string MustContainSpecialChars(string PropertyName, string allowedChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~")
            => $"{PropertyName} must contain at least one special character from the following: {allowedChars}";

        #endregion
    }
}
