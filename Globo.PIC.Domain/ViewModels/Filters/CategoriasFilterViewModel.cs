using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class CategoriasFilterViewModel : BaseFilterViewModel
	{
		/// <summary>
		/// Id Categoria
		/// </summary>
		[Description("Id Categoria")]
		public long? IdCategoria { get; set; }

		/// <summary>
		/// Texto referente ao nome da categoria
		/// </summary>
		[Description("Texto referente ao nome da categoria")]
		public string Nome { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CategoriasFilterViewModel()
		{

		}
	}
}
