using System;
using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Events;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Application.Veiculo.CommandHandlers
{
    public class AcionamentoCommandHandler :
        IRequestHandler<CreateAcionamento>,
        IRequestHandler<UpdateAcionamento>,
        IRequestHandler<DeleteAcionamento>,
        IRequestHandler<CreateAcionamentoItem>,
        IRequestHandler<UpdateAcionamentoItem>,
        IRequestHandler<DeleteAcionamentoItem>

    {
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
		private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> userRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Acionamento> acionamentoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<AcionamentoItem> acionamentoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoVeiculo> pedidoVeiculoRepository;

        /// <summary>
        /// 
        /// </summary>  
        public AcionamentoCommandHandler(
            IUnitOfWork _unitOfWork,
            IMediator _mediator,
            IUserProvider _userProvider,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy,
            ITasksProxy _projectTask,
            IRepository<Usuario> _userRepository,
            IRepository<Acionamento> _acionamentoRepository,
            IRepository<AcionamentoItem> _acionamentoItemRepository,
            IRepository<PedidoVeiculo> _pedidoVeiculoRepository,
            IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            mediator = _mediator;
            userProvider = _userProvider;
            userRepository = _userRepository;
            acionamentoRepository = _acionamentoRepository;
            acionamentoItemRepository = _acionamentoItemRepository;
            pedidoVeiculoRepository = _pedidoVeiculoRepository;
            mapper = _mapper;
        }

        protected void RunEntityValidation(Acionamento acionamento)
        {
            if (acionamento == null)
                throw new ApplicationException("O Acionamento está vazio!");
        }

        protected void RunEntityValidation(AcionamentoItem acionamentoItem)
        {
            if (acionamentoItem == null)
                throw new ApplicationException("O Acionamento está vazio!");
        }

        async Task<Unit> IRequestHandler<CreateAcionamento, Unit>.Handle(CreateAcionamento request, CancellationToken cancellationToken)
        {
            if (request.Acionamento.IdPedido <= 0)
                throw new ValidationException("O Id do Pedido não pode ser nulo.");

            var pedidoVeiculo = pedidoVeiculoRepository.GetAll().Where(p => p.IdPedido == request.Acionamento.IdPedido).FirstOrDefault();

            request.Acionamento.IdPedido = pedidoVeiculo.Id;

            request.Acionamento.PedidoVeiculo = pedidoVeiculo;

            acionamentoRepository.AddOrUpdate(request.Acionamento, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        async Task<Unit> IRequestHandler<UpdateAcionamento, Unit>.Handle(UpdateAcionamento request, CancellationToken cancellationToken)
        {
            var existAcionamento = acionamentoRepository.GetAll().Where(c => c.Id == request.Acionamento.Id).ToList();

            if (existAcionamento.Count <= 0)
                throw new NotFoundException("Acionamento não encontrado");

            acionamentoRepository.AddOrUpdate(request.Acionamento, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<DeleteAcionamento, Unit>.Handle(DeleteAcionamento request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new ValidationException("O ID Acionamento não pode ser nulo.");

            acionamentoRepository.DeletarAcionamento(request.Acionamento, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        async Task<Unit> IRequestHandler<CreateAcionamentoItem, Unit>.Handle(CreateAcionamentoItem request, CancellationToken cancellationToken)
        {
            if (request.AcionamentoItem.IdAcionamento <= 0)
                throw new ValidationException("O Id do Acionamento não pode ser nulo.");

            acionamentoItemRepository.AddOrUpdate(request.AcionamentoItem, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        async Task<Unit> IRequestHandler<UpdateAcionamentoItem, Unit>.Handle(UpdateAcionamentoItem request, CancellationToken cancellationToken)
        {
            var existAcionamentoItem = acionamentoItemRepository.GetAll().Where(c => c.Id == request.AcionamentoItem.Id).ToList();

            if (existAcionamentoItem.Count <= 0)
                throw new NotFoundException("Acionamento Item não encontrado");

            acionamentoItemRepository.AddOrUpdate(request.AcionamentoItem, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<DeleteAcionamentoItem, Unit>.Handle(DeleteAcionamentoItem request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new ValidationException("O ID Acionamento Item não pode ser nulo.");

            acionamentoItemRepository.DeletarAcionamentoItem(request.AcionamentoItem, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }
    }
}
