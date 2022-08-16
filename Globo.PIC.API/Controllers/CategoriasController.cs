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
using Globo.PIC.Domain.ViewModels.Filters;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.API.Controllers
{
	/// <summary>
	/// s
	/// </summary>
	[ApiController]
	[Route("categorias")]
	[Authorize(Policy = "MatchProfile")]
	public class CategoriasController : ControllerBase
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger<CategoriasController> logger;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Categoria> categoriaRepository;

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
		public CategoriasController(
								IRepository<Categoria> _categoriaRepository,
								ILogger<CategoriasController> _logger,
								IMapper _mapper,
								IMediator _mediator,
								IUserProvider _userProvider)
		{
			categoriaRepository = _categoriaRepository;
			logger = _logger;
			mapper = _mapper;
			mediator = _mediator;
			userProvider = _userProvider;
		}

        #region TipoVeiculos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>   
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ///

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<CategoriaViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetCategoria([FromQuery] CategoriasFilterViewModel filter, CancellationToken cancellationToken)
        {
            var categoriaVeiculos = categoriaRepository.GetAll().Where(s => s.CategoriaPai == null).ToList();

            if (categoriaVeiculos == null && categoriaVeiculos.Count <= 0) throw new NotFoundException("Registro não encontrado.");

            var categoriaVeiculosViewModel = mapper.Map<List<CategoriaViewModel>>(categoriaVeiculos);

            return Ok(categoriaVeiculosViewModel);
        }

        [HttpGet("{id:long}")]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<SubCategoriaViewModel>>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetCategoriaIdAsync([FromRoute] long id, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Id não identificado");

			var categoriaVeiculos = await mediator.Send(new GetByIdCategoriaVeiculos()
			{
				IdCategoria = id
			}, cancellationToken);

			if (categoriaVeiculos == null) throw new NotFoundException("Registro não encontrado.");

			var categoriaVeiculosViewModel = mapper.Map<SubCategoriaViewModel>(categoriaVeiculos);

			return Ok(categoriaVeiculosViewModel);
		}
		
		[HttpGet("{id:long}/subcategorias")]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<SubCategoriaViewModel>>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetSubCategoriaAsync([FromRoute] long id, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Id não identificado");

			var categoriaVeiculos = await mediator.Send(new GetByIdCategoriaVeiculos()
			{
				IdCategoria = id
			}, cancellationToken);

			if (categoriaVeiculos == null) throw new NotFoundException("Registro não encontrado.");

			var categoriaVeiculosViewModel = mapper.Map<SubCategoriaViewModel>(categoriaVeiculos);

			return Ok(categoriaVeiculosViewModel);
		}

		[HttpGet("{id:long}/subcategorias/{idCategoria:long}")]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<SubCategoriaViewModel>>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetSubCategoriaByIdAsync([FromRoute] long id, [FromRoute] long idCategoria, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Id não identificado");

			if (idCategoria == 0) throw new NotFoundException("Categoria Id não identificado");

			var categoriaVeiculos = await mediator.Send(new GetByIdCategoriaVeiculos()
			{
				IdCategoria = idCategoria
			}, cancellationToken);

			if (categoriaVeiculos == null) throw new NotFoundException("Registro não encontrado.");

			var categoriaVeiculosViewModel = mapper.Map<SubCategoriaViewModel>(categoriaVeiculos);

			return Ok(categoriaVeiculosViewModel);
		}
		
		#endregion
	}
}
