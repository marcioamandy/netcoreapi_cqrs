using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Globo.PIC.Domain.ViewModels.Filters
{
    public class RoleFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo da Role")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RoleFilterViewModel()
        {
        }
    }
}
