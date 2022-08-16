
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemConversaViewModel
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
		[Description("Descrição da Conversa")]
        public string DescricaoConversa { get; set; }

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
        [Description("Data Conversa")]
        public DateTime? DataConversa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemConversaAnexoViewModel")]
        public List<ArquivoViewModel> Arquivos { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Description("User")]
        public UsuarioViewModel Usuario { get; set; }

    }
}
