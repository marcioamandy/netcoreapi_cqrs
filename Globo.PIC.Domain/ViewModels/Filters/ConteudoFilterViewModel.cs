using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
    public class ConteudoFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca")]
        public string Search { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Flag que limita a busca a trazer apenas conteúdos sigilosos")]
		public bool ApenasSigilosos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ConteudoFilterViewModel()
		{
		}
	}
}
