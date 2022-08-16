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
    [Route("departamentos")]
    [Authorize(Policy = "MatchProfile")]
    public class DepartamentosController : ControllerBase
    {
        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<DepartamentosController> logger;

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
        public DepartamentosController(ILogger<DepartamentosController> _logger,
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
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<DepartamentoViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]        
        public async Task<IActionResult> GetDepartamentosByFilterAsync([FromQuery] DepartamentoFilterViewModel filter, CancellationToken cancellationToken)
        {
            var departamentos = await mediator.Send(new GetByDepartamentoFilter()
            {
                Filter = filter
            }, cancellationToken);
            int contador = departamentos.Count;
            int.TryParse(filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;
            departamentos = departamentos.Skip((page - 1) * filter.PerPage).Take(filter.PerPage).ToList();
            if (departamentos.Count() == 0) throw new NotFoundException("Registro não encontrado.");
            var departamentoViewModel = mapper.Map<List<DepartamentoViewModel>>(departamentos);
            var pagging = new GenericPaggingViewModel<DepartamentoViewModel>(departamentoViewModel, filter, contador);
            return Ok(pagging);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<DepartamentoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDepartamentoByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var departamento = await mediator.Send<Departamento>(new GetById()
            {
                Id = id
            }, cancellationToken);
            if (departamento == null) throw new NotFoundException("departamento não encontrado.");
            var departamentoViewModel = mapper.Map<DepartamentoViewModel>(departamento);
            return Ok(departamentoViewModel);
        }
    }
}
