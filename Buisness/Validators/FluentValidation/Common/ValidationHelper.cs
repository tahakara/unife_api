using System.Text.RegularExpressions;

namespace Buisness.Validators.FluentValidation.Common
{
    public static class ValidationHelper
    {
        public static bool BeAValidJWTBeararToken(string? token)
        {
            if (token == null) return false;
            return Regex.IsMatch(token, @"^Bearer [A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+$");

        }

        public static bool BeAValidJWTToken(string? token)
        {
            if (token == null) return false;
            return Regex.IsMatch(token, @"^[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+$");
        }

        public static bool BeAValidUuid(string? uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return true;
            return Guid.TryParse(uuid, out _);
        }

        /// <summary>
        /// Validates an email address to ensure it is in a valid format and belongs to an accepted domain.
        /// Accepted Domains are .edu, .org, .school, .academy, .college, .university, .courses, .study, .institute, .training, .education, .k12, .gov, .govt, .gouv, .go, .ac
        /// and their subdomains like .edu.tr, .org.tr, etc.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool BeAValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email)) return true;
            bool isEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            bool isAcceptedDomain = Regex.IsMatch(email, @"\.(edu|org|school|academy|college|university|courses|study|institute|training|education|k12|gov|govt|gouv|go|ac|edu|school|college|university|org)\.[a-z]{2}$|\.(edu|org|school|academy|college|university|courses|study|institute|training|education|k12|gov)$");
            return isEmail && isAcceptedDomain;
        }

        /// <summary>
        /// Validates a country code in the format of "+[1-9][0-9]{0,3}".
        /// (ITU-T E.164)
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public static bool BeAValidCountryCode(string? countryCode)
        {
            if (string.IsNullOrEmpty(countryCode)) return true;
            return Regex.IsMatch(countryCode, @"^\+[1-9][0-9]{0,3}$");
        }

        /// <summary>
        /// Validates a phone number to ensure it is in a valid format.
        /// (ITU-T E.164)
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool BeAValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return true;
            return Regex.IsMatch(phoneNumber, @"^(\(?[0-9]{1,4}\)?([ \-]?[0-9]{1,4})*)$");
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
        /// <returns>Returns true if the password is valid according to the specified rules, otherwise false.</returns>

        public static bool BeAValidPassword(string? password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password)) return false;
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9\s]).{8,100}$");
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

            const string allowedSpecials = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

            return str.Any(ch => allowedSpecials.Contains(ch));
        }

        /// <summary>
        /// Checks if the provided first name is valid according to predefined rules:
        /// - Cannot be empty.
        /// - Must be between 1 and 100 characters.
        /// - Can only contain letters and spaces.
        /// </summary>
        /// <param name="firstName">The first name to validate.</param>
        /// <returns>Returns true if the first name is valid according to the specified rules, otherwise false.</returns>
        public static bool BeAValidFirstName(string? firstName)
        {
            if (string.IsNullOrEmpty(firstName)) return false;
            return Regex.IsMatch(firstName, @"^[A-Za-z\s]{1,100}$");
        }

        /// <summary>
        /// Checks if the provided middle name is valid according to predefined rules:
        /// - Must be between 0 and 100 characters.
        /// - Can only contain letters and spaces (if not null or empty).
        /// </summary>
        /// <param name="middleName">The middle name to validate.</param>
        /// <returns>Returns true if the middle name is valid according to the specified rules, otherwise false.</returns>
        public static bool BeAValidMiddleName(string? middleName)
        {
            if (string.IsNullOrEmpty(middleName)) return true; // Empty or null is allowed
            return Regex.IsMatch(middleName, @"^[A-Za-z\s]{0,100}$");
        }

        /// <summary>
        /// Checks if the provided last name is valid according to predefined rules:
        /// - Cannot be empty.
        /// - Must be between 1 and 100 characters.
        /// - Can only contain letters and spaces.
        /// </summary>
        /// <param name="lastName">The last name to validate.</param>
        /// <returns>Returns true if the last name is valid according to the specified rules, otherwise false.</returns>
        public static bool BeAValidLastName(string? lastName)
        {
            if (string.IsNullOrEmpty(lastName)) return false;
            return Regex.IsMatch(lastName, @"^[A-Za-z\s]{1,100}$");
        }


        /// <summary>
        /// Checks if the provided OTP code is a valid 6-digit number.
        /// </summary>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        public static bool BeA6DigitValidOtpCode(string? otpCode)
        {
            if (string.IsNullOrEmpty(otpCode)) return false;
            return Regex.IsMatch(otpCode, @"^\d{6}$");
        }

        public static bool BeAValidByte(byte? value)
        {
            if (!value.HasValue) return true; // Null is considered valid
            return value >= 1 && value <= 255;
        }


        public static bool BeNumeric(string? value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (!Regex.IsMatch(value, @"^\d$")) return false;
            return int.TryParse(value, out _);
        }
        public static bool BeAStringValidYear(string? year)
        {
            if (string.IsNullOrEmpty(year)) return false;
            
            if (!Regex.IsMatch(year, @"^\d{4}$")) return false;
            
            if (int.TryParse(year, out int yearValue))
            {
                return BeAValidYear(yearValue);
            }
            
            return false;
        }

        public static bool BeAStringGreaterThanZero(string? value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return int.TryParse(value, out int intValue) && intValue > 0;
        }
        // URL Validation
        public static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return true;
            return Uri.TryCreate(url, UriKind.Absolute, out var result)
                   && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }

        // University Name Validation (Turkish characters included)
        public static bool BeAValidUniversityName(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return Regex.IsMatch(name, @"^[a-zA-Z0-9ÇĞİÖŞÜçğıöşü\s\-\.]+$");
        }

        public static bool BeFullUppercase(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return name == name.ToUpper();
        }

        // University Code Validation (Uppercase letters, numbers, hyphens)
        public static bool BeAValidUniversityCode(string? code)
        {
            if (string.IsNullOrEmpty(code)) return true;
            return Regex.IsMatch(code, @"^[A-Z0-9\-]+$");
        }

        // Year Validation
        public static bool BeAValidYear(int? year)
        {
            if (!year.HasValue) return true;
            return year >= 1000 && year <= DateTime.Now.Year;
        }

    }
}