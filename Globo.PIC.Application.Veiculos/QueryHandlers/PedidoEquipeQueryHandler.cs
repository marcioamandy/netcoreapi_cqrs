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
    public class PedidoEquipeQueryHandler :
		IRequestHandler<GetById, Equipe>, 
		IRequestHandler<ListByIdPedido, List<Equipe>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Equipe> pedidoEquipeRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoEquipeQueryHandler(
			IRepository<Equipe> _pedidoEquipeRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoEquipeRepository = _pedidoEquipeRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}
	 
        Task<Equipe> IRequestHandler<GetById, Equipe>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoEquipeRepository.GetById(request.Id, cancellationToken);
			
			return query.AsTask();
		}

        Task<List<Equipe>> IRequestHandler<ListByIdPedido, List<Equipe>>.Handle(ListByIdPedido request, CancellationToken cancellationToken)
        {
            var query = pedidoEquipeRepository.ListByIdPedido(request.IdPedido, cancellationToken);
			return query;
        } 
    }
}
