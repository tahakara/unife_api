using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.MappingHelpers
{
    /// <summary>
    /// Provides helper methods for cleaning and converting mapping-related data such as tokens, GUIDs, emails, phone numbers, and more.
    /// </summary>
    public class MappingHelper
    {
        #region Token Cleaning

        /// <summary>
        /// Cleans the access token by removing the "Bearer " prefix and trimming whitespace.
        /// </summary>
        /// <param name="token">The access token string to clean.</param>
        /// <returns>The cleaned access token, or an empty string if input is null or whitespace.</returns>
        public static string CleanAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return string.Empty;

            string trimmed = token.Trim();
            return trimmed.StartsWith("Bearer ", StringComparison.Ordinal)
                ? trimmed.Substring("Bearer ".Length).Trim()
                : trimmed;
        }

        /// <summary>
        /// Cleans the refresh token by trimming whitespace.
        /// </summary>
        /// <param name="token">The refresh token string to clean.</param>
        /// <returns>The cleaned refresh token, or an empty string if input is null or whitespace.</returns>
        public static string CleanRefreshToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return string.Empty;
            return token.Trim();
        }

        /// <summary>
        /// Cleans the recovery token by trimming whitespace.
        /// </summary>
        /// <param name="token">The recovery token string to clean.</param>
        /// <returns>The cleaned recovery token, or an empty string if input is null or whitespace.</returns>
        public static string CleanRecoveryToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return string.Empty;
            return token.Trim();
        }

        #endregion

        #region Guid Cleaning

        /// <summary>
        /// Converts a string to a <see cref="Guid"/>. Returns <see cref="Guid.Empty"/> if parsing fails.
        /// </summary>
        /// <param name="guid">The string to convert to a Guid.</param>
        /// <returns>The parsed Guid, or Guid.Empty if parsing fails.</returns>
        public static Guid CleanStringToGuid(string? guid)
        {
            return Guid.TryParse(guid, out Guid result) ? result : Guid.Empty;
        }

        #endregion

        #region OTP Cleaning

        /// <summary>
        /// Cleans the OTP code by removing all non-digit characters.
        /// </summary>
        /// <param name="otpCode">The OTP code string to clean.</param>
        /// <returns>The cleaned OTP code containing only digits, or an empty string if input is null or whitespace.</returns>
        public static string CleanOtpCode(string? otpCode)
        {
            if (string.IsNullOrWhiteSpace(otpCode) || string.IsNullOrEmpty(otpCode))
                return string.Empty;
            return new string(otpCode.Where(char.IsDigit).ToArray());
        }

        #endregion

        #region Boolean and Byte Helpers

        /// <summary>
        /// Returns the value of a nullable boolean, or false if null.
        /// </summary>
        /// <param name="value">The nullable boolean value.</param>
        /// <returns>The boolean value, or false if null.</returns>
        public static bool BoolOrDefaultBooleanValue(bool? value)
        {
            return value.HasValue ? value.Value : false;
        }

        /// <summary>
        /// Returns the value of a nullable byte, or 0 if null.
        /// </summary>
        /// <param name="value">The nullable byte value.</param>
        /// <returns>The byte value, or 0 if null.</returns>
        public static byte ByteOrDefaultByteValue(byte? value)
        {
            return value.HasValue ? value.Value : default;
        }

        /// <summary>
        /// Cleans the user type ID by returning its value or 0 if null.
        /// </summary>
        /// <param name="userTypeId">The nullable user type ID.</param>
        /// <returns>The user type ID as byte, or 0 if null.</returns>
        public static byte CleanUserTypeId(byte? userTypeId)
        {
            return ByteOrDefaultByteValue(userTypeId);
        }

        /// <summary>
        /// Cleans the recovery method ID by returning its value or 0 if null.
        /// </summary>
        /// <param name="recoveryMethodId">The nullable recovery method ID.</param>
        /// <returns>The recovery method ID as byte, or 0 if null.</returns>
        public static byte CleanRecoveryMethodId(byte? recoveryMethodId)
        {
            return ByteOrDefaultByteValue(recoveryMethodId);
        }

        #endregion

        #region String Cleaning

        /// <summary>
        /// Cleans a string by trimming whitespace, or returns an empty string if input is null or whitespace.
        /// </summary>
        /// <param name="input">The string to clean.</param>
        /// <returns>The trimmed string, or an empty string if input is null or whitespace.</returns>
        public static string CleanString(string? input)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
                return string.Empty;
            return input.Trim();
        }

        /// <summary>
        /// Cleans an email address by trimming whitespace, or returns an empty string if input is null or whitespace.
        /// </summary>
        /// <param name="email">The email address to clean.</param>
        /// <returns>The cleaned email address, or an empty string if input is null or whitespace.</returns>
        public static string CleanEmail(string? email)
        {
            return CleanString(email);
        }

        /// <summary>
        /// Cleans a phone country code by removing all non-digit characters.
        /// </summary>
        /// <param name="phoneCountryCode">The phone country code to clean.</param>
        /// <returns>The cleaned phone country code containing only digits, or an empty string if input is null or whitespace.</returns>
        public static string CleanPhoneCountryCode(string? phoneCountryCode)
        {
            if (string.IsNullOrWhiteSpace(phoneCountryCode) || string.IsNullOrEmpty(phoneCountryCode))
                return string.Empty;
            return new string(phoneCountryCode.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Cleans a phone number by removing all non-digit characters.
        /// </summary>
        /// <param name="phoneNumber">The phone number to clean.</param>
        /// <returns>The cleaned phone number containing only digits, or an empty string if input is null or whitespace.</returns>
        public static string CleanPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrEmpty(phoneNumber))
                return string.Empty;
            return new string(phoneNumber.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Cleans a name by trimming whitespace, or returns an empty string if input is null or whitespace.
        /// </summary>
        /// <param name="name">The name to clean.</param>
        /// <returns>The cleaned name, or an empty string if input is null or whitespace.</returns>
        public static string CleanName(string? name)
        {
            return CleanString(name);
        }

        #endregion
    }
}
