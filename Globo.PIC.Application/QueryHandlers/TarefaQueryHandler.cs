using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
    public class TarefaQueryHandler :
        IRequestHandler<GetByTarefaFilter, List<Tarefa>>
    {

        private readonly ITasksProxy taskTarefa;
        private readonly IProjectProxy projectProxy;

        public TarefaQueryHandler(ITasksProxy _taskTarefa, IProjectProxy _projectProxy)
        {
            taskTarefa = _taskTarefa;
            projectProxy = _projectProxy;
        }

        async Task<List<Tarefa>> IRequestHandler<GetByTarefaFilter, List<Tarefa>>.Handle(GetByTarefaFilter request, CancellationToken cancellationToken)
        {
            if (request.Filter.ProjectId <= 0)
                throw new BadRequestException("O parametro ProjectId é requerido.");

            var resultTarefa = await taskTarefa.GetResultTasksAsync(
                request.Filter.ProjectId,
                cancellationToken
            );

            var resultProject = await projectProxy.GetResultProjectAsync(request.Filter.ProjectId, cancellationToken);             

            var todosItens = resultTarefa.Select(s =>
            {                
                long projectNumber = 0;
                long.TryParse(resultProject.Number, out projectNumber);

                CentroCusto centroCusto = new CentroCusto();

                Finalidade finalidade = new Finalidade();
                if (s.AccountingSegments!=null && s.AccountingSegments.Count > 0)
                {
                    var centroCustoConvert = s.AccountingSegments
                    .Where(a => a.Type.ToLower() == "centrodecusto").FirstOrDefault();
                    if (centroCustoConvert != null)  {
                        centroCusto.Description = centroCustoConvert.Description;
                        centroCusto.Value = centroCustoConvert.Value;
                    }
                    var finalidadeConvert = s.AccountingSegments
                     .Where(a => a.Type.ToLower() == "finalidade").FirstOrDefault();
                    
                    if (finalidadeConvert != null){
                        finalidade.Description = finalidadeConvert.Description;
                        finalidade.Value = finalidadeConvert.Value;
                    }
                }
                var tarefa =new Tarefa{
                    Id = s.Id,
                    Level = s.Level,
                    Name = s.Description,
                    Chargeable = s.Chargeable,
                    ParentId = s.ParentId,
                    Number = s.Number,
                    ParentNumber = s.ParentNumber,
                    ProjectId = resultProject.Id,
                    ProjectName = resultProject.Name,
                    ProjectNumber = projectNumber,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
                };
                if (finalidade != null)
                    tarefa.Finalidade = finalidade;
                if (centroCusto != null)
                    tarefa.CentroCusto = centroCusto;
                return tarefa;
            }).ToList();

            List<Tarefa> saida = todosItens.Where(x => x.Level == 1).ToList();

            foreach (var item in saida)
            {
                SetFilhos(todosItens, item);
            }

            return saida;
        }

        void SetFilhos(List<Tarefa> todosItens, Tarefa item)
        {
            item.Niveis = todosItens.Where(x => x.ParentId == item.Id).ToList();
            if (item.Niveis.Count > 0)
            {
                string parent = "";
                if (string.IsNullOrEmpty(item.Path))
                    parent= item.Name + "/";
                else
                    parent = item.Path +"/";

                foreach (var _item in item.Niveis)
                {
                    _item.Path = parent + _item.Name ;

                    SetFilhos(todosItens, _item);
                }
            }
          
        }

    }
}
