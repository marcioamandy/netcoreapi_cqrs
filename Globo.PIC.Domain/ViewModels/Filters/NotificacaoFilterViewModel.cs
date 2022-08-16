using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
    public class NotificacaoFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca")]
        public string Search { get; set; }
    }
}
