using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemArteCompra
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
        [Description("Data do lançamento da compra")]
        public DateTime? DataCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(1, long.MaxValue)]
        [Description("Quantidade comprada")]
        public long Quantidade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor Compra")]
        public decimal ValorCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observações")]
        public string Observacoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Numero do Documento")]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemArteCompra()
        {
        }

        #region Relationship one to many properties

        public virtual IEnumerable<PedidoItemArteCompraDocumento> Documentos { get; set; }

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
