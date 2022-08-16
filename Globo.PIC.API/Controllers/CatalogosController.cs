using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels.Filters;

namespace Globo.PIC.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("catalogo")]
    [Authorize(Policy = "MatchProfile")] 
    public class CatalogosController : ControllerBase
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
        public CatalogosController(
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
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<ItemCatalogoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCatalogoAsync([FromQuery] ItemCatalogoFilterViewModel filter, CancellationToken cancellationToken)
        {
            var itemCatalogoList = await mediator.Send(new ListByItemCatalogoFilter() { Filter = filter }, cancellationToken);
            
            if (itemCatalogoList.Count == 0) throw new NotFoundException("Itens não encontrados.");
            
            var itemCatalogoViewModel = mapper.Map<List<ItemCatalogoViewModel>>(itemCatalogoList);            

            var pagging = new GenericPaggingViewModel<ItemCatalogoViewModel>(itemCatalogoViewModel, filter, 0);

            return Ok(pagging);
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(ItemCatalogoViewModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPedidoVeiculoIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {

            if (id <= 0) throw new BadRequestException("O parametro id é requerido.");

            var itemCatalogo = await mediator.Send(new GetItemCatalogoById() { Id = id }, cancellationToken);

            if (itemCatalogo == null) throw new NotFoundException("Item não encontrado.");

            var itemCatalogoViewModel = mapper.Map<ItemCatalogoViewModel>(itemCatalogo);

            return Ok(itemCatalogoViewModel);
        }
    }
}
