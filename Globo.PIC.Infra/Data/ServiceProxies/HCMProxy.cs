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
    public class HCMProxy : IHCMProxy
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<HCMProxy> logger;


        /// <summary>
		/// 
		/// </summary>
		private const string OIC_SERVICE_API = "OICServiceAPI";

        /// <summary>
        /// 
        /// </summary>F
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public HCMProxy(IHttpClientFactory _httpClientFactory,
            ILogger<HCMProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;
        }

        public async Task<Location> GetResultLocationsAsync(string location, CancellationToken cancellationToken)
        {
            string address = Environment.GetEnvironmentVariable("OIC_ENDPOINT_LOCATION");

            using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));

                    if (!string.IsNullOrEmpty(location))
                        address += string.Format(@"locationsV2?q={0}", location) + "&limit=1000";
                    else
                        address += "locationsV2?limit=1000";

                    var response = await client.GetAsync(address,
                        cancellationToken
                    );

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();
                        var result = await response.Content.ReadAsJsonAsync<Location>();
                        return result;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                        return null;
                    else
                    {
                        string returnValue = await response.Content.ReadAsStringAsync();
                        logger.LogInformation(string.Format("Falha ao Chamar GET => /localidades parameter : " + address, returnValue));

                        throw new Exception($"Falha na requisição GET => /localidades: ({response.StatusCode}) {returnValue}");
                    }
                }
                catch (Exception error)
                {
                    logger.LogInformation(string.Format("HttpRequestException ao Chamar GET => /localidades: parameter: " + address, error));

                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
