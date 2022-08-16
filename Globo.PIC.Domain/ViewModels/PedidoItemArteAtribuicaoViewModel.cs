
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemArteAtribuicaoViewModel
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
        [Range(0, long.MaxValue)]
        public long IdPedidoItemArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(1, long.MaxValue)]
        [Description("Id tipo compra do Pedido Arte Item (Estruturada / Externa)")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do novo comprador")]
        public string Comprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do antigo comprador")]
        public string CompradorAnterior { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da atribuição do comprador")]
        public DateTime? DataAtribuicao { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Justificativa da atribuição")]
        public string Justificativa { get; set; }

    }
}
