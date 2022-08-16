using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries.Filters;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
	public class ProjetoQueryHandler :
        IRequestHandler<GetByProjetosFilter, List<Projeto>>,
        IRequestHandler<GetByProjetoFilter, Projeto>
	{

		private readonly IProjectProxy project;

		public ProjetoQueryHandler(IProjectProxy _projects, IProjectProxy _project)
		{
			project = _project;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<List<Projeto>> IRequestHandler<GetByProjetosFilter, List<Projeto>>.Handle(GetByProjetosFilter request, CancellationToken cancellationToken)
        {
            return await project.GetResultProjectsByProjectNameAsync( request.ProjectName, cancellationToken);
        }

        async Task<Projeto> IRequestHandler<GetByProjetoFilter, Projeto>.Handle(GetByProjetoFilter request, CancellationToken cancellationToken)
		{
			return await project.GetResultProjectAsync(request.ProjectId, cancellationToken);
		}
	}
}
