using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Exceptions;

namespace Globo.PIC.Application.Services.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<DeletePedido>,
        IRequestHandler<UpdatePedido>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// 
        /// </summary>  
        public PedidoCommandHandler(
            IUnitOfWork _unitOfWork,
            IRepository<Pedido> _pedidoRepository
            )
        {
            unitOfWork = _unitOfWork;
            pedidoRepository = _pedidoRepository;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        async Task<Unit> IRequestHandler<DeletePedido, Unit>.Handle(DeletePedido request, CancellationToken cancellationToken)
        {
            var existPedido = await pedidoRepository.GetByIdPedidoWithOutRoles(request.id, cancellationToken);

            if (existPedido == null)
                throw new NotFoundException("Pedido não encontrado");

            pedidoRepository.DeletarPedido(request.id, cancellationToken);

            var result = unitOfWork.Commit();

            if (!result) throw new ApplicationException("An error has occured.");

            return await Unit.Task;
        }

        Task<Unit> IRequestHandler<UpdatePedido, Unit>.Handle(UpdatePedido request, CancellationToken cancellationToken)
        {
            pedidoRepository.AddOrUpdate(request.Pedido, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return Unit.Task;
        }
    }
}