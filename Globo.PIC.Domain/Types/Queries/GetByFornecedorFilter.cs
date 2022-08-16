using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;
namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByFornecedorFilter :
        IRequest<List<Supplier>>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public FornecedorFilterViewModel Filter { get; set; }
    }
}
