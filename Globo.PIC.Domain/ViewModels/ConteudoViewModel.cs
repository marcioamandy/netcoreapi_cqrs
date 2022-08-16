using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class ConteudoViewModel
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
        [Description("Codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Conteúdo")]
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status do Conteúdo")]
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sigilo ativo")]
        public bool Sigiloso { get; set; }


    }

}
