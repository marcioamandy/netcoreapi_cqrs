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
	public class PedidoItemArteCompraQueryHandler :
			IRequestHandler<GetById, PedidoItemArteCompra>,
			IRequestHandler<GetByIdPedidoItemArteCompra, PedidoItemArteCompra>,
			IRequestHandler<GetByIdPedidoItemCompraExistEntrega, bool>,
			IRequestHandler<ListByIdPedidoItemArteCompra, List<PedidoItemArteCompra>>
		{

			/// <summary>
			/// 
			/// </summary>
			private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

			/// <summary>
			/// 
			/// </summary>
			private readonly IRepository<PedidoItemArteEntrega> pedidoItemEntregaRepository;

			/// <summary>
			/// 
			/// </summary>
			private readonly IUserProvider userProvider;

			/// <summary>
			/// 
			/// </summary>
			private readonly IMediator mediator;

			public PedidoItemArteCompraQueryHandler(
				IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
				IRepository<PedidoItemArteEntrega> _pedidoItemEntregaRepository,
				IUserProvider _userProvider,
				IMediator _mediator
				)
			{
				pedidoItemCompraRepository = _pedidoItemCompraRepository;
				pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
				userProvider = _userProvider;
				mediator = _mediator;
			}


			Task<PedidoItemArteCompra> IRequestHandler<GetById, PedidoItemArteCompra>.Handle(GetById request, CancellationToken cancellationToken)
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

			Task<PedidoItemArteCompra> IRequestHandler<GetByIdPedidoItemArteCompra, PedidoItemArteCompra>.Handle(GetByIdPedidoItemArteCompra request, CancellationToken cancellationToken)
			{
				var query = pedidoItemCompraRepository.GetByIdPedidoItemCompra(request.Id, cancellationToken);
				return query.AsTask();
			}

			Task<List<PedidoItemArteCompra>> IRequestHandler<ListByIdPedidoItemArteCompra, List<PedidoItemArteCompra>>.Handle(ListByIdPedidoItemArteCompra request, CancellationToken cancellationToken)
			{
				var query = pedidoItemCompraRepository.ListByIdPedidoItemCompra(request.IdPedidoItem, cancellationToken);
				return query;
			}
		}
	}
