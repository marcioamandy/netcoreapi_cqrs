using MediatR;
using System.ComponentModel;
using System.IO;
using Globo.PIC.Domain.Entities;
using System.Threading;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class GetByIdCategoriaVeiculos :
		IRequest<Categoria>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id Categoria de Veículos para pesquisa")]
		public long IdCategoria { get; set; }
	}
}
