using Buisness.DTOs.Base;
using Buisness.DTOs.ModelBinderHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos
{
    public class VerifyOTPRequestDto : DtoBase, IDtoBase
    {
        public byte? UserTypeId { get; set; } = 0;
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid? SessionUuid { get; set; }
        [ModelBinder(BinderType = typeof(TrimmedGuidModelBinder))]
        public Guid UserUuid { get; set; } = Guid.Empty;
        public byte? OtpTypeId { get; set; } = 0;
        public string? OtpCode { get; set; } = string.Empty;
    }
}
