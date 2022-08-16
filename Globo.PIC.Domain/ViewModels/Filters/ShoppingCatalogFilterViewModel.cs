using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
    public class ShoppingCatalogFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca")]
        public string Search { get; set; }
    }
}
