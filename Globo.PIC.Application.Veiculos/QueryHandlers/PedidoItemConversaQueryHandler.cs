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
	public class PedidoItemConversaQueryHandler :
		IRequestHandler<GetById, PedidoItemConversa>,
		IRequestHandler<GetByIdPedidoItemConversa, PedidoItemConversa>,
		IRequestHandler<ListItemConversaIdByIdPedidoItem, List<PedidoItemConversa>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemConversa> pedidoItemConversaRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoItemConversaQueryHandler(
			IRepository<PedidoItemConversa> _pedidoItemConversaRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemConversaRepository = _pedidoItemConversaRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		Task<PedidoItemConversa> IRequestHandler<GetById, PedidoItemConversa>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemConversa> IRequestHandler<GetByIdPedidoItemConversa, PedidoItemConversa>.Handle(GetByIdPedidoItemConversa request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaRepository.GetByIdPedidoItemConversa(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemConversa>> IRequestHandler<ListItemConversaIdByIdPedidoItem, List<PedidoItemConversa>>.Handle(ListItemConversaIdByIdPedidoItem request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaRepository.ListByIdPedidoItemConversa(request.IdPedidoItem, cancellationToken);
			return query;
		}
	}
}
