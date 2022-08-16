using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Arte.QueryHandlers
{
	public class PedidoItemArteQueryHandler :
		IRequestHandler<GetByIdPedidoItemArte, PedidoItemArte>,
		IRequestHandler<ListItemArteByIdPedido, List<PedidoItemArte>>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly PedidoItemArteRepository pedidoItemArteRepository;

		public PedidoItemArteQueryHandler(
			IRepository<PedidoItemArte> _pedidoItemArteRepository
			)
		{
			pedidoItemArteRepository = _pedidoItemArteRepository as PedidoItemArteRepository;
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		Task<PedidoItemArte> IRequestHandler<GetByIdPedidoItemArte, PedidoItemArte>.Handle(GetByIdPedidoItemArte request, CancellationToken cancellationToken)
		{
			var query = pedidoItemArteRepository.GetByIdPedidoItem(request.Id, cancellationToken);

			return query.AsTask();
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		Task<List<PedidoItemArte>> IRequestHandler<ListItemArteByIdPedido, List<PedidoItemArte>>.Handle(ListItemArteByIdPedido request, CancellationToken cancellationToken)
		{
			var query = pedidoItemArteRepository.GetAll();

			query = query.Where(x => x.PedidoItem.IdPedido == request.IdPedido);

			return query.ToListAsync(cancellationToken);
		}
	}
}
