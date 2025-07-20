using Buisness.Abstract.DtoBase.Base;
using Buisness.Concrete.Dto;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;

namespace Buisness.DTOs.AuthDtos.SignInDtos.Response
{
    public class SignInResponseDto : DtoBase, IDtoBase
    {
        public byte? UserTypeId { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? SessionUuid { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UserUuid { get; set; }
        public byte? OtpTypeId { get; set; }

    }
}
