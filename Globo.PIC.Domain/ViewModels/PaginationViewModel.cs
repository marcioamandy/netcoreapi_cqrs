using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels
{
    public class PaginationViewModel
	{

        /// <summary>
        /// 
        /// </summary>
        [Description("Pagina Atual")]
        public long Current { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tamanho de Página")]
        public long PerPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Total")]
        public long Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Total de Páginas")]
        public long Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Página Anterior")]
        public long? Previous { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Página Posterior")]
        public long? Next { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PaginationViewModel()
        {
        }
    }
}
