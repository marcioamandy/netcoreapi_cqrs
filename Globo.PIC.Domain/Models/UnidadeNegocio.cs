using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class UnidadeNegocio
    {
        public string Id{ get; set; }
        public string Description { get; set; }


        #region Relationship one to many properties

        public virtual IEnumerable<Usuario> Usuarios { get; set; }


        #endregion

        #region Relationship many to one properties

        #endregion
    }
}
