using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels.Filters
{
    public class NomeArtisticoFilterViewModel : BaseFilterViewModel
    {
		/// <summary>
		/// 
		/// </summary>
		[Description("Nome Artistico do Talento")]
		public string NomeArtistico { get; set; }

	}
}
