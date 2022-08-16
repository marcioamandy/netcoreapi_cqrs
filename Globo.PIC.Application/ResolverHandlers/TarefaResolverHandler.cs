using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Application.ResolverHandlers
{
    public class TarefaResolverHandler :
        IMappingAction<Pedido, PedidoViewModel>,
        IMappingAction<Pedido, PedidoVeiculoViewModel>,
        IMappingAction<Pedido, PedidoArteViewModel>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly ITasksProxy task;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper mapper;

        public TarefaResolverHandler(ITasksProxy _task, IMapper _mapper)
        {

            task = _task;
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
            TarefaProxy(source, destination);
        }

        void IMappingAction<Pedido, PedidoArteViewModel>.Process(Pedido source, PedidoArteViewModel destination, ResolutionContext context)
        {
            TarefaProxy(source, destination);
        }

        void IMappingAction<Pedido, PedidoVeiculoViewModel>.Process(Pedido source, PedidoVeiculoViewModel destination, ResolutionContext context)
        {
            TarefaProxy(source, destination);
        }

        void TarefaProxy(Pedido source, PedidoViewModel destination)
        {
            var tarefa = ProxyCaching.Get<TarefaModel>(source.IdProjeto.ToString() + source.IdTarefa.ToString());

            if (tarefa == null)
            {
                var _tarefa = task.GetResultTaskAsync(source.IdProjeto, source.IdTarefa, CancellationToken.None); _tarefa.Wait();

                tarefa = _tarefa.Result;

                ProxyCaching.Set(tarefa, source.IdProjeto.ToString() + source.IdTarefa.ToString());
            }

            //destination. = mapper.Map<ProjetoViewModel>(tarefa);
        }

        
    }
}
