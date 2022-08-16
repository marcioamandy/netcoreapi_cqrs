using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;
using Globo.PIC.Domain.ViewModels.Filters;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{
    public class ListByAcionamentoFilter :
        IRequest<List<Acionamento>>
    {

        /// <summary>
		/// 
		/// </summary>
		public long id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Filter")]
        public AcionamentoFilterViewModel Filter { get; set; }
    }
}
