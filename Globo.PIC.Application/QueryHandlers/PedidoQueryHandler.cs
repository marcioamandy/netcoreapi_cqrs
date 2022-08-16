using System.Collections.Generic;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using System.Linq;
using System;
using Globo.PIC.Domain.Models;
using AutoMapper;
using Globo.PIC.Domain.Exceptions;

namespace Globo.PIC.Application.QueryHandlers
{
    public class PedidoQueryHandler :
        IRequestHandler<GetByIdPedido, Pedido>,
        IRequestHandler<ListByPedidoFilter, List<Pedido>>,

        IRequestHandler<GetByIdPedidoWithOutRoles, Pedido>
    {

        /// <summary>
        ///
        /// </summary>
        private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProjectProxy project;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_talentoRepository"></param>
        public PedidoQueryHandler(
            IRepository<Pedido> _pedidoRepository,
            IProjectProxy _project,
            IMapper _mapper
            )
        {
            pedidoRepository = _pedidoRepository;
            project = _project;
            mapper = _mapper;
        }

        Task<List<Pedido>> IRequestHandler<ListByPedidoFilter, List<Pedido>>.Handle(ListByPedidoFilter request, CancellationToken cancellationToken)
        {
            var retorno = pedidoRepository.GetAll();

            int.TryParse(request.Filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;

            var pedidos = retorno.Where(x =>
                   (!request.Filter.DataInicio.HasValue || x.DataCriacao.Value.Date >= request.Filter.DataInicio.Value.Date) &&
                   (!request.Filter.DataFim.HasValue || x.DataCriacao.Value.Date <= request.Filter.DataFim.Value.Date) &&
                   (request.Filter.LoginSolicitadoPor == null || x.CriadoPorLogin == request.Filter.LoginSolicitadoPor) &&
                   (request.Filter.Projetos == null || request.Filter.Projetos.Contains(x.IdProjeto)) &&
                   (request.Filter.Conteudos == null || request.Filter.Conteudos.Contains(x.IdConteudo)) &&
                   //(request.Filter.Titulo == null || x.Titulo.Contains(request.Filter.Titulo)) &&
                   (!request.Filter.IdStatus.HasValue || x.PedidoArte.IdStatus == request.Filter.IdStatus.Value) &&

                   //(x.PedidoArte.FlagPedidoAlimentos == request.Filter.FlagPedidoAlimentos) &&  
                   //(x.PedidoArte.FlagFastPass == request.Filter.FlagFastPass) &&

                   (!request.Filter.idTag.HasValue || x.IdTag == request.Filter.idTag.Value) &&

                   (request.Filter.LoginComprador == null || x.Itens.Any(x => x.PedidoItemArte.CompradoPorLogin == request.Filter.LoginComprador)))
                   .Skip((page - 1) * request.Filter.PerPage).Take(request.Filter.PerPage);

            if(! String.IsNullOrEmpty(request.Filter.Titulo))
            {
                long id = 0;

                if(request.Filter.Titulo.Where(c => char.IsNumber(c)).Count() > 0)
                {
                    id = Convert.ToInt64(request.Filter.Titulo);

                    pedidos = pedidos.Where(p => p.Id == id);
                }
                 else   
                    pedidos = pedidos.Where(p => p.Titulo.Contains(request.Filter.Titulo));
            }

            if (request.Filter.FlagPedidoAlimentos && request.Filter.FlagFastPass != true)
                pedidos = pedidos.Where(p => p.PedidoArte.FlagPedidoAlimentos == true);

            if (request.Filter.FlagFastPass && request.Filter.FlagPedidoAlimentos != true)
                pedidos = pedidos.Where(p => p.PedidoArte.FlagFastPass == true);

            if (request.Filter.FlagFastPass && request.Filter.FlagPedidoAlimentos)
            {
                var pedidosFast = pedidos.Where(p => p.PedidoArte.FlagFastPass == request.Filter.FlagFastPass);
                var pedidosAlim = pedidos.Where(p => p.PedidoArte.FlagPedidoAlimentos == request.Filter.FlagPedidoAlimentos);

                pedidos = pedidosFast.Union(pedidosAlim);
                
            }
                
            return Task.FromResult(pedidos.ToList());
        }

        Task<Pedido> IRequestHandler<GetByIdPedido, Pedido>.Handle(GetByIdPedido request, CancellationToken cancellationToken)
        {
            var pedido = pedidoRepository.GetById(request.Id, cancellationToken);

            if (pedido == null) throw new NotFoundException("Pedido não encontrado!");

            return pedido.AsTask();
        }

        Task<Pedido> IRequestHandler<GetByIdPedidoWithOutRoles, Pedido>.Handle(GetByIdPedidoWithOutRoles request, CancellationToken cancellationToken)
        {
            var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.Id, cancellationToken);

            return pedido.AsTask();
        }     
        
    }
}
