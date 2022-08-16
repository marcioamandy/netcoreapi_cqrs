using Globo.PIC.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
	public class SubCategoriaViewModel 
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
        [Description("Nome da SubCategoria")]
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Categoria Pai")]
        public CategoriaViewModel CategoriaPai { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Categorias Filhas")]
        public List<CategoriaViewModel> Categorias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SubCategoriaViewModel()
		{
		}
	}
}
