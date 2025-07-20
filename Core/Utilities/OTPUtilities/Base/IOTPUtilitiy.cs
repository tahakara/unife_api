using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.OTPUtilities.Base
{
    public interface IOTPUtilitiy
    {
        /// <summary>
        /// Generates a one-time password (OTP) for the given phone number.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing the generated OTP.</returns>
        string GenerateOTP();
    }
}
