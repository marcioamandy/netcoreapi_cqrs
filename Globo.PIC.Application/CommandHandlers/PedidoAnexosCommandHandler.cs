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
	public class PedidoAnexosCommandHandler :

		IRequestHandler<DeletePedidoAnexos>,
		IRequestHandler<UpdatePedidoAnexo>,
		IRequestHandler<UpdatePedidoAnexos>,
		IRequestHandler<SavePedidoAnexo>,
		IRequestHandler<SavePedidoAnexos>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoAnexo> pedidoAnexosRepository;
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
		public PedidoAnexosCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<Pedido> _pedidoRepository,
			IRepository<PedidoAnexo> _pedidoAnexosRepository)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoRepository = _pedidoRepository;
			pedidoAnexosRepository = _pedidoAnexosRepository;
		}

		protected void RunEntityValidation(PedidoAnexo pedido)
		{
			if (pedido == null)
				throw new ApplicationException("O Pedido está vazio!");
		}
		public void RunEntityValidationList(List<PedidoAnexo> pedidoAnexosViewModels)
		{
			if (pedidoAnexosViewModels.Count() == 0)
				throw new ApplicationException("O anexo do pedido está vazio!");

			foreach (var pedido in pedidoAnexosViewModels)
				RunEntityValidation(pedido);
		}

		/// <summary>
		/// atualiza um pedido item anexos
		/// </summary>
		/// <param name="request">PedidoItemAnexos</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoAnexo, Unit>.Handle(UpdatePedidoAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoAnexos);
				var pedido = await pedidoRepository.GetById(request.PedidoAnexos.IdPedido, cancellationToken);
				if (pedido == null)
					throw new NotFoundException("Pedido do anexo não encontrado!");
				else
					request.PedidoAnexos.Pedido = pedido;

				pedidoAnexosRepository.AddOrUpdate(request.PedidoAnexos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
		async Task<Unit> IRequestHandler<UpdatePedidoAnexos, Unit>.Handle(UpdatePedidoAnexos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoAnexos);
				unitOfWork.BeginTransaction();
				foreach (var pedidoAnexos in request.PedidoAnexos)
				{
					var pedido = await pedidoRepository.GetById(pedidoAnexos.IdPedido, cancellationToken);

					if (pedido == null)
						throw new NotFoundException("pedido do anexo não encontrado!");
					else
						pedidoAnexos.Pedido = pedido;

					pedidoAnexosRepository.AddOrUpdate(pedidoAnexos, cancellationToken);
					var result = unitOfWork.SaveChanges();
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

		async Task<Unit> IRequestHandler<DeletePedidoAnexos, Unit>.Handle(DeletePedidoAnexos request, CancellationToken cancellationToken)
		{
			var pedidoAnexos = await pedidoAnexosRepository.GetById(request.PedidoAnexos.IdPedido, cancellationToken);

			if (pedidoAnexos == null)
				throw new NotFoundException("Anexo do pedido não encontrada!");

			pedidoAnexosRepository.Remove(pedidoAnexos);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoAnexo, Unit>.Handle(SavePedidoAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoAnexos);

				var pedido = await pedidoRepository.GetById(request.PedidoAnexos.IdPedido, cancellationToken);
				if (pedido == null)
					throw new NotFoundException("pedido do anexo não encontrado!");
				else
					request.PedidoAnexos.Pedido = pedido;

				pedidoAnexosRepository.AddOrUpdate(request.PedidoAnexos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoAnexos, Unit>.Handle(SavePedidoAnexos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoAnexos);
				unitOfWork.BeginTransaction();
				foreach (var pedidoAnexos in request.PedidoAnexos)
				{
					var pedido = await pedidoRepository.GetById(pedidoAnexos.Id, cancellationToken);

					if (pedido == null)
						throw new NotFoundException("pedido não encontrado!");
					else
						pedidoAnexos.Pedido = pedido;

					pedidoAnexosRepository.AddOrUpdate(pedidoAnexos, cancellationToken);
					var result = unitOfWork.SaveChanges();
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
	}
}