using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeleteTrackingArte : DomainCommand
    {
        /// <summary>
        /// entidade UserRole
        /// </summary>
        [Description("Tracking")]
        public PedidoItemArteTracking PedidoItemArteTracking { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
