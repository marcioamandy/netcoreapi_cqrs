using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Globo.PIC.Domain.ViewModels
{
    public class SortNivelViewModel
    {  
        [Description("Nivel da Task")]
        public long TaskLevel { get; set; }

         
        [Description("Níveis")]
        public List<TarefaViewModel> Niveis { get; set; }

    }
}



