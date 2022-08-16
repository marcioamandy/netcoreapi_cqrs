using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProjectProxy : IDisposable
	{
		 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<ProjetoModel> GetResultProjectAsync(long projectId, CancellationToken cancellationToken);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<ProjetoModel>> GetResultProjectsAsync(CancellationToken cancellationToken);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<ProjetoModel>> GetResultProjectsByProjectNameAsync(string projectName, CancellationToken cancellationToken);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<ProjetoModel>> GetResultProjectsAsync(List<long> projectId, CancellationToken cancellationToken);
		  
	}
}
