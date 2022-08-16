using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class ChangeStatusItemVeiculoViewModel
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
        [Description("Justifictiva do Cancelamento")]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justifictiva do Devolução")]
        public string JustificativaDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ChangeStatusItemVeiculoViewModel()
        {
        }

    }
}
