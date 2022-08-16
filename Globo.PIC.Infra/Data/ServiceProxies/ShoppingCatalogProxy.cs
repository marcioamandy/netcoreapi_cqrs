using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Infra.Data.ServiceProxies
{

    /// <summary>
    /// 
    /// </summary>
    public class ShoppingCatalogProxy : IShoppingCatalogProxy
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ShoppingCatalogProxy> logger;

        /// <summary>
        /// 
        /// </summary>
        private const string OIC_SERVICE_API = "OICServiceAPI";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public ShoppingCatalogProxy(IHttpClientFactory _httpClientFactory,
            ILogger<ShoppingCatalogProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userPreferenceId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<ResultShoppingCatalog> IShoppingCatalogProxy.PostResultItemsAsync(string search, long? userPreferenceId, int offset, int limit, CancellationToken cancellationToken)
        {
            using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
            {
                try
                {
                    if (limit == 0) limit = 25;

                    logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));

                    var qs = string.Format($"applcoreApi/search/v1/fa-prc-shoppingcatalog?" +
                            $"limit={limit}&offset={offset}&smartSearchName=SSPShoppingCatalog&searchType=SEARCH");

                    if (userPreferenceId.HasValue)
                        qs += $"&userPreferenceId={userPreferenceId}";

                    var body = @"
                    {
                        ""filters"": [
                            {
                                ""text"": """ + search + @""",
                                ""filter"": ""keyword""
                            }
                        ]
                    }";

                    var content = new StringContent(body, Encoding.Default, "application/json");

                    var response = await client.PostAsync(qs, content, cancellationToken);

                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return await response.Content.ReadAsJsonAsync<ResultShoppingCatalog>();
                    }
                    else
                    {
                        string returnValue = await response.Content.ReadAsStringAsync();

                        logger.LogInformation(string.Format("Falha ao Chamar POST => /shoppingCatalog:", returnValue));

                        throw new Exception($"Falha na requisição POST => /shoppingCatalog: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogInformation(string.Format("HttpRequestException ao Chamar POST => /shoppingCatalog:", e));

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
