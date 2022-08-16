using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{

	public class ShoppingCatalogQueryHandler :
		IRequestHandler<GetByShoppingCatalogFilter, ResultShoppingCatalog>
	{

		private readonly IShoppingCatalogProxy shoppingCatalog;

		public ShoppingCatalogQueryHandler(IShoppingCatalogProxy _shoppingCatalog)
		{
			shoppingCatalog = _shoppingCatalog;
		}

		async Task<ResultShoppingCatalog> IRequestHandler<GetByShoppingCatalogFilter, ResultShoppingCatalog>.Handle(GetByShoppingCatalogFilter request, CancellationToken cancellationToken)
        {
			int offset = (request.Filter.Page - 1) * request.Filter.PerPage;

			return await shoppingCatalog.PostResultItemsAsync(request.Filter.Search, null, offset, request.Filter.PerPage, cancellationToken);
		}
    }
}
