using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries.Filters;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
    public class NivelQueryHandler :
        IRequestHandler<GetByNivelFilter, List<Nivel>>,
        IRequestHandler<GetBySortNivelFilter, List<Nivel>>
    {

        private readonly ITasksProxy taskNivel;
        private readonly IProjectProxy projectProxy;
        public NivelQueryHandler(ITasksProxy _taskNivel, IProjectProxy _projectProxy)
        {
            taskNivel = _taskNivel;
            projectProxy = _projectProxy;
        }

        async Task<List<Nivel>> IRequestHandler<GetByNivelFilter, List<Nivel>>.Handle(GetByNivelFilter request, CancellationToken cancellationToken)
        {
            List<Nivel> nivelList = new List<Nivel>();
            List<Nivel> sortLevel = new List<Nivel>();
            SortNivel sortNivel = new SortNivel();
            var resultNivel = await taskNivel.GetResultTasksAsync(
                request.Filter.ProjectId,
                cancellationToken
            );

            int i = 0;

            foreach (var nivel in resultNivel)
            {
                i++;

                nivelList.Add(
                    new Nivel
                    {
                        //TaskId = nivel.TaskId.ToString(),
                        Level = nivel.Level,
                        Name = nivel.Description,
                        //Number = nivel.Number,
                        //adicionar os demais campos do resulttask
                    });
            }

            var niveis = nivelList.Select(a => a.Level).Distinct();
            foreach (var nivel in niveis)
            {
                //sortNivel.TaskLevel = nivel;
                var currentNivel = nivelList.Where(a => a.Level == nivel).ToList();
                sortNivel.Niveis.AddRange(currentNivel);

            }
            return nivelList;
        }

        /// <summary>
        /// Serializes Entities Objects preventing the Loop Reference error
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        private static string SerializeEntityObject(object entityObject)
        {
            return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private Nivel GetLastNivel(List<Nivel> niveis, long nivel)
        {
            Nivel check = new Nivel();
            int contador = 0;

            while (check is null || contador == niveis.Count)
            {
                check = niveis.SelectMany(a => a.Niveis.Where(b => b.Level == nivel)).ToList().LastOrDefault();

                contador++;
            }

            return check;
        }

        async Task<List<Nivel>> IRequestHandler<GetBySortNivelFilter, List<Nivel>>.Handle(GetBySortNivelFilter request, CancellationToken cancellationToken)
        {
            if (request.Filter.ProjectId <= 0)
                throw new BadRequestException("O parametro ProjectId é requerido.");

            var resultNivel = await taskNivel.GetResultTasksAsync(
                request.Filter.ProjectId,
                cancellationToken
            );

            var resultProject = await projectProxy.GetResultProjectAsync(request.Filter.ProjectId, cancellationToken);
             

            var todosItens = resultNivel.Select(s =>
            {
                
                long parentId = 0;
                long.TryParse(s.ParentId, out parentId);

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
                     .Where(a => a.Type.ToLower() == "centrodecusto").FirstOrDefault();
                    
                    if (finalidadeConvert != null){
                        finalidade.Description = finalidadeConvert.Description;
                        finalidade.Value = finalidadeConvert.Value;
                    }
                }
                var nivel =new Nivel{
                    Id = s.Id,
                    Level = s.Level,
                    Name = s.Description,
                    Chargeable = s.Chargeable,
                    ParentId = parentId,
                    Number = s.Number,
                    ParentNumber = s.ParentNumber,
                    ProjectId = resultProject.Id,
                    ProjectName = resultProject.Name,
                    ProjectNumber = projectNumber,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
                };
                if (finalidade != null)
                    nivel.Finalidade = finalidade;
                if (centroCusto != null)
                    nivel.CentroCusto = centroCusto;
                return nivel;
            }).ToList();

            List<Nivel> saida = todosItens.Where(x => x.Level == 1).ToList();

            foreach (var item in saida)
            {
                SetFilhos(todosItens, item);
            }

            return saida;
        }

        void SetFilhos(List<Nivel> todosItens, Nivel item)
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
