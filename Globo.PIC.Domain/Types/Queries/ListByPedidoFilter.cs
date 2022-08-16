using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;
using Globo.PIC.Domain.ViewModels.Filters;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{

    /// <summary>
    /// 
    /// </summary>
    public class ListByPedidoFilter:
        IRequest<List<Pedido>>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public PedidoFilterViewModel Filter { get; set; }

    }
}
