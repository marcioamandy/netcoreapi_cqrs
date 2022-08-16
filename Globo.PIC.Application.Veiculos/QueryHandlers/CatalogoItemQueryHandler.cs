using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries.Filters;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
	public class CatalogoItemQueryHandler :
		IRequestHandler<GetByCatalogoItemFilter, List<CatalogoItem>>
	{

		private readonly IShoppingSearchProxy shoppingSearch;

		public CatalogoItemQueryHandler(IShoppingSearchProxy _shoppingSearch)
		{
			shoppingSearch = _shoppingSearch;
		}

		async Task<List<CatalogoItem>> IRequestHandler<GetByCatalogoItemFilter, List<CatalogoItem>>.Handle(GetByCatalogoItemFilter request, CancellationToken cancellationToken)
		{
			List<Arquivo> arquivos = new List<Arquivo>();
			List<CatalogoItem> catalogoItemList = new List<CatalogoItem>();

			var requisitioningBUIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_REQUISITIONINGBUID");
			long requisitioningBUId = long.Parse(requisitioningBUIdEnvironmentVariable);

			var body = SerializeEntityObject(new
			{
				SearchPhraseTerms = request.Filter.Search,
				RequisitioningBUId = requisitioningBUId 
			});

			var phraseSearch = await shoppingSearch.PostCreatePhaseAsync(body, cancellationToken);

			var resultItems = await shoppingSearch.GetResultItemsAsync(phraseSearch.SearchPhraseId, cancellationToken);
			  
			int i = 0;

			foreach (var item in resultItems.items)
			{
				i++;

				catalogoItemList.Add (
					new CatalogoItem
					{
						Descricao = item.ItemDescription,
						FornecedorId = i,
						FornecedorNome = "Fornecedor: " + i.ToString(),
						Arquivos = arquivos,
						Nome = item.ItemDescription,
						UnitPrice = item.UnitPrice,
						FormattedUnitPrice = item.FormattedUnitPrice,
						Acordo = item.UnitPrice>1? "ack000"+i.ToString(): "",//Todo: Mock de acordos
						IdItemOracle = item.ItemId.ToString(),
						ItemKeyOracle = item.ItemKey,
						Quantidade = i+1,
						CategoriaId = i,
						CategoriaNome = item.CategoryName,
						FabricanteId = i,
						FabricanteNome = "Fabricante " +i.ToString()
					});
			}

			return catalogoItemList;
		}

		/// <summary>
		/// Serializes Entities Objects preventing the Loop Reference error
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		private static string SerializeEntityObject(object entityObject)
		{
			return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
		}
	}
}
