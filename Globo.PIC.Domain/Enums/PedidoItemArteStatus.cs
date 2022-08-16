using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum PedidoItemArteStatus
	{
		/// <summary>
		/// Quando o item do pedido teve o cadastro finalizado e está aguardando o vinculo com um comprador.
		/// </summary>
		[Description("SOLICITACAO DE COMPRA CRIADA")]
		ITEM_SOLICITACAODECOMPRACRIADA = 1,
		 
		/// <summary>
		/// Quando o item do pedido de compra foi aprovado.
		/// </summary>
		[Description("APROVADO")]
		ITEM_APROVADO = 2,
		
		/// <summary>
		/// Quando o item do pedido de compra foi reprovado.
		/// </summary>
		[Description("REPROVADO")]
		ITEM_REPROVADO = 3,

		/// <summary>
		/// Quando o item do pedido foi cancelado
		/// </summary>
		[Description("CANCELADO")]
		ITEM_CANCELADO = 4,

		/// <summary>
		/// 
		/// </summary>
		//[Description("PENDENTE DE ANALISE")]
		//ITEM_PENDENTEDEANALISE = 5,

		///// <summary>
		///// 
		///// </summary>
		//[Description("EM ANALISE")]
		//ITEM_EMANALISE = 6,

		/// <summary>
		/// 
		/// </summary>
		[Description("DEVOLUCAO")]
		ITEM_DEVOLUCAO = 7,

		/// <summary>
		/// 
		/// </summary>
		[Description("ATRIBUIDO AO COMPRADOR")]
		ITEM_ATRIBUIDOAOCOMPRADOR = 8,

		/// <summary>
		/// 
		/// </summary>
		[Description("SOLICITACAO DE COMPRA ENVIADA")]
		ITEM_SOLICITACAODECOMPRAENVIADA = 9,

		/// <summary>
		/// 
		/// </summary>
		[Description("CANCELAMENTO SOLICITADO")]
		ITEM_CANCELAMENTOSOLICITADO = 10,

		/// <summary>
		/// 
		/// </summary>
		[Description("CANCELAMENTO NEGADO")]
		ITEM_CANCELAMENTONEGADO = 11,

		/// <summary>
		/// 
		/// </summary>
		[Description("ENTREGUE PARCIALMENTE")]
		ITEM_ENTREGUEPARCIALMENTE = 12,

		/// <summary>
		/// 
		/// </summary>
		[Description("ENTREGUE")]
		ITEM_ENTREGUE = 13,

		/// <summary>
		/// 
		/// </summary>
		[Description("REENVIO")]
		ITEM_REENVIO = 14,

		/// <summary>
		/// 
		/// </summary>
		[Description("NEGOCIADO PELO COMPRADOR")]
		NEGOCIADO_COMPRADOR = 15
	}
}
