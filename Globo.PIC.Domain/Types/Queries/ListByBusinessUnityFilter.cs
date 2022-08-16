using MediatR;
using System.ComponentModel; 
using System.Collections.Generic;
using Globo.PIC.Domain.ViewModels.Filters; 
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{

    /// <summary>
    /// 
    /// </summary>
    public class ListByBusinessUnityFilter :
        IRequest<List<UnidadeNegocio>>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public UnidadeNegocioFilterViewModel Filter { get; set; }

    }
}
