
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemArteCompraDocumentosViewModel
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
        [Description("Id do Pedido Item Compra")]
        public long IdCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login de registro do documento")]
        public string Login { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data do lançamento do documento")]
        public DateTime? DataDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(1, long.MaxValue)]
        [Description("Quantidade documento")]
        public long Quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor do item do documento")]
        public decimal ValorCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Fornecedor")]
        [MaxLength(200)]
        public string Fornecedor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observação do Pedido Item documento")]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemCompraDocumentosAnexoViewModel")]
        public List<ArquivoViewModel> Arquivos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("User")]
        public UsuarioViewModel User { get; set; }

    }
}
