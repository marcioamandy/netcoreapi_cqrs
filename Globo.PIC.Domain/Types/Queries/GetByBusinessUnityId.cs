using MediatR;
using System.ComponentModel;
using System.Threading; 
using System.Collections.Generic;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{
	public class GetByBusinessUnityId : 
		IRequest<UnidadeNegocio>,
		IRequest<List<OrganizationStructure>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("BusinessUnityId")]
		public long Id { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		[Description("Cancellation Token")]
		public CancellationToken CancellationToken { get; set; }
	}
}
