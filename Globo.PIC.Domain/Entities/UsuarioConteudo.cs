using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{

    /// <summary>
    /// 
    /// </summary>
    public class UsuarioConteudo
    {

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Login do Usuário")]
        public string Login { get; set; }

        /// <summary>
		/// 
		/// </summary>
        [Required]
        [Description("IdConteudo")]
        public long IdConteudo { get; set; }


        #region Relationship many to one properties

        public virtual Usuario Usuario { get; set; }

        public virtual Conteudo Conteudo { get; set; }

        #endregion
    }
}
