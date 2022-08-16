using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;

namespace Globo.PIC.Application.QueryHandlers
{
	public class PedidoAnexosQueryHandler :
		IRequestHandler<GetById, PedidoAnexo>,
		IRequestHandler<GetByIdPedidoAnexos, PedidoAnexo>,
		IRequestHandler<ListByIdPedidoAnexos, List<PedidoAnexo>>

	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoAnexo> pedidoAnexosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoAnexosQueryHandler(
			IRepository<PedidoAnexo> _pedidoAnexosRepository, 
			IMediator _mediator
			)
		{
			pedidoAnexosRepository = _pedidoAnexosRepository; 
			mediator = _mediator;
		}

		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoAnexo> IRequestHandler<GetById, PedidoAnexo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoAnexosRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoAnexo> IRequestHandler<GetByIdPedidoAnexos, PedidoAnexo>.Handle(GetByIdPedidoAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoAnexosRepository.GetByIdPedidoAnexos(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoAnexo>> IRequestHandler<ListByIdPedidoAnexos, List<PedidoAnexo>>.Handle(ListByIdPedidoAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoAnexosRepository.ListByIdPedidoAnexos(request.IdPedido, cancellationToken);
			return query;
		}
	}
}
