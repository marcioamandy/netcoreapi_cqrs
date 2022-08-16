using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.ViewModels.Filters;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByPedidoItemConversaFilter : 
        IRequest<List<PedidoItemConversa>>
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public PedidoFilterViewModel Filter { get; set; }

    }
}
