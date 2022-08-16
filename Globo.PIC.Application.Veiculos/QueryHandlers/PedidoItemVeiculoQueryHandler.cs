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
using AutoMapper;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Veiculo.QueryHandlers
{
    public class PedidoItemVeiculoQueryHandler :
		IRequestHandler<GetById, PedidoVeiculo>,
		IRequestHandler<GetById, PedidoItemVeiculo>,
		IRequestHandler<GetById, StatusPedidoVeiculo>,
		IRequestHandler<GetById, StatusPedidoItemVeiculo>,
		IRequestHandler<GetByIdPedidoItemVeiculo, PedidoItemVeiculo>, 
		IRequestHandler<ListItemVeiculoByIdPedido, List<PedidoItemVeiculo>>,
		IRequestHandler<ListTrackingVeiculo, List<PedidoItemVeiculoTracking>>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly PedidoVeiculoRepository pedidoVeiculoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly PedidoItemVeiculoRepository pedidoItemVeiculoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly StatusVeiculoRepository statusVeiculoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly TrackingVeiculoRepository trackingVeiculoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly StatusVeiculoItemRepository statusItemVeiculoRepository;

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
		private readonly IMapper mapper;

		public PedidoItemVeiculoQueryHandler(
			IRepository<PedidoVeiculo> _pedidoVeiculoRepository,
			IRepository<PedidoItemVeiculo> _pedidoItemVeiculoRepository,
			IRepository<StatusPedidoVeiculo> _statusVeiculoRepository,
			IRepository<StatusPedidoItemVeiculo> _statusItemVeiculoRepository,
			IRepository<PedidoItemVeiculoTracking> _trackingVeiculoRepository,
			IUserProvider _userProvider,
			IMediator _mediator,
			IMapper _mapper
			)
		{
			pedidoVeiculoRepository = _pedidoVeiculoRepository as PedidoVeiculoRepository;
			pedidoItemVeiculoRepository = _pedidoItemVeiculoRepository as PedidoItemVeiculoRepository;
			statusVeiculoRepository = _statusVeiculoRepository as StatusVeiculoRepository;
			statusItemVeiculoRepository = _statusItemVeiculoRepository as StatusVeiculoItemRepository;
			trackingVeiculoRepository = _trackingVeiculoRepository as TrackingVeiculoRepository;
			userProvider = _userProvider;
			mediator = _mediator;
			mapper = _mapper;
		}

		Task<PedidoVeiculo> IRequestHandler<GetById, PedidoVeiculo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoVeiculoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemVeiculo> IRequestHandler<GetById, PedidoItemVeiculo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = pedidoItemVeiculoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<StatusPedidoVeiculo> IRequestHandler<GetById, StatusPedidoVeiculo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = statusVeiculoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<StatusPedidoItemVeiculo> IRequestHandler<GetById, StatusPedidoItemVeiculo>.Handle(GetById request, CancellationToken cancellationToken)
		{
			var query = statusItemVeiculoRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
		}

		Task<PedidoItemVeiculo> IRequestHandler<GetByIdPedidoItemVeiculo, PedidoItemVeiculo>.Handle(GetByIdPedidoItemVeiculo request, CancellationToken cancellationToken)
		{
			var query = pedidoItemVeiculoRepository.GetByIdPedidoItemVeiculo(request.IdItem, cancellationToken);

			return query.AsTask();
		}		

		Task<List<PedidoItemVeiculo>> IRequestHandler<ListItemVeiculoByIdPedido, List<PedidoItemVeiculo>>.Handle(ListItemVeiculoByIdPedido request, CancellationToken cancellationToken)
        {
            var query = pedidoItemVeiculoRepository.GetAll();

			query = query.Where(x => x.PedidoItem.IdPedido == request.IdPedido);

			return query.ToListAsync(cancellationToken);
        }

		Task<List<PedidoItemVeiculoTracking>> IRequestHandler<ListTrackingVeiculo, List<PedidoItemVeiculoTracking>>.Handle(ListTrackingVeiculo request, CancellationToken cancellationToken)
		{
			var query = trackingVeiculoRepository.GetAll().Where(x => x.PedidoItemVeiculo.PedidoItem.Id == request.IdPedidoItem);

			return query.ToListAsync(cancellationToken);
		}
	}
}
