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
using Globo.PIC.Domain.Exceptions; 
using Globo.PIC.Domain.Enums;
namespace Globo.PIC.Application.Services.Commands
{
	/// <summary>
	/// PedidoItemCompraDocumentosCommandHandler
	/// </summary>
	public class PedidoItemCompraDocumentosCommandHandler :

		IRequestHandler<DeletePedidoItemCompraDocumentos>,
		IRequestHandler<UpdatePedidoItemCompraDocumento>,
		IRequestHandler<UpdatePedidoItemCompraDocumentos>,
		IRequestHandler<CreatePedidoItemCompraDocumento>,
		IRequestHandler<CreatePedidoItemCompraDocumentos>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteCompraDocumento> pedidoItemCompraDocumentosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArte> pedidoItemArteRepository;
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Usuario> userRepository;

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
			IRepository<PedidoItemArteCompraDocumento> _pedidoItemCompraDocumentosRepository,
			IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
			IRepository<Usuario> _userRepository,
			IRepository<PedidoItemArte> _pedidoItemArteRepository,
			IUserProvider _userProvider
			)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemCompraDocumentosRepository = _pedidoItemCompraDocumentosRepository;
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			userRepository = _userRepository;
			pedidoItemArteRepository = _pedidoItemArteRepository;
			userProvider = _userProvider;
		}

		protected void RunEntityValidation(PedidoItemArteCompraDocumento documentos)
		{
			if (documentos == null)
				throw new ApplicationException("O Pedido Item Entrega está vazio!");

			if (documentos.IdCompra == 0)
				throw new ApplicationException("O id do pedido item compra é obrigatório!");

			if (documentos.Login == "")
				throw new ApplicationException("O Login é obrigatório!");

		}
		public void RunEntityValidationList(List<PedidoItemArteCompraDocumento> pedidoItemCompraDocumentosViewModels)
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

				var compra = await pedidoItemCompraRepository.GetById(request.PedidoItemCompraDocumentos.IdCompra, cancellationToken);
 
				if (compra == null)
					throw new NotFoundException("id pedido item arte compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compra = compra;

				var pedidoItemArte =
					await pedidoItemArteRepository.GetById(request.PedidoItemCompraDocumentos.Compra.IdPedidoItemArte,
					cancellationToken);

				if (pedidoItemArte == null)
					throw new NotFoundException("id pedido item arte não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compra.PedidoItemArte = pedidoItemArte;

				var usuarioCompra = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Compra.Login, cancellationToken);
				if (usuarioCompra == null)
					throw new NotFoundException("login vinculado a compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compra.Usuario = usuarioCompra;

				if (string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Login))
					request.PedidoItemCompraDocumentos.Login = userProvider.User.Login;

				if (request.PedidoItemCompraDocumentos.DataDocumento == null)
					request.PedidoItemCompraDocumentos.DataDocumento = DateTime.Now;

				var usuario = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Login, cancellationToken);
				if (usuario == null)
					throw new NotFoundException("login não encontrado!");
				else
				{
					if (!string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Compra.PedidoItemArte.CompradoPorLogin))
						request.PedidoItemCompraDocumentos.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de Comprador externo ou ainda não possui Comprador atribuído.");
				}

				if (compra.PedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
					throw new NotFoundException("Item pedido arte ainda não foi atribuído a um Comprador.");

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

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = compra.PedidoItemArte.PedidoItem });

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

				foreach (var documento in request.PedidoItemCompraDocumentos)
				{

					var compra = await pedidoItemCompraRepository.GetById(documento.IdCompra, cancellationToken);
					if (compra == null)
						throw new NotFoundException("id pedido item compra não encontrado!");
					else
						documento.Compra = compra;
					 
					var pedidoItemArte =
						await pedidoItemArteRepository.GetById(documento.Compra.IdPedidoItemArte, cancellationToken);

					if (pedidoItemArte == null)
						throw new NotFoundException("id pedido item arte não encontrado!");
					else
						documento.Compra.PedidoItemArte = pedidoItemArte;

					var usuarioCompra = await userRepository.GetByLogin(documento.Compra.Login, cancellationToken);
					if (usuarioCompra == null)
						throw new NotFoundException("login vinculado a compra não encontrado!");
					else
						documento.Compra.Usuario = usuarioCompra;

					if (string.IsNullOrWhiteSpace(documento.Login))
						documento.Login = userProvider.User.Login;

					if (documento.DataDocumento == null)
						documento.DataDocumento = DateTime.Now;

					var usuario = await userRepository.GetByLogin(documento.Login, cancellationToken);
					if (usuario == null)
						throw new NotFoundException("login não encontrado!");
					else
					{
						//if (userProvider.IsRole(Role.PERFIL_Comprador_EXTERNA) && !string.IsNullOrWhiteSpace(request.PedidoItemEntrega.PedidoItem.LoginComprador))
						if (!string.IsNullOrWhiteSpace(documento.Compra.PedidoItemArte.CompradoPorLogin))
							documento.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de Comprador externo ou ainda não possui Comprador atribuído.");
					}

					if (documento.Compra.PedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
						throw new NotFoundException("Item pedido ainda não foi atribuído a um Comprador.");
					 
					long qtdedocumentos = 0;
					var documentos = pedidoItemCompraDocumentosRepository.GetAll()
						.Where(a => a.IdCompra == compra.Id &&
							a.Id != documento.Id)
						.Select(a => a.Quantidade).Sum();
					if (documentos > 0)
						qtdedocumentos = documentos;

					if (compra.Quantidade < documento.Quantidade + qtdedocumentos)
						throw new NotFoundException("Quantidade informada é superior a quantidade comprada.");

					var pedidoItemDocumento = await pedidoItemCompraDocumentosRepository.GetById(documento.Id, cancellationToken);
					if (pedidoItemDocumento == null)
						throw new NotFoundException("id pedido item Entrega não encontrado!");

					pedidoItemCompraDocumentosRepository.AddOrUpdate(documento, cancellationToken);
					result = unitOfWork.SaveChanges();
					if (!result) throw new ApplicationException("An error has occured.");

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = documento.Compra.PedidoItemArte.PedidoItem });
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
			 
			var compra = await pedidoItemCompraRepository.GetById(pedidoItemDocumento.IdCompra, cancellationToken);

			if (compra == null)
				throw new NotFoundException("id pedido item compra não encontrado!");
			else
				pedidoItemDocumento.Compra = compra;
			 
			var pedidoItemArte = await pedidoItemArteRepository.GetById(pedidoItemDocumento.Compra.IdPedidoItemArte, cancellationToken);
			if (pedidoItemArte == null)
				throw new NotFoundException("id pedido item arte não encontrado!");
			else
				pedidoItemDocumento.Compra.PedidoItemArte = pedidoItemArte;

			var usuarioCompra = await userRepository.GetByLogin(pedidoItemDocumento.Compra.Login, cancellationToken);
			if (usuarioCompra == null)
				throw new NotFoundException("login vinculado a compra não encontrado!");
			else
				pedidoItemDocumento.Compra.Usuario = usuarioCompra;

			pedidoItemCompraDocumentosRepository.Remove(pedidoItemDocumento);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = compra.PedidoItemArte.PedidoItem });

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<CreatePedidoItemCompraDocumento, Unit>.Handle(CreatePedidoItemCompraDocumento request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompraDocumentos);

				var compra = await pedidoItemCompraRepository.GetById(request.PedidoItemCompraDocumentos.IdCompra, cancellationToken);

				if (compra == null)
					throw new NotFoundException("id pedido item compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compra = compra;
				
				var pedidoItemArte = await pedidoItemArteRepository.GetById(request.PedidoItemCompraDocumentos.Compra.IdPedidoItemArte, cancellationToken);

				if (pedidoItemArte == null)
					throw new NotFoundException("id pedido item arte não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compra.PedidoItemArte = pedidoItemArte;

				var usuarioCompra = await userRepository.GetByLogin(request.PedidoItemCompraDocumentos.Compra.Login, cancellationToken);
				if (usuarioCompra == null)
					throw new NotFoundException("login vinculado a compra não encontrado!");
				else
					request.PedidoItemCompraDocumentos.Compra.Usuario = usuarioCompra;


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
					request.PedidoItemCompraDocumentos.Compra = compra;
					if (!string.IsNullOrWhiteSpace(request.PedidoItemCompraDocumentos.Compra.PedidoItemArte.CompradoPorLogin))
						request.PedidoItemCompraDocumentos.User = usuario;
					else
						throw new NotFoundException("login atribuido não tem perfil de comprador externo ou ainda não possui comprador atribuído.");
				}

				if (request.PedidoItemCompraDocumentos.Compra.PedidoItemArte.IdStatus 
					!= (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
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

				await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = request.PedidoItemCompraDocumentos.Compra.PedidoItemArte.PedidoItem });
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<CreatePedidoItemCompraDocumentos, Unit>.Handle(CreatePedidoItemCompraDocumentos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemCompraDocumentos);

				unitOfWork.BeginTransaction();

				foreach (var pedidoItemCompraDocumentos in request.PedidoItemCompraDocumentos)
				{
					var compra = await pedidoItemCompraRepository.GetById(pedidoItemCompraDocumentos.IdCompra, cancellationToken);
					if (compra == null)
						throw new NotFoundException("id pedido item compra não encontrado!");
					else
						pedidoItemCompraDocumentos.Compra = compra;
					 
					var pedidoItemArte = await pedidoItemArteRepository.GetById(pedidoItemCompraDocumentos.Compra.IdPedidoItemArte, cancellationToken);

					if (pedidoItemArte == null)
						throw new NotFoundException("id pedido item não encontrado!");
					else
						pedidoItemCompraDocumentos.Compra.PedidoItemArte = pedidoItemArte;

					var usuarioCompra = await userRepository.GetByLogin(pedidoItemCompraDocumentos.Compra.Login, cancellationToken);
					if (usuarioCompra == null)
						throw new NotFoundException("login vinculado a compra não encontrado!");
					else
						pedidoItemCompraDocumentos.Compra.Usuario = usuarioCompra;

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
						if (!string.IsNullOrWhiteSpace(pedidoItemCompraDocumentos.Compra.PedidoItemArte.CompradoPorLogin))
							pedidoItemCompraDocumentos.User = usuario;
						else
							throw new NotFoundException("login atribuido não tem perfil de Comprador externo ou ainda não possui Comprador atribuído.");
					}

					if (pedidoItemCompraDocumentos.Compra.PedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
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

					await mediator.Publish(new OnAtualizarQtdeItens() { PedidoItem = pedidoItemCompraDocumentos.Compra.PedidoItemArte.PedidoItem });
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