using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.Base
{
    /// <summary>
    /// Marker interface for all Data Transfer Objects (DTOs).
    /// </summary>
    public interface IDtoBase
    {
    }

    /// <summary>
    /// Marker interface for request DTOs.
    /// </summary>
    public interface IRequestDtoBase : IDtoBase
    {
        // Marker interface for request DTOs
    }

    /// <summary>
    /// Marker interface for response DTOs.
    /// </summary>
    public interface IResponseDtoBase : IDtoBase
    {
        // Marker interface for response DTOs
    }
}
