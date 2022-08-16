 
using System.ComponentModel; 

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetBySearch 
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
		public string Filter { get; set; }
	}
}
