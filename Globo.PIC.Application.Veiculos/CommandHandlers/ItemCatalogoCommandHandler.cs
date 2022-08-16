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
    public class ItemCatalogoCommandHandler :
        IRequestHandler<CreateItemCatalogo>
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
        public ItemCatalogoCommandHandler(
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

        protected void RunEntityValidation(ItemCatalogo itemCatalogo)
        {
            if (itemCatalogo == null)
                throw new ApplicationException("O Item está vazio!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<CreateItemCatalogo, Unit>.Handle(CreateItemCatalogo request, CancellationToken cancellationToken)
        {

            RunEntityValidation(request.ItemCatalogo);

            var pedido = await pedidoRepository.GetById(request.PedidoId, cancellationToken);            

            if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

            var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItemId, cancellationToken);

            if (pedidoItem == null || pedidoItem.PedidoItemVeiculo == null) throw new NotFoundException("Pedido Item não encontrado.");

            var item = await itemVeiculoRepository.GetById(request.ItemId, cancellationToken);

            if (item == null) throw new NotFoundException("Item não encontrado.");

            request.ItemCatalogo.IdItem = item.Id;
            request.ItemCatalogo.IdConteudo = pedidoItem.PedidoItemVeiculo.IdTipo;
            request.ItemCatalogo.BloqueadoOutrosConteudos = item.BloqueioEmprestimos;
            request.ItemCatalogo.AtivoAte = item.DataAtivoAte;
            request.ItemCatalogo.DataInicio = item.DataChegada;
            request.ItemCatalogo.DataFim = item.DataDevolucao;
            request.ItemCatalogo.Ativo = true;

            itemCatalogoRepository.AddOrUpdate(request.ItemCatalogo, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task; 
        }

    }
}