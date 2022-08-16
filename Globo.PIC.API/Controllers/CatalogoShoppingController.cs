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

namespace Globo.PIC.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("catalogo/shopping")]
    [Authorize(Policy = "MatchProfile")] 
    public class CatalogoShoppingController : ControllerBase
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
        public CatalogoShoppingController(
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
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<ShoppingCatalogItemViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetShoppingCatalogoAsync([FromQuery] ShoppingCatalogFilterViewModel filter, CancellationToken cancellationToken)
        {
            filter.PerPage = 24;

            var catalogo = await mediator.Send(new GetByShoppingCatalogFilter() { Filter = filter }, cancellationToken);
            
            if (catalogo == null) throw new NotFoundException("Items não encontrados.");
                        
            var catalogoItemViewModel = mapper.Map<List<ShoppingCatalogItemViewModel>>(catalogo.Items);            

            var pagging = new GenericPaggingViewModel<ShoppingCatalogItemViewModel>(catalogoItemViewModel, filter, catalogo.TotalResults);

            return Ok(pagging);
        }
    }
}
