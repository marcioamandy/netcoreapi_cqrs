using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels
{
    public class ProjetosFilterViewModel : BaseFilterViewModel
    { 

        /// <summary>
        /// 
        /// </summary>
        [Description("Campo de busca para conteúdo")]
        public string Name { get; set; }
    }
}
