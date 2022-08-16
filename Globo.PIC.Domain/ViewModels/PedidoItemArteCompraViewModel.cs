
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemArteCompraViewModel
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
        [Description("Id do Pedido Item")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item Arte")]
        public long IdPedidoItemArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login")]
        public string Login { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data do lançamento da compra")]
        public DateTime? DataCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(1, long.MaxValue)]
        [Description("Quantidade comprada")]
        public long Quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor Compra")]
        public decimal ValorCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observacoes")]
        public string Observacoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Número do Documento")]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemCompraDocumentosViewModel")]
        public List<PedidoItemArteCompraDocumentosViewModel> Documentos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Existe entregas")]
        public bool Entregas { get; set; }
        //public List<PedidoItemEntregaViewModel> Entregas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("User")]
        public UsuarioViewModel Usuario { get; set; }

    }
}
