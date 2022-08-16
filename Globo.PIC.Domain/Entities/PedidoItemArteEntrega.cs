using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemArteEntrega
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
        [Description("Id do Pedido Item Arte")]
        public long IdPedidoItemArte { get; set; }

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
        [Description("Data do lançamento da entrega")]
        public DateTime? DataEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(1, long.MaxValue)]
        [Description("Quantidade entregue")]
        public long Quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Local de Entrega")]
        [MaxLength(200)]
        public string LocalEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Recebedor(a) Entrega")]
        [MaxLength(200)]
        public string Recebedor { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Observação Entrega")]
        public string Observacao { get; set; }

 

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemArteEntrega()
        {
        }

        #region Relationship one to many properties

        #endregion

        #region Relationship many to one properties

        public virtual PedidoItemArte PedidoItemArte { get; set; }

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
