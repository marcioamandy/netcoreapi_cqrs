using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class StatusPedidoArte
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
        [Description("Nome do Status do Pedido")]
        public string Nome { get; set; }

        #region Relationship one to many properties

        public virtual IEnumerable<PedidoArte> PedidosArte { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>  
        public StatusPedidoArte()
        {
        }
    }
}
