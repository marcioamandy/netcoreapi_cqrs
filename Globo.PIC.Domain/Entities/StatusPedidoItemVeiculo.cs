using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class StatusPedidoItemVeiculo
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
        [Description("Nome do Status")]
        public string Nome { get; set; }


        #region Relationship one to many properties
        //public virtual IEnumerable<Agendamento> Agendamentos { get; set; }

        public virtual IEnumerable<PedidoItemVeiculo> Status { get; set; }
        
        public virtual IEnumerable<PedidoItemVeiculoTracking> Tracking { get; set; }
        
        #endregion

        #region Relationship many to one properties

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public StatusPedidoItemVeiculo()
        {
        }
    }
}
