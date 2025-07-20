using Core.Utilities.OTPUtilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.OTPUtilities
{
    public sealed class OTPUtilitiy : IOTPUtilitiy
    {
        public string GenerateOTP()
        {
            using var rng = new RNGCryptoServiceProvider();

            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int value = BitConverter.ToInt32(randomNumber, 0) & int.MaxValue; // Ensure non-negative
            int otp = value % 1000000;
            return otp.ToString("D6");
        }
    }
}
