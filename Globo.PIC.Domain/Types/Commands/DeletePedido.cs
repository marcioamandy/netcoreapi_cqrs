using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{

	/// <summary>
	/// 
	/// </summary>
	public class DeletePedido : DomainCommand
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Pedido")]
		public Pedido Pedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CancellationToken CancellationToken { get; set; }


	}
}
