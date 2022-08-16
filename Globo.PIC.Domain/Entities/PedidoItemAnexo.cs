using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemAnexo
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
        [Description("Id do Pedido Item")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Arquivo")]
        public string NomeArquivo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Description("Nome Original")]
        public string NomeOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo do Arquivo")]
        public string Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data do Arquivo")]
        public DateTime? DataPedidoItemAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemAnexo()
        {
        }

        #region Relationship one to many properties


        #endregion

        #region Relationship many to one properties


        public virtual PedidoItem PedidoItem { get; set; }

        #endregion
    }
}
