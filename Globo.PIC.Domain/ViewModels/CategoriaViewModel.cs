using Globo.PIC.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
	public class CategoriaViewModel 
	{

        /// <summary>
        /// 
        /// </summary>
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id Categoria Pai")]
        public long? IdCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome da Categoria")]
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CategoriaViewModel()
		{
		}
	}
}
