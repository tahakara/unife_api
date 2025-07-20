using Buisness.Abstract.DtoBase.Base;
using Buisness.Concrete.Dto;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using Npgsql.TypeMapping;

namespace Buisness.DTOs.AuthDtos.SignUpDtos.Request
{
    public class SignUpRequestDto : DtoBase, IDtoBase
    {
        public int UserTypeId { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UniversityUuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
