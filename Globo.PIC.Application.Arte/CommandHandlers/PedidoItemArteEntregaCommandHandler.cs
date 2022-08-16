using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Queries;
using Newtonsoft.Json;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Services.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class PedidoItemArteEntregaCommandHandler :

        IRequestHandler<DeletePedidoItemArteEntrega>,
        IRequestHandler<UpdatePedidoItemArteEntrega>,
        IRequestHandler<UpdatePedidoItemEntregas>,
        IRequestHandler<CreatePedidoItemArteEntrega>,
        IRequestHandler<CreatePedidoItemArteEntregas>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteEntrega> pedidoItemEntregaRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoItemArteRepository pedidoItemArteRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> userRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUserProvider userProvider;

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
        public PedidoItemArteEntregaCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<PedidoItemArteEntrega> _pedidoItemEntregaRepository,
            IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
            IRepository<PedidoItemArte> _pedidoItemRepository,
            IRepository<Usuario> _userRepository,
            IUserProvider _userProvider
            )
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
            pedidoItemCompraRepository = _pedidoItemCompraRepository;
            pedidoItemArteRepository = _pedidoItemRepository as PedidoItemArteRepository;
            userRepository = _userRepository;
            userProvider = _userProvider;
        }

        protected void RunEntityValidation(PedidoItemArteEntrega pedidoItem)
        {
            if (pedidoItem == null)
                throw new ApplicationException("O Pedido Item Entrega está vazio!");

            if (pedidoItem.Login == "")
                throw new ApplicationException("O Login é obrigatório!");

        }
        public void RunEntityValidationList(List<PedidoItemArteEntrega> pedidoItemEntregaViewModels)
        {
            if (pedidoItemEntregaViewModels.Count() == 0)
                throw new ApplicationException("O Pedido Item Arte Entrega está vazio!");

            foreach (var pedidoItem in pedidoItemEntregaViewModels)
                RunEntityValidation(pedidoItem);
        }

        /// <summary>
        /// atualiza um pedido item
        /// </summary>
        /// <param name="request">PedidoItem</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdatePedidoItemArteEntrega, Unit>.Handle(UpdatePedidoItemArteEntrega request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidation(request.PedidoItemEntrega);

                await AtualizaPedidoItemArteEntrega(request.PedidoItemEntrega, request.IdPedidoItem, cancellationToken);

            }
            catch (Exception error)
            {
                throw new ApplicationException(error.Message);
            }

            return Unit.Value;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdatePedidoItemEntregas, Unit>.Handle(UpdatePedidoItemEntregas request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidationList(request.PedidoItemEntregas);

                unitOfWork.BeginTransaction();

                foreach (var itemEntrega in request.PedidoItemEntregas)
                {
                    await AtualizaPedidoItemArteEntrega(itemEntrega, request.IdPedidoItem, cancellationToken);
                }

                unitOfWork.CommitTransaction();

            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();

                throw new ApplicationException(error.Message);
            }

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<DeletePedidoItemArteEntrega, Unit>.Handle(DeletePedidoItemArteEntrega request, CancellationToken cancellationToken)
        {
            var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(request.Id, cancellationToken);

            if (pedidoItemEntrega == null)
                throw new NotFoundException("id pedido item arte não encontrado!");

            var pedidoItemArte = await pedidoItemArteRepository.GetById(request.IdPedidoItem, cancellationToken);

            if (pedidoItemArte == null)
                throw new NotFoundException("id pedido item arte não encontrado!");

            pedidoItemEntrega.PedidoItemArte = pedidoItemArte;

            pedidoItemEntregaRepository.Remove(pedidoItemEntrega);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem });

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<CreatePedidoItemArteEntrega, Unit>.Handle(CreatePedidoItemArteEntrega request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidation(request.PedidoItemEntrega);

                await CriaPedidoItemArteEntrega(request.PedidoItemEntrega, request.IdPedidoItem, cancellationToken);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<CreatePedidoItemArteEntregas, Unit>.Handle(CreatePedidoItemArteEntregas request, CancellationToken cancellationToken)
        {
            try
            {
                RunEntityValidationList(request.PedidoItemEntregas);

                unitOfWork.BeginTransaction();

                foreach (var itemEntrega in request.PedidoItemEntregas)
                    await CriaPedidoItemArteEntrega(itemEntrega, request.IdPedidoItem, cancellationToken);

                unitOfWork.CommitTransaction();
            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();

                throw new ApplicationException(error.Message);
            }

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoItemArteEntrega"></param>
        /// <param name="idPedidoItem"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task CriaPedidoItemArteEntrega(PedidoItemArteEntrega pedidoItemArteEntrega, long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemArte = await pedidoItemArteRepository.GetByIdPedidoItem(idPedidoItem, cancellationToken);

            if (pedidoItemArte == null)
                throw new NotFoundException("Id pedido item arte não encontrado!");

            if (string.IsNullOrWhiteSpace(pedidoItemArteEntrega.Login))
                pedidoItemArteEntrega.Login = userProvider.User.Login;

            if (pedidoItemArteEntrega.DataEntrega == null)
                pedidoItemArteEntrega.DataEntrega = DateTime.Now;

            if (pedidoItemArteEntrega.Id > 0)
                pedidoItemArteEntrega.Id = 0;

            pedidoItemArteEntrega.IdPedidoItemArte = pedidoItemArte.Id;
            pedidoItemArteEntrega.PedidoItemArte = pedidoItemArte;

            var usuario = await userRepository.GetByLogin(pedidoItemArteEntrega.Login, cancellationToken);

            if (usuario == null)
                throw new NotFoundException("Login não encontrado!");

            if (pedidoItemArteEntrega.PedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                throw new NotFoundException("Item pedido ainda não foi atribuído a um Entregador.");

            if (pedidoItemArteEntrega.PedidoItemArte.QuantidadePendenteEntrega < pedidoItemArteEntrega.Quantidade)
                throw new NotFoundException("Quantidade informada é superior a quantidade pendente de Entrega.");

            if (!string.IsNullOrWhiteSpace(pedidoItemArteEntrega.PedidoItemArte.CompradoPorLogin))
                pedidoItemArteEntrega.Usuario = usuario;
            else
                throw new NotFoundException("Login atribuido não tem perfil de Entregador externo ou ainda não possui Entregador atribuído.");

            pedidoItemEntregaRepository.AddOrUpdate(pedidoItemArteEntrega, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArteEntrega.PedidoItemArte.PedidoItem });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoItemArteEntrega"></param>
        /// <param name="idPedidoItem"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task AtualizaPedidoItemArteEntrega(PedidoItemArteEntrega pedidoItemArteEntrega, long idPedidoItem, CancellationToken cancellationToken)
        {
            var pedidoItemArte = await pedidoItemArteRepository.GetByIdPedidoItem(idPedidoItem, cancellationToken);

            if (pedidoItemArte == null)
                throw new NotFoundException("id pedido item arte não encontrado!");

            pedidoItemArteEntrega.IdPedidoItemArte = pedidoItemArte.Id;
            pedidoItemArteEntrega.PedidoItemArte = pedidoItemArte;

            if (string.IsNullOrWhiteSpace(pedidoItemArteEntrega.Login))
                pedidoItemArteEntrega.Login = userProvider.User.Login;

            if (pedidoItemArteEntrega.DataEntrega == null)
                pedidoItemArteEntrega.DataEntrega = DateTime.Now;

            var usuario = await userRepository.GetByLogin(pedidoItemArteEntrega.Login, cancellationToken);

            if (usuario == null)
                throw new NotFoundException("login não encontrado!");

            pedidoItemArteEntrega.Usuario = usuario;

            if (string.IsNullOrWhiteSpace(pedidoItemArteEntrega.PedidoItemArte.CompradoPorLogin))
                throw new NotFoundException("login atribuido não tem perfil de Entregador externo ou ainda não possui Entregador atribuído.");

            if (pedidoItemArte.IdStatus != (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                throw new NotFoundException("Item pedido arte ainda não foi atribuído a um Entregador.");

            var EntregaLancada = await pedidoItemEntregaRepository.GetByIdPedidoItemEntrega(pedidoItemArteEntrega.Id, cancellationToken);

            if (EntregaLancada == null)
                throw new NotFoundException("Entrega não encontrada!");

            if ((pedidoItemArte.QuantidadePendenteEntrega + EntregaLancada.Quantidade - pedidoItemArteEntrega.Quantidade) < 0)
                throw new NotFoundException("Quantidade informada é superior a quantidade pendente de Entrega.");

            var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(pedidoItemArteEntrega.Id, cancellationToken);

            if (pedidoItemEntrega == null)
                throw new NotFoundException("id pedido item arte Entrega não encontrado!");

            pedidoItemEntregaRepository.AddOrUpdate(pedidoItemArteEntrega, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemArte.PedidoItem });
        }
    }
}