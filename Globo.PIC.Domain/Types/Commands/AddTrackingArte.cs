using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    public class AddTrackingArte : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Tracking")]
        public PedidoItemArteTracking PedidoItemArteTracking { get; set; }
    }
}
