using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.Threading;

namespace Globo.PIC.Domain.Types.Commands
{
    public class SaveViewer : DomainCommand
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Viewer")]
        public Viewer Viewer { get; set; }
    }
}
