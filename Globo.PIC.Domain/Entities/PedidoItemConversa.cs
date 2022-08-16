using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemConversa
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
		[Description("Descrição da Conversa")]
        public string DescricaoConversa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login")]
        public string Login { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Conversa")]
        public DateTime? DataConversa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id Pedido Item Conversa Origem")]
        [Range(0, long.MaxValue)]
        public long? IdPICPai { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemConversa()
        {
        }

        #region Relationship one to many properties

        //public virtual IEnumerable<PedidoItemConversa> PedidoItemConversaPc { get; set; }

        public virtual IEnumerable<PedidoItemConversaAnexo> Arquivos { get; set; }

        //public virtual IEnumerable<PedidoItem> PedidosItens { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual PedidoItem PedidoItem { get; set; }

        public virtual Usuario Usuario { get; set; }

        //public virtual PedidoItemConversa PedidoItemConversaPai { get; set; }

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
