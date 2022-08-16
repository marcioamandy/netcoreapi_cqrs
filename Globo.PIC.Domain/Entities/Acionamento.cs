using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Acionamento
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
        [Description("Id do Pedido Veiculos ")]
        [Range(1, long.MaxValue)]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Roteiro")]
        [MaxLength(200)]
        public string Roteiro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Local de Entrega")]
        [MaxLength(200)]
        public string LocalEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Saída")]
        public DateTime? DataSaida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Hora higienização Set")]
        [MaxLength(20)]
        public string HoraHigienizacaoSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Inicio gravação")]
        public DateTime? DataInicioGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Término de Gravação")]
        public DateTime? DataTerminoGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observação ")]
        [MaxLength(500)]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Cancelamento")]
        public DateTime? DataCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa do Cancelamento")]
        [MaxLength(200)]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Outra Justificativa ")]
        [MaxLength(500)]
        public string OutraJustificativa { get; set; }

        #region Relationship one to many properties

        public virtual IEnumerable<AcionamentoItem> AcionamentoPedidoItens { get; set; }

        #endregion

        #region Relationship many to one properties


        public virtual PedidoVeiculo PedidoVeiculo { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>  
        public Acionamento()
        {
        }
    }
}
