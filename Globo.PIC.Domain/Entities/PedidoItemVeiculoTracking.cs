using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemVeiculoTracking
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
        [Range(0, long.MaxValue)]
        [Description("Id do Pedido Veículos Item")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Data Tracking")]
        public DateTime? TrackingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(1, long.MaxValue)]
        [Description("Ordem do Tracking")]
        public long StatusPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(1, long.MaxValue)]
        [Description("Id do Status Pedido Item")]
        public long StatusId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status do tracking")]
        public bool Ativo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Alterado Por")]
        public string ChangeById { get; set; }


        #region Relationship belongs to

        /// <summary>
        /// 
        /// </summary>
        public virtual StatusPedidoItemVeiculo Status { get; set; }


        public virtual PedidoItemVeiculo PedidoItemVeiculo { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual Usuario ChangedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public virtual Medicao Measurement { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemVeiculoTracking()
        {
        }
    }
}
