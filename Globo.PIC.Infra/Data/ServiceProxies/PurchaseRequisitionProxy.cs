using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Globo.PIC.Infra.Data.ServiceProxies
{
	 
	/// <summary>
	/// proxy com processo de criação da RC de um pedido com itens.
	/// </summary>
	/// <remarks> 1-header: criar o header da rc na oracle
	///<step number="1">{{APIOracle21A}}/purchaseRequisitions</step>
	/// </remarks>
	/// <remarks>2-linha: cria a linha para cada item do pedido
	///<step number="2" params="RequisitionHeaderId">{{APIOracle21A}}/purchaseRequisitions/300000047425146/child/lines</step>
	/// </remarks>
	/// <remarks>3-distributions: cria o distribution do item do pedido
	///<step number="3" params="RequisitionHeaderId,RequisitionLineId">{{APIOracle21A}}/purchaseRequisitions/300000048262356/child/lines/300000048337868/child/distributions</step>
	/// </remarks>
	/// <remarks>4-submeter a requisição dos pedidos
	///<step number="4" params="RequisitionHeaderId">{{APIOracle21A}}/purchaseRequisitions/300000048262356</step>
	/// </remarks>
	public class PurchaseRequisitionProxy : IPurchaseRequisitionProxy
	{
        
		/// <summary>
		/// 
		/// </summary>
		private readonly IHttpClientFactory httpClientFactory;

		/// <summary>
		/// 
		/// </summary>
        private readonly ILogger<PurchaseRequisitionProxy> logger;

		/// <summary>
		/// 
		/// </summary>
		private const string OIC_SERVICE_API = "OICServiceAPI";


		private const string DL_SERVICE_API = "DLServiceAPI";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_httpClientFactory"></param>
		/// <param name="_logger"></param>
		public PurchaseRequisitionProxy(IHttpClientFactory _httpClientFactory,
			ILogger<PurchaseRequisitionProxy> _logger)
		{
			logger = _logger;
			httpClientFactory = _httpClientFactory;
		}

		public async Task<RequisicaoCompra> PostPurchaseRequisitionAsync(PurchaseRequisition purchaseRequisition, CancellationToken cancellationToken)
        {
			using (var client = httpClientFactory.CreateClient(DL_SERVICE_API))
			{
				var body = SerializeEntityObject(purchaseRequisition);
			 
				try
				{
					var content = new StringContent(body, Encoding.Default, "application/json");

					logger.LogInformation(string.Format("[PostPurchaseRequisition::Chamada] - {0} API => {1}", DL_SERVICE_API, client.BaseAddress.AbsoluteUri));

					logger.LogInformation(string.Format("[PostPurchaseRequisition::Body] - {0}", body));
					
					var response = await client.PostAsync("emgt/contract-mgmt/v1/purchase-requisition", content, cancellationToken);

					response.EnsureSuccessStatusCode();

					if (response.StatusCode == HttpStatusCode.Created)
					{
						return await response.Content.ReadAsJsonAsync<RequisicaoCompra>();
					}
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("[PostPurchaseRequisition::FalhaNoEnvio] - {0}", returnValue));

						throw new Exception($"Falha na requisição POST => /DL_SERVICE_API: PostPurchaseRequisitionAsync: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception error)
				{
					logger.LogInformation(string.Format("[PostPurchaseRequisition::Exception] - {0}", error.StackTrace));

					throw;
				}
			}
		}

		public async Task<RequisitionHeader> PostRequisitionHeaderAsync(string body, CancellationToken cancellationToken)
		{
			//purchaseRequisitions
			using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
			{
				try
				{

					var content = new StringContent(body, Encoding.Default, "application/json");

					logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));
					logger.LogInformation("PostRequisitionHeaderAsync:", body, OIC_SERVICE_API, client.BaseAddress.AbsoluteUri);

					var response = await client.PostAsync("resources/11.13.18.05/purchaseRequisitions", content, cancellationToken);

					response.EnsureSuccessStatusCode();
					
					if (response.StatusCode == HttpStatusCode.Created)
					{
						return await response.Content.ReadAsJsonAsync<RequisitionHeader>();
					}
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("Falha ao Chamar POST => /OIC_SERVICE_API: PostRequisitionHeaderAsync:", returnValue, body));

						throw new Exception($"Falha na requisição POST => /OIC_SERVICE_API: PostRequisitionHeaderAsync: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception error)
				{
					logger.LogInformation(string.Format("Falha ao Chamar POST => /OIC_SERVICE_API: PostRequisitionHeaderAsync: " +error.StackTrace,  body));
					throw;
				}
			}
		}

		public async Task<RequisitionLine> PostRequisitionLineAsync(string headerId, string body, CancellationToken cancellationToken)
		{
			//purchaseRequisitions
			using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
			{
				try
				{
					var content = new StringContent(body, Encoding.Default, "application/json");
					logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));
					logger.LogInformation("PostRequisitionLineAsync:" + body + "HeaderId: " + headerId, OIC_SERVICE_API, client.BaseAddress.AbsoluteUri);
					var response = await client.PostAsync("resources/11.13.18.05/purchaseRequisitions/" + headerId + "/child/lines", content, cancellationToken);

					response.EnsureSuccessStatusCode();

					if (response.StatusCode == HttpStatusCode.Created)
					{
						return await response.Content.ReadAsJsonAsync<RequisitionLine>();
					}
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("Falha ao Chamar POST => PostRequisitionLineAsync / " + headerId + " / child / lines", returnValue, body));

						throw new Exception($"Falha na requisição POST => /PostRequisitionLineAsync: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception error)
				{
					logger.LogInformation(string.Format("HttpRequestException ao Chamar POST =>  PostRequisitionLineAsync / " + headerId + " / child / lines" +error.StackTrace, body));

					throw;
				}
			}
		}
		public async Task<DistributionLine> PostRequisitionDistributionWithHeaderAsync(string headerId, string body, CancellationToken cancellationToken)
		{
			using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
			{
				try
				{
					var content = new StringContent(body, Encoding.Default, "application/json");
					logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));
					logger.LogInformation("PostRequisitionDistributionWithHeaderAsync:" + body + "HeaderId: " + headerId, OIC_SERVICE_API, client.BaseAddress.AbsoluteUri);
					var response = await client.PostAsync("resources/11.13.18.05/purchaseRequisitions/" + headerId , content, cancellationToken);
					response.EnsureSuccessStatusCode();

					if (response.StatusCode == HttpStatusCode.Created)
						return await response.Content.ReadAsJsonAsync<DistributionLine>();
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();
						logger.LogInformation(string.Format("Falha ao Chamar POST => PostRequisitionDistributionWithHeaderAsync / " + headerId , returnValue, body));
						throw new Exception($"Falha na requisição POST => /PostRequisitionDistributionWithHeaderAsync: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception error)
				{
					logger.LogInformation(string.Format("HttpRequestException ao Chamar POST =>  PostRequisitionDistributionWithHeaderAsync / " + headerId , error, body));
					throw;
				}
			}
		}

		public async Task<DistributionLine> PostRequisitionDistributionWithLineAsync(string headerId, string lineId, string body, CancellationToken cancellationToken)
		{ 
			using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
			{
				try
				{
					var content = new StringContent(body, Encoding.Default, "application/json");
					logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));
					var log = string.Format("PostRequisitionDistributionWithLineAsync:" + content + "HeaderId: " + headerId + "LineId: " + lineId);
					logger.LogInformation(log, OIC_SERVICE_API, client.BaseAddress.AbsoluteUri);

					var response = await client.PostAsync("resources/11.13.18.05/purchaseRequisitions/" + headerId+"/child/lines/"+lineId+"/child/distributions", content, cancellationToken);
					response.EnsureSuccessStatusCode();
					
					if (response.StatusCode == HttpStatusCode.Created)
						return await response.Content.ReadAsJsonAsync<DistributionLine>();
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("Falha ao Chamar POST => PostRequisitionDistributionWithLineAsync / " + headerId + " / child / lines", returnValue,body));

						throw new Exception($"Falha na requisição POST => /PostRequisitionDistributionWithLineAsync: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception error)
				{
					logger.LogInformation(string.Format("HttpRequestException ao Chamar POST =>  PostRequisitionDistributionWithLineAsync / " + headerId + " / child / lines", error, body));

					throw;
				}
			}
		}
		public async Task<HeaderLine> PostRequisitionAsync(string headerId, string body, CancellationToken cancellationToken) 
		{ 
			using (var client = httpClientFactory.CreateClient(OIC_SERVICE_API))
			{
				try
				{ 
					logger.LogInformation(string.Format("{0} Chamando API => {1}", OIC_SERVICE_API, client.BaseAddress.AbsoluteUri));
					
					var content = new StringContent(body, Encoding.Default, "application/vnd.oracle.adf.action+json");
					//{ { APIOracle21A} }/ purchaseRequisitions / 300000048262356
					logger.LogInformation("PostRequisitionAsync:" + content + "HeaderId: " + headerId, OIC_SERVICE_API, client.BaseAddress.AbsoluteUri);

					var response = await client.PostAsync("resources/11.13.18.05/purchaseRequisitions/" + headerId, content, cancellationToken);

					response.EnsureSuccessStatusCode();
					
					if (response.StatusCode == HttpStatusCode.OK)
						return await response.Content.ReadAsJsonAsync<HeaderLine>();
					else
					{
						string returnValue = await response.Content.ReadAsStringAsync();

						logger.LogInformation(string.Format("Falha ao Chamar POST => PostRequisitionAsync / " + headerId  , returnValue, body));

						throw new Exception($"Falha na requisição POST => /PostRequisitionAsync: ({response.StatusCode}): {returnValue}");
					}
				}
				catch (Exception error)
				{
					logger.LogInformation(string.Format("HttpRequestException ao Chamar POST =>  PostRequisitionAsync / " + headerId , body, error));

					throw;
				}
			}
		}

		private static string SerializeEntityObject(object entityObject)
		{
			return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
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
