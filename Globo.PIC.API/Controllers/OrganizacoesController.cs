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
using Globo.PIC.API.Configurations.Attributes;
using Globo.PIC.Domain.Enums;

namespace Globo.PIC.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("organizacoes")]
    [Authorize(Policy = "MatchProfile")]
    public class OrganizacoesController : ControllerBase
    {
        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<OrganizacoesController> logger;

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
        public OrganizacoesController(ILogger<OrganizacoesController> _logger,
                                IMapper _mapper,
                                IMediator _mediator, IUserProvider _userProvider)
        {
            logger = _logger;
            mapper = _mapper;
            mediator = _mediator;
            userProvider = _userProvider;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<OrganizacaoViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByBusinessUnityIdAsync(CancellationToken cancellationToken)
        {
            if ( userProvider.User.UnidadeNegocio==null || userProvider.User.UnidadeNegocio.Id<1)
                throw new NotFoundException("Unidade de Negócio não cadastrada para o usuário.");

            var organizations = await mediator.Send<List<OrganizationStructure>>(new GetByBusinessUnityId()
            {
                Id = userProvider.User.UnidadeNegocio.Id
            }, cancellationToken);

            if (organizations == null) throw new NotFoundException("Organizações não encontradas.");
            var organizationsViewModel = mapper.Map<List<OrganizacaoViewModel>>(organizations);
            return Ok(organizationsViewModel);
        }

        //[HttpGet("{id:long}")]
        //[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<OrganizacaoViewModel>>))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //[IsInRole(Role.GRANT_ADM_USUARIOS)]
        //public async Task<IActionResult> GetByBusinessUnityIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        //{ 
        //    var organizations = await mediator.Send<List<OrganizationStructure>>(new GetByBusinessUnityId()
        //    {
        //        Id = id
        //    }, cancellationToken);

        //    if (organizations == null) throw new NotFoundException("Organizações não encontradas.");
        //    var organizationsViewModel = mapper.Map<List<OrganizacaoViewModel>>(organizations);
        //    return Ok(organizationsViewModel);
        //}
    }
}
