using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
	public class PedidoItemCompraDocumentosModel : PedidoItemArteCompraDocumento
	{
		public new List<PedidoItemCompraDocumentosAnexosModel> Arquivos { get; set; }

	}
}
