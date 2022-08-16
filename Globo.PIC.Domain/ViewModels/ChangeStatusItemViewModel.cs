using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class ChangeStatusItemViewModel
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
        [Description("Justificativa Devolucao do Pedido Item")]
        public string JustificativaDevolucao { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[Description("Id Tipo de Compra")]
        //public int IdTipo { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[Description("Observação Pedido item")]
        //public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ChangeStatusItemViewModel()
        {
        }

    }
}
