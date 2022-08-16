using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels.Filters;

namespace Globo.PIC.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("localidades")]
    [Authorize(Policy = "MatchProfile")]
    public class LocalidadesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<LocalidadesController> logger;

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
        private readonly IUserProvider userProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_mapper"></param>
        /// <param name="_mediator"></param>
        /// <param name="_userProvider"></param>
        public LocalidadesController(ILogger<LocalidadesController> _logger,
                                IMapper _mapper,
                                IMediator _mediator,
                                IUserProvider _userProvider)
        {
            logger = _logger;
            mapper = _mapper;
            mediator = _mediator;
            userProvider = _userProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<ProjetoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLocationsAsync([FromQuery] LocationsFilterViewModel filter, CancellationToken cancellationToken)
        {
            //if ( string.IsNullOrWhiteSpace(filter.Search)) throw new NotFoundException("O filtro Search é obrigatório.");

            var location = await mediator.Send(new GetByLocationsFilter()
            {
                OrganizationName = filter.Search
            }, cancellationToken);

            if (location == null) throw new NotFoundException("Locações não encontradas.");

            var locationsViewModel = mapper.Map<LocationViewModel>(location);

            return Ok(locationsViewModel);
        }
    }
}
