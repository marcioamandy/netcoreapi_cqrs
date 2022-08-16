using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{

	/// <summary>
	/// 
	/// </summary>
	public class AprovarOpcaoPedidoItemVeiculo : DomainCommand
	{

		/// <summary>
		/// 
		/// </summary>
		public long IdPedido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdPedidoItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long idOpcao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool bloqueioEmprestimos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string justificativaBloqueio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CancellationToken CancellationToken { get; set; }


	}
}
