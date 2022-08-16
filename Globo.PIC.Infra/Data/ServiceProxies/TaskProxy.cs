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
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Infra.Data.ServiceProxies
{
	public class TaskProxy : ITasksProxy
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IHttpClientFactory httpClientFactory;

		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger<TaskProxy> logger;


		private const string DL_SERVICE_API = "DLServiceAPI";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_httpClientFactory"></param>
		/// <param name="_logger"></param>
		public TaskProxy(IHttpClientFactory _httpClientFactory,
			ILogger<TaskProxy> _logger)
		{
			logger = _logger;
			httpClientFactory = _httpClientFactory;
		}

		public async Task<TarefaModel> GetResultTaskAsync(long projectId, long taskId, CancellationToken cancellationToken)
		{
			string address = Environment.GetEnvironmentVariable("DL_ENDPOINT_PROJECTS");
			address += string.Format("{0}/tasks/", projectId);
			address += string.Format("{0}", taskId);

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
						//return await response.Content.ReadAsJsonAsync<ResultTask>();

						var tarefas = await response.Content.ReadAsJsonAsync<List<TarefaModel>>();

						if (tarefas != null && tarefas.Count > 0)
							return tarefas.FirstOrDefault();
						else
							return null;
					}
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("Falha ao Chamar GET => /Tasks:", returnValue));

						throw new Exception($"Falha na requisição GET => /" + address + ": ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception e)
				{
					logger.LogInformation(string.Format("HttpRequestException ao Chamar GET => /" + address + ":", e));

					throw;
				}
			}
		}

		public async Task<List<TarefaModel>> GetResultTasksAsync(long projectId, CancellationToken cancellationToken)
		{
			string address = Environment.GetEnvironmentVariable("DL_ENDPOINT_PROJECTS");
			address += string.Format("{0}/tasks", projectId);

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
						return await response.Content.ReadAsJsonAsync<List<TarefaModel>>(); 
					}
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("Falha ao Chamar GET => /Tasks:", returnValue));

						throw new Exception($"Falha na requisição GET => /"+address+": ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception e)
				{
					logger.LogInformation(string.Format("HttpRequestException ao Chamar GET => /" + address + ":", e));

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
