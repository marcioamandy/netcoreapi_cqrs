using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Models
{
	public class StatisticDictionary
	{

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Description("Chave")]
		public string Key { get; set; }

		/// <summary>
		/// 
		/// </summary>
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
