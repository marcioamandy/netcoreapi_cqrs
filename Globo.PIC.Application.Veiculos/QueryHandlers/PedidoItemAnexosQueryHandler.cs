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
    public class PedidoItemAnexosQueryHandler :
		IRequestHandler<GetById, PedidoItemAnexos>,
		IRequestHandler<GetByIdPedidoItemAnexos, PedidoItemAnexos>,
		IRequestHandler<ListByIdPedidoItemAnexos, List<PedidoItemAnexos>>
		
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemAnexos> pedidoItemImagemRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoItemAnexosQueryHandler(
			IRepository<PedidoItemAnexos> _pedidoItemImagemRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemImagemRepository = _pedidoItemImagemRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}
 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PedidoItemAnexos> IRequestHandler<GetById, PedidoItemAnexos>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemImagemRepository.GetById(request.Id, cancellationToken);
			
			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemAnexos> IRequestHandler<GetByIdPedidoItemAnexos, PedidoItemAnexos>.Handle(GetByIdPedidoItemAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemImagemRepository.GetByIdPedidoItemAnexo(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemAnexos>> IRequestHandler<ListByIdPedidoItemAnexos, List<PedidoItemAnexos>>.Handle(ListByIdPedidoItemAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemImagemRepository.ListByIdPedidoItemAnexo(request.IdPedidoItem, cancellationToken);
			return query;
		}
	}
}
