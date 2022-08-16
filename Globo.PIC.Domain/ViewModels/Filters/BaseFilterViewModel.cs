using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
	public abstract class BaseFilterViewModel
	{

		private int page;

		/// <summary>
		/// 
		/// </summary>
		[Description("Página Atual")]
		public int Page
		{
			set
			{
				page = value;
			}
			get
			{
				page = page > 0 ? page : 1;
				return page;
			}
		}

		private int perPage;

		/// <summary>
		/// 
		/// </summary>
		[Description("Itens por Página")]
		public int PerPage
		{
			set
			{
				perPage = value;
			}
			get
			{
				int.TryParse(System.Environment.GetEnvironmentVariable("PAGGING_PERPAGE"), out int _perPage);
				perPage = perPage > 0 ? perPage : (_perPage > 0 ? _perPage : 20);
				return perPage;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public BaseFilterViewModel()
		{			
		}
	}
}
