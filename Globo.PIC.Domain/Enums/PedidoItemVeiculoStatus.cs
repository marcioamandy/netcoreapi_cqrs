using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum PedidoItemVeiculoStatus
	{
		/// <summary>
		/// Quando o pedido item foi criado mas não foi encaminhado ao comprador.
		/// </summary>
		[Description("RASCUNHO")]
		PEDIDOITEM_RASCUNHO = 1,

		/// <summary>
		/// Quando o pedido item foi ENVIADO ao comprador 
		/// </summary>
		[Description("ENVIADO")]
		PEDIDOITEM_ENVIADO= 2,

		/// <summary>
		/// Quando o comprador envia as opções para o demandante aprovar.
		/// </summary>
		[Description("EM ANÁLISE")]
		PEDIDOITEM_EMANALISE = 3,

		/// <summary>
		/// Quando o demandante aprova o item.
		/// </summary>
		[Description("OPÇÂO APROVADA")]
		PEDIDOITEM_OPCAOAPROVADA = 4,

		/// <summary>
		/// Qunado o acionamento do item é solicitado.
		/// </summary>
		[Description("ACIONAMENTO SOLICITADO")]
		PEDIDOITEM_ACIONAMENTOSOLICITADO = 5,

		/// <summary>
		/// Quando o item é acionado.
		/// </summary>
		[Description("ACIONADO")]
		PEDIDOITEM_ACIONADO = 6,


		/// <summary>
		/// Quando a RC foi aprovada.
		/// </summary>
		[Description("RC APROVADA")]
		PEDIDOITEM_RCAPROVADA = 7,

		/// <summary>
		/// Quando o item de empréstimo expira.
		/// </summary>
		[Description("SOLICITAÇÃO DE EMPRÉSTIMO EXPRIRADA")]
		PEDIDOITEM_SOLICITACAOEMPRESTIMOEXPIRADA = 8,

		/// <summary>
		/// Quando o item do pedido de compra foi aprovado.
		/// </summary>
		[Description("APROVADO")]
		PEDIDOITEM_APROVADO = 9,

		/// <summary>
		/// Quando o item do pedido foi cancelado
		/// </summary>
		[Description("CANCELADO")]
		PEDIDOITEM_CANCELADO = 10,

		/// <summary>
		/// 
		/// </summary>
		[Description("DEVOLUCAO")]
		PEDIDOITEM_DEVOLUCAO = 11,

		/// <summary>
		/// 
		/// </summary>
		[Description("REENVIO")]
		PEDIDOITEM_REENVIO = 12,
	}
}
