using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels
{
    public class BaseViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Paginação")]
        public PaginationViewModel Pagination{ get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public BaseViewModel()
        {
        }
    }
}
