using Buisness.Features.CQRS.Base;
using Buisness.Helpers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Logout
{
    public class LogoutCommand : ICommand<BaseResponse<bool>>
    {
        public LogoutCommand() { }

        public LogoutCommand(string accessToken)
        {
            AccessToken = NormalizeToken(accessToken);
        }

        private string NormalizeToken(string token)
        {
            const string bearerPrefix = "Bearer ";
            return token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? token.Substring(bearerPrefix.Length).Trim()
                : token.Trim();
        }

        private string _accessToken = string.Empty;
        public string AccessToken
        {
            get => _accessToken;
            set => _accessToken = NormalizeToken(value);
        }

        public string? UserUuid { get; set; }
        public string? SessionUuid { get; set; }
    }
}
