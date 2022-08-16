using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.ViewModels.Filters;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByPedidoFilterCount :
        IRequest<int>
    {
        
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public PedidoFilterViewModel Filter { get; set; }

    }
}
