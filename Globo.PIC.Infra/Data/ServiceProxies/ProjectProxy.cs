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
    public class ProjectProxy : IProjectProxy
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ProjectProxy> logger;


        private const string DL_SERVICE_API = "DLServiceAPI";

        /// <summary>
        /// 
        /// </summary>F
        /// <param name="_httpClientFactory"></param>
        /// <param name="_logger"></param>
        public ProjectProxy(IHttpClientFactory _httpClientFactory,
            ILogger<ProjectProxy> _logger)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;
        }

        public async Task<ProjetoModel> GetResultProjectAsync(long projectId, CancellationToken cancellationToken)
        {
            string address = Environment.GetEnvironmentVariable("DL_ENDPOINT_PROJECTS") +
               "?projectId=" + projectId.ToString();

            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri));


                    var response = await client.GetAsync(address,
                        cancellationToken
                    );

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();

                        var projetos = await response.Content.ReadAsJsonAsync<List<ProjetoModel>>();

                        if (projetos != null && projetos.Count > 0)
                            return projetos.FirstOrDefault();
                        else
                            return null;
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
                catch (Exception error)
                {
                    logger.LogInformation(string.Format("HttpRequestException ao Chamar GET => /projects:", error));

                    throw;
                }
            }
        }
        public async Task<List<ProjetoModel>> GetResultProjectsByProjectNameAsync(string projectName, CancellationToken cancellationToken)
        {
            string address = Environment.GetEnvironmentVariable("DL_ENDPOINT_PROJECTS");

            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri));

                    if (!string.IsNullOrEmpty(projectName))
                        address += string.Format(@"?projectName=%25{0}%25", projectName);
                     
                    var response = await client.GetAsync(address,
                        cancellationToken
                    );

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();

                        return await response.Content.ReadAsJsonAsync<List<ProjetoModel>>();
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
        public async Task<List<ProjetoModel>> GetResultProjectsAsync(CancellationToken cancellationToken)
        {
            string address = Environment.GetEnvironmentVariable("DL_ENDPOINT_PROJECTS") ;

            using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
            {
                try
                {
                    logger.LogInformation(string.Format("{0} Chamando API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri));

                    var response = await client.GetAsync(address,
                        cancellationToken
                    );

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();

                        return await response.Content.ReadAsJsonAsync<List<ProjetoModel>>();
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

        public async Task<List<ProjetoModel>> GetResultProjectsAsync(List<long> projectId, CancellationToken cancellationToken) {
            List<ProjetoModel> projetos = new List<ProjetoModel>();
            var allProjects = await GetResultProjectsAsync(cancellationToken); 
            projetos = allProjects.Where(p => projectId.Any(p2 => p2 == p.Id)).ToList();
            return projetos;
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
