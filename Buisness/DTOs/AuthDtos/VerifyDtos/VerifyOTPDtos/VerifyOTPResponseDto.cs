using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;

namespace Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos
{
    public class VerifyOTPResponseDto : DtoBase, IDtoBase
    {   
        public byte? OtpTypeId { get; set; } = 0;
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UserUuid { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? SessionUuid { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
