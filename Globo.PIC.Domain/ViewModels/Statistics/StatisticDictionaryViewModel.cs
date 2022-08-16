using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels.Statistics
{
	public class StatisticDictionaryViewModel
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Chave")]
		public string Key { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Description("Rotulo")]
		public string Label { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Description("Valor")]
		public double Value { get; set; }

	}
}
