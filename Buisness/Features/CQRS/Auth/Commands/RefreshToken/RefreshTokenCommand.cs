using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : ICommand<BaseResponse<RefreshTokenCommand>>
    {
        public RefreshTokenCommand() { }

        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = NormalizeToken(refreshToken);
        }

        private string NormalizeToken(string token)
        {
            const string bearerPrefix = "Bearer ";
            return token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? token.Substring(bearerPrefix.Length).Trim()
                : token.Trim();
        }

        private string _refreshToken = string.Empty;
        public string RefreshToken
        {
            get => _refreshToken;
            set => _refreshToken = NormalizeToken(value);
        }

        public string? AccessToken { get; set; }
        public string? UserUuid { get; set; }
        public string? SessionUuid { get; set; }

    }
}
