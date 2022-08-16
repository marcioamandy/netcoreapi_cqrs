using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Infra.Data.Repositories;

namespace Globo.PIC.Application.Arte.QueryHandlers
{

    /// <summary>
    ///
    /// </summary>
	public class StatusPedidoArteQueryHandler :
		IRequestHandler<GetStatusPedidoArteById, StatusPedidoArte>
	{

        /// <summary>
        /// 
        /// </summary>
        private readonly StatusArteRepository statusArteRepository;

		/// <summary>
        /// 
        /// </summary>
        /// <param name="_statusArteRepository"></param>
        public StatusPedidoArteQueryHandler(
			IRepository<StatusPedidoArte> _statusArteRepository
			)
		{
			statusArteRepository = _statusArteRepository as StatusArteRepository;
		}
		  
		/// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StatusPedidoArte> IRequestHandler<GetStatusPedidoArteById, StatusPedidoArte>.Handle(GetStatusPedidoArteById request, CancellationToken cancellationToken)
        {
			var query = statusArteRepository.GetById(request.Id, cancellationToken);

			return query.AsTask();
			 
		}
    }
}
