
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class UnidadeNegocioFilterViewModel : BaseFilterViewModel
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca")]
		public string Search { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public UnidadeNegocioFilterViewModel()
		{
		}
	}
}
