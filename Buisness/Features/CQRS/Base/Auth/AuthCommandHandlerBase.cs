using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Base.Auth
{
    public abstract class AuthCommandHandlerBase<TCommand>
    {
        protected readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<TCommand> _logger;
        protected readonly string _baseCommandHandlerName;
        protected readonly string _commandName;
        protected readonly string _commandFullName;
        protected AuthCommandHandlerBase(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TCommand> logger,
            string commandName)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _baseCommandHandlerName = "Auth";
            _commandName = commandName;
            _commandFullName = $"{_baseCommandHandlerName}:{_commandName}";
        }
    }
}
