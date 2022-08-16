using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;

namespace Globo.PIC.Application.QueryHandlers
{
	public class PedidoItemConversaAnexosQueryHandler :
		IRequestHandler<GetById, PedidoItemConversaAnexo>,
		IRequestHandler<GetByIdPedidoItemConversaAnexos, PedidoItemConversaAnexo>,
		IRequestHandler<ListByIdPedidoItemConversaAnexos, List<PedidoItemConversaAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemConversaAnexo> pedidoItemConversaAnexoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoItemConversaAnexosQueryHandler(
			IRepository<PedidoItemConversaAnexo> _pedidoItemConversaAnexoRepository,
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
		Task<PedidoItemConversaAnexo> IRequestHandler<GetById, PedidoItemConversaAnexo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaAnexoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemConversaAnexo> IRequestHandler<GetByIdPedidoItemConversaAnexos, PedidoItemConversaAnexo>.Handle(GetByIdPedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaAnexoRepository.GetByIdPedidoItemConversaAnexo(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemConversaAnexo>> IRequestHandler<ListByIdPedidoItemConversaAnexos, List<PedidoItemConversaAnexo>>.Handle(ListByIdPedidoItemConversaAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemConversaAnexoRepository.ListByIdPedidoItemConversaAnexo (request.IdPedidoItemConversa, cancellationToken);
			return query;
		}
	}
}
