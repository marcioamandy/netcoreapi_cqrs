using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Amqp;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Net;

namespace Globo.PIC.Application.EventHandlers
{

    /// <summary>
    /// 
    /// </summary>
    public class TarefaEventHandler
        : INotificationHandler<OnTarefaDLNotificado>
    {

        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<TarefaEventHandler> logger;

        string host { get; } = Environment.GetEnvironmentVariable("AMQP_DL_HOST");
        string port { get; } = Environment.GetEnvironmentVariable("AMQP_DL_PORT");
        string user { get; } = Environment.GetEnvironmentVariable("AMQP_DL_CADPROG_USERNAME");
        string pass { get; } = Environment.GetEnvironmentVariable("AMQP_DL_CADPROG_PASSWORD");
        string topico { get; } = Environment.GetEnvironmentVariable("AMQP_TASK_TOPIC");
        string fila { get; } = Environment.GetEnvironmentVariable("AMQP_TASK_QUEUE");
        string snsArn { get; } = Environment.GetEnvironmentVariable("SNS_TAREFA_ARN");

        readonly static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

        string CNN = "amqps://{0}:{1}@{2}:{3}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        public TarefaEventHandler(ILogger<TarefaEventHandler> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task INotificationHandler<OnTarefaDLNotificado>.Handle(OnTarefaDLNotificado notification, CancellationToken cancellationToken)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
                return Unit.Task;

            var taskMessages = GetMessages(cancellationToken); taskMessages.Wait();

            return Unit.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<List<ProjetoModel>> GetMessages(CancellationToken cancellationToken)
        {
            int c = 0, chunk = 1;
            var list = new List<ProjetoModel>();

            logger.LogInformation($"[{GetAMQPLabel()}] Iniciando leitura de até {chunk} mensagens da fila.");

            try
            {
                var factory = new ConnectionFactory();

                factory.SSL.CheckCertificateRevocation = false;

                factory.SSL.RemoteCertificateValidationCallback += Callback;

                CNN = string.Format(CNN, user, pass, host, port);

                Address address = new Address(CNN);   
                Connection connection = await factory.CreateAsync(address);
                Session session = new Session(connection);
                Message message = null;

                do
                {
                    c++;

                    ReceiverLink receiver = new ReceiverLink(session, "Status", $@"{topico}::{fila}");

                    if (c > chunk)
                        break;

                    try
                    {
                        logger.LogInformation($"[{GetAMQPLabel()}] Recebendo mensagem {c} ...");

                        message = receiver.Receive(receiveTimeout);

                        if (message == null)
                        {
                            receiver.Close();
                            continue;
                        }

                        //File.AppendAllText("/Users/thiago.santos/Documents/prog/completa",(c > 1 ? ",":"") + message.Body.ToString());

                        //var fake = "{\"id\": 100000083633017,\"projectId\": 300000086333145,\"projectNumber\": \"567\",\"number\": \"43.5\",\"description\": \"ROP-Carro_PIPA (SERVIÇO_SPOT)\",\"level\": 2,\"startDate\": \"07-12-2021\",\"endDate\": \"30-12-2022\",\"chargeable\": true,\"parentId\": \"100000083633012\",\"parentNumber\": \"43\",\"accountingSegments\": [{\"type\": \"centroDeCusto\",\"value\": \"GL220202001\",\"description\": \"PRODUÇÃO DRAMATURGIA LONGA\",\"enabled\": true},{\"type\": \"projeto\",\"value\": \"PEVT000143001999\",\"description\": \"P.EVT.000143-001-999 - SUPERLIGA B DE VÔLEI FEMININO/2022/NA\",\"enabled\": true},{\"type\": \"finalidade\",\"value\": \"FG000015\",\"description\": \"CENOGRAFIA ESTÚDIO\",\"enabled\": true}]}";
                        var payload = JsonConvert.DeserializeObject<TarefaModel>(message.Body.ToString());

                        //Caso a mensagem for válida tentamos a atualização no BD e a aceitação da mensagem
                        if (ValidaPayload(payload))
                        {

                            logger.LogInformation($"[{GetAMQPLabel()}] Mensagem {c} recebida e aceita com o payload: {message.Body.ToString()}");

                            await PublishSnsRequest(payload);

                            // receiver.Accept(message);

                            logger.LogInformation($"[{GetAMQPLabel()}] Mensagem {c} atualizou a base de dados: {message.Body.ToString()}");

                        }
                        //Caso a mensagem for inválida rejeitamos ela e mandamos para a fila morta
                        else
                        {
                            Console.WriteLine($"[{GetAMQPLabel()}] A seguinte mensagem tem um payload inválido foi envianda para a fila morta: {message.Body.ToString()}");

                            // receiver.Reject(message);
                        }

                        receiver.Close();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"[{GetAMQPLabel()}] Falha ao ler mensagem {c}: {ex}");

                        // receiver.Reject(message);

                        receiver.Close();
                    }

                } while (message != null);

                session.Close();

                connection.Close();

            }
            catch (Exception ex)
            {
                logger.LogError($"[{GetAMQPLabel()}] Falha ao receber mensagens, leitura da fila cancelada: {ex}");
            }

            return await Task.FromResult(list);
        }

        bool ValidaPayload(TarefaModel p) => !( p == null );

        bool Callback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

        string GetAMQPLabel()
        {
            try
            {
                if (fila != null)
                {
                    var spt = fila.Split('.');

                    return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} | AMQP::{spt[spt.Length - 1]}";
                }
            }
            catch { }

            return "AMQP";
        }

        private async Task<bool> PublishSnsRequest(TarefaModel message)
        {
            
            var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.USEast1);

            var mensagem = new MensagemSNS<TarefaModel>(message);
                        
            PublishRequest publishReq = new PublishRequest()
            {                
                Subject = "tarefa-cadprog-sync",
                TargetArn = snsArn, 
                MessageStructure = "json",
                Message = mensagem.Serialize()
            };

            PublishResponse response = await snsClient.PublishAsync(publishReq);

            if (response.HttpStatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
            
        }
    }
}
