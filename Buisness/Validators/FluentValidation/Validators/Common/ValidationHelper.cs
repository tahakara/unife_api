using System.Text.RegularExpressions;

namespace Buisness.Validators.FluentValidation.Validators.Common
{
    public static class ValidationHelper
    {
        public static bool BeAValidUuid(string? uuid)
        {
            if (string.IsNullOrEmpty(uuid)) return true;
            return Guid.TryParse(uuid, out _);
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

        // Email Validation (for future use)
        public static bool BeAValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email)) return true;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Phone Number Validation (for future use)
        public static bool BeAValidPhoneNumber(string? phone)
        {
            if (string.IsNullOrEmpty(phone)) return true;
            return Regex.IsMatch(phone, @"^\+?[1-9]\d{1,14}$");
        }
    }
}