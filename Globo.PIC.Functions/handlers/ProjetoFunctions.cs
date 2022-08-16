using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using System;
using Globo.PIC.Domain.Models.Functions;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities.Cadprog;
using Globo.PIC.Functions.context;
using Microsoft.EntityFrameworkCore;
using Globo.PIC.Domain.Models;
using Newtonsoft.Json.Linq;

namespace Globo.PIC.Functions.handlers
{

    public class ProjetoFunctions
    {

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<bool> SyncProjetoCadprogHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            try
            {
                foreach (var record in snsEvent.Records)
                {
                    PrintLog(record, $"Recebendo: Titulo={record.Sns.Subject} Message={record.Sns.Message}");
                    PrintLog(record, $"{JsonConvert.SerializeObject(record)}");

                    var snsRecord = record.Sns;
                    var projeto = JsonConvert.DeserializeObject<ProjetoModel>(snsRecord.Message);
                    var project = new Projeto() {
                        ProjectId = projeto.Id,
                        CodigoPrograma = projeto.SourceCode,
                        ProjectNumber = projeto.Number,
                        ProjectName = projeto.Name
                    };

                    await ProcessProjectsAsync(project);               

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

        public async Task ProcessProjectsAsync(Projeto project)
        {
            using var ctx = new CadprogDbContext();

            var dbProject = await ctx.Projects.FirstOrDefaultAsync(x => x.ProjectId.Equals(project.ProjectId));

            if (dbProject != null)
                dbProject.Update(project);
            else
                await ctx.Projects.AddAsync(project);

            await ctx.SaveChangesAsync();
        }
    }    
}
