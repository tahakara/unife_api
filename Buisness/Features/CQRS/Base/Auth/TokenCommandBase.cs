using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Base.Auth
{
    public abstract class TokenCommandBase
    {
        private string _accessToken = string.Empty;

        public string? AccessToken
        {
            get => _accessToken;
            set => _accessToken = NormalizeToken(value);
        }

        protected string NormalizeToken(string? token)
        {
            const string bearerPrefix = "Bearer ";
            if (string.IsNullOrWhiteSpace(token))
                return string.Empty;

            return token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? token.Substring(bearerPrefix.Length).Trim()
                : token.Trim();
        }
    }

}
