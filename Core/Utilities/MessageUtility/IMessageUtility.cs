using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.MessageUtility
{
    /// <summary>
    /// Provides a shared contract for accessing standardized and reusable message templates.
    /// Commonly implemented by static message providers to enforce consistency across layers such as services, helpers, and utilities.
    /// </summary>
    public interface IMessageUtility
    {
        // Marker interface – no instance members required
    }

}
