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
	public class PedidoItemConversaAnexosQueryHandler :
		IRequestHandler<GetById, PedidoItemConversaAnexos>,
		IRequestHandler<GetByIdPedidoItemConversaAnexos, PedidoItemConversaAnexos>,
		IRequestHandler<ListByIdPedidoItemConversaAnexos, List<PedidoItemConversaAnexos>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemConversaAnexos> pedidoItemConversaAnexoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoItemConversaAnexosQueryHandler(
			IRepository<PedidoItemConversaAnexos> _pedidoItemConversaAnexoRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemConversaAnexoRepository = _pedidoItemConversaAnexoRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemConversaAnexos> IRequestHandler<GetById, PedidoItemConversaAnexos>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaAnexoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemConversaAnexos> IRequestHandler<GetByIdPedidoItemConversaAnexos, PedidoItemConversaAnexos>.Handle(GetByIdPedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaAnexoRepository.GetByIdPedidoItemConversaAnexo(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemConversaAnexos>> IRequestHandler<ListByIdPedidoItemConversaAnexos, List<PedidoItemConversaAnexos>>.Handle(ListByIdPedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaAnexoRepository.ListByIdPedidoItemConversaAnexo (request.IdPedidoItemConversa, cancellationToken);
			return query;
		}
	}
}
