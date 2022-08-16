using MediatR;
using System.Collections.Generic;
using System.ComponentModel; 
using Globo.PIC.Domain.Entities; 
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByNotificacaoFilter :
        IRequest<List<Notificacao>>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public NotificacaoFilterViewModel Filter { get; set; }

    }
}
