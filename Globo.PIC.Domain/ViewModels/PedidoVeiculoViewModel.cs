using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoVeiculoViewModel : PedidoViewModel 
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Acionamento")]
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
        [Description("Lista Status do PedidoVeiculo")]
        public StatusViewModel Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemVeiculoViewModel")]
        public List<PedidoItemVeiculoViewModel> Itens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista AcionamentoViewModel")]
        public List<AcionamentoViewModel> Acionamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario Acionador")]
        public UsuarioViewModel Acionador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario Comprador")]
        public UsuarioViewModel Comprador { get; set; }

    }
}
