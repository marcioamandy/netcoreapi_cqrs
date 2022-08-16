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
using Globo.PIC.Domain.Exceptions;
namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class PedidoItemAnexosCommandHandler :
	 
		IRequestHandler<DeletePedidoItemAnexos>,
		IRequestHandler<UpdatePedidoItemAnexo>,
		IRequestHandler<UpdatePedidoItemAnexos>,
		IRequestHandler<SavePedidoItemAnexo>,
		IRequestHandler<SavePedidoItemAnexos>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemAnexos> pedidoItemAnexosRepository;
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItem> pedidoItemRepository;
		 
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<StatusPedidoItem> statusItemRepository;
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
		public PedidoItemAnexosCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<StatusPedidoItem> _statusItemRepository,
			IRepository<Pedido> _pedidoRepository,
			IRepository<PedidoItemAnexos> _pedidoItemAnexosRepository)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemRepository = _pedidoItemRepository;
			statusItemRepository = _statusItemRepository;
			pedidoRepository = _pedidoRepository;
			pedidoItemAnexosRepository= _pedidoItemAnexosRepository;
		}
	 
		protected void RunEntityValidation(PedidoItemAnexos pedido)
        {
			if (pedido == null)
				throw new ApplicationException("O Pedido Item está vazio!");

			//if (pedido.ValorItens == 0)
			//	throw new ApplicationException("O valor do Item está zerado!");

			//if (pedido.LocalEntrega == "")
			//	throw new ApplicationException("O local de entrega é obrigatório!");

			//if (pedido.NomeItem == "")
			//	throw new ApplicationException("O Nome do item é obrigatório!");
		}
		public void RunEntityValidationList(List<PedidoItemAnexos> pedidoItemAnexosViewModels)
		{
			if (pedidoItemAnexosViewModels.Count() == 0)
				throw new ApplicationException("O anexo do item está vazio!");

			foreach (var pedido in pedidoItemAnexosViewModels)
				RunEntityValidation(pedido);
		}

		/// <summary>
		/// atualiza um pedido item anexos
		/// </summary>
		/// <param name="request">PedidoItemAnexos</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemAnexo, Unit>.Handle(UpdatePedidoItemAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemAnexos);
				var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItemAnexos.IdPedidoItem,cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("item do anexo não encontrado!");
				else
					request.PedidoItemAnexos.PedidoItem = pedidoItem;

				pedidoItemAnexosRepository.AddOrUpdate(request.PedidoItemAnexos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value; 
		}
        async Task<Unit> IRequestHandler<UpdatePedidoItemAnexos, Unit>.Handle(UpdatePedidoItemAnexos request, CancellationToken cancellationToken)
        {
			try
			{
				RunEntityValidationList(request.PedidoItemAnexos);
				unitOfWork.BeginTransaction();
				foreach (var pedidoAnexos in request.PedidoItemAnexos)
				{
					var pedidoItem = await pedidoItemRepository.GetById(pedidoAnexos.IdPedidoItem, cancellationToken);

					if (pedidoItem == null)
						throw new NotFoundException("item do anexo não encontrado!");
					else
						pedidoAnexos.PedidoItem = pedidoItem;

					pedidoItemAnexosRepository.AddOrUpdate(pedidoAnexos, cancellationToken);
					var result =unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");
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

       async Task<Unit> IRequestHandler<DeletePedidoItemAnexos, Unit>.Handle(DeletePedidoItemAnexos request, CancellationToken cancellationToken)
        {
			var pedidoItemAnexos = await pedidoItemAnexosRepository.GetById(request.PedidoItemAnexos.IdPedidoItem, cancellationToken);

			if (pedidoItemAnexos == null)
				throw new NotFoundException("anexo do pedido não encontrada!");

			pedidoItemAnexosRepository.Remove(pedidoItemAnexos);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Unit.Value;
		}

        async Task<Unit> IRequestHandler<SavePedidoItemAnexo, Unit>.Handle(SavePedidoItemAnexo request, CancellationToken cancellationToken)
        {
			try
			{
				RunEntityValidation(request.PedidoItemAnexos);

				var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItemAnexos.IdPedidoItem, cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("pedido do anexo não encontrado!");
				else
					request.PedidoItemAnexos.PedidoItem = pedidoItem;

				pedidoItemAnexosRepository.AddOrUpdate(request.PedidoItemAnexos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");
			}
			catch(Exception error)
            {
				throw new Exception(error.Message);
            }
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemAnexos, Unit>.Handle(SavePedidoItemAnexos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemAnexos);
				unitOfWork.BeginTransaction();
				foreach (var pedidoItemImagens in request.PedidoItemAnexos)
				{
					var pedidoItem = await pedidoItemRepository.GetById(pedidoItemImagens.Id, cancellationToken);

					if (pedidoItem == null)
						throw new NotFoundException("pedido item não encontrado!");
					else
						pedidoItemImagens.PedidoItem = pedidoItem;
					 
					pedidoItemAnexosRepository.AddOrUpdate(pedidoItemImagens, cancellationToken);
					var result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");
				}
				unitOfWork.CommitTransaction();
			}
			catch(Exception error)
            {
				unitOfWork.RollbackTransaction();
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
    }
}