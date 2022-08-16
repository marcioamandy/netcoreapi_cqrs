using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
	public class ResultProjects
	{

		/// <summary>
		/// 
		/// </summary>
		public List<ProjetoModel> Items { get; set; }

		public ResultProjects()
		{
			Items = new List<ProjetoModel>();
		}
	}
}
