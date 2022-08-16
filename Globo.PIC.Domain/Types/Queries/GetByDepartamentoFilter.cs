using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.ViewModels;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByDepartamentoFilter :
        IRequest<List<Departamento>>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public DepartamentoFilterViewModel Filter { get; set; }
    }
}
