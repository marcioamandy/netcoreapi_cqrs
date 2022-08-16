using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels.Filters
{
    public class AcionamentoFilterViewModel : BaseFilterViewModel
    {
		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Acionamento")]
		public long Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca para Id Pedido")]
		public long IdPedido { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public AcionamentoFilterViewModel()
		{
		}
	}
}
