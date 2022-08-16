using MediatR;
using System.ComponentModel;
using System.IO;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{

	/// <summary>
	/// 
	/// </summary>
	public class GetById :
		IRequest<Stream>,
		IRequest<Notificacao>,
		IRequest<Pedido>,
		IRequest<PedidoItem>,
		IRequest<StatusPedidoItemArte>,
		IRequest<PedidoItemAnexo>,
		IRequest<PedidoItemConversa>,
		IRequest<PedidoItemConversaAnexo>,
		IRequest<Equipe>,
		IRequest<PedidoAnexo>,
		IRequest<PedidoItemArteCompra>,
		IRequest<PedidoItemArteCompraDocumento>,
		IRequest<PedidoItemArteCompraDocumentoAnexo>,
		IRequest<PedidoItemArteEntrega>,
		IRequest<PedidoItemArteDevolucao>,
		IRequest<PedidoItemArteAtribuicao>,
		IRequest<StatusPedidoVeiculo>,
		IRequest<StatusPedidoItemVeiculo>,
		IRequest<PedidoArte>,
		IRequest<PedidoItemArte>,
		IRequest<PedidoVeiculo>,
		IRequest<PedidoItemVeiculo>,
		IRequest<Categoria>,
		IRequest<Departamento>,
		IRequest<Item>,
		IRequest<ItemCatalogo>,
		IRequest<ItemVeiculo>
	{ 
		/// <summary>
		/// 
		/// </summary>
		[Description("Id")]
		public long Id { get; set; }
	}
}
