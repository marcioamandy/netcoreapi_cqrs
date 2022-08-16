using MediatR;
using System.ComponentModel;
using System.Threading;
using Globo.PIC.Domain.Models;
using System.Collections.Generic;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetAgrementsByCNPJ :
        IRequest<List<Agreements>>
    {
		/// <summary>
		/// 
		/// </summary>
		[Description("cnpj do fornecedor")]
		public long Cnpj { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Cancellation Token")]
		public CancellationToken CancellationToken { get; set; }
	}
}
