using System;
using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Veiculo.CommandHandlers
{
    public class ItemVeiculoCommandHandler :
        IRequestHandler<CreateItemVeiculo>,
        IRequestHandler<ReprovarOpcoesPedidoItemVeiculo>,
        IRequestHandler<AprovarOpcaoPedidoItemVeiculo>
    //IRequestHandler<DeleteItemVeiculo>,
    //IRequestHandler<UpdateItemVeiculo>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItem> pedidoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ItemVeiculoRepository itemVeiculoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ItemCatalogoRepository itemCatalogoRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
		/// 
		/// </summary>
		private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>  
        public ItemVeiculoCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<PedidoItem> _pedidoItemRepository,
            IRepository<ItemVeiculo> _itemVeiculoRepository,
            IRepository<ItemCatalogo> _itemCatalogoRepository,
            IRepository<Pedido> _pedidoRepository,
            IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            pedidoItemRepository = _pedidoItemRepository as PedidoItemRepository;
            itemVeiculoRepository = _itemVeiculoRepository as ItemVeiculoRepository;
            itemCatalogoRepository = _itemCatalogoRepository as ItemCatalogoRepository;
            pedidoRepository = _pedidoRepository;
            mapper = _mapper;
        }

        protected void RunEntityValidation(ItemVeiculo itemVeiculo)
        {
            if (itemVeiculo == null)
                throw new ApplicationException("O Item está vazio!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<CreateItemVeiculo, Unit>.Handle(CreateItemVeiculo request, CancellationToken cancellationToken)
        {

            RunEntityValidation(request.ItemVeiculo);

            var pedido = await pedidoRepository.GetById(request.PedidoId, cancellationToken);

            if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

            var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItemId, cancellationToken);

            if (pedidoItem == null || pedidoItem.PedidoItemVeiculo == null) throw new NotFoundException("Pedido Item não encontrado.");

            request.ItemVeiculo.IdPedidoItemVeiculo = pedidoItem.PedidoItemVeiculo.Id;
            request.ItemVeiculo.Item.IdTipo = pedidoItem.PedidoItemVeiculo.IdTipo;
            request.ItemVeiculo.Item.IdSubCategoria = pedidoItem.PedidoItemVeiculo.IdSubCategoria;
            request.ItemVeiculo.DataChegada = pedidoItem.PedidoItemVeiculo.DataChegadaVeiculo;
            request.ItemVeiculo.DataDevolucao = pedidoItem.PedidoItemVeiculo.DataDevolucaoVeiculo;

            itemVeiculoRepository.AddOrUpdate(request.ItemVeiculo, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<AprovarOpcaoPedidoItemVeiculo, Unit>.Handle(AprovarOpcaoPedidoItemVeiculo request, CancellationToken cancellationToken)
        {
            DateTime AtivoAte = DateTime.Now;
            DateTime DataInicio = DateTime.Now;
            DateTime DataFinal = DateTime.Now;

            if (request.IdPedidoItem <= 0)
                throw new ValidationException("O ID Pedido Item não pode ser nulo.");

            if (request.idOpcao <= 0)
                throw new ValidationException("O ID da Opção não pode ser nulo.");

            var existItem = itemVeiculoRepository.GetAll().Where(a => a.PedidoItemVeiculo.PedidoItem.Id == request.IdPedidoItem).ToList();

            if (existItem.Count <= 0)
                throw new NotFoundException("Opções não encontradas");

            foreach (var Item in existItem)
            {
                if (Item.Id == request.idOpcao)
                {
                    Item.DataAprovacao = DateTime.Now;
                    AtivoAte = (DateTime)Item.DataAtivoAte;
                    DataInicio = (DateTime)Item.DataChegada;
                    DataFinal = (DateTime)Item.DataDevolucao;
                }
                else
                {
                    Item.DataReprovacao = DateTime.Now;
                    Item.JustificativaReprovacao = $"Reprovação automática por aprovação do item ({request.idOpcao})";
                }
                itemVeiculoRepository.AddOrUpdate(Item, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");

            }

            var pedido = await pedidoRepository.GetById(request.IdPedido, cancellationToken);

            if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

            var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

            if (pedidoItem == null || pedidoItem.PedidoItemVeiculo == null) throw new NotFoundException("Pedido Item não encontrado.");

            pedidoItem.IdItem = request.idOpcao;

            pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

            var resultItem = unitOfWork.SaveChanges();

            if (!resultItem) throw new ApplicationException("An error has occured.");

            //Verifica se o item aprovado já existe no catalogo
            var catalogo = itemCatalogoRepository.GetAll().Where(a => a.IdItem == request.idOpcao).ToList();
            if (catalogo.Count() <= 0)
            {
                ItemCatalogo itemCatalogo = new ItemCatalogo();

                itemCatalogo.IdItem = request.idOpcao;
                itemCatalogo.IdConteudo = pedido.IdConteudo;
                itemCatalogo.BloqueadoOutrosConteudos = request.bloqueioEmprestimos;
                itemCatalogo.JustificativaBloqueio = request.justificativaBloqueio;
                itemCatalogo.AtivoAte = AtivoAte;
                itemCatalogo.DataInicio = DataInicio;
                itemCatalogo.DataFim = DataFinal;
                itemCatalogo.Ativo = true;

                itemCatalogoRepository.AddOrUpdate(itemCatalogo, cancellationToken);

                var resultCatalogo = unitOfWork.SaveChanges();

                if (!resultCatalogo) throw new ApplicationException("An error has occured.");
            }
            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<ReprovarOpcoesPedidoItemVeiculo, Unit>.Handle(ReprovarOpcoesPedidoItemVeiculo request, CancellationToken cancellationToken)
        {

            if (request.IdPedidoItem <= 0)
                throw new ValidationException("O ID Pedido Item não pode ser nulo.");

            var existItem = itemVeiculoRepository.GetAll().Where(a => a.PedidoItemVeiculo.PedidoItem.Id == request.IdPedidoItem).ToList();

            if (existItem.Count <= 0)
                throw new NotFoundException("Opções não encontradas");

            foreach (var Item in existItem)
            {
                Item.DataReprovacao = DateTime.Now;
                Item.JustificativaReprovacao = request.JustificativaReprovacao;

                itemVeiculoRepository.AddOrUpdate(Item, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");
            }

            return await Unit.Task;
        }
        //      /// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //async Task<Unit> IRequestHandler<UpdatePedidoItemVeiculo, Unit>.Handle(UpdatePedidoItemVeiculo request, CancellationToken cancellationToken)
        //      {
        //          try
        //          {
        //              bool statusChange = false;

        //              RunEntityValidation(request.PedidoItemVeiculo.PedidoItem);

        //              var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItemVeiculo.PedidoItem.IdPedido, cancellationToken);

        //              if (existPedido == null)
        //                  throw new BadRequestException("id pedido não encontrado!");

        //              var existPedidoItem = existPedido.Itens.Where(x => x.Id == request.PedidoItemVeiculo.IdPedidoItem).FirstOrDefault();

        //              if (existPedidoItem == null)
        //                  throw new BadRequestException("id pedido item não encontrado!");

        //              if (request.PedidoItemVeiculo.PedidoItem.DataNecessidade == null)
        //              {
        //                  if (existPedidoItem.DataNecessidade != null)
        //                      request.PedidoItemVeiculo.PedidoItem.DataNecessidade = existPedidoItem.DataNecessidade;
        //                  else
        //                      if (existPedido.PedidoArte.DataNecessidade != null)
        //                      request.PedidoItemVeiculo.PedidoItem.DataNecessidade = existPedido.PedidoArte.DataNecessidade;
        //              }

        //              var vltotal = request.PedidoItemVeiculo.PedidoItem.Quantidade * request.PedidoItemVeiculo.PedidoItem.Valor;

        //              request.PedidoItemVeiculo.PedidoItem.ValorItens = vltotal;

        //              await mediator.Send(new UpdatePedidoItem()
        //              {
        //                  PedidoItem = request.PedidoItemVeiculo.PedidoItem
        //              }, cancellationToken);

        //              existPedidoItem = await pedidoItemRepository.GetById(request.PedidoItemVeiculo.PedidoItem.Id, cancellationToken);

        //              request.PedidoItemVeiculo.Id = existPedidoItem.PedidoItemVeiculo.Id;
        //              request.PedidoItemVeiculo.PedidoItem = existPedidoItem;

        //              /*
        //              var pedidoItemCompra = pedidoItemCompraRepository.GetAll().Where(a => a.IdPedidoItemVeiculo == request.PedidoItemVeiculo.Id).ToList();

        //              if (pedidoItemCompra.Count > 0)
        //                  throw new NotFoundException(string.Format("Item {0} não pode ser editado, já existe compra vinculada.", request.PedidoItemVeiculo.IdPedidoItem));
        //              */
        //              if (request.PedidoItemVeiculo.IdStatus == 0)
        //              {
        //                  if (existPedidoItem.PedidoItemVeiculo.IdStatus == 0)
        //                      if (string.IsNullOrWhiteSpace(existPedidoItem.RCs.FirstOrDefault().Acordo))
        //                      {
        //                          request.PedidoItemVeiculo.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;
        //                          statusChange = true;
        //                      }
        //                      else
        //                      {
        //                          request.PedidoItemVeiculo.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA;
        //                          statusChange = true;
        //                      }
        //                  else
        //                      request.PedidoItemVeiculo.IdStatus = existPedidoItem.PedidoItemVeiculo.IdStatus;
        //              }
        //              else
        //                  statusChange = true;

        //              /*
        //              if (!string.IsNullOrWhiteSpace(existPedidoItem.PedidoItemVeiculo.CompradoPorLogin) && existPedidoItem.PedidoItemVeiculo.CompradoPorLogin != null)
        //              {
        //                  request.PedidoItemVeiculo.CompradoPorLogin = existPedidoItem.PedidoItemVeiculo.CompradoPorLogin;
        //                  request.PedidoItemVeiculo.CompradoPor = existPedidoItem.PedidoItemVeiculo.CompradoPor;
        //              }
        //              else if (userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItemVeiculo.CompradoPorLogin))
        //              {
        //                  request.PedidoItemVeiculo.IdStatus = (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR;
        //                  statusChange = true;
        //                  request.PedidoItemVeiculo.CompradoPor = await userRepository.GetByLogin(request.PedidoItemVeiculo.CompradoPorLogin, cancellationToken);
        //                  if (!DBNull.Value.Equals(request.PedidoItemVeiculo.DataVinculoComprador) || (request.PedidoItemVeiculo.DataVinculoComprador != existPedidoItem.PedidoItemVeiculo.DataVinculoComprador))
        //                      request.PedidoItemVeiculo.DataVinculoComprador = DateTime.Now;

        //                  await mediator.Publish(new OnItemAtribuidoComprador() { Pedido = existPedido });
        //              }
        //              else if (!userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS) && !string.IsNullOrWhiteSpace(request.PedidoItemVeiculo.CompradoPorLogin))
        //                  throw new BadRequestException("Somente usuário da base de suprimentos pode vincular um comprador.");
        //              */
        //              var status = await statusItemRepository.GetById(request.PedidoItemVeiculo.IdStatus, cancellationToken);
        //              if (status == null)
        //                  throw new NotFoundException("status não encontrado!");
        //              else
        //                  request.PedidoItemVeiculo.Status = status;
        //              /*
        //              if (!string.IsNullOrWhiteSpace(request.PedidoItemVeiculo.CompradoPorLogin))
        //              {
        //                  if (!string.IsNullOrWhiteSpace(existPedidoItem.RCs.FirstOrDefault().Acordo))
        //                      throw new BadRequestException("Não é permitido atribuir comprador para itens com acordo.");

        //                  request.PedidoItemVeiculo.CompradoPor = await userRepository.GetByLogin(request.PedidoItemVeiculo.CompradoPorLogin, cancellationToken);
        //              }
        //              */
        //              if (!string.IsNullOrWhiteSpace(request.PedidoItemVeiculo.PedidoItem.ReprovadoPorLogin))
        //                  request.PedidoItemVeiculo.PedidoItem.ReprovadoPor = await userRepository.GetByLogin(request.PedidoItemVeiculo.PedidoItem.ReprovadoPorLogin, cancellationToken);

        //              request.PedidoItemVeiculo.TrackingVeiculo = trackingRepository.GetAll().Where(a => a.IdPedidoItem == request.PedidoItemVeiculo.Id).ToList();

        //              pedidoItemVeiculoRepository.AddOrUpdate(request.PedidoItemVeiculo, cancellationToken);

        //              var result = unitOfWork.SaveChanges();

        //              if (!result) throw new ApplicationException("An error has occured.");

        //              if (statusChange)
        //              {
        //                  await mediator.Send(new AddTrackingVeiculo()
        //                  {
        //                      PedidoItemVeiculoTracking = new PedidoItemVeiculoTracking()
        //                      {
        //                          IdPedidoItem = request.PedidoItemVeiculo.Id,
        //                          StatusId = request.PedidoItemVeiculo.IdStatus,
        //                          ChangeById = existPedido.CriadoPorLogin
        //                      }
        //                  }, cancellationToken);
        //              }

        //              //var pedidoWithOutRoles = pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItem.Pedido.Id, cancellationToken).Result;

        //              //await mediator.Publish(new OnVerificarStatusVeiculo() { Pedido = existPedido });

        //              //await mediator.Publish(new OnAtualizarQtdeItensVeiculo() { PedidoItem = request.PedidoItemVeiculo.PedidoItem });
        //          }
        //          catch (Exception error)
        //          {
        //              throw new ApplicationException(error.Message);
        //          }
        //          return Unit.Value;
        //      }

        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      /// <param name="request"></param>
        //      async Task<Unit> IRequestHandler<DeletePedidoItemVeiculo, Unit>.Handle(DeletePedidoItemVeiculo request, CancellationToken cancellationToken)
        //      {
        //          var existPedidoItem = await pedidoItemRepository.GetById(request.idItem, cancellationToken);

        //          var existPedidoItemVeiculo = await pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(request.idItem, cancellationToken);

        //          if (existPedidoItem == null)
        //              throw new NotFoundException("Pedido Item não encontrado");

        //          if (existPedidoItemVeiculo != null)
        //          {
        //              if (existPedidoItemVeiculo.IdStatus != (int)PedidoVeiculoStatus.PEDIDO_RASCUNHO)
        //                  throw new NotFoundException("O status do pedido item não posssibilita a ação desejada");
        //              else
        //                  pedidoItemRepository.DeletarPedidoItemVeiculo(request.idItem, cancellationToken);
        //          }
        //          else
        //              pedidoItemRepository.DeletarPedidoItemVeiculo(request.idItem, cancellationToken);

        //          var result = unitOfWork.Commit();

        //          if (!result) throw new ApplicationException("An error has occured.");

        //          return await Unit.Task;
        //      }

    }
}