using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
	public class PedidoItemConversaModel : PedidoItemConversa
	{
		public new List<PedidoItemConversaAnexosModel> Arquivos { get; set; }

	}
}
