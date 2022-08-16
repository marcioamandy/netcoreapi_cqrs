using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Globo.PIC.Functions.utils
{

    /// <summary>
    /// Classe Utilitaria para gerenciamento de mensagens para o SQS
    /// </summary>
    public class SQSManager
    {

        private readonly IAmazonSQS sqsClient;
        private readonly string urlQueue;

        public SQSManager(string _urlQueue)
        {
            sqsClient = new AmazonSQSClient();
            urlQueue = _urlQueue;
        }

        public async Task<SendMessageResponse> SendMessage(SendMessageRequest message)
        {
            message.QueueUrl = urlQueue;

            return await sqsClient.SendMessageAsync(message);
        }
    }
}
