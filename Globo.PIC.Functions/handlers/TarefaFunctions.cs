using Newtonsoft.Json;
using Amazon.Lambda.Core;
using System;
using Globo.PIC.Domain.Models.Functions;
using System.Threading.Tasks;
using Globo.PIC.Functions.context;
using Microsoft.EntityFrameworkCore;
using Globo.PIC.Domain.Models;
using System.Linq;

namespace Globo.PIC.Functions.handlers
{

    public class TarefaFunctions
    {

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<bool> SyncTarefaCadprogHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            try
            {
                foreach (var record in snsEvent.Records)
                {
                    PrintLog(record, $"Recebendo: Titulo={record.Sns.Subject} Message={record.Sns.Message}");
                    PrintLog(record, $"{JsonConvert.SerializeObject(record)}");

                    var snsRecord = record.Sns;
                    var tarefa = JsonConvert.DeserializeObject<TarefaModel>(snsRecord.Message);                    
                    var task = new Domain.Entities.Cadprog.Tarefa()
                    {
                        TaskId = tarefa.Id,
                        TaskNumber = tarefa.Number,
                        TaskName = tarefa.Description,
                        ParentTaskId = tarefa.ParentId,
                        ProjectId = tarefa.ProjectId,
                        DataInicioTarefa = tarefa.StartDate,
                        DataFimTarefa = tarefa.EndDate,
                        TaskLevel = tarefa.Level.ToString(),
                        Chargeable = Convert.ToInt32(tarefa.Chargeable)                        
                        //CentroResultado
                    };

                    if(tarefa.AccountingSegments != null){
                        task.CentroCusto = tarefa.AccountingSegments.Where(x => x.Type.ToLower().Equals("centrodecusto")).FirstOrDefault()?.Value;
                        task.Finalidade = tarefa.AccountingSegments.Where(x => x.Type.ToLower().Equals("finalidade")).FirstOrDefault()?.Value;
                        task.Projeto = tarefa.AccountingSegments.Where(x => x.Type.ToLower().Equals("projeto")).FirstOrDefault()?.Value;
                        task.Segmento = tarefa.AccountingSegments.Where(x => x.Type.ToLower().Equals("finalidade")).FirstOrDefault()?.Value;
                    }                    

                    await ProcessTasksAsync(task);

                    PrintLog(record, "Processamento concluído!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha ao processar a mensagem: {0}", JsonConvert.SerializeObject(ex));

                return await Task.FromResult(false);
            }            

            return await Task.FromResult(true);
        }
        
        void PrintLog(SNSRecord record, string mensagem){
            Console.WriteLine($"[{record.EventSource} {record.Sns.Timestamp}] {mensagem}");
        }

        public async Task ProcessTasksAsync(Domain.Entities.Cadprog.Tarefa tarefa)
        {

            using var ctx = new CadprogDbContext();

            var dbProject = await ctx.Tasks.FirstOrDefaultAsync(x => x.TaskId.Equals(tarefa.TaskId));

            if (dbProject != null)
                dbProject.Update(tarefa);
            else
                await ctx.Tasks.AddAsync(tarefa);

            await ctx.SaveChangesAsync();
        }
    }    
}
