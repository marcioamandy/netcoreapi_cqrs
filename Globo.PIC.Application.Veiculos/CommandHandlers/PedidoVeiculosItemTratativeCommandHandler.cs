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
	public class PedidoItemCompraCommandHandler :

		IRequestHandler<DeletePedidoItemCompra>,
		IRequestHandler<UpdatePedidoItemCompra>,
		IRequestHandler<UpdatePedidoItemCompras>,
		IRequestHandler<SavePedidoItemCompra>,
		IRequestHandler<SavePedidoItemCompras>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompra> pedidoItemCompraRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemEntrega> pedidoItemEntregaRepository;

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
		public PedidoItemCompraCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemCompra> _pedidoItemCompraRepository,
			IRepository<PedidoItemEntrega> _pedidoItemEntregaRepository,
			IRepository<PedidoItem> _pedidoItemRepository,
			IRepository<User> _userRepository,
			IUserProvider _userProvider
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
			pedidoItemRepository = _pedidoItemRepository;
			userRepository = _userRepository;
			userProvider = _userProvider;
		}

		protected void RunEntityValidation(PedidoItemCompra pedidoItem)
		{
			if (pedidoItem == null)
				throw new ApplicationException("O Pedido Item Compra está vazio!");

			if (pedidoItem.IdPedidoItem == 0)
				throw new ApplicationException("O id do pedido item é obrigatório!");

			if (pedidoItem.Login == "")
				throw new ApplicationException("O Login é obrigatório!");

		}
		public void RunEntityValidationList(List<PedidoItemCompra> pedidoItemCompraViewModels)
		{
			if (pedidoItemCompraViewModels.Count() == 0)
				throw new ApplicationException("O Pedido Item Compra está vazio!");

			foreach (var pedidoItem in pedidoItemCompraViewModels)
				RunEntityValidation(pedidoItem);
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="request">PedidoItem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemCompra, Unit>.Handle(UpdatePedidoItemCompra request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompra);

				var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.PedidoItemCompra.IdPedidoItem }, cancellationToken) ;
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemCompra.PedidoItem = pedidoItem;

				var pedidoItemEntrega = pedidoItemEntregaRepository.GetAll().Where(a => a.IdPedidoItem == request.PedidoItemCompra.IdPedidoItem).ToList();
				if (pedidoItemEntrega.Count > 0)
					throw new NotFoundException(string.Format("Compra {0} não pode ser editada, já existe entrega vinculada.", request.PedidoItemCompra.Id));

				if (string.IsNullOrWhiteSpace(request.PedidoItemCompra.Login))
					request.PedidoItemCompra.Login = userProvider.User.Login;

				if (request.PedidoItemCompra.DataCompra == null)
					request.PedidoItemCompra.DataCompra = DateTime.Now;

				var usuario = await userRepository.GetByLogin(request.PedidoItemCompra.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					//if (userProvider.IsRole(Role.PERFIL_COMPRADOR_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemCompra.PedidoItem.LoginComprador))
					if (!string.IsNullOrWhiteSpace(request.PedidoItemCompra.PedidoItem.LoginComprador))
						request.PedidoItemCompra.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
				}

				if (pedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

				var compraLancada = await pedidoItemCompraRepository.GetByIdPedidoItemCompra(request.PedidoItemCompra.Id, cancellationToken);
				if (compraLancada == null)
					throw new NotFoundException("Compra não encontrada!");

				if (((pedidoItem.QuantidadePendenteCompra + compraLancada.Quantidade) - request.PedidoItemCompra.Quantidade) < 0)
					throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

				var pedidoItemCompra = await pedidoItemCompraRepository.GetById(request.PedidoItemCompra.Id, cancellationToken);
				if (pedidoItemCompra == null)
					throw new NotFoundException("id pedido item Compra não encontrado!");

				pedidoItemCompraRepository.AddOrUpdate(request.PedidoItemCompra, cancellationToken);
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
		async Task<Unit> IRequestHandler<UpdatePedidoItemCompras, Unit>.Handle(UpdatePedidoItemCompras request, CancellationToken cancellationToken)
		{
			var result = true;
			try
			{
				RunEntityValidationList(request.PedidoItemCompras);

				unitOfWork.BeginTransaction();

				foreach (var currentPedidoItem in request.PedidoItemCompras)
				{
					var pedidoItemEntrega = pedidoItemEntregaRepository.GetAll().Where(a => a.IdPedidoItem == currentPedidoItem.IdPedidoItem).ToList();
					if (pedidoItemEntrega != null)
						throw new NotFoundException(string.Format("Compra {0} não pode ser editada, já existe entrega vinculada.", currentPedidoItem.Id));

					//var pedidoItemCompra = await pedidoItemCompraRepository.GetById(currentPedidoItem.Id, cancellationToken);
					//if (pedidoItemCompra == null)
					//	throw new NotFoundException("id pedido item Compra não encontrado!");
					//else
					//	currentPedidoItem = pedidoItemCompra;

					var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentPedidoItem.IdPedidoItem }, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						currentPedidoItem.PedidoItem = pedidoItem;

					if (string.IsNullOrWhiteSpace(currentPedidoItem.Login))
						currentPedidoItem.Login = userProvider.User.Login;

					if (currentPedidoItem.DataCompra == null)
						currentPedidoItem.DataCompra = DateTime.Now;

					var usuario = await userRepository.GetByLogin(currentPedidoItem.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_COMPRADOR_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemCompra.PedidoItem.LoginComprador))
						if (!string.IsNullOrWhiteSpace(currentPedidoItem.PedidoItem.LoginComprador))
							currentPedidoItem.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
					}

					if (pedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

					var compraLancada = await pedidoItemCompraRepository.GetByIdPedidoItemCompra(currentPedidoItem.Id, cancellationToken);
					if (compraLancada == null)
						throw new NotFoundException("Compra não encontrada!");

					if (((pedidoItem.QuantidadePendenteCompra + compraLancada.Quantidade) - currentPedidoItem.Quantidade) < 0)
						throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

					var pedidoItemCompra = await pedidoItemCompraRepository.GetById(currentPedidoItem.Id, cancellationToken);
					if (pedidoItemCompra == null)
						throw new NotFoundException("id pedido item Compra não encontrado!");

					pedidoItemCompraRepository.AddOrUpdate(currentPedidoItem, cancellationToken);
					result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });
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

		async Task<Unit> IRequestHandler<DeletePedidoItemCompra, Unit>.Handle(DeletePedidoItemCompra request, CancellationToken cancellationToken)
		{
			var pedidoItemCompra = await pedidoItemCompraRepository.GetById(request.Id, cancellationToken);

			if (pedidoItemCompra == null)
				throw new NotFoundException("id pedido item compra não encontrado!");

			var pedidoItem = await pedidoItemRepository.GetById(request.IdPedidoItem, cancellationToken);

			var existEntrega = await pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(pedidoItemCompra.IdPedidoItem, cancellationToken);
			if (existEntrega.Count > 0)
				throw new NotFoundException(string.Format("Existe entregas para a compra {0}, não é permitido excluir a compra.",pedidoItemCompra.Id));

			pedidoItemCompraRepository.Remove(pedidoItemCompra);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemCompra, Unit>.Handle(SavePedidoItemCompra request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompra);

				var pedidoItem = mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.PedidoItemCompra.IdPedidoItem }, cancellationToken).GetAwaiter().GetResult(); ;
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemCompra.PedidoItem = pedidoItem;

				if (string.IsNullOrWhiteSpace(request.PedidoItemCompra.Login))
					request.PedidoItemCompra.Login = userProvider.User.Login;

				if (request.PedidoItemCompra.DataCompra == null)
					request.PedidoItemCompra.DataCompra = DateTime.Now;

				if (request.PedidoItemCompra.Id > 0)
					request.PedidoItemCompra.Id = 0;

				var usuario = await userRepository.GetByLogin(request.PedidoItemCompra.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					//if (userProvider.IsRole(Role.PERFIL_COMPRADOR_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemCompra.PedidoItem.LoginComprador))
					if (!string.IsNullOrWhiteSpace(request.PedidoItemCompra.PedidoItem.LoginComprador))
						request.PedidoItemCompra.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
				}

				if (pedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

				if (pedidoItem.QuantidadePendenteCompra < request.PedidoItemCompra.Quantidade)
					throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

				pedidoItemCompraRepository.AddOrUpdate(request.PedidoItemCompra, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemCompras, Unit>.Handle(SavePedidoItemCompras request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemCompras);

				unitOfWork.BeginTransaction();

				foreach (var pedidoItemCompras in request.PedidoItemCompras)
				{
					var pedidoItem = await pedidoItemRepository.GetById(pedidoItemCompras.IdPedidoItem, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						pedidoItemCompras.PedidoItem = pedidoItem;

					if (string.IsNullOrWhiteSpace(pedidoItemCompras.Login))
						pedidoItemCompras.Login = userProvider.User.Login;

					if (pedidoItemCompras.DataCompra == null)
						pedidoItemCompras.DataCompra = DateTime.Now;

					if (pedidoItemCompras.Id > 0)
						pedidoItemCompras.Id = 0;

					var usuario = await userRepository.GetByLogin(pedidoItemCompras.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_COMPRADOR_EXTERNA) && !string.IsNullOrWhiteSpace(pedidoItemCompras.PedidoItem.LoginComprador))
						if (!string.IsNullOrWhiteSpace(pedidoItemCompras.PedidoItem.LoginComprador))
							pedidoItemCompras.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
					}

					if (pedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

					if (pedidoItem.QuantidadePendenteCompra < pedidoItemCompras.Quantidade)
						throw new NotFoundException("Quantidade informada é superior a quantidade pendente de compra.");

					pedidoItemCompraRepository.AddOrUpdate(pedidoItemCompras, cancellationToken);
					var result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItem });
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