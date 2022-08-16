using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoServiceViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("Id do pedido")]
        public long id_pedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Solicitante")]
        public string nm_login_solicitante_pedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status do Pedido")]
        public string id_status_pedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Pedido ativo")]
        public bool st_ativo { get; set; }
    }
}
