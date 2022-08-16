using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetByIds : IRequest
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Ids")]
		public IEnumerable<long> Ids { get; set; }
	}
}
