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
        public byte UserTypeId { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        // Internal properties
        public Guid UserUuid { get; set; } = Guid.Empty;
        public Guid SessionUuid { get; set; } = Guid.Empty;
        public byte OtpTypeId { get; set; } = 0;
        public string OtpCode { get; set; } = string.Empty;
    }
}
