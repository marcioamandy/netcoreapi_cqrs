using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeletePedidoEquipe : DomainCommand
    {
                
        /// <summary>
        /// id do pedido
        /// </summary>
        public long IdPedido { get; set; }

        /// <summary>
        /// Login do usuário
        /// </summary>
        public string Login { get; set; }

    }
}
