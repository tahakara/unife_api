using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.MappingHelpers
{
    public class MappingHelper
    {
        public static string CleanAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return string.Empty;

            string trimmed = token.Trim();
            return trimmed.StartsWith("Bearer ", StringComparison.Ordinal)
                ? trimmed.Substring("Bearer ".Length).Trim()
                : trimmed;
        }

        public static Guid CleanStringToGuid(string? guid)
        {
            return Guid.TryParse(guid, out Guid result) ? result : Guid.Empty;
        }

        public static string CleanOtpCode(string? otpCode)
        {
            if (string.IsNullOrWhiteSpace(otpCode) || string.IsNullOrEmpty(otpCode))
                return string.Empty;
            return new string(otpCode.Where(char.IsDigit).ToArray());
        }
        public static string CleanRefreshToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return string.Empty;
            return token.Trim();
        }

        public static string CleanRecoveryToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return string.Empty;
            return token.Trim();
        }

        public static bool BoolOrDefaultBooleanValue(bool? value)
        {
            return value.HasValue ? value.Value : false;
        }

        public static byte ByteOrDefaultByteValue(byte? value)
        {
            return value.HasValue ? value.Value : default;
        }

        public static byte CleanUserTypeId(byte? userTypeId)
        {
            return ByteOrDefaultByteValue(userTypeId);
        }

        public static byte CleanRecoveryMethodId(byte? recoveryMethodId)
        {
            return ByteOrDefaultByteValue(recoveryMethodId);
        }

        public static string CleanString(string? input)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Trim();
        }
        public static string CleanEmail(string? email)
        {
            return CleanString(email);
        }

        public static string CleanPhoneCountryCode(string? phoneCountryCode)
        {
            if (string.IsNullOrWhiteSpace(phoneCountryCode) || string.IsNullOrEmpty(phoneCountryCode))
                return string.Empty;
            return new string(phoneCountryCode.Where(char.IsDigit).ToArray());
        }

        public static string CleanPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrEmpty(phoneNumber))
                return string.Empty;
            return new string(phoneNumber.Where(char.IsDigit).ToArray());
        }

        public static string CleanName(string? name)
        {
            return CleanString(name);
        }
    }
}
