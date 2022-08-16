using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
	public class PedidoItemCompraDocumentosAnexosQueryHandler :
		IRequestHandler<GetById, PedidoItemCompraDocumentosAnexos>,
		IRequestHandler<GetByIdPedidoItemCompraDocumentosAnexos, PedidoItemCompraDocumentosAnexos>,
		IRequestHandler<ListByIdPedidoItemCompraDocumentosAnexos, List<PedidoItemCompraDocumentosAnexos>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompraDocumentosAnexos> pedidoItemCompraAnexoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoItemCompraDocumentosAnexosQueryHandler(
			IRepository<PedidoItemCompraDocumentosAnexos> _pedidoItemCompraAnexoRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemCompraAnexoRepository = _pedidoItemCompraAnexoRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemCompraDocumentosAnexos> IRequestHandler<GetById, PedidoItemCompraDocumentosAnexos>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraAnexoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemCompraDocumentosAnexos> IRequestHandler<GetByIdPedidoItemCompraDocumentosAnexos, PedidoItemCompraDocumentosAnexos>.Handle(GetByIdPedidoItemCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraAnexoRepository.GetByIdPedidoItemCompraDocumentosAnexos(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemCompraDocumentosAnexos>> IRequestHandler<ListByIdPedidoItemCompraDocumentosAnexos, List<PedidoItemCompraDocumentosAnexos>>.Handle(ListByIdPedidoItemCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraAnexoRepository.ListByIdPedidoItemCompraDocumentosAnexos(request.IdCompra, cancellationToken);
			return query;
		}
	}
}
