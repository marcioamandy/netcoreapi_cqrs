using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using System.Linq;
using System;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
    public class ConteudoQueryHandler :
		IRequestHandler<GetByConteudoFilter, List<Conteudo>>,
        IRequestHandler<GetByConteudoFilterCount, int>
    {
		/// <summary>
		///
		/// </summary>
		 
        private readonly IConteudoServiceProxy starServiceProxy;
		private readonly IRepository<Conteudo> conteudoRepository;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_talentoRepository"></param>
		public ConteudoQueryHandler(  IConteudoServiceProxy _starServiceProxy, IRepository<Conteudo> _conteudoRepository)
		{
			 
			starServiceProxy = _starServiceProxy;
			conteudoRepository = _conteudoRepository;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<Conteudo>> IRequestHandler<GetByConteudoFilter, List<Conteudo>>.Handle(GetByConteudoFilter _request, CancellationToken cancellationToken)
		{
			var request = conteudoRepository.GetAll();

			if (_request.Filter != null && !string.IsNullOrEmpty(_request.Filter.Search))
			{
				request = request.Where(x => x.Nome.ToLower().Contains(_request.Filter.Search.ToLower()));
			}

			return request.ToListAsync(cancellationToken);

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<int> IRequestHandler<GetByConteudoFilterCount, int>.Handle(GetByConteudoFilterCount _request, CancellationToken cancellationToken)
        {
            var request = conteudoRepository.GetAll();
            if (!string.IsNullOrEmpty(_request.Filter.Search))
            {
                request = request.Where(x =>
                    x.Nome.ToLower().Contains(_request.Filter.Search.ToLower()));
            }

            return await request.CountAsync(cancellationToken);
        }
    }
}
