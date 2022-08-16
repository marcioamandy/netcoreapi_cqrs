using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;
using Globo.PIC.Domain.ViewModels.Filters;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{
    public class ListByAcionamentoItemFilter :
        IRequest<List<AcionamentoItem>>
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public AcionamentoItemFilterViewModel Filter { get; set; }
    }
}
