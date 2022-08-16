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

namespace Globo.PIC.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("projetos")]
    [Authorize(Policy = "MatchProfile")]
    public class ProjetosController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ProjetosController> logger;

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
        public ProjetosController(ILogger<ProjetosController> _logger,
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
        public async Task<IActionResult> GetProjectsAsync([FromQuery] ProjetosFilterViewModel filter, CancellationToken cancellationToken)
        {
            var projetosList = await mediator.Send(new GetByProjetosFilter()
            { 
                ProjectName = filter.Name
            }, cancellationToken);

            if (projetosList == null) throw new NotFoundException("projeto não encontrado.");

            var projetoViewModel = mapper.Map<List<ProjetoViewModel>>(projetosList);

            var pagging = new GenericPaggingViewModel<ProjetoViewModel>(projetoViewModel, filter, 0);
            return Ok(pagging);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<ProjetoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProjectAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var projeto = await mediator.Send<ProjetoModel>(new GetByProjetoFilter()
            {
                ProjectId = id
            }, cancellationToken);

            if (projeto == null) throw new NotFoundException("projeto não encontrado.");
            return Ok(projeto);
        }

    }
}
