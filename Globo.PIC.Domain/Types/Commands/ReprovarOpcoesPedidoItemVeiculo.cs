using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{

	/// <summary>
	/// 
	/// </summary>
	public class ReprovarOpcoesPedidoItemVeiculo : DomainCommand
	{

		/// <summary>
		/// 
		/// </summary>
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string JustificativaReprovacao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CancellationToken CancellationToken { get; set; }


	}
}