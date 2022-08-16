using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    public interface IHCMProxy : IDisposable
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<Location> GetResultLocationsAsync(string location, CancellationToken cancellationToken);
	}
}
