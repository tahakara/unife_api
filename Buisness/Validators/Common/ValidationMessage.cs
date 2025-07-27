using Core.Utilities.MessageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.Common
{
    /// <summary>
    /// Provides a set of standardized validation message templates for use in validation logic.
    /// </summary>
    public sealed class ValidationMessages : IMessageUtility
    {
        // Attention use {PropertyName} for property names in the messages.
        // FluentValidation will automatically replace {PropertyName} with the actual property name.

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

        #region JWT

        /// <summary>
        /// Returns a message indicating that the property must be a valid Bearer token.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string InvalidJWTBeararTokenFormat(string PropertyName)
            => $"{PropertyName} must be a valid Bearer token, starting with 'Bearer ' followed by a valid token.";

        /// <summary>
        /// Returns a message indicating that the property must be a valid JWT token.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string InvalidJWTFormat(string PropertyName)
            => $"{PropertyName} must be a valid JWT token with three parts separated by dots ('.'). Each part must be a base64url encoded string.";

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

        #region Credential Formats

        /// <summary>
        /// Returns a message indicating that the property must be a valid email address.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string EmailFormat(string PropertyName)
            => $"{PropertyName} must be a valid email address.";

        /// <summary>
        /// Returns a message indicating that the property must be a valid email address ending with an accepted domain.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string NotAccepedEmailFormat(string PropertyName)
        {
            var acceptedDomains = new[]
            {
                ".edu", ".org", ".school", ".academy", ".college", ".university",
                ".courses", ".study", ".institute", ".training", ".education",
                ".k12", ".gov", ".govt", ".gouv", ".go", ".ac"
            };

            string domains = string.Join(", ", acceptedDomains);

            return $"{PropertyName} must be a valid email address ending with one of the accepted domains. Accepted domains: {domains}.";
        }

        /// <summary>
        /// Returns a message indicating that the property must be a valid phone country code.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string PhoneCountryCodeFormat(string PropertyName)
            => $"{PropertyName} must be a valid phone country code, starting with '+' and followed by digits.";

        /// <summary>
        /// Returns a message indicating that the property must be a valid phone number.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string PhoneNumberFormat(string PropertyName)
            => $"{PropertyName} must be a valid phone number. It can include spaces, dashes, or parentheses for formatting.";

        /// <summary>
        /// Returns a message indicating that either email or phone credentials must be provided.
        /// </summary>
        /// <param name="emailPropertyName">The name of the email property.</param>
        /// <param name="phoneCountryCodePropertyName">The name of the phone country code property.</param>
        /// <param name="phonePropertyName">The name of the phone number property.</param>
        /// <returns>The formatted message.</returns>
        public static string EitherEmailOrPhoneCredential(string emailPropertyName, string phoneCountryCodePropertyName, string phonePropertyName)
            => $"Either {emailPropertyName} or {phoneCountryCodePropertyName} {phonePropertyName} must be provided, but not both. If one is provided, the other must be null or empty.";

        #endregion

        #region Password

        /// <summary>
        /// Returns a message indicating that the property must be a valid password.
        /// </summary>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The formatted message.</returns>
        public static string PasswordMustBeAValidFormat(string PropertyName)
            => $"{PropertyName} must be a valid password. It must contain at least one uppercase letter, one lowercase letter, one digit, and one special character. The length must be between 8 and 64 characters.";

        #endregion
    }
}
