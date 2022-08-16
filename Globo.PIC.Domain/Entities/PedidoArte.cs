using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoArte
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
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Necessidade")]
        public DateTime? DataNecessidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Local de Utilização")]
        public string LocalUtilizacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descrição da Cena")]
        public string DescricaoCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        public long IdStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de Solicitação do cancelamento do pedido")]
        public DateTime? DataSolicitacaoCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de confirmação da compra do pedido")]
        public DateTime? DataConfirmacaoCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login Base")]
        public string BaseLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de vinculo login base ao pedido")]
        public DateTime? DataVinculoBase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Reenvio")]
        public DateTime? DataReenvio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Edição Reenvio")]
        public DateTime? DataEdicaoReenvio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Pedido de Alimentos")]
        public bool FlagPedidoAlimentos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("FastPass")]
        public bool FlagFastPass { get; set; }

        /// <summary>
        /// 
        /// </summary>  
        public PedidoArte()
        {
        }

        #region Relationship one to many properties


        #endregion

        #region Relationship one to one properties

        public virtual Pedido Pedido { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual StatusPedidoArte Status { get; set; }

        public virtual Usuario Base { get; set; }

        #endregion

    }
}
