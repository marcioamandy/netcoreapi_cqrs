using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.ViewModels.Filters;
using MediatR;

namespace Globo.PIC.Domain.Types.Queries
{

    /// <summary>
    /// 
    /// </summary>
    public class ListByItemCatalogoFilter : IRequest<List<ItemCatalogo>>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public ItemCatalogoFilterViewModel Filter { get; set; }

    }
}
