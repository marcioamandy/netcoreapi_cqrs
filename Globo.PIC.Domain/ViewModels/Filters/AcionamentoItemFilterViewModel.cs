using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels.Filters
{
    public class AcionamentoItemFilterViewModel : BaseFilterViewModel
    {
		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Acionamento Item")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Acionamento")]
		public long IdAcionamento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido Item")]
		public long IdPedidoItem { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public AcionamentoItemFilterViewModel()
		{
		}
	}
}
