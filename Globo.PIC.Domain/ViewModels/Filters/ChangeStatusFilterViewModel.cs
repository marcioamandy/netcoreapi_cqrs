using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class ChangeStatusFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Status")]
        public long StatusId { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public ChangeStatusFilterViewModel()
        {

        }
    }
}
