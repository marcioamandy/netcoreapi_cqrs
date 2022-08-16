using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdatePedidoStatus : DomainCommand
	{
		
		/// <summary>
		/// 
		/// </summary>
		[Description("IdStatus")]
		public long IdStatus { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[Description("IdPedido")]
		public long IdPedido { get; set; }
	}
}
