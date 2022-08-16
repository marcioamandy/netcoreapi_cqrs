using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeleteTrackingVeiculo : DomainCommand
    {
        /// <summary>
        /// entidade UserRole
        /// </summary>
        [Description("Tracking")]
        public PedidoItemVeiculoTracking TrackingVeiculo { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
