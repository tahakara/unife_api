using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using Npgsql.TypeMapping;

namespace Buisness.DTOs.AuthDtos.SignUpDtos.Request
{
    public class SignUpRequestDto : DtoBase, IDtoBase
    {
        public byte UserTypeId { get; set; } = 0;
        public Guid UniversityUuid { get; set; } = Guid.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
