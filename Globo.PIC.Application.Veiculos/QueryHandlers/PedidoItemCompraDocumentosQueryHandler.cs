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
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
	public class PeditoItemCompraDocumentosQueryHandler :
		IRequestHandler<GetById, PedidoItemCompraDocumentos>,
		IRequestHandler<GetByIdPedidoItemCompraDocumentos, PedidoItemCompraDocumentos>,
		IRequestHandler<ListItemCompraDocumentosIdByIdPedidoItemCompra, List<PedidoItemCompraDocumentos>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompraDocumentos> pedidoItemCompraDocumentosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PeditoItemCompraDocumentosQueryHandler(
			IRepository<PedidoItemCompraDocumentos> _pedidoItemCompraDocumentosRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemCompraDocumentosRepository = _pedidoItemCompraDocumentosRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		Task<PedidoItemCompraDocumentos> IRequestHandler<GetById, PedidoItemCompraDocumentos>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraDocumentosRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemCompraDocumentos> IRequestHandler<GetByIdPedidoItemCompraDocumentos, PedidoItemCompraDocumentos>.Handle(GetByIdPedidoItemCompraDocumentos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraDocumentosRepository.GetByIdPedidoItemCompraDocumentos(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemCompraDocumentos>> IRequestHandler<ListItemCompraDocumentosIdByIdPedidoItemCompra, List<PedidoItemCompraDocumentos>>.Handle(ListItemCompraDocumentosIdByIdPedidoItemCompra request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraDocumentosRepository.ListByIdPedidoItemCompraDocumentos(request.IdPedidoItemCompra, cancellationToken);
			return query;
		}
	}
}
