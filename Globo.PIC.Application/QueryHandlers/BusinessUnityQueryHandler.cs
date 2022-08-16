using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using System.Linq;
using Globo.PIC.Domain.Types.Queries; 

namespace Globo.PIC.Application.QueryHandlers
{
	public class BusinessUnityQueryHandler :
        IRequestHandler<GetByBusinessUnityId, UnidadeNegocio>,
        IRequestHandler<ListByBusinessUnityFilter, List<UnidadeNegocio>>

    { 

        private readonly IRepository<UnidadeNegocio> businessUnitRepository;

        private readonly IMediator mediator;
        public BusinessUnityQueryHandler(IRepository<UnidadeNegocio> _businessUnitRepository, IMediator _mediator)
		{
            businessUnitRepository = _businessUnitRepository;
            mediator = _mediator;
		}
         

        Task<UnidadeNegocio> IRequestHandler<GetByBusinessUnityId, UnidadeNegocio>.Handle(GetByBusinessUnityId request, CancellationToken cancellationToken)
        {
            var retorno = businessUnitRepository.GetById(request.Id,cancellationToken);
            return retorno.AsTask(); 
        }

        Task<List<UnidadeNegocio>> IRequestHandler<ListByBusinessUnityFilter, List<UnidadeNegocio>>.Handle(ListByBusinessUnityFilter request, CancellationToken cancellationToken)
        {
            var retorno = businessUnitRepository.GetAll();

            if (request.Filter != null && !string.IsNullOrEmpty(request.Filter.Search))
            {
                retorno = retorno.Where(x => x.Nome.ToLower().Contains(request.Filter.Search.ToLower()));
            }
            return retorno.ToListAsync(cancellationToken);
        }
    }
}
