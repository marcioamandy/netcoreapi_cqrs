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
	public class PeditoItemEntregaQueryHandler :
		IRequestHandler<GetById, PedidoItemEntrega>,
		IRequestHandler<GetByIdPedidoItemEntrega, PedidoItemEntrega>,
		IRequestHandler<ListItemEntregaIdByIdPedidoItemCompra, List<PedidoItemEntrega>>,
		IRequestHandler<ListByIdPedidoItemEntrega, List<PedidoItemEntrega>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemEntrega> pedidoItemEntregaRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PeditoItemEntregaQueryHandler(
			IRepository<PedidoItemEntrega> _pedidoItemEntregaRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		Task<PedidoItemEntrega> IRequestHandler<GetById, PedidoItemEntrega>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemEntrega> IRequestHandler<GetByIdPedidoItemEntrega, PedidoItemEntrega>.Handle(GetByIdPedidoItemEntrega request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.GetByIdPedidoItemEntrega(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemEntrega>> IRequestHandler<ListItemEntregaIdByIdPedidoItemCompra, List<PedidoItemEntrega>>.Handle(ListItemEntregaIdByIdPedidoItemCompra request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(request.IdPedidoItem, cancellationToken);
			return query;
		}

		Task<List<PedidoItemEntrega>> IRequestHandler<ListByIdPedidoItemEntrega, List<PedidoItemEntrega>>.Handle(ListByIdPedidoItemEntrega request, CancellationToken cancellationToken)
		{
			var query = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(request.IdPedidoItem, cancellationToken);
			return query;
		}
	}
}
