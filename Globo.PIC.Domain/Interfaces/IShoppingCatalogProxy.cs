using Globo.PIC.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{

	/// <summary>
	/// 
	/// </summary>
	public interface IShoppingCatalogProxy : IDisposable
	{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userPreferenceId"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ResultShoppingCatalog> PostResultItemsAsync(string search, long? userPreferenceId, int offset, int limit, CancellationToken cancellationToken);

	}
}