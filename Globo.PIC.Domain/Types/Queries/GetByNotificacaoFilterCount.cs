using MediatR;
using System.ComponentModel; 
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByNotificacaoFilterCount :
        IRequest<int>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public NotificacaoFilterViewModel Filter { get; set; }

    }
}
