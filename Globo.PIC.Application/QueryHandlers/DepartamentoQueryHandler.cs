using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using System.Linq;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Exceptions;

namespace Globo.PIC.Application.QueryHandlers
{
    public class DepartamentoQueryHandler :
		IRequestHandler<GetByDepartamentoFilter, List<Departamento>>,
		IRequestHandler<GetById, Departamento>
	{ 
		private readonly IRepository<Departamento> departamentoRepository;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_talentoRepository"></param>
		public DepartamentoQueryHandler(  IConteudoServiceProxy _starServiceProxy, 
			IRepository<Departamento> _departamentoRepository)
		{
			departamentoRepository = _departamentoRepository;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<Departamento>> IRequestHandler<GetByDepartamentoFilter, List<Departamento>>.Handle(GetByDepartamentoFilter _request, CancellationToken cancellationToken)
		{
			var request = departamentoRepository.GetAll();

			if (_request.Filter != null && !string.IsNullOrEmpty(_request.Filter.Search))
			{
				request = request.Where(x => x.Nome.ToLower().Contains(_request.Filter.Search.ToLower()));
			}

			return request.ToListAsync(cancellationToken);

		}

        Task<Departamento> IRequestHandler<GetById, Departamento>.Handle(GetById request, CancellationToken cancellationToken)
        {
			var departamento = departamentoRepository.GetById(request.Id, cancellationToken);

			if (departamento == null) throw new NotFoundException("Pedido não encontrado!");

			return departamento.AsTask();
		}
    }
}
