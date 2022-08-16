using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    public class AddTrackingVeiculo : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Tracking")]
        public PedidoItemVeiculoTracking PedidoItemVeiculoTracking { get; set; }
    }
}
