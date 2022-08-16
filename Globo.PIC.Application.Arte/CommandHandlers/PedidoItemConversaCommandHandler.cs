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
using Globo.PIC.Domain.Types.Queries.Filters;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.ViewModels;
namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class PedidoItemConversaCommandHandler :

		IRequestHandler<DeletePedidoItemConversa>,
		IRequestHandler<UpdatePedidoItemConversa>,
		IRequestHandler<UpdatePedidoItemConversas>,
		IRequestHandler<SavePedidoItemConversa>,
		IRequestHandler<SavePedidoItemConversas>
	{

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
		private readonly IRepository<User> userRepository;

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
		public PedidoItemConversaCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemConversa> _pedidoItemConversaRepository,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<User> _userRepository
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemConversaRepository = _pedidoItemConversaRepository;
			pedidoItemRepository = _pedidoItemRepository;
			userRepository = _userRepository;
		}

		protected void RunEntityValidation(PedidoItemConversa pedidoItem)
		{
			if (pedidoItem == null)
				throw new ApplicationException("O Pedido Item Conversa está vazio!");

			if (pedidoItem.IdPedidoItem == 0)
				throw new ApplicationException("O id do pedido item é obrigatório!");

			if (pedidoItem.Login == "")
				throw new ApplicationException("O Login é obrigatório!");
			
		}
		public void RunEntityValidationList(List<PedidoItemConversa> pedidoItemConversaViewModels)
		{
			if (pedidoItemConversaViewModels.Count() == 0)
				throw new ApplicationException("O Pedido Item conversa está vazio!");

			foreach (var pedidoItem in pedidoItemConversaViewModels)
				RunEntityValidation(pedidoItem);
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="request">PedidoItem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemConversa, Unit>.Handle(UpdatePedidoItemConversa request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemConversa);

				var usuario = await userRepository.GetByLogin(request.PedidoItemConversa.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
					request.PedidoItemConversa.User = usuario;

				var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItemConversa.IdPedidoItem, cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemConversa.PedidoItem = pedidoItem;


				var pedidoItemConversa = await pedidoItemConversaRepository.GetById(request.PedidoItemConversa.Id, cancellationToken);
				if (pedidoItemConversa == null)
					throw new NotFoundException("id pedido item conversa não encontrado!");

				pedidoItemConversaRepository.AddOrUpdate(request.PedidoItemConversa, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
		async Task<Unit> IRequestHandler<UpdatePedidoItemConversas, Unit>.Handle(UpdatePedidoItemConversas request, CancellationToken cancellationToken)
		{
			var result = true;
			try
			{
				RunEntityValidationList(request.PedidoItemConversas);

				unitOfWork.BeginTransaction();

				foreach (var currentPedidoItem in request.PedidoItemConversas)
				{
					//var pedidoItemConversa = await pedidoItemConversaRepository.GetById(currentPedidoItem.Id, cancellationToken);
					//if (pedidoItemConversa == null)
					//	throw new NotFoundException("id pedido item conversa não encontrado!");
					//else
					//	currentPedidoItem = pedidoItemConversa;

					var usuario = await userRepository.GetByLogin(currentPedidoItem.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
						currentPedidoItem.User = usuario;

					var pedidoItem = await pedidoItemRepository.GetById(currentPedidoItem.IdPedidoItem, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						currentPedidoItem.PedidoItem = pedidoItem;

					pedidoItemConversaRepository.AddOrUpdate(currentPedidoItem, cancellationToken);
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

		async Task<Unit> IRequestHandler<DeletePedidoItemConversa, Unit>.Handle(DeletePedidoItemConversa request, CancellationToken cancellationToken)
		{
			var pedidoItemConversa = await pedidoItemConversaRepository.GetById(request.Id, cancellationToken);

			if (pedidoItemConversa == null)
				throw new NotFoundException("id pedido item não encontrado!");

			pedidoItemConversaRepository.Remove(pedidoItemConversa);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemConversa, Unit>.Handle(SavePedidoItemConversa request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemConversa);
				var usuario = await userRepository.GetByLogin(request.PedidoItemConversa.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
					request.PedidoItemConversa.User = usuario;

				var pedidoItem = await pedidoItemRepository.GetById(request.PedidoItemConversa.IdPedidoItem, cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemConversa.PedidoItem = pedidoItem;

				pedidoItemConversaRepository.AddOrUpdate(request.PedidoItemConversa, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemConversas, Unit>.Handle(SavePedidoItemConversas request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemConversas);
				
				unitOfWork.BeginTransaction();
				
				foreach (var pedidoItemConversas in request.PedidoItemConversas)
				{
					var usuario = await userRepository.GetByLogin(pedidoItemConversas.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
						pedidoItemConversas.User = usuario;

					var pedidoItem = await pedidoItemRepository.GetById(pedidoItemConversas.IdPedidoItem, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						pedidoItemConversas.PedidoItem = pedidoItem;

					pedidoItemConversaRepository.AddOrUpdate(pedidoItemConversas, cancellationToken);
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