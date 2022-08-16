using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using System.Linq;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Enums;

namespace Globo.PIC.Application.QueryHandlers
{
    public class ConteudoQueryHandler :
        IRequestHandler<GetByConteudoFilter, List<Conteudo>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUserProvider userProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly IConteudoServiceProxy starServiceProxy;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Conteudo> conteudoRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_talentoRepository"></param>
        public ConteudoQueryHandler(
            IConteudoServiceProxy _starServiceProxy,
            IRepository<Conteudo> _conteudoRepository,
            IUserProvider _userProvider)
        {

            starServiceProxy = _starServiceProxy;
            conteudoRepository = _conteudoRepository;
            userProvider = _userProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Conteudo>> IRequestHandler<GetByConteudoFilter, List<Conteudo>>.Handle(GetByConteudoFilter _request, CancellationToken cancellationToken)
        {
            var conteudos = conteudoRepository.GetAll();

            long[] idsSigilosos = userProvider.User.Conteudos.Where(x => x.Sigiloso).Select(x => x.Id).ToArray() ?? new long[] { };
                    
            if (_request.Filter != null)
            {
                if (_request.Filter.ApenasSigilosos)
                {
                    if (userProvider.IsRole(Role.GRANT_ADM_USUARIOS))
                        conteudos = conteudos.Where(x => x.Sigiloso);
                    else
                        conteudos = conteudos.Where(x => idsSigilosos.Any(i => i.Equals(x.Id)));
                }
                else
                {
                    //Caso usuario possuir conteúdo sigiloso
                    if (userProvider.User.Conteudos.Any(x => x.Sigiloso))
                        conteudos = conteudos.Where(x => !x.Sigiloso || idsSigilosos.Any(i => i.Equals(x.Id)));
                    else
                        conteudos = conteudos.Where(x => !x.Sigiloso);
                }

                if (!string.IsNullOrEmpty(_request.Filter.Search))
                    conteudos = conteudos.Where(x => x.Nome.ToLower().Contains(_request.Filter.Search.ToLower()));
            }

            return conteudos.ToListAsync(cancellationToken);

        }
    }
}
