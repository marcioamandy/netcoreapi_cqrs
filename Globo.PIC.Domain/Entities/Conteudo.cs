using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Conteudo
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("Codigo")]
        public string Codigo{ get; set; }

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



        [Description("Pedidos")]

        #region Relationship one to many properties

        public virtual IEnumerable<Pedido> Pedidos { get; set; }

        public virtual IEnumerable<UsuarioConteudo> UsuariosConteudos { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Conteudo()
        {
        }
    }
}
