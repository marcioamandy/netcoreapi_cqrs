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
	public interface ITasksProxy : IDisposable
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ProjectId"></param>
		/// /// <param name="cancellationToken"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<List<TarefaModel>> GetResultTasksAsync(long projectId, CancellationToken cancellationToken);

		Task<TarefaModel> GetResultTaskAsync(long projectId, long taskId, CancellationToken cancellationToken);

	}
}
