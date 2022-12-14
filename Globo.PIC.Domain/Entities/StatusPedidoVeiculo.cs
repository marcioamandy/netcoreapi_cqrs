using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{   
    public class StatusPedidoVeiculo
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

        public virtual IEnumerable<PedidoVeiculo> PedidosVeiculo { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>  
        public StatusPedidoVeiculo()
        {
        }
    }
}
