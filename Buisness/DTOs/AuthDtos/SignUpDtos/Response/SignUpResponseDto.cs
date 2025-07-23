using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;

namespace Buisness.DTOs.AuthDtos.SignUpDtos.Response
{
    public class SignUpResponseDto : DtoBase, IDtoBase
    {
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UniversityUuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
