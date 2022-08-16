using System.ComponentModel;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Commands
{
    public class UpdateRCLineStatus : DomainCommand
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("StatusLineOCRC")]
        public StatusLineOCRC StatusLineOCRC { get; set; }
    }
}
