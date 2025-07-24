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
        // General
        public static string NotEmptyFormat(string propertyName) => "{propertyName} cannot be empty.";
        public static string NotNullFormat(string propertyName) => "{propertyName} cannot be null.";
        public static string RequiredFormat(string propertyName) => "{propertyName} is required.";
        public static string InvalidFormat(string propertyName) => "{propertyName} is in an invalid format.";
        public static string InvalidByte(string propertyName)
            => $"{propertyName} must be a valid byte value (0-255).";

        // JWT
        public static string InvalidJWTBeararTokenFormat(string propertyName)
            => $"{propertyName} must be a valid Bearer token, starting with 'Bearer ' followed by a valid token.";
        public static string InvalidJWTFormat(string propertyName)
            => $"{propertyName} must be a valid JWT token with three parts separated by dots ('.'). Each part must be a base64url encoded string.";

        // Comparison
        public static string MissmatchedValuesFormat(string propertyName1, string propertyName2)
            => $"{propertyName1} and {propertyName2} must match.";

        public static string GreaterThanZeroFormat(string propertyName)
            => $"{propertyName} must be greater than zero.";

        // Range
        public static string MinLengthFormat(string propertyName, int minLength)
            => $"{propertyName} must be at least {minLength} characters long.";
        public static string MaxLengthFormat(string propertyName, int maxLength) 
            => $"{propertyName} cannot exceed {maxLength} characters.";
        public static string LengthBetweenFormat(string propertyName, int minLength, int maxLength)
            => $"{propertyName} must be between {minLength} and {maxLength} characters long.";


        // Contains
        public static string MustContainUppercase(string propertyName)
            => $"{propertyName} must contain at least one uppercase letter (A–Z).";
        public static string MustContainLowercase(string propertyName)
            => $"{propertyName} must contain at least one lowercase letter (a–z).";
        public static string MustContainDigit(string propertyName)
            => $"{propertyName} must contain at least one digit (0–9).";
        public static string MustContainSpecialChars(string propertyName, string allowedChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~")
            => $"{propertyName} must contain at least one special character from the following: {allowedChars}";


        // Credential Formats
        public static string EmailFormat(string propertyName)
            => $"{propertyName} must be a valid email address.";
        public static string NotAccepedEmailFormat(string propertyName)
        {
            var acceptedDomains = new[]
            {
                ".edu", ".org", ".school", ".academy", ".college", ".university",
                ".courses", ".study", ".institute", ".training", ".education",
                ".k12", ".gov", ".govt", ".gouv", ".go", ".ac"
            };

            string domains = string.Join(", ", acceptedDomains);

            return $"{propertyName} must be a valid email address ending with one of the accepted domains. Accepted domains: {domains}.";
        }

        public static string PhoneCountryCodeFormat(string propertyName)
            => $"{propertyName} must be a valid phone country code, starting with '+' and followed by digits.";
        public static string PhoneNumberFormat(string propertyName)
            => $"{propertyName} must be a valid phone number. It can include spaces, dashes, or parentheses for formatting.";

        public static string EitherEmailOrPhoneCredential(string emailPropertyName, string phoneCountryCodePropertyName, string phonePropertyName)
            => $"Either {emailPropertyName} or {phoneCountryCodePropertyName} {phonePropertyName} must be provided, but not both. If one is provided, the other must be null or empty.";

        // Password
        public static string PasswordMustBeAValidFormat(string propertyName)
            => $"{propertyName} must be a valid password. It must contain at least one uppercase letter, one lowercase letter, one digit, and one special character. The length must be between 8 and 64 characters.";
        
    }
}
