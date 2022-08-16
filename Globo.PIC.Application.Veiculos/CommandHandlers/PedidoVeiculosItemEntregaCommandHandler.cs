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
using Globo.PIC.Domain.Enums;
namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class PedidoItemEntregaCommandHandler :

		IRequestHandler<DeletePedidoItemEntrega>,
		IRequestHandler<UpdatePedidoItemEntrega>,
		IRequestHandler<UpdatePedidoItemEntregas>,
		IRequestHandler<SavePedidoItemEntrega>,
		IRequestHandler<SavePedidoItemEntregas>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemEntrega> pedidoItemEntregaRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompra> pedidoItemCompraRepository;

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
		public PedidoItemEntregaCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemEntrega> _pedidoItemEntregaRepository,
			IRepository<PedidoItemCompra> _pedidoItemCompraRepository,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<User> _userRepository,
			IUserProvider _userProvider
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			pedidoItemRepository = _pedidoItemRepository;
			userRepository = _userRepository;
			userProvider = _userProvider;
		}

		protected void RunEntityValidation(PedidoItemEntrega pedidoItem)
		{
			if (pedidoItem == null)
				throw new ApplicationException("O Pedido Item Entrega está vazio!");

			if (pedidoItem.IdPedidoItem == 0)
				throw new ApplicationException("O id do pedido item é obrigatório!");

			if (pedidoItem.Login == "")
				throw new ApplicationException("O Login é obrigatório!");

		}
		public void RunEntityValidationList(List<PedidoItemEntrega> pedidoItemEntregaViewModels)
		{
			if (pedidoItemEntregaViewModels.Count() == 0)
				throw new ApplicationException("O Pedido Item Entrega está vazio!");

			foreach (var pedidoItem in pedidoItemEntregaViewModels)
				RunEntityValidation(pedidoItem);
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="request">PedidoItem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemEntrega, Unit>.Handle(UpdatePedidoItemEntrega request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemEntrega);

				var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.PedidoItemEntrega.IdPedidoItem }, cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemEntrega.PedidoItem = pedidoItem;

				if (string.IsNullOrWhiteSpace(request.PedidoItemEntrega.Login))
					request.PedidoItemEntrega.Login = userProvider.User.Login;

				if (request.PedidoItemEntrega.DataEntrega == null)
					request.PedidoItemEntrega.DataEntrega = DateTime.Now;

				var usuario = await userRepository.GetByLogin(request.PedidoItemEntrega.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					//if (userProvider.IsRole(Role.PERFIL_EntregaDOR_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginEntregador))
					if (!string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginComprador))
						request.PedidoItemEntrega.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de Entregador externo ou ainda não possui Entregador atribuído.");
				}

				if (pedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido ainda não foi atribuído a um Entregador.");

				var EntregaLancada = await pedidoItemEntregaRepository.GetByIdPedidoItemEntrega(request.PedidoItemEntrega.Id, cancellationToken);
				if (EntregaLancada == null)
					throw new NotFoundException("Entrega não encontrada!");

				if (((pedidoItem.QuantidadePendenteEntrega + EntregaLancada.Quantidade) - request.PedidoItemEntrega.Quantidade) < 0)
					throw new NotFoundException("Quantidade informada é superior a quantidade pendente de Entrega.");

				var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(request.PedidoItemEntrega.Id, cancellationToken);
				if (pedidoItemEntrega == null)
					throw new NotFoundException("id pedido item Entrega não encontrado!");

				pedidoItemEntregaRepository.AddOrUpdate(request.PedidoItemEntrega, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
		async Task<Unit> IRequestHandler<UpdatePedidoItemEntregas, Unit>.Handle(UpdatePedidoItemEntregas request, CancellationToken cancellationToken)
		{
			var result = true;
			try
			{
				RunEntityValidationList(request.PedidoItemEntregas);

				unitOfWork.BeginTransaction();

				foreach (var currentPedidoItem in request.PedidoItemEntregas)
				{
					//var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(currentPedidoItem.Id, cancellationToken);
					//if (pedidoItemEntrega == null)
					//	throw new NotFoundException("id pedido item Entrega não encontrado!");
					//else
					//	currentPedidoItem = pedidoItemEntrega;

					var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentPedidoItem.IdPedidoItem }, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						currentPedidoItem.PedidoItem = pedidoItem;

					if (string.IsNullOrWhiteSpace(currentPedidoItem.Login))
						currentPedidoItem.Login = userProvider.User.Login;

					if (currentPedidoItem.DataEntrega == null)
						currentPedidoItem.DataEntrega = DateTime.Now;

					var usuario = await userRepository.GetByLogin(currentPedidoItem.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_EntregaDOR_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginEntregador))
						if (!string.IsNullOrWhiteSpace(currentPedidoItem.PedidoItem.LoginComprador))
							currentPedidoItem.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de Entregador externo ou ainda não possui Entregador atribuído.");
					}

					if (currentPedidoItem.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um Entregador.");

					var EntregaLancada = await pedidoItemEntregaRepository.GetByIdPedidoItemEntrega(currentPedidoItem.Id, cancellationToken);
					if (EntregaLancada == null)
						throw new NotFoundException("Entrega não encontrada!");

					if (((currentPedidoItem.PedidoItem.QuantidadePendenteEntrega + EntregaLancada.Quantidade) - currentPedidoItem.Quantidade) < 0)
						throw new NotFoundException("Quantidade informada é superior a quantidade pendente de Entrega.");

					var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(currentPedidoItem.Id, cancellationToken);
					if (pedidoItemEntrega == null)
						throw new NotFoundException("id pedido item Entrega não encontrado!");

					pedidoItemEntregaRepository.AddOrUpdate(currentPedidoItem, cancellationToken);
					result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = currentPedidoItem.PedidoItem });
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

		async Task<Unit> IRequestHandler<DeletePedidoItemEntrega, Unit>.Handle(DeletePedidoItemEntrega request, CancellationToken cancellationToken)
		{
			var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(request.Id, cancellationToken);

			if (pedidoItemEntrega == null)
				throw new NotFoundException("id pedido item não encontrado!");

			var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

			if (pedidoItem == null)
				throw new NotFoundException("id pedido item não encontrado!");

			pedidoItemEntrega.PedidoItem = pedidoItem;

			pedidoItemEntregaRepository.Remove(pedidoItemEntrega);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemEntrega, Unit>.Handle(SavePedidoItemEntrega request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemEntrega);

				var pedidoItem = mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.PedidoItemEntrega.IdPedidoItem }, cancellationToken).GetAwaiter().GetResult();
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemEntrega.PedidoItem = pedidoItem;

				if (string.IsNullOrWhiteSpace(request.PedidoItemEntrega.Login))
					request.PedidoItemEntrega.Login = userProvider.User.Login;

				if (request.PedidoItemEntrega.DataEntrega == null)
					request.PedidoItemEntrega.DataEntrega = DateTime.Now;

				if (request.PedidoItemEntrega.Id > 0)
					request.PedidoItemEntrega.Id = 0;

				var usuario = await userRepository.GetByLogin(request.PedidoItemEntrega.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					//if (userProvider.IsRole(Role.PERFIL_EntregaDOR_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginEntregador))
					if (!string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginComprador))
						request.PedidoItemEntrega.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de Entregador externo ou ainda não possui Entregador atribuído.");
				}

				if (request.PedidoItemEntrega.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido ainda não foi atribuído a um Entregador.");

				if (request.PedidoItemEntrega.PedidoItem.QuantidadePendenteEntrega < request.PedidoItemEntrega.Quantidade)
					throw new NotFoundException("Quantidade informada é superior a quantidade pendente de Entrega.");

				pedidoItemEntregaRepository.AddOrUpdate(request.PedidoItemEntrega, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = request.PedidoItemEntrega.PedidoItem });
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemEntregas, Unit>.Handle(SavePedidoItemEntregas request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemEntregas);

				unitOfWork.BeginTransaction();

				foreach (var pedidoItemEntregas in request.PedidoItemEntregas)
				{
					var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = pedidoItemEntregas.IdPedidoItem }, cancellationToken);
					if (pedidoItem  == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						pedidoItemEntregas.PedidoItem = pedidoItem;

					if (string.IsNullOrWhiteSpace(pedidoItemEntregas.Login))
						pedidoItemEntregas.Login = userProvider.User.Login;

					if (pedidoItemEntregas.DataEntrega == null)
						pedidoItemEntregas.DataEntrega = DateTime.Now;

					if (pedidoItemEntregas.Id > 0)
						pedidoItemEntregas.Id = 0;

					var usuario = await userRepository.GetByLogin(pedidoItemEntregas.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_EntregaDOR_EXTERNA) && !string.IsNullOrWhiteSpace(pedidoItemEntregas.PedidoItem.LoginEntregador))
						if (!string.IsNullOrWhiteSpace(pedidoItemEntregas.PedidoItem.LoginComprador))
							pedidoItemEntregas.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de Entregador externo ou ainda não possui Entregador atribuído.");
					}

					if (pedidoItemEntregas.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um Entregador.");

					if (pedidoItemEntregas.PedidoItem.QuantidadePendenteEntrega < pedidoItemEntregas.Quantidade)
						throw new NotFoundException("Quantidade informada é superior a quantidade pendente de Entrega.");

					pedidoItemEntregaRepository.AddOrUpdate(pedidoItemEntregas, cancellationToken);
					var result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemEntregas.PedidoItem });
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