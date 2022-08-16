using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using System.Linq;
using System;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Types.Queries.Filters;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.QueryHandlers
{
	public class StatusItemQueryHandler :
		IRequestHandler<GetById, StatusPedidoItem>,
		IRequestHandler<GetByStatusItemId, StatusPedidoItem>
	{
		/// <summary>
		///
		/// </summary>
		private readonly StatusItemRepository statusItemRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_talentoRepository"></param>
		public StatusItemQueryHandler(
			IRepository<StatusPedidoItem> _statusItemRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			statusItemRepository = (StatusItemRepository)_statusItemRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

        public Task<StatusPedidoItem> Handle(GetById request, CancellationToken cancellationToken)
        {
			var queryFilter = statusItemRepository.GetById(request.Id, cancellationToken);

			return queryFilter.AsTask();
		}

		public Task<StatusPedidoItem> Handle(GetByStatusItemId request, CancellationToken cancellationToken)
		{
			var queryFilter = statusItemRepository.GetById(request.Id, cancellationToken);

			return queryFilter.AsTask();
		}

	}
}
