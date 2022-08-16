using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.ViewModels.Filters;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByPedidoItemConversaFilterCount :
        IRequest<int>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public PedidoItemConversaFilterViewModel Filter { get; set; }
    }
}
