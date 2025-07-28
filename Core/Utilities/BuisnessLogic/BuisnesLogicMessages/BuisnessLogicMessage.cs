using Core.Utilities.MessageUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.BuisnessLogic.BuisnesLogicMessages
{
    public abstract class BuisnessLogicMessage : IMessageUtility
    {
        
        public static string Successfuly(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} successfully."
                : $"{propertyName} successfully on the {location}.";
        public static string Success(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} succeeded."
                : $"{propertyName} succeeded on the {location}.";

        public static string Failed(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} failed."
                : $"{propertyName} failed on the {location}.";

        public static string Error(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} error."
                : $"{propertyName} error on the {location}.";

        public static string ErrorOccurred(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"Error occurred on {propertyName}."
                : $"{propertyName} error occurred on the {location}.";

        public static string Valid(string propertyName) 
            => $"{propertyName} is valid.";
        public static string Invalid(string propertyName) 
            => $"{propertyName} is invalid.";
        public static string ValidationFailed(string propertyName) 
            => $"{propertyName} validation failed.";
        public static string Required(string propertyName) 
            => $"{propertyName} is required.";
        public static string Required(IEnumerable<string> propertyNames)
        {
            var names = string.Join(", ", propertyNames);
            return $"{names} are required.";
        }
        public static string AtLeastOneRequired(IEnumerable<string> propertyNames)
        {
            var names = string.Join(", ", propertyNames);
            return $"At least one of the following is required: {names}.";
        }

        public static string InvalidOrExpired(string propertyName) 
            => $"{propertyName} is invalid or expired.";


        public static string NotFound(string propertyName) 
            => $"{propertyName} not found.";

        public static string NotFound(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} not found."
                : $"{propertyName} not found on the {location}.";

        public static string NotFound(IEnumerable<string> propertyNames, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{string.Join(", ", propertyNames)} not found."
                : $"{string.Join(", ", propertyNames)} not found on the {location}.";

        public static string Revoked(string propertyName)
            => $"{propertyName} is revoked.";

        public static string Revoked(IEnumerable<string> propertyNames)
        {
            var names = string.Join(", ", propertyNames);
            return $"{names} are revoked.";
        }
        public static string RevokcationFailed(string propertyName)
            => $"{propertyName} revocation failed.";



        public static string Accepted(string propertyName)
            => $"{propertyName} is accepted.";
        public static string NotAccepted(string propertyName) 
            => $"{propertyName} is not accepted.";
        public static string NotAccepted(IEnumerable<string> propertyNames)
        {
            var names = string.Join(", ", propertyNames);
            return $"{names} are not accepted.";
        }

        public static string Blacklisted(string propertyName)
            => $"{propertyName} is blacklisted.";

        public static string Blacklisted(IEnumerable<string> propertyNames)
        {
            var names = string.Join(", ", propertyNames);
            return $"{names} are blacklisted.";
        }
        public static string BlacklistFailed(string propertyName)
            => $"{propertyName} blacklist failed.";
     
        public static string ToManyAttempts(string propertyName)
            => $"{propertyName} has too many attempts. Please try again later.";

        public static string AlreadyExists(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} already exists."
                : $"{propertyName} already exists on the {location}.";

        public static string Already(string propertyName, string propertyStatus)
            => string.IsNullOrWhiteSpace(propertyStatus)
                ? $"{propertyName} already."
                : $"{propertyName} already {propertyStatus}.";

        public static string Not(string propertyName, string propertyStatus)
            => string.IsNullOrWhiteSpace(propertyStatus)
                ? $"{propertyName} not."
                : $"{propertyName} not {propertyStatus}.";

        public static string Created(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} created successfully."
                : $"{propertyName} created successfully on the {location}.";

        public static string NotCreated(string propertyName, string? location = null)
            => string.IsNullOrWhiteSpace(location)
                ? $"{propertyName} not created."
                : $"{propertyName} not created on the {location}.";

        public static string ExceedsLimit(string propertyName, int limit)
            => $"{propertyName} exceeds the limit of {limit}.";
    }
}
