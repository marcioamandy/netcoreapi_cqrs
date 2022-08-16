using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Item
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
        [Range(0, long.MaxValue)]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da SubCategoria")]
        [Range(0, long.MaxValue)]
        public long IdSubCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor do item")]
        public decimal Valor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Fornecedor")]
        [MaxLength(200)]
        public string Fornecedor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("CNPJ do Fornecedor")]
        [MaxLength(14)]
        public string CodigoFornecedor { get; set; }

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
        [Description("Nro Acordo")]
        [MaxLength(100)]
        public string NroAcordo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Código do item")]
        [MaxLength(100)]
        public string CodItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Linha Acordo")]
        public long? LinhaAcordo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observações do Item")]
        [MaxLength(500)]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Acordo Jurídico")]
        [MaxLength(100)]
        public string AcordoJuridico { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo de Negociação")]
        [Range(1, long.MaxValue)]
        public long IdTipoNegociacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Item()
        {
        }

        #region Relationship one to many properties

        public virtual IEnumerable<ItemAnexo> Arquivos { get; set; }

        public virtual IEnumerable<ItemCatalogo> ItemCatalogos { get; set; }

        public virtual IEnumerable<PedidoItem> PedidoItens { get; set; }

        #endregion

        #region Relationship one to one properties

        public virtual ItemVeiculo ItemVeiculo { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual Categoria Tipo { get; set; }

        public virtual Categoria SubCategoria { get; set; }

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
