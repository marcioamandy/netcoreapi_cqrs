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
    public class ListByCategoriaFilter:
        IRequest<List<Categoria>>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public CategoriasFilterViewModel Filter { get; set; }

    }
}
