using Buisness.DTOs;
using Buisness.DTOs.Common;
using Buisness.Features.CQRS.Universities.Commands.CreateUniversity;
using Buisness.Features.CQRS.Universities.Commands.DeleteUniversity;
using Buisness.Features.CQRS.Universities.Commands.UpdateUniversity;
using Buisness.Features.CQRS.Universities.Queries.GetPagedUniversities;
using Buisness.Features.CQRS.Universities.Queries.GetUniversityById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class UniversityController : BaseController
    {
        public UniversityController(IMediator mediator, ILogger<UniversityController> logger) 
            : base(mediator, logger)
        {
        }

        /// <summary>
        /// Yeni üniversite oluşturur
        /// </summary>
        /// <param name="command">Üniversite oluşturma bilgileri</param>
        /// <returns>Oluşturulan üniversite bilgileri</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateUniversityCommand command)
        {
            return await SendCommand(command);
        }

        /// <summary>
        /// uuid ile üniversite getirir
        /// </summary>
        /// <param name="uuid">Üniversite uuid'si</param>
        /// <returns>Üniversite bilgileri</returns>
        [HttpGet("{uuid:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUuid(Guid uuid)
        {
            var query = new GetUniversityByIdQuery(uuid);
            return await SendQuery(query);
        }

        /// <summary>
        /// Sayfalı üniversite listesi getirir
        /// </summary>
        /// <param name="currentPage">Sayfa numarası (varsayılan: 1)</param>
        /// <param name="pageSize">Sayfa boyutu (varsayılan: 10, maksimum: 100)</param>
        /// <param name="orderBy">Sıralama alanı (varsayılan: UniversityName)</param>
        /// <param name="orderDirection">Sıralama yönü: asc/desc (varsayılan: asc)</param>
        /// <returns>Sayfalı üniversite listesi</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int currentPage = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string orderBy = "CreatedAt",
            [FromQuery] string orderDirection = "asc")
        {
            var query = new GetPagedUniversitiesQuery(currentPage, pageSize, orderBy, orderDirection);
            return await SendQueryDirect(query); 
        }

        /// <summary>
        /// UUID'li iniversiteyi Günceller
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUniversty([FromBody] UpdateUniversityCommand command)
        {
            return await SendCommand(command);
        }

        /// <summary>
        /// UUID'li üniversiteyi siler
        /// </summary>
        /// <param name="command">Silme komutu</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteUniversityCommand command)
        {
            return await SendCommand(command);
        }
    }
}