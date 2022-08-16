using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Infra.Helpers;

namespace Globo.PIC.Infra.Data.ServiceProxies
{
	/// <summary>
	/// 
	/// </summary>
	public class NodeServiceProxy : INodeServiceProxy
	{
		/// <summary>
		/// 
		/// </summary>
		private const string NODE_SERVICE_API = "NodeServiceAPI";

		/// <summary>
		/// 
		/// </summary>
		private readonly IHttpClientFactory httpClientFactory;

		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger<NodeServiceProxy> logger;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_httpClientFactory"></param>
		/// <param name="_logger"></param>
		public NodeServiceProxy(IHttpClientFactory _httpClientFactory,
			ILogger<NodeServiceProxy> _logger)
		{
			logger = _logger;
			httpClientFactory = _httpClientFactory;
		}

		public async Task<Stream> PutPdfGenAsync(string orderJSON, CancellationToken cancellationToken)
		{
			using (var client = httpClientFactory.CreateClient(NODE_SERVICE_API))
			{
				try
				{
					logger.LogInformation(string.Format("{0} Chamando API => {1}", NODE_SERVICE_API, client.BaseAddress.AbsoluteUri));

					var content = new StringContent(orderJSON, System.Text.Encoding.UTF8, "application/json");

					var response = await client.PutAsync("/pdf-gen", content, cancellationToken);

					response.EnsureSuccessStatusCode();

					if (response.StatusCode == HttpStatusCode.OK)
					{
						return response.Content.ReadAsStreamAsync().Result;
					}
					else
					{
						string returnValue = response.Content.ReadAsStringAsync().Result;

						logger.LogInformation(string.Format("Falha ao Chamar /psf-gen:", returnValue));

						throw new Exception($"Falha na requisição do PDF para o pedido: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (HttpRequestException e)
				{
					
					logger.LogInformation(string.Format("HttpRequestException ao Chamar /psf-gen:", e));
					
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
