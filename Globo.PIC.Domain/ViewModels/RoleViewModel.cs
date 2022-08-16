using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globo.PIC.Domain.Entities;
namespace Globo.PIC.Domain.ViewModels
{
    
    public class RoleViewModel
	{
        /// <summary>
        /// 
        /// </summary>
        [Description("Descrição da Role")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Nome da Role")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo da Role")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RoleViewModel() { }
    }
}
