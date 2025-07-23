using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;

namespace Buisness.DTOs.AuthDtos.SignInDtos.Response
{
    public class SignInResponseDto : DtoBase, IDtoBase
    {
        public byte UserTypeId { get; set; } = 0;
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid SessionUuid { get; set; } = Guid.Empty;
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid UserUuid { get; set; } = Guid.Empty;
        public byte? OtpTypeId { get; set; }

    }
}
