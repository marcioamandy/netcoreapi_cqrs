using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels.Filters
{
    public class NomeCompletoFilterViewModel
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Nome Completo do Talento")]
        public string NomeCompleto { get; set; }
    }
}
