using Buisness.Abstract.DtoBase.Base;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.LogoutDtos
{
    public class LogoutUserObjectDto : IDtoBase
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid? UserUuid { get; set; } = null;
        public Guid? UniversityUuid { get; set; } = null;
        public UserTypeId? UserTypeId { get; set; } = 0;
    }
}
