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
    public sealed class ValidationMessages : ValidationMessageUtility, IMessageUtility
    {
        // Attention use {PropertyName} for property names in the messages.
        // FluentValidation will automatically replace {PropertyName} with the actual property name.

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
