
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{

	/// <summary>
    /// 
    /// </summary>
    public class ShoppingCatalogItemViewModel
    {

        /// <summary>
        ///
        /// </summary>
        [Description("Id único do Item no catálogo")]
        public long CatalogItemKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Código do acordo")]
        public string Acordo { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Id do acordo")]
        public long? AcordoId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Status do acordo")]
        public string AcordoStatus { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Id da linha do acordo")]
        public long? AcordoLinhaId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Status da linha do acordo")]
        public string AcordoLinhaStatus { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Nome do Fornecedor")]
        public string Fornecedor { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Id do fornecedor")]
        public long? FornecedorId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Sigla da moeda corrente do acordo")]
        public string Moeda { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Categoria do Item")]
        public string Categoria { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Id Categoria do Item")]
        public long CategoriaId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Valor combinado do Item no catálogo")]
        public decimal Valor { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Tipo de Item no catálogo")]
        public string TipoDocumento { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Url da Thumg do Item no catálogo")]
        public string ImagemUrl { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Id do Item")]
        public long? ItemId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Código do Item")]
        public string ItemCodigo { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Unidade de medida")]
        public string UOM { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Código da unidade de medida")]
        public string UOMCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Descrição do item no catálogo")]
        public string Descricao { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Descrição completa do item no catálogo")]
        public string DescricaoCompleta { get; set; }

    }
}
 