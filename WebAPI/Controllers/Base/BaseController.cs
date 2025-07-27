using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Request.Query;
using Buisness.Features.CQRS.Base.Generic.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;

        protected BaseController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected IActionResult CreateResponse<T>(BaseResponse<T> response)
        {
            return response.StatusCode switch
            {
                200 => Ok(response),
                201 => StatusCode(201, response),
                202 => Accepted(response),
                204 => NoContent(),
                400 => BadRequest(response),
                404 => NotFound(response),
                409 => Conflict(response),
                500 => StatusCode(500, response),
                _ => StatusCode(response.StatusCode, response)
            };
        }

        protected async Task<IActionResult> SendCommand<T>(ICommand<BaseResponse<T>> command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreateResponse(result);
            }
            catch (ValidationException validationEx)
            {
                return StatusCode(400, BaseResponse<T>.Failure(validationEx.Message, validationEx.Errors.Select(e => e.ErrorMessage).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Command işlemi sırasında hata oluştu: {CommandType}", typeof(T).Name);
                return StatusCode(500, BaseResponse<T>.Failure("Hata", new List<string> { ex.Message }, 500));
            }
        }

        protected async Task<IActionResult> SendQuery<T>(IQuery<BaseResponse<T>> query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return CreateResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query işlemi sırasında hata oluştu: {QueryType}", typeof(T).Name);
                return StatusCode(500, BaseResponse<T>.Failure("Hata", new List<string> { ex.Message }, 500));
            }
        }
        protected async Task<IActionResult> SendQueryDirect<T>(IQuery<T> query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query işlemi sırasında hata oluştu: {QueryType}", typeof(T).Name);
                return StatusCode(500, new { error = "Hata", details = ex.Message });
            }
        }
    }
}