using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
	public class OrganizationQueryHandler :
       IRequestHandler<GetByBusinessUnityId, List<OrganizationStructure>>
    {

		private readonly IOrganizationStructureProxy organization;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<UnidadeNegocio> unidadeNegocioRepository;

        private readonly IMediator mediator;
        public OrganizationQueryHandler(IOrganizationStructureProxy _organization, 
            IMediator _mediator, IRepository<UnidadeNegocio> _unidadeNegocioRepository)
		{
            organization = _organization;
            mediator = _mediator;
            unidadeNegocioRepository = _unidadeNegocioRepository;

        }

        async Task<List<OrganizationStructure>> IRequestHandler<GetByBusinessUnityId, List<OrganizationStructure>>.Handle(GetByBusinessUnityId request, CancellationToken cancellationToken)
        {
            var unidadeNegocio=unidadeNegocioRepository.GetById(request.Id, cancellationToken);

            if (unidadeNegocio == null) throw new NotFoundException("Unidade Negocio não encontrada!");
            
            var retorno = await organization.GetOrganizationStructures(request.Id, cancellationToken);

            return retorno;
        }
    }
}
