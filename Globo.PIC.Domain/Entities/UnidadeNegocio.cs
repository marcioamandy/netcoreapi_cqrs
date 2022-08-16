using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class UnidadeNegocio
    {

        /// <summary>
        /// id da business unity
        /// </summary>
        [Key]
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// código da business unity
        /// </summary>
        [Description("código da business unity")]
        public string Codigo { get; set; }

        /// <summary>
        /// nome da business unity
        /// </summary>
        [Description("nome da da business unity")]
        public string Nome { get; set; }

        /// <summary>
        /// código da business unity
        /// </summary>
        [Description("código da inventory organization")]
        public string CodigoOrganizacaoInventario { get; set; }


        /// <summary>
        /// nome da business unity
        /// </summary>
        [Description("Unidade Federativa")]
        public string Uf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UnidadeNegocio()
        {
        }
         

        #region Relationship one to many properties

        public virtual IEnumerable<Usuario> Usuarios { get; set; }


        #endregion
    }

}
