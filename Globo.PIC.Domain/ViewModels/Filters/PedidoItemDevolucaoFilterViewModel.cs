using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class PedidoItemDevolucaoFilterViewModel : BaseFilterViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Id tipo devolução do Pedido Item")]
		public long idTipo { get; set; }


		/// <summary>
		/// 
		/// </summary>
		[Description("Justificativa da devolução")]
		public string Justificativa { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public PedidoItemDevolucaoFilterViewModel()
		{
		}
	}
}
