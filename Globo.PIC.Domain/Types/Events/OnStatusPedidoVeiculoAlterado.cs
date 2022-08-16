using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using MediatR;

namespace Globo.PIC.Domain.Types.Events
{

	/// <summary>
	/// 
	/// </summary>
	public class OnStatusPedidoVeiculoAlterado : INotification
	{
		/// <summary>
		/// 
		/// </summary>
		public long idStatus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string JustificativaCancelamento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string JustificativaDevolucao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public PedidoVeiculo Pedido { get; set; }


	}
}
