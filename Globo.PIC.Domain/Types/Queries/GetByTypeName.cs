using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Models;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{
	public class GetByTypeName : 
		IRequest<List<Expenditures>>
    {
		/// <summary>
		/// 
		/// </summary>
		[Description("TypeName")]
		public string TypeName { get; set; }
	}
}
