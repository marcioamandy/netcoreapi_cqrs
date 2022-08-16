using MediatR;
using System.Collections.Generic;
using System.ComponentModel; 
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.ViewModels;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByUserFilter :
        IRequest<List<Usuario>>
    {

        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public UserFilterViewModel Filter { get; set; }

    }
}

