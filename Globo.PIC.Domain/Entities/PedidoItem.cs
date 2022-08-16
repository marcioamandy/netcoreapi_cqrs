using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItem
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
        [Range(1, long.MaxValue)]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Número Sequencial do Item Pedido")]
        [MaxLength(20)]
        public string Numero { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item Pai Master de um Item do Acessorio")]
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
        [DefaultValue(0)]
        [Range(1, long.MaxValue)]
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
        public PedidoItem()
        {
        }

        #region Relationship one to many properties

        public virtual IEnumerable<RC> RCs { get; set; }

        public virtual IEnumerable<PedidoItemAnexo> Arquivos { get; set; }

        public virtual IEnumerable<PedidoItemConversa> PedidoItemConversas { get; set; }

        public virtual IEnumerable<PedidoItem> PedidoItensFilhos { get; set; }

        public virtual IEnumerable<AcionamentoItem> AcionamentoItens { get; set; }

        #endregion

        #region Relationship with one properties

        public virtual PedidoItemArte PedidoItemArte { get; set; }

        public virtual PedidoItemVeiculo PedidoItemVeiculo { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual Pedido Pedido { get; set; }

        public virtual PedidoItem PedidoItemPai { get; set; }

        public virtual Item Item { get; set; }

        public virtual Usuario CanceladoPor { get; set; }

        public virtual Usuario DevolvidoPor { get; set; }

        public virtual Usuario AprovadoPor { get; set; }
        
        public virtual Usuario ReprovadoPor { get; set; }

        #endregion
    }
}
