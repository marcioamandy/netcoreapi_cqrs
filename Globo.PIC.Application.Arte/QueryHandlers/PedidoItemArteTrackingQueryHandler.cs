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

namespace Globo.PIC.Application.Arte.QueryHandlers
{
	public class PedidoItemArteTrackingQueryHandler :
		IRequestHandler<ListTrackingByIdItemPedido, List<PedidoItemArteTracking>>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteTracking> trackingRepository;

		public PedidoItemArteTrackingQueryHandler(
			IRepository<PedidoItemArteTracking> _trackingRepository
			)
		{
			trackingRepository = _trackingRepository;
		}

        Task<List<PedidoItemArteTracking>> IRequestHandler<ListTrackingByIdItemPedido, List<PedidoItemArteTracking>>.Handle(ListTrackingByIdItemPedido request, CancellationToken cancellationToken)
        {
			var query = trackingRepository.ListTrackingByIdItemPedido(request.IdPedidoItem, cancellationToken);

			return query;
		}
    }
}
