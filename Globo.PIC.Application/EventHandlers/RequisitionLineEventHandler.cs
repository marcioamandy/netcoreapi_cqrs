using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Amqp;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Types.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Globo.PIC.Application.EventHandlers
{

    /// <summary>
    /// 
    /// </summary>
    public class RequisitionLineEventHandler
        : INotificationHandler<OnStatusLineAlterada>

    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<RequisitionLineEventHandler> logger;

        string host { get; } = Environment.GetEnvironmentVariable("AMQP_DL_HOST");
        string port { get; } = Environment.GetEnvironmentVariable("AMQP_DL_PORT");
        string user { get; } = Environment.GetEnvironmentVariable("AMQP_DL_PIC_USERNAME");
        string pass { get; } = Environment.GetEnvironmentVariable("AMQP_DL_PIC_PASSWORD");
        string topico { get; } = Environment.GetEnvironmentVariable("AMQP_LINE_STATUS_TOPIC");
        string fila { get; } = Environment.GetEnvironmentVariable("AMQP_LINE_STATUS_QUEUE");
        readonly static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

        string CNN = "amqps://{0}:{1}@{2}:{3}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_mediator"></param>
        /// <param name="_rCRepository"></param>
        public RequisitionLineEventHandler(ILogger<RequisitionLineEventHandler> _logger, IMediator _mediator, IRepository<RC> _rCRepository)
        {
            logger = _logger;
            mediator = _mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task INotificationHandler<OnStatusLineAlterada>.Handle(OnStatusLineAlterada notification, CancellationToken cancellationToken)
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
        async Task<List<StatusLineOCRC>> GetMessages(CancellationToken cancellationToken)
        {
            int c = 0, chunk = 100;
            var list = new List<StatusLineOCRC>();

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

                        var payload = JsonConvert.DeserializeObject<StatusLineOCRC>(message.Body.ToString());

                        //Caso a mensagem for válida tentamos a atualização no BD e a aceitação da mensagem
                        if (ValidaPayload(payload))
                        {

                            logger.LogInformation($"[{GetAMQPLabel()}] Mensagem {c} recebida e aceita com o payload: {message.Body.ToString()}");

                            //payload.Requisitions[0].Number = "RCGRJ20000996";
                            //payload.Requisitions[0].Status = "APPROVED";
                            //payload.Status = "APPROVED";
                            //payload.Purchases[0].Number = "OCGCP20004013";

                            await mediator.Send(new UpdateRCLineStatus() { StatusLineOCRC = payload }, cancellationToken);

                            receiver.Accept(message);

                            logger.LogInformation($"[{GetAMQPLabel()}] Mensagem {c} atualizou a base de dados: {message.Body.ToString()}");

                        }
                        //Caso a mensagem for inválida rejeitamos ela e mandamos para a fila morta
                        else
                        {
                            Console.WriteLine($"[{GetAMQPLabel()}] A seguinte mensagem tem um payload inválido foi envianda para a fila morta: {message.Body.ToString()}");

                            receiver.Reject(message);
                        }

                        receiver.Close();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"[{GetAMQPLabel()}] Falha ao ler mensagem {c}: {ex}");

                        receiver.Reject(message);

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

        bool ValidaPayload(StatusLineOCRC p) => !(
            p == null ||
            p.Requisitions == null ||
            p.Requisitions.Count != 1 ||
            string.IsNullOrWhiteSpace(p.Requisitions[0].Number) ||
            !(p.InterfaceSourceCodeLine ?? string.Empty).ToUpper().Equals("PIC")
         //!(p.Type ?? string.Empty).ToUpper().Equals("REQUISITION")
         );

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
            catch {}

            return "AMQP";
        }
    }
}
