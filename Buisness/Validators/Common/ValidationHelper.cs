using System.Text.RegularExpressions;

namespace Buisness.Validators.Common
{
    /// <summary>
    /// Provides static helper methods for validating various common data formats such as JWT tokens, emails, phone numbers, names, and more.
    /// </summary>
    public static partial class ValidationHelper
    {
        /// <summary>
        /// Validates a JWT Bearer token format (with "Bearer" prefix).
        /// </summary>
        /// <param name="token">The token string to validate.</param>
        /// <returns>True if the token matches the expected Bearer JWT format; otherwise, false.</returns>
        public static bool BeAValidJWTBeararToken(string? token)
        {
            if (token == null) return false;
            return JwtBearerRegex().IsMatch(token);
        }

        /// <summary>
        /// Validates a JWT token format (without "Bearer" prefix).
        /// </summary>
        /// <param name="token">The token string to validate.</param>
        /// <returns>True if the token matches the expected JWT format; otherwise, false.</returns>
        public static bool BeAValidJWTToken(string? token)
        {
            if (token == null) return false;
            return JwtTokenRegex().IsMatch(token);
        }

        /// <summary>
        /// Validates a UUID (Universally Unique Identifier) format.
        /// </summary>
        /// <param name="uuid">The UUID string to validate.</param>
        /// <returns>True if the string is a valid UUID or is null/empty; otherwise, false.</returns>
        public static bool BeAValidUuid(string? uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return true;
            return Guid.TryParse(uuid, out _);
        }

        /// <summary>
        /// Checks if the provided string contains at least one lowercase letter.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Returns true if the string contains at least one lowercase letter, otherwise false.</returns>

        public static bool ContainLowercase(string? str)
        {
            return !string.IsNullOrEmpty(str) && str.Any(char.IsLower);
        }

        /// <summary>
        /// Checks if the provided string contains at least one uppercase letter.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Returns true if the string contains at least one uppercase letter, otherwise false.</returns>

        public static bool ContainUppercase(string? str)
        {
            return !string.IsNullOrEmpty(str) && str.Any(char.IsUpper);
        }

        /// <summary>
        /// Checks if the provided string contains at least one digit.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Returns true if the string contains at least one digit, otherwise false.</returns>
        public static bool ContainDigit(string? str)
        {
            return !string.IsNullOrEmpty(str) && str.Any(char.IsDigit);
        }

        /// <summary>
        /// Checks if the provided string contains at least one special character (non-alphanumeric and non-whitespace).
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Returns true if the string contains at least one special character, otherwise false.</returns>

        public static bool ContainAsciiSpecialCharacter(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            const string allowedSpecials = "!\"#$%&'()*+,-./:;<=>?@[\\]^_{|}~";

            return str.Any(ch => allowedSpecials.Contains(ch));
        }

        /// <summary>
        /// Checks if the provided string is fully uppercase.
        /// </summary>
        /// <param name="name">The string to check.</param>
        /// <returns>True if the string is not null or empty and all characters are uppercase; otherwise, false.</returns>
        public static bool BeFullUppercase(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return name == name.ToUpper();
        }

        /// <summary>
        /// Validates if the provided byte value is between 1 and 255.
        /// </summary>
        public static bool BeAValidByte(byte? value)
        {
            if (!value.HasValue) return true; // Null is considered valid
            return value >= 1 && value <= 255;
        }


        /// <summary>
        /// Validates whether the provided URL is a valid absolute HTTP or HTTPS URL.
        /// </summary>
        /// <param name="url">The URL string to validate.</param>
        /// <returns>
        /// True if the URL is null, empty, or a valid absolute HTTP/HTTPS URL; otherwise, false.
        /// </returns>
        public static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return true;
            return Uri.TryCreate(url, UriKind.Absolute, out var result)
                   && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Validates an email address to ensure it is in a valid format and belongs to an accepted domain.
        /// Accepted domains include .edu, .org, .school, .academy, .college, .university, .courses, .study, .institute, .training, .education, .k12, .gov, .govt, .gouv, .go, .ac and their subdomains.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email is valid and belongs to an accepted domain; otherwise, false.</returns>
        public static bool BeAValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email)) return true;

            bool isEmail = EmailRegex().IsMatch(email);
            bool isAcceptedDomain = AcceptedDomainRegex().IsMatch(email);

            return isEmail && isAcceptedDomain;
        }

        /// <summary>
        /// Validates a country code in the format of "+[1-9][0-9]{0,3}". (ITU-T E.164)
        /// </summary>
        /// <param name="countryCode">The country code string to validate.</param>
        /// <returns>True if the country code is valid or is null/empty; otherwise, false.</returns>
        public static bool BeAValidCountryCode(string? countryCode)
        {
            if (string.IsNullOrEmpty(countryCode)) return true;
            return CountryCodeRegex().IsMatch(countryCode);
        }

        /// <summary>
        /// Validates a phone number to ensure it is in a valid format. (ITU-T E.164)
        /// </summary>
        /// <param name="phoneNumber">The phone number string to validate.</param>
        /// <returns>True if the phone number is valid or is null/empty; otherwise, false.</returns>
        public static bool BeAValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return true;
            return PhoneNumberRegex().IsMatch(phoneNumber);
        }

        /// <summary>
        /// Checks if the provided password is valid according to predefined rules:
        /// - Contains at least one lowercase letter.
        /// - Contains at least one uppercase letter.
        /// - Contains at least one digit.
        /// - Contains at least one special character.
        /// - Must be between 8 and 100 characters long.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the password is valid according to the specified rules; otherwise, false.</returns>
        public static bool BeAValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            return PasswordRegex().IsMatch(password);
        }

        /// <summary>
        /// Checks if the provided first name is valid according to predefined rules:
        /// - Cannot be empty.
        /// - Must be between 1 and 100 characters.
        /// - Can only contain letters and spaces.
        /// </summary>
        /// <param name="firstName">The first name to validate.</param>
        /// <returns>True if the first name is valid; otherwise, false.</returns>
        public static bool BeAValidFirstName(string? firstName)
        {
            if (string.IsNullOrEmpty(firstName)) return false;
            return FirstNameRegex().IsMatch(firstName);
        }

        /// <summary>
        /// Checks if the provided middle name is valid according to predefined rules:
        /// - Must be between 0 and 100 characters.
        /// - Can only contain letters and spaces (if not null or empty).
        /// </summary>
        /// <param name="middleName">The middle name to validate.</param>
        /// <returns>True if the middle name is valid or is null/empty; otherwise, false.</returns>
        public static bool BeAValidMiddleName(string? middleName)
        {
            if (string.IsNullOrEmpty(middleName)) return true;
            return MiddleNameRegex().IsMatch(middleName);
        }

        /// <summary>
        /// Checks if the provided last name is valid according to predefined rules:
        /// - Cannot be empty.
        /// - Must be between 1 and 100 characters.
        /// - Can only contain letters and spaces.
        /// </summary>
        /// <param name="lastName">The last name to validate.</param>
        /// <returns>True if the last name is valid; otherwise, false.</returns>
        public static bool BeAValidLastName(string? lastName)
        {
            if (string.IsNullOrEmpty(lastName)) return false;
            return LastNameRegex().IsMatch(lastName);
        }

        /// <summary>
        /// Checks if the provided OTP code is a valid 6-digit number.
        /// </summary>
        /// <param name="otpCode">The OTP code to validate.</param>
        /// <returns>True if the OTP code is a valid 6-digit number; otherwise, false.</returns>
        public static bool BeA6DigitValidOtpCode(string? otpCode)
        {
            if (string.IsNullOrEmpty(otpCode)) return false;
            return OtpCodeRegex().IsMatch(otpCode);
        }

        /// <summary>
        /// Checks if the provided string is a single numeric digit.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <returns>True if the string is a single digit and can be parsed as an integer; otherwise, false.</returns>
        public static bool BeNumeric(string? value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (!SingleDigitRegex().IsMatch(value)) return false;
            return int.TryParse(value, out _);
        }

        /// <summary>
        /// Validates if the provided year is between 1000 and the current year.
        /// </summary>
        /// <param name="year">The year to validate.</param>
        /// <returns>True if the year is valid or is null; otherwise, false.</returns>
        public static bool BeAValidYear(int? year)
        {
            if (!year.HasValue) return true;
            return year >= 1000 && year <= DateTime.Now.Year;
        }

        /// <summary>
        /// Validates if the provided boolean value is either true or false.
        /// </summary>
        /// <param name="value">The boolean value to validate.</param>
        /// <returns>True if the value is true, false, or null; otherwise, false.</returns>
        public static bool BeAValidBoolean(bool? value)
        {
            if (!value.HasValue) return true; // Null is considered valid
            return value.Value == true || value.Value == false;
        }

        /// <summary>
        /// Validates if the provided string represents a valid year (4 digits and within valid range).
        /// </summary>
        /// <param name="year">The year string to validate.</param>
        /// <returns>True if the string is a valid year; otherwise, false.</returns>
        public static bool BeAStringValidYear(string? year)
        {
            if (string.IsNullOrEmpty(year)) return false;

            if (!YearRegex().IsMatch(year)) return false;

            if (int.TryParse(year, out int yearValue))
                return BeAValidYear(yearValue);

            return false;
        }

        /// <summary>
        /// Validates a university name, allowing Turkish characters, spaces, hyphens, and periods.
        /// </summary>
        /// <param name="name">The university name to validate.</param>
        /// <returns>True if the university name is valid; otherwise, false.</returns>
        public static bool BeAValidUniversityName(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return UniversityNameRegex().IsMatch(name);
        }

        /// <summary>
        /// Validates a university code (uppercase letters, numbers, hyphens).
        /// </summary>
        /// <param name="code">The university code to validate.</param>
        /// <returns>True if the university code is valid or is null/empty; otherwise, false.</returns>
        public static bool BeAValidUniversityCode(string? code)
        {
            if (string.IsNullOrEmpty(code)) return true;
            return UniversityCodeRegex().IsMatch(code);
        }

        // GENERATED REGEX METHODS

        /// <summary>
        /// Gets a compiled regex for validating JWT Bearer tokens.
        /// </summary>
        [GeneratedRegex(@"^Bearer [A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+$")]
        private static partial Regex JwtBearerRegex();

        /// <summary>
        /// Gets a compiled regex for validating JWT tokens.
        /// </summary>
        [GeneratedRegex(@"^[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+$")]
        private static partial Regex JwtTokenRegex();

        /// <summary>
        /// Gets a compiled regex for validating email addresses.
        /// </summary>
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();

        /// <summary>
        /// Gets a compiled regex for validating accepted email domains.
        /// </summary>
        [GeneratedRegex(@"\.(edu|org|school|academy|college|university|courses|study|institute|training|education|k12|gov|govt|gouv|go|ac|edu|school|college|university|org)\.[a-z]{2}$|\.(edu|org|school|academy|college|university|courses|study|institute|training|education|k12|gov)$")]
        private static partial Regex AcceptedDomainRegex();

        /// <summary>
        /// Gets a compiled regex for validating country codes.
        /// </summary>
        [GeneratedRegex(@"^\+[1-9][0-9]{0,3}$")]
        private static partial Regex CountryCodeRegex();

        /// <summary>
        /// Gets a compiled regex for validating phone numbers.
        /// </summary>
        [GeneratedRegex(@"^(\(?[0-9]{1,4}\)?([ \-]?[0-9]{1,4})*)$")]
        private static partial Regex PhoneNumberRegex();

        /// <summary>
        /// Gets a compiled regex for validating passwords.
        /// </summary>
        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9\s]).{8,100}$")]
        private static partial Regex PasswordRegex();

        /// <summary>
        /// Gets a compiled regex for validating first names.
        /// </summary>
        [GeneratedRegex(@"^[A-Za-z\s]{1,100}$")]
        private static partial Regex FirstNameRegex();

        /// <summary>
        /// Gets a compiled regex for validating middle names.
        /// </summary>
        [GeneratedRegex(@"^[A-Za-z\s]{0,100}$")]
        private static partial Regex MiddleNameRegex();

        /// <summary>
        /// Gets a compiled regex for validating last names.
        /// </summary>
        [GeneratedRegex(@"^[A-Za-z\s]{1,100}$")]
        private static partial Regex LastNameRegex();

        /// <summary>
        /// Gets a compiled regex for validating 6-digit OTP codes.
        /// </summary>
        [GeneratedRegex(@"^\d{6}$")]
        private static partial Regex OtpCodeRegex();

        /// <summary>
        /// Gets a compiled regex for validating single digit numbers.
        /// </summary>
        [GeneratedRegex(@"^\d$")]
        private static partial Regex SingleDigitRegex();

        /// <summary>
        /// Gets a compiled regex for validating 4-digit years.
        /// </summary>
        [GeneratedRegex(@"^\d{4}$")]
        private static partial Regex YearRegex();

        /// <summary>
        /// Gets a compiled regex for validating university names (including Turkish characters).
        /// </summary>
        [GeneratedRegex(@"^[a-zA-Z0-9ÇĞİÖŞÜçğıöşü\s\-\.]+$")]
        private static partial Regex UniversityNameRegex();

        /// <summary>
        /// Gets a compiled regex for validating university codes.
        /// </summary>
        [GeneratedRegex(@"^[A-Z0-9\-]+$")]
        private static partial Regex UniversityCodeRegex();
    }
}
