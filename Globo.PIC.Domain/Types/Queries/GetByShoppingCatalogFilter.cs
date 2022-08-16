using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;
using MediatR;

namespace Globo.PIC.Domain.Types.Queries
{

    /// <summary>
    /// 
    /// </summary>
    public class GetByShoppingCatalogFilter : IRequest<ResultShoppingCatalog>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public ShoppingCatalogFilterViewModel Filter { get; set; }

    }
}
