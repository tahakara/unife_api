using Core.Utilities.MessageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.Common
{
    /// <summary>
    /// ValidationMessages provides a set of standardized validation message templates.
    /// </summary>
    public sealed class ValidationMessages : IMessageUtility
    {
        // Attention use {PropertyName} for property names in the messages.
        // FluentValidation will automatically replace {PropertyName} with the actual property name.
        // Also parameter

        // General
        public static string NotEmptyFormat(string PropertyName) => "{PropertyName} cannot be empty.";
        public static string NotNullFormat(string PropertyName) => "{PropertyName} cannot be null.";
        public static string RequiredFormat(string PropertyName) => "{PropertyName} is required.";
        public static string InvalidFormat(string PropertyName) => "{PropertyName} is in an invalid format.";
        public static string InvalidByte(string PropertyName)
            => $"{PropertyName} must be a valid byte value (0-255).";
        public static string InvalidBoolean(string PropertyName)
            => $"{PropertyName} must be a valid boolean value (true or false).";

        // JWT
        public static string InvalidJWTBeararTokenFormat(string PropertyName)
            => $"{PropertyName} must be a valid Bearer token, starting with 'Bearer ' followed by a valid token.";
        public static string InvalidJWTFormat(string PropertyName)
            => $"{PropertyName} must be a valid JWT token with three parts separated by dots ('.'). Each part must be a base64url encoded string.";

        // Comparison
        public static string MissmatchedValuesFormat(string propertyName1, string propertyName2)
            => $"{propertyName1} and {propertyName2} must match.";

        public static string GreaterThanZeroFormat(string PropertyName)
            => $"{PropertyName} must be greater than zero.";

        // Range
        public static string MinLengthFormat(string PropertyName, int minLength)
            => $"{PropertyName} must be at least {minLength} characters long.";
        public static string MaxLengthFormat(string PropertyName, int maxLength) 
            => $"{PropertyName} cannot exceed {maxLength} characters.";
        public static string LengthBetweenFormat(string PropertyName, int minLength, int maxLength)
            => $"{PropertyName} must be between {minLength} and {maxLength} characters long.";


        // Contains
        public static string MustContainUppercase(string PropertyName)
            => $"{PropertyName} must contain at least one uppercase letter (A–Z).";
        public static string MustContainLowercase(string PropertyName)
            => $"{PropertyName} must contain at least one lowercase letter (a–z).";
        public static string MustContainDigit(string PropertyName)
            => $"{PropertyName} must contain at least one digit (0–9).";
        public static string MustContainSpecialChars(string PropertyName, string allowedChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~")
            => $"{PropertyName} must contain at least one special character from the following: {allowedChars}";


        // Credential Formats
        public static string EmailFormat(string PropertyName)
            => $"{PropertyName} must be a valid email address.";
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

        public static string PhoneCountryCodeFormat(string PropertyName)
            => $"{PropertyName} must be a valid phone country code, starting with '+' and followed by digits.";
        public static string PhoneNumberFormat(string PropertyName)
            => $"{PropertyName} must be a valid phone number. It can include spaces, dashes, or parentheses for formatting.";

        public static string EitherEmailOrPhoneCredential(string emailPropertyName, string phoneCountryCodePropertyName, string phonePropertyName)
            => $"Either {emailPropertyName} or {phoneCountryCodePropertyName} {phonePropertyName} must be provided, but not both. If one is provided, the other must be null or empty.";

        // Password
        public static string PasswordMustBeAValidFormat(string PropertyName)
            => $"{PropertyName} must be a valid password. It must contain at least one uppercase letter, one lowercase letter, one digit, and one special character. The length must be between 8 and 64 characters.";
        
    }
}
