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
	public class StatusQueryHandler :
		IRequestHandler<GetById, Status>,
		IRequestHandler<GetByStatusId, Status>

	{
		/// <summary>
		///
		/// </summary>
		private readonly StatusRepository statusRepository;

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
		public StatusQueryHandler(
			IRepository<Status> _statusRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			statusRepository = (StatusRepository)_statusRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		Task<Status> IRequestHandler<GetById, Status>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var queryFilter = statusRepository.GetById(request.Id, cancellationToken);

			return queryFilter.AsTask();
		}

		//      Task<Pedido> IRequestHandler<GetById, Pedido>.Handle(GetById request, CancellationToken cancellationToken)
		//      {
		//	var queryFilter = pedidoRepository.GetById(request.Id, cancellationToken);

		//	return queryFilter.AsTask();
		//}
		Task<Status> IRequestHandler<GetByStatusId, Status>.Handle(GetByStatusId request, CancellationToken cancellationToken)
		{
			var queryFilter = statusRepository.GetById(request.Id, cancellationToken);

			return queryFilter.AsTask();
		}
	}
}
