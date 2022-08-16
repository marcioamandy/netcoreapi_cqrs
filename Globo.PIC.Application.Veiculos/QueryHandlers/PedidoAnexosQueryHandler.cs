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
	public class PedidoAnexosQueryHandler :
		IRequestHandler<GetById, PedidoAnexos>,
		IRequestHandler<GetByIdPedidoAnexos, PedidoAnexos>,
		IRequestHandler<ListByIdPedidoAnexos, List<PedidoAnexos>>

	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoAnexos> pedidoAnexosRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoAnexosQueryHandler(
			IRepository<PedidoAnexos> _pedidoAnexosRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoAnexosRepository = _pedidoAnexosRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoAnexos> IRequestHandler<GetById, PedidoAnexos>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoAnexosRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoAnexos> IRequestHandler<GetByIdPedidoAnexos, PedidoAnexos>.Handle(GetByIdPedidoAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoAnexosRepository.GetByIdPedidoAnexos(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoAnexos>> IRequestHandler<ListByIdPedidoAnexos, List<PedidoAnexos>>.Handle(ListByIdPedidoAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoAnexosRepository.ListByIdPedidoAnexos(request.IdPedido, cancellationToken);
			return query;
		}
	}
}
