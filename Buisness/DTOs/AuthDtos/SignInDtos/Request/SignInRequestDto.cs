using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.SignInDtos.Request
{
    public class SignInRequestDto : DtoBase, IDtoBase
    {
        public byte? UserTypeId { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? UserUuid { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? SessionUuid { get; set; }
        public string? Email { get; set; }
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public byte? OtpTypeId { get; set; }
        public string? OtpCode { get; set; }    
    }
}
