using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
	public class UsuarioConteudoViewModel
	{		  
		/// <summary>
		/// 
		/// </summary>
		public UsuarioConteudoViewModel()
		{
		}

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Usuário")]
        public string Login { get; set; }

        /// <summary>
		/// 
		/// </summary>
        [Required]
        [Description("IdConteudo")]
        public long IdConteudo { get; set; }

        /// <summary>
		/// 
		/// </summary>        
        [Description("NomeConteudo")]
        public string NomeConteudo { get
            {
                return Conteudo?.Nome;
            }
        }

        #region Relationship many to one properties

        public virtual UsuarioViewModel Usuario { get; set; }

        public virtual ConteudoViewModel Conteudo { get; set; }

        #endregion
    }
}
