using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base
{
    public interface IBuisnessLogicResult
    {
        bool Success { get; }
        int StatusCode { get; }
        string? Message { get; }
    }
}
