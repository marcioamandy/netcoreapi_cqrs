using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class AcionamentoViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Veiculos ")]
        public long IdPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Roteiro")]
        public string Roteiro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Local de Entrega")]
        public string LocalEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Saída")]
        public DateTime? DataSaida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Hora higienização Set")]
        public string HoraHigienizacaoSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Inicio gravação")]
        public DateTime? DataInicioGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Término de Gravação")]
        public DateTime? DataTerminoGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observação ")]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cena de Ação")]
        public bool Cancelado { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista AcionamentoItem")]
        public List<AcionamentoItemViewModel> AcionamentoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo PedidoVeiculo")]
        public PedidoVeiculoViewModel PedidoVeiculo { get; set; }

    }
}
