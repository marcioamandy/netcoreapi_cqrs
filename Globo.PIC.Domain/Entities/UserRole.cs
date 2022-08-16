using Globo.PIC.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Globo.PIC.Domain.Entities
{
    public class UserRole
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
        [Description("Role")]
        public string Name { get; set; }


        #region Relationship many to one properties

        public virtual Usuario User { get; set; }

        #endregion
    }
}
