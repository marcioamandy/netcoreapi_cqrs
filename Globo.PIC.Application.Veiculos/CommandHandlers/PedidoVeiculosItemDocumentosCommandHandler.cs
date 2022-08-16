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
	public class PedidoItemCompraDocumentosCommandHandler :

		IRequestHandler<DeletePedidoItemCompraDocumentos>,
		IRequestHandler<UpdatePedidoItemCompraDocumento>,
		IRequestHandler<UpdatePedidoItemCompraDocumentos>,
		IRequestHandler<SavePedidoItemCompraDocumento>,
		IRequestHandler<SavePedidoItemCompraDocumentos>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompraDocumentos> pedidoItemCompraDocumentosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompra> pedidoItemCompraRepository;

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
		public PedidoItemCompraDocumentosCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemCompraDocumentos> _pedidoItemCompraDocumentosRepository,
			IRepository<PedidoItemCompra> _pedidoItemCompraRepository,
			IRepository<User> _userRepository,
			IUserProvider _userProvider
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemCompraDocumentosRepository = _pedidoItemCompraDocumentosRepository;
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			userRepository = _userRepository;
			userProvider = _userProvider;
		}

		protected void RunEntityValidation(PedidoItemCompraDocumentos documentos)
		{
			if (documentos == null)
				throw new ApplicationException("O Pedido Item Entrega está vazio!");

			if (documentos.IdCompra == 0)
				throw new ApplicationException("O id do pedido item compra é obrigatório!");

			if (documentos.Login == "")
				throw new ApplicationException("O Login é obrigatório!");

		}
		public void RunEntityValidationList(List<PedidoItemCompraDocumentos> pedidoItemCompraDocumentosViewModels)
		{
			if (pedidoItemCompraDocumentosViewModels.Count() == 0)
				throw new ApplicationException("O Pedido Item Compra Documentos está vazio!");

			foreach (var documento in pedidoItemCompraDocumentosViewModels)
				RunEntityValidation(documento);
		}

		/// <summary>
		/// atualiza um pedido item
		/// </summary>
		/// <param name="request">PedidoItem</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemCompraDocumento, Unit>.Handle(UpdatePedidoItemCompraDocumento request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompraDocumentos);

				var compra = await mediator.Send<PedidoItemCompra>(new GetByIdPedidoItemCompra() { Id = request.PedidoItemCompraDocumentos.IdCompra }, cancellationToken);
				if (compra == null)
					throw new NotFoundException("id pedido item compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compras = compra;

				var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.PedidoItemCompraDocumentos.Compras.IdPedidoItem }, cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compras.PedidoItem = pedidoItem;

				var usuarioCompra = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Compras.Login, cancellationToken);
				if (usuarioCompra == null)
					throw new NotFoundException("login vinculado a compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compras.User = usuarioCompra;

				if (string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Login))
					request.PedidoItemCompraDocumentos.Login = userProvider.User.Login;

				if (request.PedidoItemCompraDocumentos.DataDocumento == null)
					request.PedidoItemCompraDocumentos.DataDocumento = DateTime.Now;

				var usuario = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					//if (userProvider.IsRole(Role.PERFIL_Comprador_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginComprador))
					if (!string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Compras.PedidoItem.LoginComprador))
						request.PedidoItemCompraDocumentos.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de Comprador externo ou ainda não possui Comprador atribuído.");
				}

				if (compra.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido ainda não foi atribuído a um Comprador.");

				//var DocumentoLancado = await pedidoItemCompraDocumentosRepository.GetByIdPedidoItemCompraDocumentos(request.PedidoItemCompraDocumentos.Id, cancellationToken);
				//if (DocumentoLancado == null)
				//	throw new NotFoundException("Documento não encontrado!");

				long qtdedocumentos = 0;
				var documentos = pedidoItemCompraDocumentosRepository.GetAll()
					.Where(	a => a.IdCompra == compra.Id && 
							a.Id != request.PedidoItemCompraDocumentos.Id)
					.Select(a => a.Quantidade).Sum();
				if (documentos > 0)
					qtdedocumentos = documentos;

				if (compra.Quantidade < request.PedidoItemCompraDocumentos.Quantidade + qtdedocumentos)
					throw new NotFoundException("Quantidade informada é superior a quantidade comprada.");

				var pedidoItemEntrega = await pedidoItemCompraDocumentosRepository.GetById(request.PedidoItemCompraDocumentos.Id, cancellationToken);
				if (pedidoItemEntrega == null)
					throw new NotFoundException("id pedido item Entrega não encontrado!");

				pedidoItemCompraDocumentosRepository.AddOrUpdate(request.PedidoItemCompraDocumentos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = compra.PedidoItem });

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
		async Task<Unit> IRequestHandler<UpdatePedidoItemCompraDocumentos, Unit>.Handle(UpdatePedidoItemCompraDocumentos request, CancellationToken cancellationToken)
		{
			var result = true;
			try
			{
				RunEntityValidationList(request.PedidoItemCompraDocumentos);

				unitOfWork.BeginTransaction();

				foreach (var currentDocumento in request.PedidoItemCompraDocumentos)
				{
					//var pedidoItemEntrega = await pedidoItemEntregaRepository.GetById(currentPedidoItem.Id, cancellationToken);
					//if (pedidoItemEntrega == null)
					//	throw new NotFoundException("id pedido item Entrega não encontrado!");
					//else
					//	currentPedidoItem = pedidoItemEntrega;

					var compra = await mediator.Send<PedidoItemCompra>(new GetByIdPedidoItemCompra() { Id = currentDocumento.IdCompra }, cancellationToken);
					if (compra == null)
						throw new NotFoundException("id pedido item compra não encontrado!");
					else
						currentDocumento.Compras = compra;

					var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = currentDocumento.Compras.IdPedidoItem }, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						currentDocumento.Compras.PedidoItem = pedidoItem;

					var usuarioCompra = await userRepository.GetByLogin(currentDocumento.Compras.Login, cancellationToken);
					if (usuarioCompra == null)
						throw new NotFoundException("login vinculado a compra não encontrado!");
					else
						currentDocumento.Compras.User = usuarioCompra;

					if (string.IsNullOrWhiteSpace(currentDocumento.Login))
						currentDocumento.Login = userProvider.User.Login;

					if (currentDocumento.DataDocumento == null)
						currentDocumento.DataDocumento = DateTime.Now;

					var usuario = await userRepository.GetByLogin(currentDocumento.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_Comprador_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginComprador))
						if (!string.IsNullOrWhiteSpace(currentDocumento.Compras.PedidoItem.LoginComprador))
							currentDocumento.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de Comprador externo ou ainda não possui Comprador atribuído.");
					}

					if (currentDocumento.Compras.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um Comprador.");

					//var DocumentoLancado = await pedidoItemCompraDocumentosRepository.GetByIdPedidoItemCompraDocumentos(currentDocumento.Id, cancellationToken);
					//if (DocumentoLancado == null)
					//	throw new NotFoundException("Documento não encontrado!");

					long qtdedocumentos = 0;
					var documentos = pedidoItemCompraDocumentosRepository.GetAll()
						.Where(a => a.IdCompra == compra.Id &&
							a.Id != currentDocumento.Id)
						.Select(a => a.Quantidade).Sum();
					if (documentos > 0)
						qtdedocumentos = documentos;

					if (compra.Quantidade < currentDocumento.Quantidade + qtdedocumentos)
						throw new NotFoundException("Quantidade informada é superior a quantidade comprada.");

					var pedidoItemDocumento = await pedidoItemCompraDocumentosRepository.GetById(currentDocumento.Id, cancellationToken);
					if (pedidoItemDocumento == null)
						throw new NotFoundException("id pedido item Entrega não encontrado!");

					pedidoItemCompraDocumentosRepository.AddOrUpdate(currentDocumento, cancellationToken);
					result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = currentDocumento.Compras.PedidoItem });
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

		async Task<Unit> IRequestHandler<DeletePedidoItemCompraDocumentos, Unit>.Handle(DeletePedidoItemCompraDocumentos request, CancellationToken cancellationToken)
		{
			var pedidoItemDocumento = await pedidoItemCompraDocumentosRepository.GetById(request.Id, cancellationToken);

			if (pedidoItemDocumento == null)
				throw new NotFoundException("id pedido item Documento não encontrado!");

			var compra = await mediator.Send<PedidoItemCompra>(new GetByIdPedidoItemCompra() { Id = pedidoItemDocumento.IdCompra }, cancellationToken);
			if (compra == null)
				throw new NotFoundException("id pedido item compra não encontrado!");
			else
				pedidoItemDocumento.Compras = compra;

			var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = pedidoItemDocumento.Compras.IdPedidoItem }, cancellationToken);
			if (pedidoItem == null)
				throw new NotFoundException("id pedido item não encontrado!");
			else
				pedidoItemDocumento.Compras.PedidoItem = pedidoItem;

			var usuarioCompra = await userRepository.GetByLogin(pedidoItemDocumento.Compras.Login, cancellationToken);
			if (usuarioCompra == null)
				throw new NotFoundException("login vinculado a compra não encontrado!");
			else
				pedidoItemDocumento.Compras.User = usuarioCompra;

			pedidoItemCompraDocumentosRepository.Remove(pedidoItemDocumento);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = compra.PedidoItem });

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemCompraDocumento, Unit>.Handle(SavePedidoItemCompraDocumento request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompraDocumentos);

				var compra = await mediator.Send<PedidoItemCompra>(new GetByIdPedidoItemCompra() { Id = request.PedidoItemCompraDocumentos.IdCompra }, cancellationToken);
				if (compra == null)
					throw new NotFoundException("id pedido item compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compras = compra;

				var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = request.PedidoItemCompraDocumentos.Compras.IdPedidoItem }, cancellationToken);
				if (pedidoItem == null)
					throw new NotFoundException("id pedido item não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compras.PedidoItem = pedidoItem;

				var usuarioCompra = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Compras.Login, cancellationToken);
				if (usuarioCompra == null)
					throw new NotFoundException("login vinculado a compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compras.User = usuarioCompra;


				if (string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Login))
					request.PedidoItemCompraDocumentos.Login = userProvider.User.Login;

				if (request.PedidoItemCompraDocumentos.DataDocumento == null)
					request.PedidoItemCompraDocumentos.DataDocumento = DateTime.Now;

				if (request.PedidoItemCompraDocumentos.Id > 0)
					request.PedidoItemCompraDocumentos.Id = 0;

				var usuario = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					request.PedidoItemCompraDocumentos.Compras = compra;
					//if (userProvider.IsRole(Role.PERFIL_Comprador_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginComprador))
					if (!string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Compras.PedidoItem.LoginComprador))
						request.PedidoItemCompraDocumentos.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
				}

				if (request.PedidoItemCompraDocumentos.Compras.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido ainda não foi atribuído a um comprador.");

				long qtdedocumentos = 0;
				var documentos = pedidoItemCompraDocumentosRepository.GetAll()
					.Where(a => a.IdCompra == compra.Id)
					.Select(a => a.Quantidade).Sum();
				if (documentos > 0)
					qtdedocumentos = documentos;

 				if (compra.Quantidade < request.PedidoItemCompraDocumentos.Quantidade + qtdedocumentos)
					throw new NotFoundException("Quantidade informada é superior a quantidade comprada.");

				pedidoItemCompraDocumentosRepository.AddOrUpdate(request.PedidoItemCompraDocumentos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = request.PedidoItemCompraDocumentos.Compras.PedidoItem });
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemCompraDocumentos, Unit>.Handle(SavePedidoItemCompraDocumentos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemCompraDocumentos);

				unitOfWork.BeginTransaction();

				foreach (var pedidoItemCompraDocumentos in request.PedidoItemCompraDocumentos)
				{
					var compra = await mediator.Send<PedidoItemCompra>(new GetByIdPedidoItemCompra() { Id = pedidoItemCompraDocumentos.IdCompra }, cancellationToken);
					if (compra == null)
						throw new NotFoundException("id pedido item compra não encontrado!");
					else
						pedidoItemCompraDocumentos.Compras = compra;

					var pedidoItem = await mediator.Send<PedidoItem>(new GetByIdPedidoItem() { Id = pedidoItemCompraDocumentos.Compras.IdPedidoItem }, cancellationToken);
					if (pedidoItem == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						pedidoItemCompraDocumentos.Compras.PedidoItem = pedidoItem;

					var usuarioCompra = await userRepository.GetByLogin(pedidoItemCompraDocumentos.Compras.Login, cancellationToken);
					if (usuarioCompra == null)
						throw new NotFoundException("login vinculado a compra não encontrado!");
					else
						pedidoItemCompraDocumentos.Compras.User = usuarioCompra;

					if (string.IsNullOrWhiteSpace(pedidoItemCompraDocumentos.Login))
						pedidoItemCompraDocumentos.Login = userProvider.User.Login;

					if (pedidoItemCompraDocumentos.DataDocumento == null)
						pedidoItemCompraDocumentos.DataDocumento = DateTime.Now;

					if (pedidoItemCompraDocumentos.Id > 0)
						pedidoItemCompraDocumentos.Id = 0;

					var usuario = await userRepository.GetByLogin(pedidoItemCompraDocumentos.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_Comprador_EXTERNA) && !string.IsNullOrWhiteSpace(pedidoItemEntregas.PedidoItem.LoginComprador))
						if (!string.IsNullOrWhiteSpace(pedidoItemCompraDocumentos.Compras.PedidoItem.LoginComprador))
							pedidoItemCompraDocumentos.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de Comprador externo ou ainda não possui Comprador atribuído.");
					}

					if (pedidoItemCompraDocumentos.Compras.PedidoItem.IdStatus != (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um Comprador.");

					long qtdedocumentos = 0;
					var documentos = pedidoItemCompraDocumentosRepository.GetAll()
						.Where(a => a.IdCompra == compra.Id)
						.Select(a => a.Quantidade).Sum();
					if (documentos > 0)
						qtdedocumentos = documentos;

					if (compra.Quantidade < pedidoItemCompraDocumentos.Quantidade + qtdedocumentos)
						throw new NotFoundException("Quantidade informada é superior a quantidade comprada.");

					pedidoItemCompraDocumentosRepository.AddOrUpdate(pedidoItemCompraDocumentos, cancellationToken);
					var result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemCompraDocumentos.Compras.PedidoItem });
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