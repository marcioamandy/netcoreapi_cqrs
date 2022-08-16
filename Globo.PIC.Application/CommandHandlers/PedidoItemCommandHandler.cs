using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Queries;
using Newtonsoft.Json;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Models;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Services.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class PedidoItemCommandHandler :
        IRequestHandler<UpdatePedidoItem>
    //IRequestHandler<UpdatePedidoItens>,
    //IRequestHandler<CreatePedidoItem>

    {

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoItemRepository pedidoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly PedidoItemArteRepository pedidoItemArteRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteDevolucao> pedidoItemDevolucaoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Domain.Entities.StatusPedidoItemArte> statusItemRepository;
        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemAnexo> pedidoItemAnexosRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteTracking> trackingRepository;

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
        private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;

        /// <summary>
        /// 
        /// </summary>  
        public PedidoItemCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IRepository<PedidoItem> _pedidoItemRepository,
            IRepository<PedidoItemArteDevolucao> _pedidoItemDevolucaoRepository,
            IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
            IRepository<Domain.Entities.StatusPedidoItemArte> _statusItemRepository,
            IRepository<PedidoItemArte> _pedidoItemArteRepository,
            IRepository<Pedido> _pedidoRepository,
            IRepository<PedidoItemAnexo> _pedidoItemAnexosRepository,
            IRepository<PedidoItemArteTracking> _trackingRepository,
            IRepository<Usuario> _userRepository,
            IUserProvider _userProvider,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy)
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            pedidoItemRepository = _pedidoItemRepository as PedidoItemRepository;
            pedidoItemArteRepository = _pedidoItemArteRepository as PedidoItemArteRepository;
            pedidoItemDevolucaoRepository = _pedidoItemDevolucaoRepository;
            pedidoItemCompraRepository = _pedidoItemCompraRepository;
            statusItemRepository = _statusItemRepository;
            pedidoRepository = _pedidoRepository;
            pedidoItemAnexosRepository = _pedidoItemAnexosRepository;
            trackingRepository = _trackingRepository;
            userRepository = _userRepository;
            userProvider = _userProvider;
            purchaseRequisitionProxy = _purchaseRequisitionProxy;
        }

        protected void RunEntityValidation(PedidoItem pedido)
        {
            if (pedido == null)
                throw new ApplicationException("O Pedido Item está vazio!");

            if (pedido.LocalEntrega == "")
                throw new ApplicationException("O local de entrega é obrigatório!");

            if (pedido.NomeItem == "")
                throw new ApplicationException("O Nome do item é obrigatório!");
        }

        public void RunEntityValidationList(List<PedidoItem> pedidoItemViewModels)
        {
            if (pedidoItemViewModels.Count() == 0)
                throw new ApplicationException("O Pedido Item está vazio!");

            foreach (var pedido in pedidoItemViewModels)
                RunEntityValidation(pedido);
        }

        async Task<Unit> IRequestHandler<UpdatePedidoItem, Unit>.Handle(UpdatePedidoItem request, CancellationToken cancellationToken)
        {         

            if (!string.IsNullOrWhiteSpace(request.PedidoItem.DevolvidoPorLogin))
                request.PedidoItem.DevolvidoPor = await userRepository.GetByLogin(request.PedidoItem.DevolvidoPorLogin, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.PedidoItem.AprovadoPorLogin))
                request.PedidoItem.AprovadoPor = await userRepository.GetByLogin(request.PedidoItem.AprovadoPorLogin, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.PedidoItem.ReprovadoPorLogin))
                request.PedidoItem.ReprovadoPor = await userRepository.GetByLogin(request.PedidoItem.ReprovadoPorLogin, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.PedidoItem.CanceladoPorLogin))
                request.PedidoItem.CanceladoPor = await userRepository.GetByLogin(request.PedidoItem.CanceladoPorLogin, cancellationToken);

            pedidoItemRepository.AddOrUpdate(request.PedidoItem, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return Unit.Value;
        }
    }
}
