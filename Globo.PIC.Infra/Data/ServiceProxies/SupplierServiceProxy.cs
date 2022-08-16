using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Entities;
using System.Dynamic;
using Newtonsoft.Json;
using System.Threading;

namespace Globo.PIC.Infra.Data.ServiceProxies
{
    public class SupplierServiceProxy : ISupplierProxy
    {


        private const string DL_SERVICE_API = "DLServiceAPI";

        private const string STAR_SERVICE_API = "StarServiceAPI";


        /// <summary>
        /// 
        /// </summary> 
        private readonly string TOKEN = "";

        /// <summary>
        /// 
        /// </summary> 
        private readonly string STAR_USER = "";

        /// <summary>
        /// 
        /// </summary> 
        private readonly string STAR_PASSWORD = "";
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<SupplierServiceProxy> logger;


        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public SupplierServiceProxy(IHttpClientFactory _httpClientFactory,
            ILogger<SupplierServiceProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;

            STAR_USER = Environment.GetEnvironmentVariable("STAR_USERNAME");
            STAR_PASSWORD = Environment.GetEnvironmentVariable("STAR_PASSWORD");

            var result = GetToken(STAR_USER, STAR_PASSWORD);
            result.Wait();
            TOKEN = result.Result;

        }

        private async Task<string> GetToken(string usuario, string senha)
        {
            //HttpResponseMessage token = null;

            using (var client = httpClientFactory.CreateClient(STAR_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", STAR_SERVICE_API, client.BaseAddress.AbsoluteUri));

                    //Http content
                    dynamic expando = new ExpandoObject();
                    expando.username = usuario;
                    expando.password = senha;

                    var login = JsonConvert.SerializeObject(expando);

                    var content = new StringContent(login, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/authorization/service/login", content);

                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var token = await response.Content.ReadAsStringAsync();
                        //KeyValuePair<string, string> obj = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(token);
                        var obj = JsonConvert.DeserializeObject<AcessToken>(token);
                        return obj.access_token;
                    }
                    else
                    {
                        string returnValue = response.Content.ReadAsStringAsync().Result;

                        logger.LogInformation(string.Format("Falha ao Chamar /StarService:", returnValue));

                        throw new Exception($"Falha na requisição do STAR para o talento: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (HttpRequestException e)
                {

                    logger.LogInformation(string.Format("HttpRequestException ao Chamar /STAR:", e));

                    throw;
                }
            }
        }
        public async Task<Supplier> GetSuppliersByCode(long code, CancellationToken cancellationToken)
        {
            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    string address = "emgt/negotiation-purchase/v1/suppliers/";
                    //if (code >0)
                    //    address += string.Format(@"?name=%25{0}%25", code);

                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri + address));
                    client.DefaultRequestHeaders.Add("Authorization", TOKEN);
                    var response = await client.GetAsync(address + code);
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        var objeto = JsonConvert.DeserializeObject<Supplier>(res);
                        return objeto;
                    }
                    else
                    {
                        string returnValue = response.Content.ReadAsStringAsync().Result;

                        logger.LogInformation(string.Format("Falha ao Chamar /SUPPLIER_SERVICE_API:" + address, returnValue));

                        throw new Exception($"Falha na requisição do SUPPLIER_SERVICE_API: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (HttpRequestException e)
                {

                    logger.LogInformation(string.Format("HttpRequestException ao Chamar /SUPPLIER_SERVICE_API:", e));

                    throw;
                }
            }
        }
        public async Task<List<Supplier>> GetSuppliers(string name, CancellationToken cancellationToken)
        {
            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    string address = "emgt/negotiation-purchase/v1/suppliers/";
                    if (name != string.Empty)
                        address += string.Format(@"?name=%25{0}%25", name);

                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri + address));
                    client.DefaultRequestHeaders.Add("Authorization", TOKEN);
                    var response = await client.GetAsync(address);
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        var lista = JsonConvert.DeserializeObject<List<Supplier>>(res);
                        return lista;
                    }
                    else
                    {
                        string returnValue = response.Content.ReadAsStringAsync().Result;

                        logger.LogInformation(string.Format("Falha ao Chamar /SUPPLIER_SERVICE_API:" + address, returnValue));

                        throw new Exception($"Falha na requisição do SUPPLIER_SERVICE_API: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (HttpRequestException e)
                {

                    logger.LogInformation(string.Format("HttpRequestException ao Chamar /SUPPLIER_SERVICE_API:", e));

                    throw;
                }
            }
        }

        public async Task<List<Agreements>> GetAgreementBySuppliers(long code, CancellationToken cancellationToken)
        {
            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    string address = "emgt/negotiation-purchase/v1/suppliers/";

                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri + address));
                    client.DefaultRequestHeaders.Add("Authorization", TOKEN);
                    var response = await client.GetAsync(address + code + "/agreements");
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        var objeto = JsonConvert.DeserializeObject<List<Agreements>>(res);
                        return objeto;
                    }
                    else
                    {
                        string returnValue = response.Content.ReadAsStringAsync().Result;

                        logger.LogInformation(string.Format("Falha ao Chamar /SUPPLIER_SERVICE_API:" + address, returnValue));

                        throw new Exception($"Falha na requisição do SUPPLIER_SERVICE_API: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (HttpRequestException e)
                {
                    logger.LogInformation(string.Format("HttpRequestException ao Chamar /SUPPLIER_SERVICE_API:", e));

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
