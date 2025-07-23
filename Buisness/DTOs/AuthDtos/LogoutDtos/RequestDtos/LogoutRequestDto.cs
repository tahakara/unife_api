using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos
{
    public class LogoutRequestDto : DtoBase, IDtoBase
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
