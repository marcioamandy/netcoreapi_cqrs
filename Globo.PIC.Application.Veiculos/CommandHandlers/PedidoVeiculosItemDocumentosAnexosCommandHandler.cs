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
	public class PedidoItemCompraDocumentosAnexosCommandHandler :

		IRequestHandler<DeletePedidoItemCompraDocumentosAnexos>,
		IRequestHandler<UpdatePedidoItemCompraDocumentosAnexo>,
		IRequestHandler<UpdatePedidoItemCompraDocumentosAnexos>,
		IRequestHandler<SavePedidoItemCompraDocumentosAnexo>,
		IRequestHandler<SavePedidoItemCompraDocumentosAnexos>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompraDocumentosAnexos> pedidoItemCompraDocumentosAnexoRepository;
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompraDocumentos> pedidoItemCompraDocumentosRepository;

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
		public PedidoItemCompraDocumentosAnexosCommandHandler(
			IUnitOfWork _unitOfWork,
			IMediator _mediator,
			IRepository<PedidoItemCompraDocumentos> _pedidoItemCompraDocumentosRepository,
			IRepository<PedidoItemCompraDocumentosAnexos> _pedidoItemCompraDocumentosAnexoRepository)
		{
			unitOfWork = _unitOfWork;
			mediator = _mediator;
			pedidoItemCompraDocumentosRepository = _pedidoItemCompraDocumentosRepository;
			pedidoItemCompraDocumentosAnexoRepository = _pedidoItemCompraDocumentosAnexoRepository;
		}

		protected void RunEntityValidation(PedidoItemCompraDocumentosAnexos pedidoItem)
		{
			if (pedidoItem == null)
				throw new ApplicationException("O Pedido Item Compra está vazio!");
		}

		public void RunEntityValidationList(List<PedidoItemCompraDocumentosAnexos> pedidoItemCompraAnexoViewModels)
		{
			if (pedidoItemCompraAnexoViewModels.Count() == 0)
				throw new ApplicationException("O anexo da Compra está vazia!");

			foreach (var pedidoItem in pedidoItemCompraAnexoViewModels)
				RunEntityValidation(pedidoItem);
		}

		/// <summary>
		/// atualiza um pedido item anexo
		/// </summary>
		/// <param name="request">PedidoItemAnexo</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns></returns>
		async Task<Unit> IRequestHandler<UpdatePedidoItemCompraDocumentosAnexo, Unit>.Handle(UpdatePedidoItemCompraDocumentosAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompraAnexo);
				var pedidoItemCompraDocumentos = await pedidoItemCompraDocumentosRepository.GetById(request.PedidoItemCompraAnexo.IdDocumento, cancellationToken);
				if (pedidoItemCompraDocumentos == null)
					throw new NotFoundException("item Compra anexo não encontrado!");
				else
					request.PedidoItemCompraAnexo.Documentos = pedidoItemCompraDocumentos;

				pedidoItemCompraDocumentosAnexoRepository.AddOrUpdate(request.PedidoItemCompraAnexo, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");

			}
			catch (Exception error)
			{
				throw new ApplicationException(error.Message);
			}
			return Unit.Value;
		}
		async Task<Unit> IRequestHandler<UpdatePedidoItemCompraDocumentosAnexos, Unit>.Handle(UpdatePedidoItemCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			var result = true;
			try
			{
				RunEntityValidationList(request.PedidoItemCompraAnexos);

				unitOfWork.BeginTransaction();

				foreach (var pedidoAnexo in request.PedidoItemCompraAnexos)
				{
					var pedidoItemCompraDocumentos = await pedidoItemCompraDocumentosRepository.GetById(pedidoAnexo.IdDocumento, cancellationToken);

					if (pedidoItemCompraDocumentos == null)
						throw new NotFoundException("item Compra do anexo não encontrado!");
					else
						pedidoAnexo.Documentos = pedidoItemCompraDocumentos;

					pedidoItemCompraDocumentosAnexoRepository.AddOrUpdate(pedidoAnexo, cancellationToken);
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

		async Task<Unit> IRequestHandler<DeletePedidoItemCompraDocumentosAnexos, Unit>.Handle(DeletePedidoItemCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			var pedidoItemCompraDocumentosAnexo = await pedidoItemCompraDocumentosAnexoRepository.GetById(request.PedidoItemCompraAnexo.IdDocumento, cancellationToken);

			if (pedidoItemCompraDocumentosAnexo == null)
				throw new NotFoundException("Anexo da Compra não encontrada!");

			pedidoItemCompraDocumentosAnexoRepository.Remove(pedidoItemCompraDocumentosAnexo);
			var result = unitOfWork.SaveChanges();

			if (!result) throw new ApplicationException("An error has occured.");

			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemCompraDocumentosAnexo, Unit>.Handle(SavePedidoItemCompraDocumentosAnexo request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidation(request.PedidoItemCompraDocumentosAnexos);

				var pedidoItemCompra = await pedidoItemCompraDocumentosRepository.GetById(request.PedidoItemCompraDocumentosAnexos.IdDocumento, cancellationToken);
				if (pedidoItemCompra == null)
					throw new NotFoundException("item pedido do anexo não encontrado!");
				else
					request.PedidoItemCompraDocumentosAnexos.Documentos = pedidoItemCompra;

				pedidoItemCompraDocumentosAnexoRepository.AddOrUpdate(request.PedidoItemCompraDocumentosAnexos, cancellationToken);
				var result = unitOfWork.SaveChanges();
				if (!result) throw new ApplicationException("An error has occured.");
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			}
			return Unit.Value;
		}

		async Task<Unit> IRequestHandler<SavePedidoItemCompraDocumentosAnexos, Unit>.Handle(SavePedidoItemCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			try
			{
				RunEntityValidationList(request.PedidoItemCompraDocumentosAnexos);

				unitOfWork.BeginTransaction();

				foreach (var pedidoItemCompraDocumentosAnexos in request.PedidoItemCompraDocumentosAnexos)
				{
					var pedidoItemCompraDocumentos = await pedidoItemCompraDocumentosRepository.GetById(pedidoItemCompraDocumentosAnexos.IdDocumento, cancellationToken);

					if (pedidoItemCompraDocumentos == null)
						throw new NotFoundException("pedido item Compra Documentos não encontrado!");
					else
						pedidoItemCompraDocumentosAnexos.Documentos = pedidoItemCompraDocumentos;

					pedidoItemCompraDocumentosAnexoRepository.AddOrUpdate(pedidoItemCompraDocumentosAnexos, cancellationToken);

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