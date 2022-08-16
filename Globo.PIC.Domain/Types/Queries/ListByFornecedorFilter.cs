using MediatR;
using System.ComponentModel; 
using System.Collections.Generic;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Domain.Types.Queries
{

    /// <summary>
    /// 
    /// </summary>
    public class ListByFornecedorFilter :
        IRequest<List<Supplier>>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public FornecedorFilterViewModel Filter { get; set; }

    }
}
