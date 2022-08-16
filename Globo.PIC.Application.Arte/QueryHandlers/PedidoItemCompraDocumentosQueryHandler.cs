using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Events; 
namespace Globo.PIC.Application.Arte.QueryHandlers
{
	public class PeditoItemCompraDocumentosQueryHandler :
		IRequestHandler<GetById, PedidoItemArteCompraDocumento>,
		IRequestHandler<GetByIdPedidoItemArteCompraDocumentos, PedidoItemArteCompraDocumento>,
		IRequestHandler<ListItemArteCompraDocumentosIdByIdPedidoItemCompra, List<PedidoItemArteCompraDocumento>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteCompraDocumento> pedidoItemCompraDocumentosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PeditoItemCompraDocumentosQueryHandler(
			IRepository<PedidoItemArteCompraDocumento> _pedidoItemCompraDocumentosRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemCompraDocumentosRepository = _pedidoItemCompraDocumentosRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		Task<PedidoItemArteCompraDocumento> IRequestHandler<GetById, PedidoItemArteCompraDocumento>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraDocumentosRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemArteCompraDocumento> IRequestHandler<GetByIdPedidoItemArteCompraDocumentos, PedidoItemArteCompraDocumento>.Handle(GetByIdPedidoItemArteCompraDocumentos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraDocumentosRepository.GetByIdPedidoItemCompraDocumentos(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemArteCompraDocumento>> IRequestHandler<ListItemArteCompraDocumentosIdByIdPedidoItemCompra, List<PedidoItemArteCompraDocumento>>.Handle(ListItemArteCompraDocumentosIdByIdPedidoItemCompra request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraDocumentosRepository.ListByIdPedidoItemCompraDocumentos(request.IdPedidoItemCompra, cancellationToken);
			return query;
		}
	}
}
