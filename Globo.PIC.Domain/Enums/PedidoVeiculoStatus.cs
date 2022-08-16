using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum PedidoVeiculoStatus
	{
		/// <summary>
		/// Quando o pedido foi criado mas não foi encaminhado ao aprovador.
		/// </summary>
		[Description("RASCUNHO")]
		PEDIDO_RASCUNHO = 1,

		/// <summary>
		/// Quando o pedido foi ENVIADO ao comprador 
		/// </summary>
		[Description("ENVIADO")]
		PEDIDO_ENVIADO= 2,

		/// <summary>
		/// Quando o usuário de suprimentos acessou o pedido.
		/// </summary>
		[Description("EM ANDAMENTO")]
		PEDIDO_EMANDAMENTO = 3,

		/// <summary>
		/// Quando o pedido foi aceito, negociado, aprovado e está aguardando entrega.
		/// </summary>
		[Description("FINALIZADO")]
		PEDIDO_FINALIZADO = 4,

		/// <summary>
		/// Quando o pedido é cancelado pelo demandante ou gestor.
		/// </summary>
		[Description("CANCELADO")]
		PEDIDO_CANCELADO = 5,

		/// <summary>
		/// Quando o pedido é devolvido pelo solicitante ou comprador.
		/// </summary>
		[Description("DEVOLVIDO")]
		PEDIDO_DEVOLVIDO = 6,


		/// <summary>
		/// Quando o pedido é aprovado pelo comprador.
		/// </summary>
		[Description("APROVADO")]
		PEDIDO_APROVADO = 7
	}
}
