using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class TrackingArteViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Data Tracking")]
        public DateTime? TrackingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
		[Range(1, long.MaxValue)]
        [Description("Ordem do Status Pedido Item")]
        public long StatusPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
		[Range(1, long.MaxValue)]
        [Description("Id do Status Pedido Item")]
        public long StatusId { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Range(0, long.MaxValue)]
		[Description("Id do Pedido Item")]
		public long IdPedidoItemArte { get; set; }

		///// <summary>
		///// 
		///// </summary>
		//[Description("Ressalva")]
        //public string Ressalva { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Alterador Por")]
        public UsuarioViewModel ChangedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UserViewModel")]
        public StatusViewModel Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status do tracking")]
        public bool Ativo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
		[Description("Status Atual")]
		public bool Current { get; set; }

        #region Relationship many to one properties

        //public Status Status { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public TrackingArteViewModel()
        {
        }
    }
}