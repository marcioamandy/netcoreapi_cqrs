using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class ConteudoServiceViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("Codigo")]
        public long codigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do conteúdo")]
        public string nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status")]
        public string status { get; set; }
    }
}
