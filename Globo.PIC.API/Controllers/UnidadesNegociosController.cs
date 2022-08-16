using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels.Filters;

namespace Globo.PIC.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("unidadesnegocios")]
    [Authorize(Policy = "MatchProfile")]
    public class UnidadesNegociosController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="_mediator"></param>
        public UnidadesNegociosController(
                                IMapper _mapper,
                                IMediator _mediator)
        {
            mapper = _mapper;
            mediator = _mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<UnidadeNegocioViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBusinessUnityByFilterAsync([FromQuery] UnidadeNegocioFilterViewModel filter, CancellationToken cancellationToken)
        {

            var businessUnities = await mediator.Send(new ListByBusinessUnityFilter()
            {
                Filter = filter
            }, cancellationToken) ;

            int contador = businessUnities.Count;

            int.TryParse(filter.Page.ToString(), out int page);

            page = page <= 0 ? 1 : page;

            businessUnities = businessUnities.Skip((page - 1) * filter.PerPage).Take(filter.PerPage).ToList();

            if (businessUnities.Count() == 0) throw new NotFoundException("Registro não encontrado.");

            var businessUnityViewModel = mapper.Map<List<UnidadeNegocioViewModel>>(businessUnities);

            var pagging = new GenericPaggingViewModel<UnidadeNegocioViewModel>(businessUnityViewModel, filter, contador);

            return Ok(pagging);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<UnidadeNegocioViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBusinessUnityByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var businessUnity = await mediator.Send<UnidadeNegocio>(new GetByBusinessUnityId()
            {
                Id = id
            }, cancellationToken);

            if (businessUnity == null) throw new NotFoundException("conteúdo não encontrado.");

            var businessUnityViewModel = mapper.Map<UnidadeNegocioViewModel>(businessUnity);

            return Ok(businessUnityViewModel);
        }
    }
}
