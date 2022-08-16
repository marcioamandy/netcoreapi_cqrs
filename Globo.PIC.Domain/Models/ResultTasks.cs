using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
	public class ResultTasks
	{

		/// <summary>
		/// 
		/// </summary>
		public List<TarefaModel> Items { get; set; }

		public ResultTasks()
		{
			Items = new List<TarefaModel>();
		}
	}
}
