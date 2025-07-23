using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos
{
    public class AccessTokenDto : DtoBase
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
