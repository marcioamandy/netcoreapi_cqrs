using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels.Filters;
using Globo.PIC.Domain.Types.Queries;
using Microsoft.AspNetCore.Http;
using System;

namespace Globo.PIC.API.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[ApiController]
	[Route("pedidos")]
	[Authorize(Policy = "MatchProfile")]
	public class PedidosVeiculosController : ControllerBase
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger<PedidosVeiculosController> logger;

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
		public PedidosVeiculosController(ILogger<PedidosVeiculosController> _logger,
								IMapper _mapper,
								IMediator _mediator,
								IUserProvider _userProvider)
		{
			logger = _logger;
			mapper = _mapper;
			mediator = _mediator;
			userProvider = _userProvider;
		}

        #region Pedidos de Veículos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>   
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("veiculos")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<PedidoVeiculoViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPedidoVeiculoAsync([FromQuery] PedidoFilterViewModel filter, CancellationToken cancellationToken)
        {
            var pedidos = await mediator.Send(new ListByPedidoFilter()
            {
                Filter = filter
            }, cancellationToken);

            //Todo: fazer o pagging com query, primeiro Get/Count e depois lista;
            int contador = pedidos.Count;
            int.TryParse(filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;
            pedidos = pedidos.Skip((page - 1) * filter.PerPage).Take(filter.PerPage).ToList();

            if (pedidos.Count() == 0) throw new NotFoundException("Registro não encontrado.");

            var vm = mapper.Map<List<PedidoVeiculoViewModel>>(pedidos);

            var pagging = new GenericPaggingViewModel<PedidoVeiculoViewModel>(vm, filter, contador);

            return Ok(pagging);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos")]
		[ProducesResponseType(200, Type = typeof(PedidoVeiculoViewModel))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetPedidoVeiculoIdAsync([FromRoute] long id, CancellationToken cancellationToken)
		{

			if (id <= 0) throw new BadRequestException("O parametro id é requerido.");

			//var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);
			var pedido = await mediator.Send(new GetByIdPedidoWithOutRoles() { Id = id }, cancellationToken);

			if (pedido == null) throw new NotFoundException("Registro não encontrado.");

			var result = await mediator.Send(new VincularCompradorPedidoVeiculo()
			{
				IdPedido = id,
				IdStatus = (int)Domain.Enums.PedidoVeiculoStatus.PEDIDO_ENVIADO
			}, cancellationToken);

			PedidoVeiculoViewModel pedidoVM = mapper.Map<PedidoVeiculoViewModel>(pedido);

			return Ok(pedidoVM);
		}

		/// </summary>
		/// <param name="orderVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost("veiculos")]
		[ProducesResponseType(201, Type = typeof(PedidoVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostPedidoVeiculoAsync([FromBody] PedidoVeiculoViewModel pedidoVM, CancellationToken cancellationToken)
		{			
			if (pedidoVM == null)
				throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

			var pedidoVeiculo = mapper.Map<PedidoVeiculo>(pedidoVM);

			await mediator.Send(new CreatePedidoVeiculo() { PedidoVeiculo = pedidoVeiculo }, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoVeiculoViewModel>(pedidoVeiculo));
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos")]
		[ProducesResponseType(200, Type = typeof(PedidoItemVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutPedidoVeiculos(
			[FromRoute] long idItem,
			[FromBody] PedidoVeiculoViewModel pedidoViewModel,
			[FromRoute] long id,
			CancellationToken cancellationToken)
		{
			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			pedidoViewModel.Id = id;

			var pedido = mapper.Map<PedidoVeiculo>(pedidoViewModel);

			await mediator.Send(new UpdatePedidoVeiculo()
			{
				PedidoVeiculo = pedido
			}, cancellationToken);

			var result = mapper.Map<PedidoVeiculoViewModel>(pedido);

			return Ok(result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="changeStatusVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/mudar-status")]
		[ProducesResponseType(200, Type = typeof(ChangeStatusVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutChangeStatus(
			[FromRoute] long id,
			[FromBody] ChangeStatusVeiculoViewModel statusVM,
			CancellationToken cancellationToken
			)
		{
			if (id <= 0)
				throw new BadRequestException("O parametro id é requerido.");

			if (statusVM == null)
				throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

			if (statusVM.StatusId <= 0)
				throw new BadRequestException("O parametro status id é requerido.");

			await mediator.Send(new MudarStatusPedidoVeiculo()
			{
				IdPedido = id,
				JustificativaCancelamento = statusVM.JustificativaCancelamento,
				JustificativaDevolucao = statusVM.JustificativaDevolucao,
				IdStatus = statusVM.StatusId
			}, cancellationToken);

			return Ok(statusVM);
		}

		#endregion

		#region Acionamento
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>   
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/acionamentos")]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<AcionamentoViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetAcionamentoAsync(
			[FromRoute] long id,
			[FromQuery] AcionamentoFilterViewModel filter,
			CancellationToken cancellationToken)
		{
			if (id <= 0)
				throw new BadRequestException("O parametro id do pedido é requerido.");

			filter.Id = id;

			var pedidos = await mediator.Send<List<Acionamento>>(new ListByAcionamentoFilter()
			{
				id = id,
				Filter = filter
			}, cancellationToken);

			var vm = pedidos.Select(x => mapper.Map<AcionamentoViewModel>(x));

			if (pedidos.Count() == 0) throw new NotFoundException("Registro não encontrado.");

			var pagging = new GenericPaggingViewModel<AcionamentoViewModel>(vm, filter, pedidos.Count);

			return Ok(pagging);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>B
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/acionamentos/{idAcionamento:long}")]
		[ProducesResponseType(200, Type = typeof(AcionamentoViewModel))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetAcionamentoIdAsync(
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			CancellationToken cancellationToken)
		{

			if (id <= 0) throw new BadRequestException("O parametro id do pedido é requerido.");

			if (idAcionamento <= 0) throw new BadRequestException("O parametro id do acionamento é requerido.");

			var acionamento = await mediator.Send(new GetByIdAcionamento() { Id = idAcionamento }, cancellationToken);

			if (acionamento == null) throw new NotFoundException("Registro não encontrado.");

			var vm = mapper.Map<AcionamentoViewModel>(acionamento);

			return Ok(vm);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpDelete("{id:long}/veiculos/acionamentos/{idAcionamento:long}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeleteAcionamentoAsync(
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			CancellationToken cancellationToken)
		{

			if (id <= 0)
				throw new BadRequestException("O parametro id do pedido é requerido.");

			if (idAcionamento <= 0)
				throw new BadRequestException("O parametro id do acionamento é requerido.");

			var acionamento = await mediator.Send(new DeleteAcionamento()
			{
				id = idAcionamento,
				CancellationToken = cancellationToken
			});

			//Verificar o NotFound
			if (acionamento == null) return NotFound();

			//Verificar esse retorno
			return StatusCode(204);
		}

		/// </summary>
		/// <param name="orderVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost("{id:long}/veiculos/acionamentos")]
		[ProducesResponseType(201, Type = typeof(AcionamentoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostAcionamentoAsync(
			[FromRoute] long id,
			[FromBody] AcionamentoViewModel acionamentoVM,
			CancellationToken cancellationToken)
		{
			if (id <= 0)
				throw new BadRequestException("O parametro id do pedido é requerido.");

			if (acionamentoVM == null)
				throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

			/*Verificar o parâmetro de id do pedido dentro do commandHandler*/
			acionamentoVM.IdPedido = id;

			var acionamento = mapper.Map<Acionamento>(acionamentoVM);

			await mediator.Send(new CreateAcionamento() { Acionamento = acionamento }, cancellationToken);

			acionamento.IdPedido = id;

			return StatusCode(StatusCodes.Status201Created, mapper.Map<AcionamentoViewModel>(acionamento));
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/acionamentos/{idAcionamento:long}")]
		[ProducesResponseType(200, Type = typeof(AcionamentoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutAcionamentos(
			[FromBody] AcionamentoViewModel acionamentoViewModel,
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			CancellationToken cancellationToken)
		{

			if (id <= 0)
				throw new BadRequestException("O parametro id do pedido é requerido.");

			if (idAcionamento == 0)
				throw new BadRequestException("O parametro id do acionamento é requerido.");

			acionamentoViewModel.Id = idAcionamento;
			acionamentoViewModel.IdPedido = id;

			var acionamento = mapper.Map<Acionamento>(acionamentoViewModel);

			await mediator.Send(new UpdateAcionamento()
			{
				Acionamento = acionamento
			}, cancellationToken);

			var result = mapper.Map<AcionamentoViewModel>(acionamento);

			return Ok(result);
		}
		#endregion

		#region Acionamento Item

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>   
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/acionamentos/{idAcionamento:long}/itens")]
		[ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<AcionamentoItemViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetAcionamentoItemAsync(
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			[FromQuery] AcionamentoItemFilterViewModel filter,
			CancellationToken cancellationToken)
		{
			if (id <= 0)
				throw new BadRequestException("O parametro id do pedido é requerido.");

			if (idAcionamento <= 0)
				throw new BadRequestException("O parametro id do acionamento é requerido.");

			var acionamento = await mediator.Send<List<AcionamentoItem>>(new ListByAcionamentoItemFilter()
			{
				Filter = filter
			}, cancellationToken);

			var vm = acionamento.Select(x => mapper.Map<AcionamentoItemViewModel>(x));

			if (acionamento.Count() == 0) throw new NotFoundException("Registro não encontrado.");

			var pagging = new GenericPaggingViewModel<AcionamentoItemViewModel>(vm, filter, acionamento.Count);

			return Ok(pagging);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>B
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/acionamentos/{idAcionamento:long}/itens/{idItem:long}")]
		[ProducesResponseType(200, Type = typeof(AcionamentoItemViewModel))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetAcionamentoItemIdAsync(
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			[FromRoute] long idItem,
			CancellationToken cancellationToken)
		{

			if (id <= 0) throw new BadRequestException("O parametro id do pedido é requerido."); 

			if (idAcionamento <= 0) throw new BadRequestException("O parametro id do acionamento é requerido."); 

			if (idItem <= 0) throw new BadRequestException("O parametro id do item de acionamento é requerido."); 

			var acionamento = await mediator.Send(new GetByIdAcionamentoItem() { Id = id }, cancellationToken);

			if (acionamento == null) throw new NotFoundException("Registro não encontrado.");

			var vm = mapper.Map<AcionamentoItemViewModel>(acionamento);

			return Ok(vm);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpDelete("{id:long}/veiculos/acionamentos/{idAcionamento:long}/itens/{idItem:long}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeleteAcionamentoItemAsync(
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			[FromRoute] long idItem,
			CancellationToken cancellationToken)
		{

			if (id <= 0) throw new BadRequestException("O parametro id do pedido é requerido."); 

			if (idAcionamento <= 0) throw new BadRequestException("O parametro id do acionamento é requerido."); 

			if (idItem <= 0) throw new BadRequestException("O parametro id do item de acionamento é requerido.");

			var acionamento = await mediator.Send(new DeleteAcionamentoItem()
			{
				id = id,
				CancellationToken = cancellationToken
			});

			//Verificar o NotFound
			if (acionamento == null) return NotFound();

			//Verificar esse retorno
			return StatusCode(204);
		}

		/// </summary>
		/// <param name="orderVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost("{id:long}/veiculos/acionamentos/{idAcionamento:long}/itens")]
		[ProducesResponseType(201, Type = typeof(AcionamentoItemViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostAcionamentoItemAsync(
			[FromRoute] long id,
			[FromRoute] long idAcionamento,
			[FromBody] AcionamentoItemViewModel acionamentoItemVM,
			CancellationToken cancellationToken)
		{
			if (id <= 0) throw new BadRequestException("O parametro id do pedido é requerido."); 

			if (idAcionamento <= 0) throw new BadRequestException("O parametro id do acionamento é requerido."); 

			if (acionamentoItemVM == null)
				throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

			var acionamento = mapper.Map<AcionamentoItem>(acionamentoItemVM);

			await mediator.Send(new CreateAcionamentoItem() { AcionamentoItem = acionamento }, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<AcionamentoItemViewModel>(acionamento));
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/acionamentos/{idAcionamento:long}/itens/{idItem:long}")]
		[ProducesResponseType(200, Type = typeof(AcionamentoItemViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutAcionamentoItem(
			[FromRoute] long idItem,
			[FromRoute] long idAcionamento,
			[FromRoute] long id,
			[FromBody] AcionamentoItemViewModel acionamentoItemViewModel,
			CancellationToken cancellationToken)
		{
			if (id <= 0) throw new BadRequestException("O parametro id do pedido é requerido.");

			if (idAcionamento <= 0) throw new BadRequestException("O parametro id do acionamento é requerido.");

			if (idItem <= 0) throw new BadRequestException("O parametro id do item de acionamento é requerido.");

			acionamentoItemViewModel.Id = id;

			var acionamento = mapper.Map<AcionamentoItem>(acionamentoItemViewModel);

			await mediator.Send(new UpdateAcionamentoItem()
			{
				AcionamentoItem = acionamento
			}, cancellationToken);

			var result = mapper.Map<AcionamentoItemViewModel>(acionamento);

			return Ok(result);
		}

		#endregion

		#region PedidoItem

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/itens")]
		[ProducesResponseType(200, Type = typeof(List<PedidoItemVeiculoViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListItemByIdPedidoAsync([FromRoute] long id, [FromQuery] PedidoItemFilterViewModel filter, CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);

			if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

			var pedidoItens = await mediator.Send(new ListItemVeiculoByIdPedido() { IdPedido = id }, cancellationToken);

			if (pedidoItens.Count == 0) throw new NotFoundException("Registro não encontrado.");

			//pedidoItens = await mediator.Send(new ListItemVeiculoByIdPedido() { IdPedido = id }, cancellationToken);

			var pedidoItemViewModel = mapper.Map<List<PedidoItemVeiculoViewModel>>(pedidoItens);

			var pagging = new GenericPaggingViewModel<PedidoItemVeiculoViewModel>(pedidoItemViewModel, filter, pedidoItens.Count);

			return Ok(pagging);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/itens/{idItem:long}")]
		[ProducesResponseType(200, Type = typeof(PedidoItemVeiculoViewModel))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListItemByIdPedidoItemAsync([FromRoute] long id, [FromRoute] long idItem, [FromQuery] PedidoItemFilterViewModel filter, CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);

			if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

			var pedidoItens = await mediator.Send(new GetByIdPedidoItemVeiculo() { IdItem = idItem }, cancellationToken);

			if (pedidoItens == null) throw new NotFoundException("Registro não encontrado.");

			PedidoItemVeiculoViewModel pedidoItemVM = mapper.Map<PedidoItemVeiculoViewModel>(pedidoItens);

			//Veículo de Pesquisa / Veículo de Catálogo / Veículo de Empréstimo
			/*
			if (pedido.IdConteudo != pedidoItens.PedidoItem.Pedido.IdConteudo)
				pedidoItemVM.OrigemVeiculo = "Veículo de Empréstimo";
			else if (pedidoItemVM.IdItem == null)
				pedidoItemVM.OrigemVeiculo = "Veículo de Pesquisa";
			else
				pedidoItemVM.OrigemVeiculo = "Veículo de Catálogo";
			*/
			return Ok(pedidoItemVM);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/itens/{idItem:long}/opcoes")]
		[ProducesResponseType(200, Type = typeof(List<ItemVeiculoViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListOpcoesByItemAsync([FromRoute] long id, [FromRoute] long idItem, [FromQuery] PedidoItemFilterViewModel filter, CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			var opcoesItens = await mediator.Send(new ListItemVeiculoByPedidoItemVeiculo() { PedidoId = id, PedidoItemId = idItem }, cancellationToken);

			var pedidoItemVM = mapper.Map<List<ItemVeiculoViewModel>>(opcoesItens);

			return Ok(pedidoItemVM);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/itens/{idItem:long}/opcoes/{idOpcao:long}")]
		[ProducesResponseType(200, Type = typeof(ItemVeiculoViewModel))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListOpcoesByIdAsync([FromRoute] long id, [FromRoute] long idItem, [FromRoute] long idOpcao, CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			if (idOpcao == 0)
				throw new NotFoundException("Opção Id não identificado");

			var opcoesItens = await mediator.Send(new GetItemVeiculoById() { Id = idOpcao }, cancellationToken);

			var pedidoItemVM = mapper.Map<ItemVeiculoViewModel>(opcoesItens);

			return Ok(pedidoItemVM);

		}

		/// <summary>
		/// apaga um pedido item
		/// </summary>
		/// <param name="id">id do pedidoitem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		[HttpDelete("{id:long}/veiculos/itens/{idItem:long}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeletePedidoItemVeiculoAsync([FromRoute] long id, [FromRoute] long idItem, CancellationToken cancellationToken)
		{
			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			var pedidoItem = await mediator.Send(new DeletePedidoItemVeiculo()
			{
				id = id,
				idItem = idItem
			}, cancellationToken);

			return StatusCode(204);
		}

		/// <summary>
		/// apaga todos od itens do pedido
		/// </summary>
		/// <param name="id">id do pedidoitem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		[HttpDelete("{id:long}/veiculos/itens")]
		[ProducesResponseType(200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(422)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> DeletePedidoItensVeiculoByIdPedidoAsync([FromRoute] long id, CancellationToken cancellationToken)
		{
			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			await mediator.Send(new DeletePedidoItensVeiculo()
			{
				id = id
			}, cancellationToken);

			return StatusCode(204);
		}

		/// <summary>
		/// insere uma opcao ao item
		/// </summary>
		/// <returns></returns>
		[HttpPost("{id:long}/veiculos/itens/{idItem:long}/opcoes")]
		[ProducesResponseType(200, Type = typeof(ItemVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostOpcaoPedidoItemVeiculo(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromBody] ItemVeiculoViewModel opcaoViewModel,
			CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			var itemVeiculo = mapper.Map<ItemVeiculo>(opcaoViewModel);

			await mediator.Send(new CreateItemVeiculo()
			{
				PedidoId = id,
				PedidoItemId = idItem,
				ItemVeiculo = itemVeiculo
			}, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<ItemVeiculoViewModel>(itemVeiculo));
		}

		/// <summary>
		/// insere um pedido item
		/// </summary>
		/// <returns></returns>
		[HttpPost("{id:long}/veiculos/itens")]
		[ProducesResponseType(200, Type = typeof(PedidoItemVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PostPedidoItemVeiculo(
			[FromRoute] long id,
			[FromBody] PedidoItemVeiculoViewModel pedidoItemVeiculoViewModel,
			CancellationToken cancellationToken)
		{

			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);

			if (pedido == null) throw new NotFoundException("Registro não encontrado.");

			pedidoItemVeiculoViewModel.IdPedido = id;
			var pedidoItemVeiculo = mapper.Map<PedidoItemVeiculo>(pedidoItemVeiculoViewModel);

			await mediator.Send(new CreatedPedidoItemVeiculo()
			{
				PedidoItemVeiculo = pedidoItemVeiculo
			}, cancellationToken);

			return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoItemVeiculoViewModel>(pedidoItemVeiculo));
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="login"></param>
		/// <param name="UserVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/itens/{idItem:long}")]
		[ProducesResponseType(200, Type = typeof(PedidoItemVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutPedidoItemVeiculo(
			[FromRoute] long idItem,
			[FromBody] PedidoItemVeiculoViewModel pedidoItemViewModel,
			[FromRoute] long id,
			CancellationToken cancellationToken)
		{
			if (id == 0)
				throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			pedidoItemViewModel.IdPedido = id;
			pedidoItemViewModel.Id = idItem;

			var pedidoItem = mapper.Map<PedidoItemVeiculo>(pedidoItemViewModel);

			await mediator.Send(new UpdatePedidoItemVeiculo()
			{
				PedidoItemVeiculo = pedidoItem
			}, cancellationToken);

			var result = mapper.Map<PedidoItemVeiculoViewModel>(pedidoItem);

			return Ok(result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="changeStatusVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/itens/{idItem:long}/mudar-status")]
		[ProducesResponseType(200, Type = typeof(ChangeStatusItemVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutChangeStatusItem(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromBody] ChangeStatusItemVeiculoViewModel statusVM,
			CancellationToken cancellationToken
			)
		{
			if (id <= 0)
				throw new BadRequestException("O parametro id é requerido.");

			if (idItem <= 0)
				throw new BadRequestException("O parametro id do item é requerido.");

			if (statusVM == null)
				throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

			if (statusVM.StatusId <= 0)
				throw new BadRequestException("O parametro status id é requerido.");

			await mediator.Send(new MudarStatusPedidoItemVeiculo()
			{
				IdPedidoItem = idItem,
				JustificativaCancelamento = statusVM.JustificativaCancelamento,
				JustificativaDevolucao = statusVM.JustificativaDevolucao,
				IdStatus = statusVM.StatusId
			}, cancellationToken);

			return Ok(statusVM);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="changeStatusVM"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/itens/{idItem:long}/opcoes/reprovar-opcoes")]
		[ProducesResponseType(200, Type = typeof(List<ItemVeiculoViewModel>))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutReprovarItemOpcoes(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromBody] ReprovarOpcoesPedidoItemVeiculo reprovarVM,
			CancellationToken cancellationToken
			)
		{
			if (id <= 0)
				throw new BadRequestException("O parametro id é requerido.");

			if (idItem <= 0)
				throw new BadRequestException("O parametro id do item é requerido.");

			if (string.IsNullOrWhiteSpace(reprovarVM.JustificativaReprovacao))
				throw new BadRequestException("Justificativa para a reprovação é obrigatória");

			await mediator.Send(new ReprovarOpcoesPedidoItemVeiculo()
			{
				IdPedidoItem = idItem,
				JustificativaReprovacao = reprovarVM.JustificativaReprovacao
			}, cancellationToken);

			var opcoesItens = await mediator.Send(new ListItemVeiculoByPedidoItemVeiculo() {
				PedidoId = id,
				PedidoItemId = idItem
			}, cancellationToken);

			var pedidoItemVM = mapper.Map<List<ItemVeiculoViewModel>>(opcoesItens);

			return Ok(pedidoItemVM);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPut("{id:long}/veiculos/itens/{idItem:long}/opcoes/{idOpcao:long}/aprovar-opcoes")]
		[ProducesResponseType(200, Type = typeof(ItemVeiculoViewModel))]
		[ProducesResponseType(400, Type = typeof(string))]
		[ProducesResponseType(422, Type = typeof(string))]
		[ProducesResponseType(500)]
		public async Task<IActionResult> PutAprovarItemOpcoes(
			[FromRoute] long id,
			[FromRoute] long idItem,
			[FromRoute] long idOpcao,
			[FromBody] AprovarOpcaoPedidoItemVeiculo aprovarVM,
			CancellationToken cancellationToken
			)
		{
			if (idItem == 0)
				throw new NotFoundException("Pedido Item Id não identificado");

			if (idOpcao == 0)
				throw new NotFoundException("Opção Id não identificado");

			await mediator.Send(new AprovarOpcaoPedidoItemVeiculo()
			{
				IdPedido = id,
				IdPedidoItem = idItem,
				idOpcao = idOpcao,
				bloqueioEmprestimos = aprovarVM.bloqueioEmprestimos,
				justificativaBloqueio = aprovarVM.justificativaBloqueio
			}, cancellationToken);

			var opcoesItens = await mediator.Send(new GetItemVeiculoById() { Id = idOpcao }, cancellationToken);

			var pedidoItemVM = mapper.Map<ItemVeiculoViewModel>(opcoesItens);

			return Ok(pedidoItemVM);

		}
		
		/// <summary>
		/// 
		private PedidoVeiculoViewModel ToViewModel(PedidoVeiculo pedidoVeiculo)
		{
			var pedidoVM = mapper.Map<PedidoVeiculoViewModel>(pedidoVeiculo);
			return pedidoVM;
		}

		#endregion

		#region PedidoItemTracking
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet("{id:long}/veiculos/itens/{idItem:long}/tracking")]
		[ProducesResponseType(200, Type = typeof(List<TrackingVeiculoViewModel>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> ListItemTrackingByIdPedidoItemAsync(
			[FromRoute] long id,
			[FromRoute] long idItem,
			CancellationToken cancellationToken)
		{
			if (id == 0) throw new NotFoundException("Pedido Id não identificado");

			if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

			TrackingFilterViewModel filter = new TrackingFilterViewModel();
			filter.Id = idItem;

			var tracking = await mediator.Send(new ListTrackingVeiculo() { IdPedidoItem = idItem }, cancellationToken);
			//var tracking = TrackingVeiculoViewModel.GetAll().Where(s => s.CategoriaPai == null).ToList();

			if (tracking.Count == 0) throw new NotFoundException("Registro não encontrado.");

			var trackingViewModel = mapper.Map<List<TrackingVeiculoViewModel>>(tracking);

			trackingViewModel.Last().Current = true;

			var pagging = new GenericPaggingViewModel<TrackingVeiculoViewModel>(trackingViewModel, filter, tracking.Count);

			return Ok(pagging);

		}
		#endregion


	}
}
