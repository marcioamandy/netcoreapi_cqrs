using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels.Filters
{
	public class ItemCatalogoFilterViewModel : BaseFilterViewModel
	{
		/// <summary>
		/// Filtro de Data 'Dê', range de dadas início/fim
		/// </summary>
		[Description("Filtro de Data Inicial, range de datas início/fim")]
		public DateTime? DataInicio { get; set; }

		/// <summary>
		/// Filtro de Data 'Até', range de dadas início/fim
		/// </summary>
		[Description("Filtro de Data Final, range de datas início/fim")]
		public DateTime? DataFim { get; set; }

		/// <summary>
		/// Lista de Conteudos
		/// </summary>
		[Description("Lista de Conteudos")]
		public List<long> Conteudos { get; set; }

		/// <summary>
		/// Lista de Conteudos
		/// </summary>
		[Description("Bloqueado para empréstimo")]
		public bool BloqueadoEmprestimo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Nome do Item")]
		public string NomeItem { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Descricao do Item")]
		public string Descricao { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ItemCatalogoFilterViewModel()
		{

		}
	}
}
