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
using Microsoft.AspNetCore.Http;

namespace Globo.PIC.API.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[ApiController]
	[Route("pedidos")]
	[Authorize(Policy = "MatchProfile")]
	public class PedidosController : ControllerBase
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger<PedidosController> logger;

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
		public PedidosController(ILogger<PedidosController> _logger,
								IMapper _mapper,
								IMediator _mediator,
								IUserProvider _userProvider)
		{
			logger = _logger;
			mapper = _mapper;
			mediator = _mediator;
			userProvider = _userProvider;
		}

        #region Pedidos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>   
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<PedidoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPedidoAsync([FromQuery] PedidoFilterViewModel filter, CancellationToken cancellationToken)
        {
            var pedidos = await mediator.Send<List<Pedido>>(new ListByPedidoFilter()
            {
                Filter = filter
            }, cancellationToken);

            var vm = pedidos.Select(x => PedidoModelToViewModel(x));

            if (pedidos.Count() == 0) throw new NotFoundException("Registro não encontrado.");

            var pagging = new GenericPaggingViewModel<PedidoViewModel>(vm, filter, pedidos.Count);

            return Ok(pagging);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>B
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(PedidoViewModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPedidoIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {

            if (id <= 0) throw new BadRequestException("O parametro id é requerido.");

            var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);

            if (pedido == null) throw new NotFoundException("Registro não encontrado.");

			var vm = PedidoModelToViewModel(pedido);

			return Ok(vm);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpDelete("{id:long}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeletePedidoAsync([FromRoute] long id, CancellationToken cancellationToken)
		{

			if (id <= 0)
				throw new BadRequestException("O parametro id é requerido.");

			var pedido = await mediator.Send(new DeletePedido()
			{
				id = id,
				CancellationToken = cancellationToken
			});

			//Verificar o NotFound
			if (pedido == null) return NotFound();

			//Verificar esse retorno
			return StatusCode(204);
		}

		#endregion

		#region PedidoEquipe

		/// <summary>
		/// insere um pedido item
		/// </summary>
		/// <returns></returns>
		[HttpPost("{id:long}/equipe")]
		[ProducesResponseType(200, Type = typeof(PedidoEquipeViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostPedidoEquipe(
			[FromRoute] long id,
			[FromBody] PedidoEquipeViewModel pedidoEquipeViewModel,
			CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			pedidoEquipeViewModel.IdPedido = id;

			var pedidoEquipe = mapper.Map<Equipe>(pedidoEquipeViewModel);

			await mediator.Send(new  SavePedidoEquipe()
			{
				PedidoEquipe = pedidoEquipe
			}, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoEquipeViewModel>(pedidoEquipe));

		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost("{id:long}/equipe/bulk")]
		[ProducesResponseType(200, Type = typeof(List<PedidoEquipeViewModel>))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostPedidoEquipes(
			[FromRoute] long id,
			[FromBody] List<PedidoEquipeViewModel> pedidoEquipeViewModelList,
			CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			foreach (var pedido in pedidoEquipeViewModelList)
				pedido.IdPedido = id;

			var equipes = mapper.Map<List<Equipe>>(pedidoEquipeViewModelList);

			await mediator.Send(new SavePedidoEquipes()
			{
				PedidoEquipes = equipes
			}, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<List<PedidoEquipeViewModel>>(equipes));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/equipe")]
		[ProducesResponseType(200, Type = typeof(List<PedidoEquipeViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListEquipeByIdPedidoAsync([FromRoute] long id, CancellationToken cancellationToken)
		{

			var pedidoEquipe = await mediator.Send<List<Equipe>>(new ListByIdPedido() { IdPedido = id }, cancellationToken);

			if (pedidoEquipe.Count == 0) throw new NotFoundException("Registro não encontrado.");

			var pedidoEquipeViewModel = mapper.Map<List<PedidoEquipeViewModel>>(pedidoEquipe);

			return Ok(pedidoEquipeViewModel);

		}

		/// <summary>
		/// apaga um pedido item
		/// </summary>
		/// <param name="id">id do pedidoitem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		[HttpDelete("{id:long}/equipe/{login}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeletePedidoEquipeAsync([FromRoute] long id, [FromRoute] string login, CancellationToken cancellationToken)
		{

			if (id <= 0) return BadRequest("O parametro id é obrigatório.");

			if (string.IsNullOrWhiteSpace(login)) return BadRequest("O parametro login é obrigatório.");

			await mediator.Send(new DeletePedidoEquipe()
			{
				IdPedido = id,
				Login = login
			}, cancellationToken);

			return StatusCode(204);
		}

		#endregion

		#region PedidoItemConversa
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/itens/{idItem:long}/conversas")]
		[ProducesResponseType(200, Type = typeof(List<PedidoItemConversaViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListItemConversaByIdPedidoItemAsync([FromRoute] long id, [FromRoute] long idItem, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			PedidoItemConversaFilterViewModel filter = new PedidoItemConversaFilterViewModel();
			filter.Id = idItem;
			filter.IdPedido = id;

			var pedidoItemConversa = await mediator.Send<List<PedidoItemConversa>>(new ListItemConversaIdByIdPedidoItem() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);

			if (pedidoItemConversa.Count == 0) throw new NotFoundException("Registro não encontrado.");

			var pedidoItemConversaViewModel = mapper.Map<List<PedidoItemConversaViewModel>>(pedidoItemConversa);

			var pagging = new GenericPaggingViewModel<PedidoItemConversaViewModel>(pedidoItemConversaViewModel, filter, pedidoItemConversa.Count);

			return Ok(pagging);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/itens/{idItem:long}/conversas/{idConversa:long}")]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<PedidoItemConversaViewModel>>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetItemConversaByIdAsync([FromRoute] long id, [FromRoute] long idItem, [FromRoute] long idConversa, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			if (idConversa == 0) throw new NotFoundException("Pedido Item Conversa Id não identificado");

			var pedidoItemConversa = await mediator.Send(new GetByIdPedidoItemConversa()
			{
				IdPedido = id,
				IdPedidoItem = idItem,
				Id = idConversa
			}, cancellationToken);

			if (pedidoItemConversa == null) throw new NotFoundException("Registro não encontrado.");

			var itemConversaViewModel = mapper.Map<PedidoItemConversaViewModel>(pedidoItemConversa);

			return Ok(itemConversaViewModel);
		}

		/// <summary>
		/// insere um pedido item
		/// </summary>
		/// <returns></returns>
		[HttpPost("{id:long}/itens/{idItem:long}/conversas")]
		[ProducesResponseType(200, Type = typeof(PedidoItemConversaViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostPedidoItemConversa([FromRoute] long id, [FromRoute] long idItem, [FromBody] PedidoItemConversaViewModel pedidoItemConversaViewModel, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			pedidoItemConversaViewModel.IdPedidoItem = idItem;
			var pedidoItemConversa = mapper.Map<PedidoItemConversa>(pedidoItemConversaViewModel);

			await mediator.Send(new SavePedidoItemConversa()
			{
				PedidoItemConversa = pedidoItemConversa
			}, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoItemConversaViewModel>(pedidoItemConversa));

		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost("{id:long}/itens/{idItem:long}/conversas/bulk")]
		[ProducesResponseType(200, Type = typeof(List<PedidoItemConversaViewModel>))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostPedidoItemConversaBulk(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromBody] List<PedidoItemConversaViewModel> pedidoItemConversaViewModelList,
			CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			foreach (var pedidoItemConversa in pedidoItemConversaViewModelList)
				pedidoItemConversa.IdPedidoItem = idItem;

			var pedidoItemConversas = mapper.Map<List<PedidoItemConversa>>(pedidoItemConversaViewModelList);

			await mediator.Send(new SavePedidoItemConversas()
			{
				PedidoItemConversas = pedidoItemConversas
			}, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<List<PedidoItemConversaViewModel>>(pedidoItemConversas));
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/itens/{idItem:long}/conversas/{idConversa:long}")]
		[ProducesResponseType(200, Type = typeof(PedidoItemConversaViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutPedidoItemConversa(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromRoute] long idConversa,
			[FromBody] PedidoItemConversaViewModel pedidoItemConversaViewModel,

			CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			if (idConversa == 0) throw new NotFoundException("Pedido Item Conversa Id não identificado");

			pedidoItemConversaViewModel.IdPedidoItem = idItem;
			pedidoItemConversaViewModel.Id = idConversa;
			var pedidoItemConversa = mapper.Map<PedidoItemConversa>(pedidoItemConversaViewModel);

			await mediator.Send(new UpdatePedidoItemConversa()
			{
				PedidoItemConversa = pedidoItemConversa
			}, cancellationToken);

			var result = mapper.Map<PedidoItemConversaViewModel>(pedidoItemConversa);

			return Ok(result);
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/itens/{idItem:long}/conversas/bulk")]
		[ProducesResponseType(200, Type = typeof(List<PedidoItemConversaViewModel>))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutPedidoItemConversaBulk(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromBody] List<PedidoItemConversaViewModel> pedidoItemConversaViewModelList,
			CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			foreach (var pedidoItemConversas in pedidoItemConversaViewModelList)
				pedidoItemConversas.IdPedidoItem = idItem;

			var pedidoItemConversa = mapper.Map<List<PedidoItemConversa>>(pedidoItemConversaViewModelList);

			await mediator.Send(new UpdatePedidoItemConversas()
			{
				PedidoItemConversas = pedidoItemConversa
			}, cancellationToken);

			var result = mapper.Map<List<PedidoItemConversaViewModel>>(pedidoItemConversa);

			return Ok(result);
		}

		/// <summary>
		/// apaga um pedido item
		/// </summary>
		/// <param name="id">id do pedidoitem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		[HttpDelete("{id:long}/itens/{idItem:long}/conversas/{idConversa:long}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeletePedidoItemConversaAsync([FromRoute] long id, [FromRoute] long idItem, [FromRoute] long idConversa, CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			if (idConversa == 0) throw new NotFoundException("Pedido Item Conversa Id não identificado");

			var pedidoItemConversa = await mediator.Send(new DeletePedidoItemConversa()
			{
				Id = idConversa,
				IdPedidoItem = idItem
			}, cancellationToken);

			return StatusCode(204);
		}
		#endregion

		PedidoViewModel PedidoModelToViewModel(Pedido pedido)
		{
			if (pedido.PedidoArte != null)
				return mapper.Map<PedidoArteViewModel>(pedido);
			else if (pedido.PedidoVeiculo != null)			
				return mapper.Map<PedidoVeiculoViewModel>(pedido);			
			else
				return mapper.Map<PedidoViewModel>(pedido);
		}
	}
}
