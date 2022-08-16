using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
	public class PedidoItemCompraModel : PedidoItemArteCompra
	{
		public new List<PedidoItemCompraDocumentosModel> Documentos { get; set; }

	}
}
