using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Arte.QueryHandlers
{
	public class StatusPedidoItemArteQueryHandler :
		IRequestHandler<GetByStatusPedidoItemArteId, StatusPedidoItemArte>
	{

        /// <summary>
        /// 
        /// </summary>
        private readonly StatusArteItemRepository statusArteRepository;
        //private readonly IRepository<StatusArteRepository> statusArteRepository;

        public StatusPedidoItemArteQueryHandler(
			IRepository<StatusPedidoItemArte> _statusArteRepository
			)
		{
			statusArteRepository = _statusArteRepository as StatusArteItemRepository;
		}
		  

        Task<StatusPedidoItemArte> IRequestHandler<GetByStatusPedidoItemArteId, StatusPedidoItemArte>.Handle(GetByStatusPedidoItemArteId request, CancellationToken cancellationToken)
        {
			var query = statusArteRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
			 
		}
    }
}
