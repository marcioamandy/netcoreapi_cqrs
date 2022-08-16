using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Application.ResolverHandlers
{
    public class ProjetoResolverHandler :
        IMappingAction<Pedido, PedidoViewModel>,
        IMappingAction<Pedido, PedidoVeiculoViewModel>,
        IMappingAction<Pedido, PedidoArteViewModel>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IProjectProxy project;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper mapper;

        public ProjetoResolverHandler(IProjectProxy _project, IMapper _mapper)
        {

            project = _project;
            mapper = _mapper;

            //PedidoModel pedidoModel = new PedidoModel();

            //if (pedido.PedidoArte != null)
            //{
            //    pedidoModel = mapper.Map<PedidoArteModel>(pedido);


            //    //var compradores = pedido.Itens.Where(a => a.PedidoItemArte.CompradoPor != null)
            //    //     .Select(a => a.PedidoItemArte.CompradoPor).Distinct();

            //    //if (compradores.Count() > 0)
            //    //    pedidoModel.Compradores = compradores.ToList();
            //}
            //else if (pedido.PedidoVeiculo != null)
            //{
            //    pedidoModel = mapper.Map<PedidoVeiculoModel>(pedido);
            //}
        }

        void IMappingAction<Pedido, PedidoViewModel>.Process(Pedido source, PedidoViewModel destination, ResolutionContext context)
        {
            ProjetoProxy(source, destination);
        }

        void IMappingAction<Pedido, PedidoArteViewModel>.Process(Pedido source, PedidoArteViewModel destination, ResolutionContext context)
        {
            ProjetoProxy(source, destination);
        }

        void IMappingAction<Pedido, PedidoVeiculoViewModel>.Process(Pedido source, PedidoVeiculoViewModel destination, ResolutionContext context)
        {
            ProjetoProxy(source, destination);
        }

        void ProjetoProxy(Pedido source, PedidoViewModel destination)
        {
            var projeto = ProxyCaching.Get<ProjetoModel>(source.IdProjeto);

            if (projeto == null)
            {
                var _projeto = project.GetResultProjectAsync(source.IdProjeto, CancellationToken.None); _projeto.Wait();

                projeto = _projeto.Result;

                ProxyCaching.Set(projeto, source.IdProjeto);
            }

            destination.Projeto = mapper.Map<ProjetoViewModel>(projeto);
        }

    }

    public static class ProxyCaching
    {
        static Dictionary<string, object> dic = new Dictionary<string, object>();

        public static void Set<T>(T obj, object key)
        {
            if (obj != null)
                dic.TryAdd($"{typeof(T)}{key}", obj);
        }

        public static T Get<T>(object key)
        {
            object obj = null;

            dic.TryGetValue($"{typeof(T)}{key}", out obj);

            return (T)obj;
        }
    }
}
