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
	public class TrackingQueryHandler :
		IRequestHandler<ListTrackingByIdItemPedido, List<Tracking>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Tracking> trackingRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public TrackingQueryHandler(
			IRepository<Tracking> _trackingRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			trackingRepository = _trackingRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		Task<List<Tracking>> IRequestHandler<ListTrackingByIdItemPedido, List<Tracking>>.Handle(ListTrackingByIdItemPedido request, CancellationToken cancellationToken)
		{
			var query = trackingRepository.ListTrackingByIdItemPedido(request.IdPedidoItem, cancellationToken);

			return query;
		}
	}
}
