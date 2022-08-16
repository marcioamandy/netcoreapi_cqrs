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
using Globo.PIC.Domain.Types.Events;
using Microsoft.AspNetCore.Http;
using Globo.PIC.Domain.Enums;

namespace Globo.PIC.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("pedidos")]
    [Authorize(Policy = "MatchProfile")]
    public class PedidosArtesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PedidosArtesController> logger;

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
        public PedidosArtesController(ILogger<PedidosArtesController> _logger,
                                IMapper _mapper,
                                IMediator _mediator,
                                IUserProvider _userProvider)
        {
            logger = _logger;
            mapper = _mapper;
            mediator = _mediator;
            userProvider = _userProvider;
        }

        #region Pedidos de Compras

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>   
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("artes")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<PedidoArteViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPedidoAsync([FromQuery] PedidoFilterViewModel filter, CancellationToken cancellationToken)
        {
            var pedidos = await mediator.Send<List<Pedido>>(new ListByPedidoFilter()
            {
                Filter = filter
            }, cancellationToken);

            //Todo: fazer o pagging com query, primeiro Get/Count e depois lista;
            int contador = pedidos.Count;
            int.TryParse(filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;
            pedidos = pedidos.Skip((page - 1) * filter.PerPage).Take(filter.PerPage).ToList();

            if (pedidos.Count() == 0) throw new NotFoundException("Registro não encontrado.");

            //var pedidoViewModel = mapper.Map<List<PedidoViewModel>>(pedidos);

            var pagging = new PedidoArteViewModel[] { }; //new GenericPaggingViewModel<CompraViewModel>(vm, filter, contador);

            return Ok(pagging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}/artes")]
        [ProducesResponseType(200, Type = typeof(PedidoArteViewModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPedidoIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {

            if (id <= 0) throw new BadRequestException("O parametro id é requerido.");

            var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);

            if (pedido == null) throw new NotFoundException("Registro não encontrado.");

            PedidoArteViewModel pedidoVM = mapper.Map<PedidoArteViewModel>(pedido);

            return Ok(pedidoVM);
        }

        /// </summary>
        /// <param name="orderVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("artes")]
        [ProducesResponseType(201, Type = typeof(PedidoArteViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoAsync([FromBody] PedidoArteViewModel pedidoVM, CancellationToken cancellationToken)
        {

            if (pedidoVM == null)
                throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

            var pedido = mapper.Map<PedidoArte>(pedidoVM);

            await mediator.Send(new CreatePedidoArte() { PedidoArte = pedido }, cancellationToken);

            if (pedido.Id <= 0)
                throw new BadRequestException("Falha ao criar pedido!");

            return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoArteViewModel>(pedido));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="UserVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/artes")]
        [ProducesResponseType(200, Type = typeof(PedidoArteViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoAsync([FromRoute] long id, [FromBody] PedidoArteViewModel pedidoVM, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new BadRequestException("O parametro id é requerido.");

            if (pedidoVM == null)
                throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

            pedidoVM.Id = id;

            var pedidoArte = mapper.Map<PedidoArte>(pedidoVM);

            pedidoArte.IdPedido = id;

            await mediator.Send(new UpdatePedidoArte()
            {
                PedidoArte = pedidoArte,
                LoginComprador = pedidoVM.CompradoPorLogin
            }, cancellationToken);

            return Ok(ToViewModel(pedidoArte));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changeStatusVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/artes/mudar-status")]
        [ProducesResponseType(200, Type = typeof(ChangeStatusViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutChangeStatus(
            [FromRoute] long id,
            [FromBody] ChangeStatusViewModel statusVM,
            CancellationToken cancellationToken
            )
        {
            if (id <= 0) throw new BadRequestException("O parametro id é requerido.");

            if (statusVM == null) throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

            if (statusVM.StatusId <= 0) throw new BadRequestException("O parametro status id é requerido.");

            var status = await mediator.Send(new GetStatusPedidoArteById() { Id = statusVM.StatusId }, cancellationToken);

            if (status == null) throw new NotFoundException("Registro do status não encontrado.");

            var pedido = await mediator.Send<Pedido>(new GetByIdPedidoWithOutRoles() { Id = id }, cancellationToken);

            if (pedido == null) throw new NotFoundException("Registro não encontrado.");

            if (pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_CANCELADO)
            {
                switch (statusVM.StatusId)
                {
                    case (int)PedidoArteStatus.PEDIDO_CANCELADO:
                        {
                            if (userProvider.User.Login == pedido.CriadoPorLogin)
                            {
                                pedido.JustificativaCancelamento = statusVM.JustificativaCancelamento;

                                await mediator.Publish(new OnCancelamentoPedidoSolicitado() { Pedido = pedido });
                            }
                            else
                            {
                                if (userProvider.User.Login == pedido.PedidoArte.BaseLogin) // Não foi solicitado o cancelamento de todos os itens no pedido para comprador, caso mude tem que adicionar aqui
                                {
                                    if (!string.IsNullOrWhiteSpace(statusVM.JustificativaCancelamento))
                                        pedido.JustificativaCancelamento = statusVM.JustificativaCancelamento;

                                    await mediator.Publish(new OnCancelamentoPedidoSolicitado() { Pedido = pedido });
                                }
                            }

                            break;
                        }
                    case (int)PedidoArteStatus.PEDIDO_DEVOLVIDO:
                        {
                            pedido.JustificativaDevolucao = statusVM.JustificativaDevolucao;

                            await mediator.Publish(new OnDevolucaoPedido() { Pedido = pedido });

                            break;
                        }
                    case (int)PedidoArteStatus.PEDIDO_APROVADO:
                        {
                            await mediator.Publish(new OnAprovacaoPedido() { Pedido = pedido });

                            break;
                        }
                    case (int)PedidoArteStatus.PEDIDO_ENVIADO:
                        {

                            if (pedido.Itens.Any(s => s.DataNecessidade.HasValue == false))
                                throw new BadRequestException("A Data Necessidade do Pedido deve ser preenchida.");

                            if (userProvider.User.UnidadeNegocio == null)
                                throw new BadRequestException(
                                    "Enviar o pedido requer que o usuário tenha uma Unidade de Negócio previamente configurada.");

                            pedido.PedidoArte.IdStatus = statusVM.StatusId;
                            pedido.PedidoArte.Status = status;

                            await mediator.Send(new UpdatePedido()
                            {
                                Pedido = pedido
                            }, cancellationToken);

                            var itens = pedido.Itens.Where(
                                a => !string.IsNullOrWhiteSpace(a.RCs.FirstOrDefault().Acordo) &&
                                a.RCs.FirstOrDefault().AcordoId.HasValue &&
                                a.RCs.FirstOrDefault().AcordoLinhaId.HasValue);

                            //Agrupa para criar uma RC por acordo.
                            var gruposItens = itens.GroupBy(x => x.RCs.FirstOrDefault()?.Acordo);

                            foreach (var grupo in gruposItens)
                                    await mediator.Send(new SendRC() { PedidoItens = grupo.ToArray() }, cancellationToken);

                            await mediator.Publish(new OnNovoPedidoArte()
                            {
                                Pedido = pedido
                            }, cancellationToken);

                            break;
                        }
                    default:
                        {
                            pedido.PedidoArte.IdStatus = statusVM.StatusId;
                            pedido.PedidoArte.Status = status;

                            await mediator.Send(new UpdatePedido()
                            {
                                Pedido = pedido
                            }, cancellationToken);

                            break;
                        }
                }
            }
            else
                throw new BadRequestException(string.Format("Pedido {0} já está cancelado e não é permitida nenhuma modificação.", pedido.Id));

            return Ok(statusVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changeStatusVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/artes/itens/mudar-status")]
        [ProducesResponseType(200, Type = typeof(ChangeStatusItensViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutChangeStatusItens(
            [FromRoute] long id,
            [FromBody] ChangeStatusItensViewModel statusVM,
            CancellationToken cancellationToken
            )
        {
            if (id <= 0)
                throw new BadRequestException("O parametro id é requerido.");

            if (statusVM.IdItens.Count() == 0)
                throw new NotFoundException("Pedido Item Id não identificado");

            if (statusVM == null)
                throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

            if (statusVM.StatusId <= 0)
                throw new BadRequestException("O parametro status id é requerido.");

            var request = new MudarStatusPedidoItemArte()
            {
                IdPedido = id,
                IdPedidoItens = statusVM.IdItens,
                IdStatus = statusVM.StatusId,
                JustificativaCancelamento = statusVM.JustificativaCancelamento,
                JustificativaDevolucao = statusVM.JustificativaDevolucao
            };

            await mediator.Send(request, cancellationToken);

            statusVM.StatusId = request.IdStatus;

            return Ok(statusVM);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="changeStatusVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpPut("{id:long}/artes/devolver")]
        //[ProducesResponseType(200, Type = typeof(List<PedidoItemArteDevolucaoViewModel>))]
        //[ProducesResponseType(400, Type = typeof(string))]
        //[ProducesResponseType(422, Type = typeof(string))]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> PutDevolver(
        //	[FromRoute] long id,
        //	[FromBody] PedidoItemDevolucaoFilterViewModel devolverVM,
        //	CancellationToken cancellationToken
        //	)
        //{
        //	if (id <= 0)
        //		throw new BadRequestException("Pedido Id não identificado.");

        //	PedidoItemArteDevolucaoViewModel pedidoItemDevolucao = new PedidoItemArteDevolucaoViewModel();
        //	pedidoItemDevolucao.idTipo = devolverVM.idTipo;
        //	pedidoItemDevolucao.Justificativa = devolverVM.Justificativa;
        //	/*
        //	foreach (var devolver in devolverVM)
        //	{
        //		pedidoItemDevolucao.Add(new PedidoItemDevolucaoViewModel()
        //		{
        //			idTipo = devolver.idTipo,
        //			Justificativa = devolver.Justificativa
        //		});
        //	}
        //	*/
        //	var pIDevolucao = mapper.Map<PedidoItemArteDevolucao>(pedidoItemDevolucao);

        //	await mediator.Send(new UpdatePedidoItensDevolucao()
        //	{
        //		PedidoItemDevolucao = pIDevolucao,
        //		IdPedido = id
        //	}, cancellationToken);

        //	return Ok(devolverVM);

        //}

        // POST api/orders
        /// <summary>
        ///

        private PedidoArteViewModel ToViewModel(PedidoArte pedido)
        {
            var pedidoVM = mapper.Map<PedidoArteViewModel>(pedido);
            return pedidoVM;
        }

        #endregion

        #region PedidoItem

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}/artes/itens")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListItemByIdPedidoAsync([FromRoute] long id, [FromQuery] PedidoItemFilterViewModel filter, CancellationToken cancellationToken)
        {

            if (id == 0)
                throw new NotFoundException("Pedido Id não identificado");

            var pedido = await mediator.Send(new GetByIdPedido() { Id = id }, cancellationToken);

            if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

            var pedidoItensArte = await mediator.Send(new ListItemArteByIdPedido() { IdPedido = id }, cancellationToken);

            if (pedidoItensArte.Count == 0) throw new NotFoundException("Registro não encontrado.");

            var pedidoItemViewModel = mapper.Map<List<PedidoItemArteViewModel>>(pedidoItensArte);

            var pagging = new GenericPaggingViewModel<PedidoItemArteViewModel>(pedidoItemViewModel, filter, pedidoItensArte.Count);

            return Ok(pagging);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<PedidoItemArteViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetItemPedidoByIdAsync([FromRoute] long id, [FromRoute] long idItem, CancellationToken cancellationToken)
        {
            var pedidoItem = await mediator.Send(new GetByIdPedidoItemArte()
            {
                IdPedido = id,
                Id = idItem
            }, cancellationToken);

            if (pedidoItem == null) throw new NotFoundException("Registro não encontrado.");

            var itemViewModel = mapper.Map<PedidoItemArteViewModel>(pedidoItem);

            return Ok(itemViewModel);
        }

        /// <summary>
        /// insere um pedido item
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id:long}/artes/itens")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItem(
            [FromRoute] long id,
            [FromBody] PedidoItemArteViewModel pedidoItemViewModel,
            CancellationToken cancellationToken)
        {

            if (id <= 0) throw new BadRequestException("O parametro id é requerido.");

            pedidoItemViewModel.IdPedido = id;

            var pedidoItem = mapper.Map<PedidoItemArte>(pedidoItemViewModel);

            await mediator.Send(new CreatePedidoItemArte()
            {
                PedidoItemArte = pedidoItem
            }, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoItemArteViewModel>(pedidoItem));

        }

        /// <summary>
        /// atualiza um pedido item
        /// </summary>
        /// <param name="login"></param>
        /// <param name="UserVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItem(
            [FromRoute] long idItem,
            [FromBody] PedidoItemArteViewModel pedidoItemViewModel,
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0)
                throw new NotFoundException("Pedido Item Id não identificado");

            pedidoItemViewModel.IdPedido = id;
            pedidoItemViewModel.Id = idItem;

            var pedidoItem = mapper.Map<PedidoItemArte>(pedidoItemViewModel);

            await mediator.Send(new UpdatePedidoItemArte()
            {
                PedidoItemArte = pedidoItem
            }, cancellationToken);

            var result = mapper.Map<PedidoItemArteViewModel>(pedidoItem);

            return Ok(result);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="changeStatusVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/mudar-status")]
        [ProducesResponseType(200, Type = typeof(ChangeStatusItemViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutChangeStatusItem(
            [FromRoute] long idItem,
            [FromRoute] long id,
            [FromBody] ChangeStatusItemViewModel statusVM,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new BadRequestException("O parametro id é requerido.");

            if (idItem == 0)
                throw new NotFoundException("Pedido Item Id não identificado");

            if (statusVM == null)
                throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

            if (statusVM.StatusId <= 0)
                throw new BadRequestException("O parametro status id é requerido.");

            var request = new MudarStatusPedidoItemArte()
            {
                IdPedido = id,
                IdPedidoItens = new long[] { idItem },
                IdStatus = statusVM.StatusId,
                JustificativaCancelamento = statusVM.JustificativaCancelamento,
                JustificativaDevolucao = statusVM.JustificativaDevolucao
            };

            await mediator.Send(request, cancellationToken);

            statusVM.StatusId = request.IdStatus;

            return Ok(statusVM);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="changeStatusVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/atribuir-comprador")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteAtribuicaoViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutChangeCompradorItem(
            [FromRoute] long idItem,
            [FromRoute] long id,
            [FromBody] PedidoItemArteAtribuicaoViewModel compradorVM,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new BadRequestException("O parametro id é requerido.");

            if (idItem == 0)
                throw new NotFoundException("Pedido Item Id não identificado");

            if (compradorVM == null)
                throw new BadRequestException("O objeto enviado não corresponde ao formato esperado.");

            var request = new MudarCompradorPedidoItemArte()
            {
                IdPedido = id,
                IdPedidoItem = idItem,
                IdTipo = compradorVM.IdTipo,
                Comprador = compradorVM.Comprador,
                CompradorAnterior = compradorVM.CompradorAnterior,
                Justificativa = compradorVM.Justificativa
            };

            await mediator.Send(request, cancellationToken);

            return Ok(compradorVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changeStatusVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/devolver")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteDevolucaoViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutDevolverItem(
            [FromRoute] long idItem,
            [FromRoute] long id,
            [FromBody] PedidoItemDevolucaoFilterViewModel devolverVM,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new BadRequestException("Pedido Id não identificado.");

            if (idItem == 0)
                throw new NotFoundException("Pedido Item Id não identificado");

            PedidoItemArteDevolucaoViewModel pedidoItemDevolucao = new PedidoItemArteDevolucaoViewModel();

            pedidoItemDevolucao.idTipo = devolverVM.idTipo;
            pedidoItemDevolucao.Justificativa = devolverVM.Justificativa;
            pedidoItemDevolucao.IdPedidoItemOriginal = idItem;

            var pIDevolucao = mapper.Map<PedidoItemArteDevolucao>(pedidoItemDevolucao);

            await mediator.Send(new UpdatePedidoItemArteDevolucao()
            {
                PedidoItemDevolucao = pIDevolucao,
                IdPedido = id,
                IdPedidoItem = idItem
            }, cancellationToken);

            return Ok(devolverVM);
        }

        ///// <summary>
        ///// apaga um pedido item
        ///// </summary>
        ///// <param name="id">id do pedidoitem</param>
        ///// <param name="cancellationToken">cancellation token</param>
        ///// <returns></returns>
        [HttpDelete("{id:long}/artes/itens/{idItem:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePedidoItemArteAsync([FromRoute] long id, [FromRoute] long idItem, CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0)
                throw new NotFoundException("Pedido Item Id não identificado");

            await mediator.Send(new DeletePedidoItemArte()
            {
                IdPedidoItem = idItem,
                IdPedido = id
            }, cancellationToken);

            return StatusCode(204);
        }

        ///// <summary>
        ///// apaga items de um pedido
        ///// </summary>
        ///// <param name="id">id do pedidoitem</param>
        ///// <param name="cancellationToken">cancellation token</param>
        ///// <returns></returns>
        [HttpDelete("{id:long}/artes/itens")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePedidoItensArteAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new NotFoundException("Pedido Id não identificado");

            await mediator.Send(new DeleteAllPedidoItemArte()
            {
                IdPedido = id
            }, cancellationToken);

            return StatusCode(204);
        }

        #endregion

        #region PedidoItemTracking

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/tracking")]
        [ProducesResponseType(200, Type = typeof(List<TrackingArteViewModel>))]
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

            var tracking = await mediator.Send<List<PedidoItemArteTracking>>(
                new ListTrackingByIdItemPedido() { IdPedidoItem = idItem }, cancellationToken);

            if (tracking.Count == 0) throw new NotFoundException("Registro não encontrado.");

            var trackingViewModel = mapper.Map<List<TrackingArteViewModel>>(tracking);

            trackingViewModel.Last().Current = true;

            var pagging = new GenericPaggingViewModel<TrackingArteViewModel>(trackingViewModel, filter, tracking.Count);

            return Ok(pagging);
        }

        #endregion

        #region PedidoItemCompras

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/compras")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteCompraViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListItemCompraByIdPedidoItemAsync([FromRoute] long id, [FromRoute] long idItem, CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            PedidoItemCompraFilterViewModel filter = new PedidoItemCompraFilterViewModel();

            filter.Id = idItem;
            filter.IdPedido = id;

            var pedidoItemCompra = await mediator.Send<List<PedidoItemArteCompra>>(new ListByIdPedidoItemArteCompra() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);

            if (pedidoItemCompra.Count == 0) throw new NotFoundException("Registro não encontrado.");

            var pedidoItemCompraViewModel = mapper.Map<List<PedidoItemArteCompraViewModel>>(pedidoItemCompra);

            foreach (var currentCompras in pedidoItemCompraViewModel)
            {
                currentCompras.Entregas = await mediator.Send(new GetByIdPedidoItemCompraExistEntrega()
                {
                    IdPedidoItem = idItem
                }, cancellationToken);
            }

            var pagging = new GenericPaggingViewModel<PedidoItemArteCompraViewModel>(pedidoItemCompraViewModel, filter, pedidoItemCompra.Count);

            return Ok(pagging);

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<PedidoItemArteCompraViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetItemCompraByIdAsync([FromRoute] long id, [FromRoute] long idItem, [FromRoute] long idCompra, CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Compra Id não identificado");

            var pedidoItemCompra = await mediator.Send(new GetByIdPedidoItemArteCompra()
            {
                IdPedidoItem = idItem
            }, cancellationToken);

            if (pedidoItemCompra == null) throw new NotFoundException("Registro não encontrado.");

            var itemCompraViewModel = mapper.Map<PedidoItemArteCompraViewModel>(pedidoItemCompra);

            itemCompraViewModel.Entregas = await mediator.Send(new GetByIdPedidoItemCompraExistEntrega()
            {
                IdPedido = id,
                IdPedidoItem = idItem,
                Id = idCompra
            }, cancellationToken);

            return Ok(itemCompraViewModel);
        }

        ///// <summary>
        ///// insere um pedido item
        ///// </summary>
        ///// <returns></returns>
        [HttpPost("{id:long}/artes/itens/{idItem:long}/compras")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteCompraViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItemArteCompra([FromRoute] long id, [FromRoute] long idItem, [FromBody] PedidoItemArteCompraViewModel pedidoItemArteCompraViewModel, CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            var pedidoItemArteCompra = mapper.Map<PedidoItemArteCompra>(pedidoItemArteCompraViewModel);

            await mediator.Send(new CreatePedidoItemArteCompra()
            {
                IdPedidoItem = idItem,
                PedidoItemArteCompra = pedidoItemArteCompra
            }, cancellationToken);

            var vm = mapper.Map<PedidoItemArteCompraViewModel>(pedidoItemArteCompra);

            vm.IdPedidoItem = idItem;

            return StatusCode(StatusCodes.Status201Created, vm);

        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPost("{id:long}/artes/itens/{idItem:long}/compras/bulk")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteCompraViewModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItemArtesCompraBulk(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromBody] List<PedidoItemArteCompraViewModel> pedidoItemArteCompraViewModelList,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Arte Id não identificado");

            foreach (var pedidoItemCompra in pedidoItemArteCompraViewModelList)
                pedidoItemCompra.IdPedidoItem = idItem;

            var pedidoItemArteCompras = mapper.Map<List<PedidoItemArteCompra>>(pedidoItemArteCompraViewModelList);

            await mediator.Send(new CreatePedidoItemArteCompras()
            {
                PedidoItemArteCompras = pedidoItemArteCompras
            }, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, mapper.Map<List<PedidoItemArteCompraViewModel>>(pedidoItemArteCompras));

        }

        /// <summary>
        /// atualiza um pedido item
        /// </summary>
        /// <param name="login"></param>
        /// <param name="UserVM"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteCompraViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItemArteCompra(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromBody] PedidoItemArteCompraViewModel pedidoItemCompraViewModel,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Arte Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Arte Item Compra Id não identificado");

            pedidoItemCompraViewModel.IdPedidoItem = idItem;
            pedidoItemCompraViewModel.Id = idCompra;
            var pedidoItemArteCompra = mapper.Map<PedidoItemArteCompra>(pedidoItemCompraViewModel);

            await mediator.Send(new UpdatePedidoItemArteCompra()
            {
                PedidoItemArteCompra = pedidoItemArteCompra
            }, cancellationToken);

            var result = mapper.Map<PedidoItemArteCompraViewModel>(pedidoItemArteCompra);

            return Ok(result);
        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/compras/bulk")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteCompraViewModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItemArteCompraBulk(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromBody] List<PedidoItemArteCompraViewModel> pedidoItemCompraViewModelList,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            foreach (var pedidoItemCompras in pedidoItemCompraViewModelList)
                pedidoItemCompras.IdPedidoItem = idItem;

            var pedidoItemArteCompras = mapper.Map<List<PedidoItemArteCompra>>(pedidoItemCompraViewModelList);

            await mediator.Send(new UpdatePedidoItemArteCompras()
            {
                PedidoItemArteCompras = pedidoItemArteCompras
            }, cancellationToken);

            var result = mapper.Map<List<PedidoItemArteCompraViewModel>>(pedidoItemArteCompras);

            return Ok(result);
        }

        /// <summary>
        /// apaga um pedido item arte compra
        /// </summary>
        /// <param name="id">id do pedidoitem</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        [HttpDelete("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePedidoItemArteCompraAsync([FromRoute] long id, [FromRoute] long idItem, [FromRoute] long idCompra, CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Arte Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Arte Compra Id não identificado");

            var pedidoItemArteCompra = await mediator.Send(new DeletePedidoItemArteCompra()
            {
                Id = idCompra,
                IdPedidoItemArte = idItem
            }, cancellationToken);

            return StatusCode(204);
        }

        #endregion

        #region PedidoItemComprasDocumentos

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteCompraDocumentosViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListItemCompraDocumentosByIdPedidoItemAsync(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Compra Id não identificado");

            PedidoItemCompraDocumentosFilterViewModel filter = new PedidoItemCompraDocumentosFilterViewModel();
            filter.IdCompra = idCompra;
            filter.IdItem = idItem;
            filter.IdPedido = id;

            var pedidoItemCompraDocumentos = await mediator.Send<List<PedidoItemArteCompraDocumento>>
                (new ListItemArteCompraDocumentosIdByIdPedidoItemCompra()
                { IdPedido = id, IdPedidoItem = idItem, IdPedidoItemCompra = idCompra }, cancellationToken);

            if (pedidoItemCompraDocumentos.Count == 0) throw new NotFoundException("Registro não encontrado.");

            var pedidoItemCompraDocumentosViewModel = mapper.Map<List<PedidoItemArteCompraDocumentosViewModel>>(pedidoItemCompraDocumentos);

            var pagging = new GenericPaggingViewModel<PedidoItemArteCompraDocumentosViewModel>(pedidoItemCompraDocumentosViewModel, filter, pedidoItemCompraDocumentos.Count);

            return Ok(pagging);

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos/{idDocumento:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<PedidoItemArteCompraDocumentosViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetItemCompraDocumentosByIdAsync(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromRoute] long idDocumento,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Compra Id não identificado");

            if (idDocumento == 0) throw new NotFoundException("Documento Id não identificado");

            var pedidoItemCompraDocumentos = await mediator.Send(new GetByIdPedidoItemArteCompraDocumentos()
            {
                Id = idDocumento
            }, cancellationToken);

            if (pedidoItemCompraDocumentos == null) throw new NotFoundException("Registro não encontrado.");

            var itemCompraDocumentosViewModel = mapper.Map<ArquivoViewModel>(pedidoItemCompraDocumentos);

            return Ok(itemCompraDocumentosViewModel);
        }

        ///// <summary>
        ///// insere um pedido item
        ///// </summary>
        ///// <returns></returns>
        [HttpPost("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteCompraDocumentosViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItemCompraDocumentos(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromBody] PedidoItemArteCompraDocumentosViewModel pedidoItemCompraDocumentosViewModel,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Compra Id não identificado");

            pedidoItemCompraDocumentosViewModel.IdCompra = idCompra;

            var pedidoItemCompraDocumentos = mapper.Map<PedidoItemArteCompraDocumento>(pedidoItemCompraDocumentosViewModel);

            await mediator.Send(new CreatePedidoItemCompraDocumento()
            {
                PedidoItemCompraDocumentos = pedidoItemCompraDocumentos
            }, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoItemArteCompraDocumentosViewModel>(pedidoItemCompraDocumentos));

        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPost("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos/bulk")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteCompraDocumentosViewModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItemCompraDocumentosBulk(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromBody] List<PedidoItemArteCompraDocumentosViewModel> pedidoItemCompraDocumentosViewModelList,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Compra Id não identificado");

            foreach (var pedidoItemComprasDocumentos in pedidoItemCompraDocumentosViewModelList)
                pedidoItemComprasDocumentos.IdCompra = idCompra;

            var pedidoItemCompraDocumentos = mapper.Map<List<PedidoItemArteCompraDocumento>>(pedidoItemCompraDocumentosViewModelList);

            await mediator.Send(new CreatePedidoItemCompraDocumentos()
            {
                PedidoItemCompraDocumentos = pedidoItemCompraDocumentos
            }, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, mapper.Map<List<PedidoItemArteCompraDocumentosViewModel>>(pedidoItemCompraDocumentos));

        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos/{idDocumento:long}")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteCompraDocumentosViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItemCompraDocumentos(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromRoute] long idDocumento,
            [FromBody] PedidoItemArteCompraDocumentosViewModel pedidoItemCompraDocumentosViewModel,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Compra Id não identificado");

            if (idDocumento == 0) throw new NotFoundException("Documento Id não identificado");

            pedidoItemCompraDocumentosViewModel.IdCompra = idCompra;
            pedidoItemCompraDocumentosViewModel.Id = idDocumento;
            var pedidoItemCompraDocumentos = mapper.Map<PedidoItemArteCompraDocumento>(pedidoItemCompraDocumentosViewModel);

            await mediator.Send(new UpdatePedidoItemCompraDocumento()
            {
                PedidoItemCompraDocumentos = pedidoItemCompraDocumentos
            }, cancellationToken);

            var result = mapper.Map<PedidoItemArteCompraDocumentosViewModel>(pedidoItemCompraDocumentos);

            return Ok(result);
        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos/bulk")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteCompraDocumentosViewModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItemCompraDocumentosBulk(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromBody] List<PedidoItemArteCompraDocumentosViewModel> pedidoItemCompraDocumentosViewModelList,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Compra Id não identificado");

            foreach (var pedidoItemComprasDocumentos in pedidoItemCompraDocumentosViewModelList)
                pedidoItemComprasDocumentos.IdCompra = idCompra;

            var pedidoItemCompraDocumentos = mapper.Map<List<PedidoItemArteCompraDocumento>>(pedidoItemCompraDocumentosViewModelList);

            await mediator.Send(new UpdatePedidoItemCompraDocumentos()
            {
                PedidoItemCompraDocumentos = pedidoItemCompraDocumentos
            }, cancellationToken);

            var result = mapper.Map<List<PedidoItemArteCompraDocumentosViewModel>>(pedidoItemCompraDocumentos);

            return Ok(result);
        }

        ///// <summary>
        ///// apaga um pedido item
        ///// </summary>
        ///// <param name="id">id do pedidoitem</param>
        ///// <param name="cancellationToken">cancellation token</param>
        ///// <returns></returns>
        [HttpDelete("{id:long}/artes/itens/{idItem:long}/compras/{idCompra:long}/documentos/{idDocumento:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePedidoItemCompraDocumentosAsync(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idCompra,
            [FromRoute] long idDocumento,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idCompra == 0) throw new NotFoundException("Pedido Item Compra Id não identificado");

            if (idDocumento == 0) throw new NotFoundException("Documento Id não identificado");

            var pedidoItemCompraDocumentos = await mediator.Send(new DeletePedidoItemCompraDocumentos()
            {
                Id = idDocumento
            }, cancellationToken);

            return StatusCode(204);
        }

        #endregion

        #region PedidoItemEntregas

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/entregas")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteEntregaViewModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListItemArteEntregaByIdPedidoItemAsync(
            [FromRoute] long id,
            [FromRoute] long idItem,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            PedidoItemEntregaFilterViewModel filter = new PedidoItemEntregaFilterViewModel();

            filter.IdPedidoItem = idItem;
            filter.IdPedido = id;

            var pedidoItemEntrega = await mediator.Send(new ListItemArteEntregaIdByIdPedidoItem() { IdPedido = id, IdPedidoItem = idItem }, cancellationToken);

            if (pedidoItemEntrega.Count == 0) throw new NotFoundException("Registro não encontrado.");

            var pedidoItemEntregaViewModel = mapper.Map<List<PedidoItemArteEntregaViewModel>>(pedidoItemEntrega);

            var pagging = new GenericPaggingViewModel<PedidoItemArteEntregaViewModel>(pedidoItemEntregaViewModel, filter, pedidoItemEntrega.Count);

            return Ok(pagging);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpGet("{id:long}/artes/itens/{idItem:long}/entregas/{idEntrega:long}")]
        [ProducesResponseType(200, Type = typeof(GenericPaggingViewModel<List<PedidoItemArteEntregaViewModel>>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetItemCompraEntregaByIdAsync(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idEntrega,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idEntrega == 0) throw new NotFoundException("Pedido Item Entrega Id não identificado");

            var pedidoItemEntrega = await mediator.Send(new GetByIdPedidoItemArteEntrega()
            {
                IdPedido = id,
                IdPedidoItem = idItem,
                Id = idEntrega
            }, cancellationToken);

            if (pedidoItemEntrega == null) throw new NotFoundException("Registro não encontrado.");

            var itemEntregaViewModel = mapper.Map<PedidoItemArteEntregaViewModel>(pedidoItemEntrega);

            return Ok(itemEntregaViewModel);
        }

        ///// <summary>
        ///// insere um pedido item
        ///// </summary>
        ///// <returns></returns>
        [HttpPost("{id:long}/artes/itens/{idItem:long}/entregas")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteEntregaViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItemArteEntrega(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromBody] PedidoItemArteEntregaViewModel pedidoItemEntregaViewModel, CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            pedidoItemEntregaViewModel.IdPedidoItem = idItem;

            var pedidoItemEntrega = mapper.Map<PedidoItemArteEntrega>(pedidoItemEntregaViewModel);

            await mediator.Send(new CreatePedidoItemArteEntrega()
            {
                IdPedidoItem = idItem,
                PedidoItemEntrega = pedidoItemEntrega
            }, cancellationToken);

            var vm = mapper.Map<PedidoItemArteEntregaViewModel>(pedidoItemEntrega);

            vm.IdPedidoItem = idItem;

            return StatusCode(StatusCodes.Status201Created, mapper.Map<PedidoItemArteEntregaViewModel>(vm));
        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPost("{id:long}/artes/itens/{idItem:long}/entregas/bulk")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteEntregaViewModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPedidoItemArteEntregaBulk(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromBody] List<PedidoItemArteEntregaViewModel> pedidoItemEntregaViewModelList,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            foreach (var pedidoItemEntrega in pedidoItemEntregaViewModelList)
                pedidoItemEntrega.IdPedidoItem = idItem;

            var pedidoItemEntregas = mapper.Map<List<PedidoItemArteEntrega>>(pedidoItemEntregaViewModelList);

            await mediator.Send(new CreatePedidoItemArteEntregas()
            {
                IdPedidoItem = idItem,
                PedidoItemEntregas = pedidoItemEntregas
            }, cancellationToken);

            var vms = mapper.Map<List<PedidoItemArteEntregaViewModel>>(pedidoItemEntregas);

            foreach (var vm in vms) vm.IdPedidoItem = idItem;

            return StatusCode(StatusCodes.Status201Created, mapper.Map<List<PedidoItemArteEntregaViewModel>>(pedidoItemEntregas));
        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/entregas/{idEntrega:long}")]
        [ProducesResponseType(200, Type = typeof(PedidoItemArteEntregaViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItemArteEntrega(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idEntrega,
            [FromBody] PedidoItemArteEntregaViewModel pedidoItemEntregaViewModel,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idEntrega == 0) throw new NotFoundException("Pedido Item Entrega Id não identificado");

            pedidoItemEntregaViewModel.IdPedidoItem = idItem;
            pedidoItemEntregaViewModel.Id = idEntrega;

            var pedidoItemEntrega = mapper.Map<PedidoItemArteEntrega>(pedidoItemEntregaViewModel);

            await mediator.Send(new UpdatePedidoItemArteEntrega()
            {
                IdPedidoItem = idItem,
                PedidoItemEntrega = pedidoItemEntrega
            }, cancellationToken);

            var result = mapper.Map<PedidoItemArteEntregaViewModel>(pedidoItemEntrega);

            return Ok(result);
        }

        ///// <summary>
        ///// atualiza um pedido item
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="UserVM"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        [HttpPut("{id:long}/artes/itens/{idItem:long}/entregas/bulk")]
        [ProducesResponseType(200, Type = typeof(List<PedidoItemArteEntregaViewModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPedidoItemArteEntregaBulk(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromBody] List<PedidoItemArteEntregaViewModel> pedidoItemEntregaViewModelList,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            foreach (var pedidoItemEntregas in pedidoItemEntregaViewModelList)
                pedidoItemEntregas.IdPedidoItem = idItem;

            var pedidoItemEntrega = mapper.Map<List<PedidoItemArteEntrega>>(pedidoItemEntregaViewModelList);

            await mediator.Send(new UpdatePedidoItemEntregas()
            {
                IdPedidoItem = idItem,
                PedidoItemEntregas = pedidoItemEntrega
            }, cancellationToken);

            var result = mapper.Map<List<PedidoItemArteEntregaViewModel>>(pedidoItemEntrega);

            return Ok(result);
        }

        ///// <summary>
        ///// apaga um pedido item
        ///// </summary>
        ///// <param name="id">id do pedidoitem</param>
        ///// <param name="cancellationToken">cancellation token</param>
        ///// <returns></returns>
        [HttpDelete("{id:long}/artes/itens/{idItem:long}/entregas/{idEntrega:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePedidoItemArteEntregaAsync(
            [FromRoute] long id,
            [FromRoute] long idItem,
            [FromRoute] long idEntrega,
            CancellationToken cancellationToken)
        {
            if (id == 0) throw new NotFoundException("Pedido Id não identificado");

            if (idItem == 0) throw new NotFoundException("Pedido Item Id não identificado");

            if (idEntrega == 0) throw new NotFoundException("Pedido Item Entrega Id não identificado");

            var pedidoItemEntrega = await mediator.Send(new DeletePedidoItemArteEntrega()
            {
                Id = idEntrega,
                IdPedidoItem = idItem
            }, cancellationToken);

            return StatusCode(204);
        }

        #endregion

    }
}
