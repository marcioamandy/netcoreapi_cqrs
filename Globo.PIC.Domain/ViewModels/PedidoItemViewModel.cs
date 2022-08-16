using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido")]
        [Range(0, long.MaxValue)]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item Master")]
        [Range(1, long.MaxValue)]
        public long? IdPedidoItemPai { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Item")]
        [Range(1, long.MaxValue)]
        public long? IdItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Número Sequencial do Item Pedido")]
        [MaxLength(20)]
        public string Numero { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens")]
        public long Quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor total dos itens")]
        public decimal ValorItens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Valor do item")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor Unitário")]
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Necessidade")]
        public DateTime? DataNecessidade { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de Entrega")]
        [MaxLength(100)]
        public string LocalEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Entrega")]
        public DateTime? DataEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Item")]
        [MaxLength(200)]
        public string NomeItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descricao do Item")]
        [MaxLength(500)]
        public string Descricao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Unidade de medida para o Item")]
        [MaxLength(20)]
        public string UnidadeMedida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa do Item")]
        [MaxLength(500)]
        public string Justificativa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Cancelamento do Item")]
        [MaxLength(500)]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Devolucao do Item")]
        [MaxLength(500)]
        public string JustificativaDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Cancelamento")]
        public string CanceladoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data do cancelamento do item")]
        public DateTime? DataCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login da Devolução")]
        public string DevolvidoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da devolução do item")]
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login da Aprovação")]
        public string AprovadoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da aprovação do item")]
        public DateTime? DataAprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login de Reprovação")]
        public string ReprovadoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da reprovação do item")]
        public DateTime? DataReprovacao { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Observação do Pedido Item")]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Existe devolucoes")]
        public bool hasDevolucoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista RCs")]
        public List<RCViewModel> RCs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemAnexosViewModel")]
        public List<ArquivoViewModel> Arquivos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemConversas")]
        public List<PedidoItemConversaViewModel> PedidoItemConversas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItem")]
        public List<PedidoItemViewModel> PedidoItensFilhos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista Acionamento Itens")]
        public List<AcionamentoItemViewModel> AcionamentoItens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Item vinculado ao Pedido Item")]
        public ItemViewModel Item { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UserViewModel")]
        public UsuarioViewModel CanceladoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UserViewModel")]
        public UsuarioViewModel DevolvidoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UserViewModel")]
        public UsuarioViewModel AprovadoPor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UserViewModel")]
        public UsuarioViewModel ReprovadoPor { get; set; }
    }
}
