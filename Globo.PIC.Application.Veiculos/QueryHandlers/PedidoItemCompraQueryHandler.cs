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
	public class PedidoItemCompraQueryHandler :
		IRequestHandler<GetById, PedidoItemCompra>,
		IRequestHandler<GetByIdPedidoItemCompra, PedidoItemCompra>,
		IRequestHandler<GetByIdPedidoItemCompraExistEntrega, bool>,
		IRequestHandler<ListByIdPedidoItemCompra, List<PedidoItemCompra>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemCompra> pedidoItemCompraRepository;

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

		public PedidoItemCompraQueryHandler(
			IRepository<PedidoItemCompra> _pedidoItemCompraRepository,
			IRepository<PedidoItemEntrega> _pedidoItemEntregaRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemCompraRepository = _pedidoItemCompraRepository;
			pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}


		Task<PedidoItemCompra> IRequestHandler<GetById, PedidoItemCompra>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<bool> IRequestHandler<GetByIdPedidoItemCompraExistEntrega, bool>.Handle(GetByIdPedidoItemCompraExistEntrega request, CancellationToken cancellationToken)
		{
			
			bool result = false;

			var existEntrega = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(request.IdPedidoItem, cancellationToken).GetAwaiter().GetResult();
			if (existEntrega.Count == 0)
				result = false;
			else
				result = true;

			return Task.FromResult(result);
		}

		Task<PedidoItemCompra> IRequestHandler<GetByIdPedidoItemCompra, PedidoItemCompra>.Handle(GetByIdPedidoItemCompra request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraRepository.GetByIdPedidoItemCompra(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemCompra>> IRequestHandler<ListByIdPedidoItemCompra, List<PedidoItemCompra>>.Handle(ListByIdPedidoItemCompra request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraRepository.ListByIdPedidoItemCompra(request.IdPedidoItem, cancellationToken);
			return query;
		}
	}
}
