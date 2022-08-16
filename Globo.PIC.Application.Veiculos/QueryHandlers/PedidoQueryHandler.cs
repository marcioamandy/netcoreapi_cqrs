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
using Globo.PIC.Domain.Models;
using AutoMapper;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Enums;

namespace Globo.PIC.Application.QueryHandlers
{
    public class PedidoQueryHandler :
        IRequestHandler<GetByIdPedido, Pedido>,
        IRequestHandler<GetByPedidoFilter, List<Pedido>>,
        IRequestHandler<GetByPedidoFilterCount, int>,
        IRequestHandler<ListByPedidoFiltro, List<PedidoModel>>,
        IRequestHandler<GetByIdPedidoModel, PedidoModel>,
        IRequestHandler<GetByIdPedidoWithOutRoles, Pedido>
    {

        /// <summary>
        ///
        /// </summary>
        private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUserProvider userProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProjectProxy project;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

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
            IUserProvider _userProvider,
            IProjectProxy _project,
            IMediator _mediator,
            IMapper _mapper
            )
        {
            pedidoRepository = _pedidoRepository;
            userProvider = _userProvider;
            project = _project;
            mediator = _mediator;
            mapper = _mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<int> IRequestHandler<GetByPedidoFilterCount, int>.Handle(GetByPedidoFilterCount _request, CancellationToken cancellationToken)
        {
            var request = pedidoRepository.GetAll();
            
            if (!string.IsNullOrEmpty(_request.Filter.Search))
            {
                request = request.Where(x =>
                    x.Ativo == true &&
                    x.Id.ToString().ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LoginSolicitante.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.DescricaoCena.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LocalEntrega.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LocalUtilizacao.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LoginAutorizacao.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LoginCancelamento.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.Observacao.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.Titulo.ToLower().Contains(_request.Filter.Search.ToLower())
                );
            }

            return await request.CountAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Pedido>> IRequestHandler<GetByPedidoFilter, List<Pedido>>.Handle(GetByPedidoFilter _request, CancellationToken cancellationToken)
        {
            var request = pedidoRepository.GetAll();
            if (_request.Filter != null && !string.IsNullOrEmpty(_request.Filter.Search))
            {
                request = request.Where(
                    x => x.Ativo == true &&
                    x.Id.ToString().ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LoginSolicitante.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.DescricaoCena.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LocalEntrega.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LocalUtilizacao.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LoginAutorizacao.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.LoginCancelamento.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.Observacao.ToLower().Contains(_request.Filter.Search.ToLower()) ||
                    x.Titulo.ToLower().Contains(_request.Filter.Search.ToLower())
                );
            }

            /*
			 consumir o proxy aqui
			 */
            return request.ToListAsync(cancellationToken);
        }

        async Task<List<PedidoModel>> IRequestHandler<ListByPedidoFiltro, List<PedidoModel>>.Handle(ListByPedidoFiltro request, CancellationToken cancellationToken)
        {
            var retorno = pedidoRepository.GetAllWithRules(cancellationToken);

            int.TryParse(request.Filter.Page.ToString(), out int page);
            page = page <= 0 ? 1 : page;

            //var pedidos = retorno.Result.Skip((page - 1) * request.Filter.PerPage).Take(request.Filter.PerPage).Where(x =>
            //       x.Ativo == true && 
            //       (!request.Filter.DataInicio.HasValue || x.DataCriacao.Value.Date >= request.Filter.DataInicio.Value.Date) &&
            //       (!request.Filter.DataFim.HasValue || x.DataCriacao.Value.Date <= request.Filter.DataFim.Value.Date) &&
            //       (request.Filter.LoginSolicitadoPor == null || x.LoginSolicitante == request.Filter.LoginSolicitadoPor) &&
            //       (request.Filter.Conteudos == null || request.Filter.Conteudos.Contains(x.IdConteudo)) &&
            //       (request.Filter.Titulo == null || x.Titulo.Contains(request.Filter.Titulo)) &&
            //       (!request.Filter.IdStatus.HasValue || x.IdStatus == request.Filter.IdStatus.Value) &&
            //       (request.Filter.LoginComprador == null || x.Itens.Any(x => x.LoginComprador == request.Filter.LoginComprador)) );


            var pedidos = retorno.Result.Where(x =>
                   x.Ativo == true &&
                   (!request.Filter.DataInicio.HasValue || x.DataCriacao.Value.Date >= request.Filter.DataInicio.Value.Date) &&
                   (!request.Filter.DataFim.HasValue || x.DataCriacao.Value.Date <= request.Filter.DataFim.Value.Date) &&
                   (request.Filter.LoginSolicitadoPor == null || x.LoginSolicitante == request.Filter.LoginSolicitadoPor) &&
                   (request.Filter.Projetos == null || request.Filter.Projetos.Contains(x.IdProjeto)) &&
                   (request.Filter.Conteudos == null || request.Filter.Conteudos.Contains(x.IdConteudo)) &&
                   (request.Filter.Titulo == null || x.Titulo.Contains(request.Filter.Titulo)) &&
                   (!request.Filter.IdStatus.HasValue || x.IdStatus == request.Filter.IdStatus.Value) &&
                   (request.Filter.LoginComprador == null || x.Itens.Any(x => x.LoginComprador == request.Filter.LoginComprador)));
            
            List<PedidoModel> pedidoModelList = new List<PedidoModel>();
          
            var projets = await project.GetResultProjectsAsync(
                pedidos.Select(x => x.IdProjeto).Distinct().ToList(), cancellationToken);

            foreach (var pedido in pedidos)
            {
                PedidoModel pedidoModel = new PedidoModel();
                pedidoModel = mapper.Map<PedidoModel>(pedido);
                pedidoModel.Tipo = Enum.GetName(typeof(TipoCompra), pedido.IdTipo);
                pedidoModel.Projeto = projets.FirstOrDefault(x => x.Id == pedido.IdProjeto);

                var compradores = pedido.Itens.Where(a => a.UserComprador != null)
                     .Select(a => a.UserComprador).Distinct();
                if (compradores.Count() > 0)
                    pedidoModel.Compradores = mapper.Map<List<User>>(compradores);

                pedidoModelList.Add(pedidoModel);
            }
         
            return pedidoModelList;
        }

        Task<Pedido> IRequestHandler<GetByIdPedido, Pedido>.Handle(GetByIdPedido request, CancellationToken cancellationToken)
        {
            var pedido = pedidoRepository.GetByIdPedido(request.Id, cancellationToken);

            return pedido.AsTask();
        }

        Task<Pedido> IRequestHandler<GetByIdPedidoWithOutRoles, Pedido>.Handle(GetByIdPedidoWithOutRoles request, CancellationToken cancellationToken)
        {
            var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.Id, cancellationToken);

            return pedido.AsTask();
        }

        async Task<Domain.Models.PedidoModel> IRequestHandler<GetByIdPedidoModel, PedidoModel>.Handle(GetByIdPedidoModel request, CancellationToken cancellationToken)
        {
            var pedido = await pedidoRepository.GetByIdPedido(request.Id, cancellationToken);

            if (pedido == null) throw new NotFoundException("Pedido não encontrado!");

            var projeto =  await project.GetResultProjectAsync(pedido.IdProjeto, cancellationToken);

            Func<Pedido, PedidoModel> lazyLoad = item => {
                var res = mapper.Map<PedidoModel>(item);
                res.Tipo = Enum.GetName(typeof(TipoCompra), item.IdTipo);
                //res.Conteudo = projeto;
                res.Projeto = projeto;
                return res;
            };
           
            var pedidoModel = lazyLoad(pedido);
            var compradores = pedido.Itens.Where(a=>a.UserComprador!=null)
                .Select(a => a.UserComprador).Distinct();
            pedidoModel.Compradores = mapper.Map<List<User>>(compradores);
            return pedidoModel;

        }        
        
    }
}
