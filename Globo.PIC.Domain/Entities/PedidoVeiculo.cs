using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoVeiculo
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido")]
        [Range(1, long.MaxValue)]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login Acionamento")]
        public string AcionadoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Acionamento")]
        public DateTime? DataAcionamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Chegada")]
        public DateTime? DataChegada { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Devolução")]
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de Faturamento")]
        public string LocalFaturamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        public long IdStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("CompradoPorLogin")]
        public string CompradoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Pedido de pré-produção")]
        public bool PreProducao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data que o pedido foi enviado")]
        public DateTime? DataEnvio { get; set; }

        /// <summary>
        /// 
        /// </summary>  
        public PedidoVeiculo()
        {
        }

        #region Relationship one to many properties

        public virtual IEnumerable<Acionamento> Acionamento { get; set; }

        #endregion

        #region Relationship one to one properties

        public virtual Pedido Pedido { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual StatusPedidoVeiculo Status { get; set; }

        public virtual Usuario Acionador { get; set; }

        public virtual Usuario Comprador { get; set; }

        #endregion

    }
}
