using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Infra.Data.ServiceProxies
{
    public class TokenDLProxy : ITokenDLProxy
    {


        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;
         
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ProjectProxy> logger;
         


        private const string DL_TOKEN = "DLToken";

        /// <summary>
        /// 
        /// </summary>F
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public TokenDLProxy(IHttpClientFactory _httpClientFactory,
            ILogger<ProjectProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory; 
        } 
        public async Task<TokenDL> GetDLToken(CancellationToken cancellationToken)
        {
            using (var client = httpClientFactory.CreateClient(DL_TOKEN))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_TOKEN, client.BaseAddress.AbsoluteUri));

                    string GrantType = Environment.GetEnvironmentVariable("TOKEN_DL_GRANT_TYPE");
                    string ClientId = Environment.GetEnvironmentVariable("TOKEN_DL_CLIENT_ID");
                    string ClientSecret = Environment.GetEnvironmentVariable("TOKEN_DL_CLIENT_SECRET");
                    string Resource = Environment.GetEnvironmentVariable("TOKEN_DL_RESOURCE");
                    string TenantId = Environment.GetEnvironmentVariable("TOKEN_DL_TENANT_ID");       
                 
                    var body = new Dictionary<string, string>
                    {
                        { "grant_type", GrantType }, 
                        { "client_id", ClientId }, 
                        { "client_secret", ClientSecret }, 
                        { "resource", Resource } ,
                        {"tenant_id", TenantId}
                    };
                    var req = new HttpRequestMessage(HttpMethod.Post, TenantId +"/oauth2/token") { Content = new FormUrlEncodedContent(body) };
                    var response = await client.SendAsync(req); 

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();
                        dynamic json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                        var expiresOn = 
                            DateTimeOffset.FromUnixTimeSeconds(long.Parse(json["expires_on"].Value)).DateTime;
                        TokenDL tokenDL = new TokenDL(); 
                        tokenDL.ExpiresOn = expiresOn;
                        tokenDL.Token = json["access_token"].Value;
                        return tokenDL;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else
                    {
                        string returnValue = await response.Content.ReadAsStringAsync();

                        logger.LogInformation(string.Format("Falha ao Chamar GET => /projects:", returnValue));

                        throw new Exception($"Falha na requisição GET => /projects: ({response.StatusCode}): {returnValue}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogInformation(string.Format("HttpRequestException ao Chamar GET => /projects:", e));

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
