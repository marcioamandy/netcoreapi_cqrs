using System;
using System.Collections.Generic;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class GenericPaggingViewModel <T>
	{
		/// <summary>
		/// Resultado generico paginado.
		/// </summary>
		public List<T> Result { get; set; }

		/// <summary>
		/// Propriedade que carrega a paginação.
		/// </summary>
		public PaginationViewModel Pagination { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public GenericPaggingViewModel(IEnumerable<T> result, BaseFilterViewModel baseFilter, int count)
		{
			PaggingViewModel(result.ToList(), baseFilter, count);
		}

		public GenericPaggingViewModel(List<T> result, BaseFilterViewModel baseFilter, int count)
		{
			PaggingViewModel(result, baseFilter, count);
		}
		/// <summary>
		/// 
		/// </summary>
		private void PaggingViewModel(List<T> result, BaseFilterViewModel baseFilter, int count)
        {
			Result = result ?? new List<T>();
			Pagination = new PaginationViewModel();

			var total = (int)Math.Ceiling(count / (double)baseFilter.PerPage);

			int.TryParse(baseFilter.Page.ToString(), out int current);
			current = current <= 0 ? 1 : current;

			if (current > 1)
				Pagination.Previous = (current - 1);

			if (current < total)
				Pagination.Next = (current + 1);

			Pagination.Total = total;
			Pagination.Current = current;
			Pagination.Count = count;
			Pagination.PerPage = baseFilter.PerPage;
		}
    }
}
