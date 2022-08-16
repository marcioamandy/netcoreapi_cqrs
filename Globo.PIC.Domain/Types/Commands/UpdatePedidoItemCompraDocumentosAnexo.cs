﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdatePedidoItemCompraDocumentosAnexo : DomainCommand
    {
        /// <summary>
        /// entidade pedido item
        /// </summary>
        [Description("PedidoItemCompraAnexo")]
        public PedidoItemArteCompraDocumentoAnexo PedidoItemCompraAnexo { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
