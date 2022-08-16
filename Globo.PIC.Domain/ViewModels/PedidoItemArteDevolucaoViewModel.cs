
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemArteDevolucaoViewModel
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
        [Description("Id do Pedido Item Arte")]
        public long IdPedidoItemArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item original")]
        public long IdPedidoItemOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(1, long.MaxValue)]
        [Description("Id tipo devolução do Pedido Item")]
        public long idTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do comprador")]
        public string Comprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data do lançamento da devolucao")]
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(1, long.MaxValue)]
        [Description("Quantidade devolvida")]
        public long Quantidade { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Justificativa da devolução")]
        public string Justificativa { get; set; }

    }
}
