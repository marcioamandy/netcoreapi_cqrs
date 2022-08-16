using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoEquipeViewModel
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
        [Required]
        [Description("Id do Pedido")]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Login")]
        public string Login { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        [Description("UserViewModel")]
        public UsuarioViewModel Usuario { get; set; }
    }
}
