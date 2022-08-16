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
    public class ProjetoEventHandler
        : INotificationHandler<OnProjetoDLNotificado>
    {

        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<ProjetoEventHandler> logger;

        string host { get; } = Environment.GetEnvironmentVariable("AMQP_DL_HOST");
        string port { get; } = Environment.GetEnvironmentVariable("AMQP_DL_PORT");
        string user { get; } = Environment.GetEnvironmentVariable("AMQP_DL_CADPROG_USERNAME");
        string pass { get; } = Environment.GetEnvironmentVariable("AMQP_DL_CADPROG_PASSWORD");
        string topico { get; } = Environment.GetEnvironmentVariable("AMQP_PROJECT_TOPIC");
        string fila { get; } = Environment.GetEnvironmentVariable("AMQP_PROJECT_QUEUE");
        string snsArn { get; } = Environment.GetEnvironmentVariable("SNS_PROJETO_ARN");

        readonly static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

        string CNN = "amqps://{0}:{1}@{2}:{3}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        public ProjetoEventHandler(ILogger<ProjetoEventHandler> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task INotificationHandler<OnProjetoDLNotificado>.Handle(OnProjetoDLNotificado notification, CancellationToken cancellationToken)
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

                        //var fake = "{ \"id\": 300000072327081, \"name\": \"Edição Pós Produção G5\", \"number\": \"118\", \"description\": \"Edição Pós Produção G5\", \"status\": \"ACTIVE\", \"sourceCode\": null, \"businessUnit\": {  \"id\": \"300000061900773\", \"description\": \"GCP-CENTRAL\" } }";
                        //var payload = JsonConvert.DeserializeObject<ProjetoModel>(fake);

                        var payload = JsonConvert.DeserializeObject<ProjetoModel>(message.Body.ToString());

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

        bool ValidaPayload(ProjetoModel p) => !( p == null );

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

        private async Task<bool> PublishSnsRequest(ProjetoModel message)
        {

            var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.USEast1);

            var mensagem = new MensagemSNS<ProjetoModel>(message);
                        
            PublishRequest publishReq = new PublishRequest()
            {
                Subject = "projeto-cadprog-sync",
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
