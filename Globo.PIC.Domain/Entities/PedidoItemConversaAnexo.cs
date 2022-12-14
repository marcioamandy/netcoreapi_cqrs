using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemConversaAnexo
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
        [Description("Id do Pedido Item Conversa")]
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
        //[Required]
        [Description("Tipo")]
        public string Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da gravação do anexo")]
        public DateTime? DataPedidoItemConversa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemConversaAnexo()
        {
        }

        #region Relationship one to many properties

        //public virtual IEnumerable<PedidoItem> PedidosItens { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual PedidoItemConversa PedidoItemConversa { get; set; }

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
