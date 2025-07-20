using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/verification-test")]
    [ApiController]
    public class VerificationCodeTestController : ControllerBase
    {
        private readonly IOTPCodeService _verificationCodeService;

        //public VerificationCodeTestController(IOTPCodeService verificationCodeService)
        //{
        //    _verificationCodeService = verificationCodeService;
        //}

        //[HttpPost("set")]
        //public async Task<IActionResult> SetCode([FromQuery] string type, [FromQuery] string userUuid, [FromQuery] string key, [FromQuery] string code, [FromQuery] int expirationSeconds = 600)
        //{
        //    await _verificationCodeService.SetCodeAsync(type, userUuid, key, code, TimeSpan.FromSeconds(expirationSeconds));
        //    return Ok(new { message = "Code set", type, userUuid, key, code, expirationSeconds });
        //}

        //[HttpGet("get")]
        //public async Task<IActionResult> GetCode([FromQuery] string type, [FromQuery] string userUuid, [FromQuery] string key)
        //{
        //    var code = await _verificationCodeService.GetCodeAsync(type, userUuid, key);
        //    if (code == null)
        //        return NotFound(new { message = "Code not found", type, userUuid, key });
        //    return Ok(new { code, type, userUuid, key });
        //}

        //[HttpDelete("revoke")]
        //public async Task<IActionResult> RevokeCode([FromQuery] string type, [FromQuery] string userUuid, [FromQuery] string key)
        //{
        //    await _verificationCodeService.RemoveCodeAsync(type, userUuid, key);
        //    return Ok(new { message = "Code revoked", type, userUuid, key });
        //}
    }
}