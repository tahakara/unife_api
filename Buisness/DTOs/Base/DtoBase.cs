using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.Base
{
    /// <summary>
    /// Abstract base class for all Data Transfer Objects (DTOs).
    /// <para>
    /// This class is typically used as a base for mapping incoming commands to DTOs.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in derived classes for internal processing.
    /// </para>
    /// </summary>
    public abstract class DtoBase : IDtoBase
    {
    }

    /// <summary>
    /// Abstract base class for request DTOs.
    /// </summary>
    public abstract class RequestDtoBase : DtoBase, IRequestDtoBase
    {
    }

    /// <summary>
    /// Abstract base class for response DTOs.
    /// </summary>
    public abstract class ResponseDtoBase : DtoBase, IResponseDtoBase
    {
    }
}