using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.UserDtos.UpdateUserProfileDtos
{
    public class UpdateUserProfileResponseDto : ResponseDtoBase
    {
        public Guid UserUuid { get; set; } = Guid.Empty;
        public Guid UniversityUuid { get; set; } = Guid.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsPhoneNumberVerified { get; set; } = false;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
