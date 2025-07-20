using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults
{
    public record BuisnessLogicResult : IBuisnessLogicResult
    {
        // Ana constructor
        public BuisnessLogicResult(bool success, int statusCode, string? message = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
        }

        // Success için varsayılan 200
        public BuisnessLogicResult(bool success, string? message = null) 
            : this(success, success ? 200 : 400, message)
        {
        }

        public bool Success { get; }
        public int StatusCode { get; }
        public string? Message { get; }
    }
}
