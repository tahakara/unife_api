using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.UserDtos.UpdateUserProfileDtos
{
    public class UpdateUserProfileRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
