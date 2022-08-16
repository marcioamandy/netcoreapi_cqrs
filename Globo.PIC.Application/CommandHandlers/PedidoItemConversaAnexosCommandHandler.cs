using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using System.Linq;
using System.Collections.Generic;
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
	public class PedidoItemConversaAnexosCommandHandler :

		IRequestHandler<DeletePedidoItemConversaAnexos>,
		IRequestHandler<UpdatePedidoItemConversaAnexo>,
		IRequestHandler<UpdatePedidoItemConversaAnexos>,
		IRequestHandler<SavePedidoItemConversaAnexo>,
		IRequestHandler<SavePedidoItemConversaAnexos>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemConversaAnexo> pedidoItemConversaAnexoRepository;
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemConversa> pedidoItemConversaRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItem> pedidoItemRepository;

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
		public PedidoItemConversaAnexosCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemConversa> _pedidoItemConversaRepository,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<PedidoItemConversaAnexo> _pedidoItemConversaAnexoRepository)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemConversaRepository = _pedidoItemConversaRepository;
			pedidoItemRepository = _pedidoItemRepository;
			pedidoItemConversaAnexoRepository = _pedidoItemConversaAnexoRepository;
		}

		protected void RunEntityValidation(PedidoItemConversaAnexo pedidoItem)
		{
			if (pedidoItem == null)
				throw new ApplicationException("O Pedido Item Conversa está vazio!");
		}

		public void RunEntityValidationList(List<PedidoItemConversaAnexo> pedidoItemConversaAnexoViewModels)
		{
			if (pedidoItemConversaAnexoViewModels.Count() == 0)
				throw new ApplicationException("O anexo da conversa está vazia!");

			foreach (var pedidoItem in pedidoItemConversaAnexoViewModels)
				RunEntityValidation(pedidoItem);
		}

		/// <summary>
		/// atualiza um pedido item anexo
		/// </summary>
		/// <param name="request">PedidoItemAnexo</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemConversaAnexo, Unit>.Handle(UpdatePedidoItemConversaAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemConversaAnexo);
				var pedidoItemConversa = await pedidoItemConversaRepository.GetById(request.PedidoItemConversaAnexo.IdPedidoItem, cancellationToken);
				if (pedidoItemConversa == null)
					throw new NotFoundException("item conversa anexo não encontrado!");
				else
					request.PedidoItemConversaAnexo.PedidoItemConversa = pedidoItemConversa;

				pedidoItemConversaAnexoRepository.AddOrUpdate(request.PedidoItemConversaAnexo, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
		async Task<Unit> IRequestHandler<UpdatePedidoItemConversaAnexos, Unit>.Handle(UpdatePedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			var result = true;
			try
			{
				RunEntityValidationList(request.PedidoItemConversaAnexos);

				unitOfWork.BeginTransaction();

				foreach (var pedidoAnexo in request.PedidoItemConversaAnexos)
				{
					var pedidoItemConversa = await pedidoItemConversaRepository.GetById(pedidoAnexo.IdPedidoItem, cancellationToken);

					if (pedidoItemConversa == null)
						throw new NotFoundException("item conversa do anexo não encontrado!");
					else
						pedidoAnexo.PedidoItemConversa = pedidoItemConversa;

					pedidoItemConversaAnexoRepository.AddOrUpdate(pedidoAnexo, cancellationToken);
					result = unitOfWork.SaveChanges();
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

		async Task<Unit> IRequestHandler<DeletePedidoItemConversaAnexos, Unit>.Handle(DeletePedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			var pedidoItemConversaAnexo = await pedidoItemConversaAnexoRepository.GetById(request.PedidoItemConversaAnexo.IdPedidoItem, cancellationToken);

			if (pedidoItemConversaAnexo == null)
				throw new NotFoundException("Anexo da conversa não encontrada!");

			pedidoItemConversaAnexoRepository.Remove(pedidoItemConversaAnexo);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemConversaAnexo, Unit>.Handle(SavePedidoItemConversaAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemConversaAnexo);

				var pedidoItemConversa = await pedidoItemConversaRepository.GetById(request.PedidoItemConversaAnexo.IdPedidoItem, cancellationToken);
				if (pedidoItemConversa == null)
					throw new NotFoundException("item pedido do anexo não encontrado!");
				else
					request.PedidoItemConversaAnexo.PedidoItemConversa = pedidoItemConversa;

				pedidoItemConversaAnexoRepository.AddOrUpdate(request.PedidoItemConversaAnexo, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemConversaAnexos, Unit>.Handle(SavePedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemConversaAnexos);

				unitOfWork.BeginTransaction();

				foreach (var pedidoItemConversaAnexos in request.PedidoItemConversaAnexos)
				{
					var pedidoItemConversa = await pedidoItemConversaRepository.GetById(pedidoItemConversaAnexos.Id, cancellationToken);

					if (pedidoItemConversa == null)
						throw new NotFoundException("pedido item conversa não encontrado!");
					else
						pedidoItemConversaAnexos.PedidoItemConversa = pedidoItemConversa;

					pedidoItemConversaAnexoRepository.AddOrUpdate(pedidoItemConversaAnexos, cancellationToken);

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