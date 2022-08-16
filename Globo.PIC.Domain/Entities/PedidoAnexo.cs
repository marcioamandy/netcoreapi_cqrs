using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoAnexo
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
        [Description("Id do Pedido")]
        public long IdPedido { get; set; }

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
        //[Required]
        [Description("Tipo")]
        public string Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da gravação do anexo")]
        public DateTime? DataPedidoAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoAnexo()
        {
        }

        #region Relationship one to many properties


        #endregion

        #region Relationship many to one properties

        public virtual Pedido Pedido { get; set; }

        #endregion
    }

}
