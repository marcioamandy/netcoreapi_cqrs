using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;

namespace Globo.PIC.Application.QueryHandlers
{
	public class PedidoItemArteEntregaQueryHandler :
		IRequestHandler<GetById, PedidoItemArteEntrega>,
		IRequestHandler<GetByIdPedidoItemArteEntrega, PedidoItemArteEntrega>,
		IRequestHandler<ListItemArteEntregaIdByIdPedidoItem, List<PedidoItemArteEntrega>>,
		IRequestHandler<ListByIdPedidoItemArteEntrega, List<PedidoItemArteEntrega>>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteEntrega> pedidoItemEntregaRepository;

		public PedidoItemArteEntregaQueryHandler(
			IRepository<PedidoItemArteEntrega> _pedidoItemEntregaRepository
			)
		{
			pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
		}

		Task<PedidoItemArteEntrega> IRequestHandler<GetById, PedidoItemArteEntrega>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemArteEntrega> IRequestHandler<GetByIdPedidoItemArteEntrega, PedidoItemArteEntrega>.Handle(GetByIdPedidoItemArteEntrega request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.GetByIdPedidoItemEntrega(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<List<PedidoItemArteEntrega>> IRequestHandler<ListItemArteEntregaIdByIdPedidoItem, List<PedidoItemArteEntrega>>.Handle(ListItemArteEntregaIdByIdPedidoItem request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(request.IdPedidoItem, cancellationToken);

			return query;
		}

		Task<List<PedidoItemArteEntrega>> IRequestHandler<ListByIdPedidoItemArteEntrega, List<PedidoItemArteEntrega>>.Handle(ListByIdPedidoItemArteEntrega request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(request.IdPedidoItem, cancellationToken);

			return query;
		}
	}
}
