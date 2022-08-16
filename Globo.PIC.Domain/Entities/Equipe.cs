using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Equipe
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(1, long.MaxValue)]
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
        [Description("Login")]
        public string Login { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Equipe()
        {
        }

        #region Relationship one to many properties

        //public virtual IEnumerable<PedidoItem> PedidosItens { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual Pedido Pedido { get; set; }

        public virtual Usuario Usuario { get; set; }

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
