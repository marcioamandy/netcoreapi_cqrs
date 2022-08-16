using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Infra.Data.ServiceProxies
{
    public class LineProxy : ILineProxy
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<LineProxy> logger;


        private const string DL_SERVICE_API = "DLServiceAPI";

        /// <summary>
        /// 
        /// </summary>F
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public LineProxy(IHttpClientFactory _httpClientFactory,
            ILogger<LineProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;
        }

        public async Task<bool> DeleteCancelLineAsync(string headId, CancellationToken cancellationToken)
        {
            string address = Environment.GetEnvironmentVariable("DL_ENDPOINT") +
               "emgt/contract-mgmt/v1/purchase-requisition/" + headId.ToString() + "/cancel";

            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri));


                    var response = await client.DeleteAsync(address,
                        cancellationToken
                    );

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return true;
                    }
                    else
                    {
                        string returnValue = await response.Content.ReadAsStringAsync();

                        logger.LogInformation(string.Format("Não é possível cancelar o Item porque a RC foi aprovada no Oracle - Falha ao Chamar GET => /Cancelamento RC:", returnValue));

                        return false;
                        //throw new Exception($" Não é possível cancelar o Item porque a RC foi aprovada no Oracle.");
                    }
                }
                catch (Exception error)
                {
                    logger.LogInformation(string.Format("HttpRequestException ao Chamar GET => /projects:", error));

                    throw;
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
