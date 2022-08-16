using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class DepartamentoViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("Id")]
        public long Id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Departamento")]
        public string Nome { get; set; }
   }

}
