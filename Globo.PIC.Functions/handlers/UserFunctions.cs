using System.Collections.Generic;
using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Globo.PIC.Functions.utils;
using System;
using Globo.PIC.Domain.Models.Functions;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Amazon.Lambda.SQSEvents;
using Globo.PIC.Domain.ViewModels;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Reflection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Globo.PIC.Functions.handlers
{

    public class UserFunctions
    {
        private string URL_SQS_USUARIO { get; } = Environment.GetEnvironmentVariable("SQS_QUEUE_USUARIO");

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse GetUserHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var adService = new ADServiceNovell().GetUser("tcordeir");

            var body = new Dictionary<string, string> {};

            if(adService != null){
                body["Nome"] = adService.Name;
                body["Sobrenome"] = adService.LastName;
                body["Email"] = adService.Email;
            }

            Console.WriteLine("USUARIO: ", JsonConvert.SerializeObject(body, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
            }));

            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(body),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<bool> GetUserADHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            try
            {
                List<object> usuariosEncontrados = new List<object>();

                foreach (var record in sqsEvent.Records)
                {
                   var sqsRecord = record.Body;

                   Console.WriteLine(
                       $"[{record.EventSource} {DateTime.Now.ToString()}] " +
                       $"Message={sqsRecord}");

                    var vm = JsonConvert.DeserializeObject<SNSMessage>(sqsRecord);
                    
                    var uvm = JsonConvert.DeserializeObject<UsuarioViewModel>(vm.Message);                     

                   var usuario = new ADServiceNovell().GetUser(uvm.Login);

                   if (usuario != null)
                       usuariosEncontrados.Add(usuario);
                }

                if (usuariosEncontrados.Count > 0)
                {
                   SQSManager sqsUsuarioCliente = new SQSManager(URL_SQS_USUARIO);

                   foreach (var usuario in usuariosEncontrados)
                   {
                       Console.WriteLine($"Enviando usuário: {JsonConvert.SerializeObject(usuario)}");

                       await sqsUsuarioCliente.SendMessage(new SendMessageRequest()
                       {
                           MessageBody = JsonConvert.SerializeObject(usuario)
                       });

                       Console.WriteLine("Enviado com sucesso!");
                   }
                } else {
                       Console.WriteLine("Não foi possível encontrar um usuário na busca do AD!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha ao processar a mensagem: {0}", JsonConvert.SerializeObject(ex));

                return await Task.FromResult(false);
            }
            

            return await Task.FromResult(true);
        }
    }
}
