using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Types.Queries
{
	/// <summary>
	/// 
	/// </summary>
	public class ListSubCategoriaIdByIdTipoVeiculo :
		IRequest<List<Categoria>>
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("id tipo para pesquisa de subCategorias")]
		public long IdTipoVeiculo { get; set; }
	}
}
