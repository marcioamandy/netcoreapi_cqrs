using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels
{
    public class ConteudosFilterViewModel : BaseFilterViewModel
    { 

        /// <summary>
        /// 
        /// </summary>
        [Description("Campo de busca projetos")]
        public string Name { get; set; }
    }
}
