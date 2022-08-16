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
using Globo.PIC.API.Configurations.Attributes;
using Globo.PIC.Domain.Enums;

namespace Globo.PIC.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("fornecedores")]
    [Authorize(Policy = "MatchProfile")]
    public class FornecedoresController : ControllerBase
    {
        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<FornecedoresController> logger;

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
        /// <param name="_logger"></param>
        /// <param name="_mapper"></param>
        /// <param name="_mediator"></param>
        /// <param name="_userProvider"></param>
        public FornecedoresController(ILogger<FornecedoresController> _logger,
                                IMapper _mapper,
                                IMediator _mediator)
        {
            logger = _logger;
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
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<FornecedorViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFornecedorByFilterAsync([FromQuery] FornecedorFilterViewModel filter, CancellationToken cancellationToken)
        {
            if (filter.Search.Length >= 3)
            {
                var fornecedores = await mediator.Send(new GetByFornecedorFilter()
                {
                    Filter = filter
                }, cancellationToken);

                //paginação
                int contador = fornecedores.Count;
                int.TryParse(filter.Page.ToString(), out int page);
                page = page <= 0 ? 1 : page;
                fornecedores = fornecedores.Skip((page - 1) * filter.PerPage).Take(filter.PerPage).ToList();
                if (fornecedores.Count() == 0) throw new NotFoundException("Registro não encontrado.");

                var fornecedoresViewModel = mapper.Map<List<FornecedorViewModel>>(fornecedores);
                var pagging = new GenericPaggingViewModel<FornecedorViewModel>(fornecedoresViewModel, filter, contador);
                return Ok(pagging);
            }
            else
            {
                throw new BadRequestException("É necessário buscar por ao menos 3 letras.");
            }

        }


        [HttpGet("{cnpj}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<FornecedorViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFornecedorByCnpjAsync([FromRoute] long cnpj, CancellationToken cancellationToken)
        {
            if (cnpj.ToString().Length >= 14)
            {
                var fornecedor = await mediator.Send<Supplier>(new GetByCnpj()
                {
                    Cnpj = cnpj
                }, cancellationToken);

                if (fornecedor == null) throw new NotFoundException("Fornecedor não encontrado.");
                var fornecedorViewModel = mapper.Map<FornecedorViewModel>(fornecedor);
                return Ok(fornecedorViewModel);
            }
            else
            {
                throw new BadRequestException("É necessário informar ao menos 14 números!");
            }
        }

        [HttpGet("{cnpj}/acordos")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<AgreementsViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAcordoByFornecedorCnpjAsync([FromRoute] long cnpj, CancellationToken cancellationToken)
        {
            if (cnpj.ToString().Length >= 14)
            {
                var acordos = await mediator.Send<List<Agreements>>(new GetAgrementsByCNPJ()
                {
                    Cnpj = cnpj
                }, cancellationToken);

                if (acordos == null) throw new NotFoundException("Acordos não encontrados.");
                var fornecedorViewModel = mapper.Map<List<AgreementsViewModel>>(acordos);
                return Ok(fornecedorViewModel);
            }
            else
            {
                throw new BadRequestException("É necessário informar ao menos 14 números!");
            }
        }
    }
}
