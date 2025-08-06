using Buisness.Features.CQRS.User.Command;
using Buisness.Features.CQRS.User.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.User
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, ILogger<UserController> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserProfile([FromHeader(Name = "Authorization")] string? accessToken)
        {
            var query = new GetUserProfileQuery();
            return await SendCommand(query);
        }

        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserProfile([FromHeader(Name = "Authorization")] string? accessToken, 
            [FromBody] UpdateUserProfileCommand command)
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        [HttpPost("disable-account")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisableAccount([FromHeader(Name = "Authorization")] string? accessToken, 
            [FromBody] DisableAccountCommand command)
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        //[HttpPost("create-university")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //public async Task<IActionResult> CreateUniversity([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] CreateUniversityCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPut("update-university")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateUniversity([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] UpdateUniversityCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpDelete("delete-university")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteUniversity([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] DeleteUniversityCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPost("disable-university")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DisableUniversity([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] DisableUniversityCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPost("enable-university")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> EnableUniversity([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] EnableUniversityCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPost("create-faculty")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //public async Task<IActionResult> CreateFaculty([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] CreateFacultyCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPut("update-faculty")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateFaculty([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] UpdateFacultyCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpDelete("delete-faculty")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteFaculty([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] DeleteFacultyCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPost("disable-faculty")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DisableFaculty([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] DisableFacultyCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}

        //[HttpPost("enable-faculty")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> EnableFaculty([FromHeader(Name = "Authorization")] string? accessToken, 
        //    [FromBody] EnableFacultyCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return await SendCommand(command);
        //}


    }
}
