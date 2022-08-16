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

namespace Globo.PIC.Infra.Data.ServiceProxies
{
    public class ConteudoServiceProxy : IConteudoServiceProxy
    {
        /// <summary>
        /// 
        /// </summary> 
        private const string STAR_SERVICE_API = "StarServiceAPI";


        private const string CONTEUDO_SERVICE_API = "ConteudoServiceAPI";

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
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ConteudoServiceProxy> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public ConteudoServiceProxy(IHttpClientFactory _httpClientFactory,
            ILogger<ConteudoServiceProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;

            STAR_USER = Environment.GetEnvironmentVariable("STAR_USERNAME");
            STAR_PASSWORD = Environment.GetEnvironmentVariable("STAR_PASSWORD");

            var result = GetToken(STAR_USER, STAR_PASSWORD);
            result.Wait();
            TOKEN = result.Result;

        }

        public async Task<string> GetToken(string usuario, string senha)
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

        public async Task<List<Conteudo>> GetConteudos()
        {
            using (var client = httpClientFactory.CreateClient(CONTEUDO_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", CONTEUDO_SERVICE_API, client.BaseAddress.AbsoluteUri));

                    client.DefaultRequestHeaders.Add("Authorization", TOKEN);

                    var response = await client.GetAsync("?Status=''A''");

                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject<List<Conteudo>>(res);
                        return obj;
                    }
                    else
                    {
                        string returnValue = response.Content.ReadAsStringAsync().Result;

                        logger.LogInformation(string.Format("Falha ao Chamar /CONTEUDO_SERVICE_API:", returnValue));

                        throw new Exception($"Falha na requisição do CONTEUDO_SERVICE_API: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (HttpRequestException e)
                {

                    logger.LogInformation(string.Format("HttpRequestException ao Chamar /CONTEUDO_SERVICE_API:", e));

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
