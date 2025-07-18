using Buisness.Abstract.ServicesBase;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
using Buisness.Concrete.ServiceManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices
{
    public class AdminService : ServiceManagerBase, IAdminService
    {
        public AdminService(ILogger<AdminService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {

        }
    }
}
