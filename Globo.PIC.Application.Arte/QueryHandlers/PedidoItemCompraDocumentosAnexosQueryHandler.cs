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
namespace Globo.PIC.Application.Arte.QueryHandlers
{
	public class PedidoItemCompraDocumentosAnexosQueryHandler :
		IRequestHandler<GetById, PedidoItemArteCompraDocumentoAnexo>,
		IRequestHandler<GetByIdPedidoItemCompraDocumentosAnexos, PedidoItemArteCompraDocumentoAnexo>,
		IRequestHandler<ListByIdPedidoItemArteCompraDocumentosAnexos, List<PedidoItemArteCompraDocumentoAnexo>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArteCompraDocumentoAnexo> pedidoItemCompraAnexoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		public PedidoItemCompraDocumentosAnexosQueryHandler(
			IRepository<PedidoItemArteCompraDocumentoAnexo> _pedidoItemCompraAnexoRepository,
			IUserProvider _userProvider,
			IMediator _mediator
			)
		{
			pedidoItemCompraAnexoRepository = _pedidoItemCompraAnexoRepository;
			userProvider = _userProvider;
			mediator = _mediator;
		}

		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemArteCompraDocumentoAnexo> IRequestHandler<GetById, PedidoItemArteCompraDocumentoAnexo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraAnexoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PedidoItemArteCompraDocumentoAnexo> IRequestHandler<GetByIdPedidoItemCompraDocumentosAnexos, PedidoItemArteCompraDocumentoAnexo>.Handle(GetByIdPedidoItemCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraAnexoRepository.GetByIdPedidoItemCompraDocumentosAnexos(request.Id, cancellationToken);
			return query.AsTask();
		}

		Task<List<PedidoItemArteCompraDocumentoAnexo>> IRequestHandler<ListByIdPedidoItemArteCompraDocumentosAnexos, List<PedidoItemArteCompraDocumentoAnexo>>.Handle(ListByIdPedidoItemArteCompraDocumentosAnexos request, CancellationToken cancellationToken)
		{
			var query = pedidoItemCompraAnexoRepository.ListByIdPedidoItemCompraDocumentosAnexos(request.IdCompra, cancellationToken);
			return query;
		}
	}
}
